using ContentCompiler.Tools;
using MagickaForge.Components.Abilities;
using MagickaForge.Components.Animations;
using MagickaForge.Pipeline;
using MagickaForge.Pipeline.Json;
using MagickaForge.Pipeline.Json.Characters;
using MagickaForge.Pipeline.Json.Items;
using MagickaForge.Pipeline.Json.Levels;
using MagickaForge.Pipeline.Json.Models;
using MagickaForge.Pipeline.Json.PhysicsEntities;

namespace ContentCompiler.Data
{
    public class XNBVerifier
    {
        public VerifyResult VerifyXNB(PipelineJsonObject pipelineObject, string path)
        {
            switch (pipelineObject)
            {
                case Character character:
                    return VerifyCharacter(character, path);
                case Item item:
                    return VerifyItem(item, path);
                case Level level:
                    return VerifyLevel(level, path);
                case NonEmbeddedModel model:
                    return VerifyModel(model, path);
                case NonEmbeddedSkinnedModel skinnedModel:
                    return VerifySkinnedModel(skinnedModel, path);
                case PhysicsEntity physicsEntity:
                    return VerifyPhysicsEntity(physicsEntity, path);
                default:
                    return null;
            }
        }

        private VerifyResult VerifyCharacter(Character character, string path)
        {
            var verifyResult = new VerifyResult();

            foreach (var gib in character.Gibs)
            {
                var modelPath = GetAssetPath(path, gib.Model);

                if (modelPath == null)
                {
                    verifyResult.AddWarning($"Gib model file does not exist: {Path.GetFullPath(gib.Model, path)}");
                }
            }

            foreach (var model in character.Models)
            {
                var modelPath = GetAssetPath(path, model.Model);

                if (modelPath == null)
                {
                    verifyResult.AddWarning($"Character model file does not exist: {Path.GetFullPath(model.Model, path)}");
                }
            }

            var validAnimations = new HashSet<string>();
            var validBones = new HashSet<string>();

            var skeletonPath = GetAssetPath(path, character.AnimationSkeleton);
            if (skeletonPath == null)
            {
                verifyResult.AddWarning($"Animation skeleton file does not exist: {Path.GetFullPath(character.AnimationSkeleton, path)}");
            }
            else
            {
                var skeleton = (NonEmbeddedSkinnedModel)PipelineJsonObject.ForgeTypeToInstance(ForgeType.SkinnedModel, false);
                skeleton.Import(skeletonPath);

                foreach (var animationClip in skeleton.SkinnedModel.Animations)
                {
                    validAnimations.Add(animationClip.Name);
                }
                foreach (var bone in skeleton.SkinnedModel.Bones)
                {
                    validBones.Add(bone.Name);
                }
            }

            if (validAnimations.Count > 0)
            {
                foreach (var animationSet in character.Animations)
                {
                    foreach (var animationClip in animationSet.AnimationClips)
                    {
                        if (!validAnimations.Contains(animationClip.AnimationKey))
                        {
                            verifyResult.AddWarning($"Animation {animationClip.AnimationType} : Animation clip key '{animationClip.AnimationKey}' does not exist in the animation skeleton");
                        }
                    }
                }
            }

            if (validBones.Count > 0)
            {
                VerifyCharacterBones(verifyResult, character, validBones);
            }

            foreach (var item in character.Equipment)
            {
                var itemPath = GetAssetPath(path, item.Item);

                if (itemPath == null)
                {
                    verifyResult.AddWarning($"Item file does not exist: {Path.GetFullPath(item.Item, path)}");
                }
            }

            VerifyCharacterLimits(verifyResult, character);

            foreach (var ability in character.Abilities)
            {
                if (ability is CastSpellAbility && string.IsNullOrEmpty(ability.FuzzyExpression))
                {
                    verifyResult.AddError($"All CastSpell abilities require a fuzzy expression.");
                }
            }

            return verifyResult;
        }

        private void VerifyCharacterLimits(VerifyResult result, Character character)
        {
            if (character.Lights.Length > Character.MaxLights)
            {
                result.AddError($"Character has too many lights. [{character.Lights.Length} > {Character.MaxLights}]");
            }
            if (character.Equipment.Length > Character.MaxEquipmentSlots)
            {
                result.AddError($"Character has too many item attachments. [{character.Equipment.Length} > {Character.MaxLights}]");
            }
            if (character.Animations.Length > Character.MaxAnimationSets)
            {
                result.AddError($"Character has too many animation sets. [{character.Animations.Length} > {Character.MaxAnimationSets}]");
            }
        }

