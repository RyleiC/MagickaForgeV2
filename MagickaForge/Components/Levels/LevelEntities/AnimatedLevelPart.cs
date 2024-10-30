using MagickaForge.Components.Graphics.Models;
using MagickaForge.Components.Levels.Liquid;
using MagickaForge.Components.Levels.Navigation;
using MagickaForge.Components.XNB;
using MagickaForge.Utils.Structures;

namespace MagickaForge.Components.Levels.LevelEntities
{
    public class AnimatedLevelPart
    {
        public string Name { get; set; }
        public bool AffectShields { get; set; }
        public Model model { get; set; }
        public MeshSetting[] MeshSettings { get; set; }
        public LiquidDeclaration[] Liquids { get; set; }
        public Locator[] Locators { get; set; }
        public float AnimationDuration { get; set; }
        public AnimationDataChannel[] AnimationDataChannels { get; set; }
        public Effect[] Effects { get; set; }
        public string[] LightNames { get; set; }
        public Matrix[] LightPositions { get; set; }
        public AnimationTriMesh MaterialMesh { get; set; }
        public NavigationMesh NavMesh { get; set; }
        public AnimatedLevelPart[] Children { get; set; }
        public AnimatedLevelPart() { }
        public AnimatedLevelPart(BinaryReader binaryReader, Header header)
        {
            Name = binaryReader.ReadString();
            AffectShields = binaryReader.ReadBoolean();
            model = new Model(binaryReader); //EVIL
            MeshSettings = new MeshSetting[binaryReader.ReadInt32()];
            for (var i = 0; i < MeshSettings.Length; i++)
            {
                MeshSettings[i] = new MeshSetting(binaryReader);
            }
            Liquids = new LiquidDeclaration[binaryReader.ReadInt32()];
            for (var i = 0; i < Liquids.Length; i++)
            {
                Liquids[i] = new LiquidDeclaration(binaryReader, header);
            }
            Locators = new Locator[binaryReader.ReadInt32()];
            for (var i = 0; i < Locators.Length; i++)
            {
                Locators[i] = new Locator(binaryReader);
            }
            AnimationDuration = binaryReader.ReadSingle();
            AnimationDataChannels = new AnimationDataChannel[binaryReader.ReadInt32()];
            for (var i = 0; i < AnimationDataChannels.Length; i++)
            {
                AnimationDataChannels[i] = new AnimationDataChannel(binaryReader);
            }
            Effects = new Effect[binaryReader.ReadInt32()];
            for (var i = 0; i < Effects.Length; i++)
            {
                Effects[i] = new Effect(binaryReader);
            }
            LightNames = new string[binaryReader.ReadInt32()];
            LightPositions = new Matrix[LightNames.Length];
            for (var i = 0; i < LightNames.Length; i++)
            {
                LightNames[i] = binaryReader.ReadString();
                LightPositions[i] = new Matrix(binaryReader);
            }
            if (binaryReader.ReadBoolean())
            {
                MaterialMesh = new AnimationTriMesh(binaryReader);
            }
            if (binaryReader.ReadBoolean())
            {
                NavMesh = new NavigationMesh(binaryReader);
            }
            Children = new AnimatedLevelPart[binaryReader.ReadInt32()];
            for (var i = 0; i < Children.Length; i++)
            {
                Children[i] = new AnimatedLevelPart(binaryReader, header);
            }

        }
        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Name);
            binaryWriter.Write(AffectShields);
            model.Write(binaryWriter);
            binaryWriter.Write(MeshSettings.Length);
            for (var i = 0; i < MeshSettings.Length; i++)
            {
                binaryWriter.Write(MeshSettings[i].Key);
                binaryWriter.Write(MeshSettings[i].Value);
                binaryWriter.Write(MeshSettings[i].Highlight);
            }
            binaryWriter.Write(Liquids.Length);
            for (var i = 0; i < Liquids.Length; i++)
            {
                Liquids[i].Write(binaryWriter);
            }
            binaryWriter.Write(Locators.Length);
            foreach (var locator in Locators)
            {
                locator.Write(binaryWriter);
            }
            binaryWriter.Write(AnimationDuration);
            binaryWriter.Write(AnimationDataChannels.Length);
            foreach (AnimationDataChannel animationDataChannel in AnimationDataChannels)
            {
                animationDataChannel.Write(binaryWriter);
            }
            binaryWriter.Write(Effects.Length);
            foreach (Effect effect in Effects)
            {
                effect.Write(binaryWriter);
            }
            binaryWriter.Write(LightNames.Length);
            for (var i = 0; i < LightNames.Length; i++)
            {
                binaryWriter.Write(LightNames[i]);
                LightPositions[i].Write(binaryWriter);
            }
            var hasCollision = MaterialMesh != null;
            binaryWriter.Write(hasCollision);
            if (hasCollision)
            {
                MaterialMesh.Write(binaryWriter);
            }
            var hasNavMesh = NavMesh != null;
            binaryWriter.Write(hasNavMesh);
            if (hasNavMesh)
            {
                NavMesh.Write(binaryWriter);
            }
            binaryWriter.Write(Children.Length);
            foreach (var child in Children)
            {
                child.Write(binaryWriter);
            }
        }
    }
}