        private void VerifyCharacterBones(VerifyResult result, Character character, HashSet<string> validBones)
        {
            foreach (var light in character.Lights)
            {
                if (!validBones.Contains(light.Bone, StringComparer.OrdinalIgnoreCase))
                {
                    result.AddWarning($"{light.Bone} is not a bone in the animation skeleton");
                }
            }

            foreach (var effect in character.Effects)
            {
                if (!validBones.Contains(effect.Bone, StringComparer.OrdinalIgnoreCase))
                {
                    result.AddWarning($"Effect: {effect.Bone} is not a bone in the animation skeleton");
                }
            }

            foreach (var animationSet in character.Animations)
            {
                foreach (var animationClip in animationSet.AnimationClips)
                {
                    foreach (var animationAction in animationClip.AnimationActions)
                    {
                        if (animationAction is Grip grip)
                        {
                            if (!string.IsNullOrEmpty(grip.BoneA) && !validBones.Contains(grip.BoneA, StringComparer.OrdinalIgnoreCase))
                            {
                                result.AddWarning($"Grip action: BoneB {grip.BoneA} is not a bone in the animation skeleton");
                            }

                            if (!string.IsNullOrEmpty(grip.BoneB) && !validBones.Contains(grip.BoneB, StringComparer.OrdinalIgnoreCase))
                            {
                                result.AddWarning($"Grip action: BoneA {grip.BoneB} is not a bone in the animation skeleton");
                            }
                        }
                        else if (animationAction is CastSpellEvent spell)
                        {
                            if (!string.IsNullOrEmpty(spell.Bone) && !validBones.Contains(spell.Bone, StringComparer.OrdinalIgnoreCase))
                            {
                                result.AddWarning($"CastSpell action: {spell.Bone} is not a bone in the animation skeleton");
                            }
                        }
                        else if (animationAction is PlayEffect playEffect)
                        {
                            if (!validBones.Contains(playEffect.Bone, StringComparer.OrdinalIgnoreCase))
                            {
                                result.AddWarning($"PlayEffect action: {playEffect.Bone} is not a bone in the animation skeleton");
                            }
                        }
                        else if (animationAction is SetItemAttach setAttachment)
                        {
                            if (!validBones.Contains(setAttachment.Bone, StringComparer.OrdinalIgnoreCase))
                            {
                                result.AddWarning($"SetItemAttach action: {setAttachment.Bone} is not a bone in the animation skeleton");
                            }
                        }
                    }
                }
            }

            foreach (var item in character.Equipment)
            {
                if (!validBones.Contains(item.Bone, StringComparer.OrdinalIgnoreCase))
                {
                    result.AddWarning($"Item {item.Item} is attached to invalid bone {item.Bone} which is not in the animation skeleton");
                }
            }
        }

        private VerifyResult VerifyItem(Item item, string path)
        {
            var verifyResult = new VerifyResult();

            if (item.CanBePickedUp)
            {
                if (string.IsNullOrEmpty(item.LocalizedDescription))
                {
                    verifyResult.AddWarning("Item is grabbable but has no localization name");
                }
                if (string.IsNullOrEmpty(item.LocalizedDescription))
                {
                    verifyResult.AddWarning("Item is grabbable but has no localization description");
                }
            }

            if (item.Lights.Length > Item.MaxLights)
            {
                verifyResult.AddError($"Item has too many lights. [{item.Lights.Length} > {Item.MaxLights}]");
            }
            if (!string.IsNullOrEmpty(item.Model) && GetAssetPath(path, item.Model) == null)
            {
                verifyResult.AddWarning($"Item model file does not exist: {Path.GetFullPath(item.Model, path)}");
            }
            if (!string.IsNullOrEmpty(item.ProjectileModel) && GetAssetPath(path, item.ProjectileModel) == null)
            {
                verifyResult.AddWarning($"Item projectile model file does not exist: {Path.GetFullPath(item.ProjectileModel, path)}");
            }

            return verifyResult;
        }

        private VerifyResult VerifyLevel(Level level, string path)
        {
            var verifyResult = new VerifyResult();
            if (level.CollisionMeshes.Length > Level.MaxCollisionMeshes)
            {
                verifyResult.AddError($"Level has too many collision meshes. [{level.CollisionMeshes.Length} > {Level.MaxCollisionMeshes}]");
            }
            return verifyResult;
        }

        private VerifyResult VerifyModel(NonEmbeddedModel model, string path)
        {
            return new VerifyResult();
        }

        private VerifyResult VerifySkinnedModel(NonEmbeddedSkinnedModel skinnedModel, string path)
        {
            return new VerifyResult();
        }

        private VerifyResult VerifyPhysicsEntity(PhysicsEntity physicsEntity, string path)
        {
            return new VerifyResult();
        }

        private string GetAssetPath(string basePath, string assetPath)
        {
            var outPath = Path.GetFullPath(assetPath + FileExtensions.XNBExtension, basePath);

            if (!File.Exists(outPath))
            {
                return null;
            }

            return outPath;
        }
    }
}
