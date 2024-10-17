using MagickaForge.Forges.Components;
using MagickaForge.Forges.Components.Auras;
using MagickaForge.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagickaForge.Forges.Items
{
    public class Item
    {
        public static readonly byte[] XNB_HEADER =
        {
            0x58, 0x4E, 0x42, 0x77, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x4C,
            0x4D, 0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2E, 0x43, 0x6F, 0x6E, 0x74,
            0x65, 0x6E, 0x74, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x73, 0x2E, 0x49,
            0x74, 0x65, 0x6D, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C, 0x20, 0x4D,
            0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2C, 0x20, 0x56, 0x65, 0x72, 0x73,
            0x69, 0x6F, 0x6E, 0x3D, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x2C,
            0x20, 0x43, 0x75, 0x6C, 0x74, 0x75, 0x72, 0x65, 0x3D, 0x6E, 0x65, 0x75,
            0x74, 0x72, 0x61, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01
        };

        public string? Name { get; set; }
        public string? LocalizedName { get; set; }
        public string? LocalizedDescription { get; set; }
        public Sound[]? Sounds { get; set; }
        public bool Grabbable { get; set; }
        public bool Bound { get; set; }
        public int BlockStrength { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<WeaponClass>))]
        public WeaponClass WeaponClass { get; set; }
        public float CooldownTime { get; set; }
        public bool HideModel { get; set; }
        public bool HideEffects { get; set; }
        public bool PauseSounds { get; set; }
        public Resistance[]? Resistances { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<PassiveAbilities>))]
        public PassiveAbilities PassiveAbilityType { get; set; }
        public float PassiveAbilityStrength { get; set; }
        public string[]? Effects { get; set; }
        public Light[]? Lights { get; set; }
        public bool HasSpecialAbility { get; set; }
        public float SpecialAbilityCooldown { get; set; }
        public SpecialAbility? SpecialAbility { get; set; }
        public float MeleeRange { get; set; }
        public bool MeleeMultihit { get; set; }
        public ConditionCollection[]? MeleeConditions { get; set; }
        public float RangedRange { get; set; }
        public bool Facing { get; set; }
        public float Homing { get; set; }
        public float RangedElevation { get; set; }
        public float RangedDanger { get; set; }
        public float GunRange { get; set; }
        public int GunClip { get; set; }
        public int GunRate { get; set; }
        public float GunAccuracy { get; set; }
        public string? GunSound { get; set; }
        public string? GunMuzzleEffect { get; set; }
        public string? GunShellEffect { get; set; }
        public float GunTracerVelocity { get; set; }
        public string? GunNonTracer { get; set; }
        public string? GunTracer { get; set; }
        public ConditionCollection[]? GunConditions { get; set; }
        public string? ProjectileModel { get; set; }
        public ConditionCollection[]? RangedConditions { get; set; }
        public float Scale { get; set; }
        public string? Model { get; set; }

        public Aura[]? Auras { get; set; }


        public void ItemToXNB(string outputPath)
        {
            BinaryWriter bw = new (File.Create(outputPath));
            bw.Write(XNB_HEADER);
            bw.Write(Name!);
            bw.Write(LocalizedName!);
            bw.Write(LocalizedDescription!);
            bw.Write(Sounds!.Length);
            for (int i = 0; i < Sounds.Length; i++)
            {
                bw.Write(Sounds[i].Cue!);
                bw.Write((int)Sounds[i].Bank);
            }
            bw.Write(Grabbable);
            bw.Write(Bound);
            bw.Write(BlockStrength);
            bw.Write((byte)WeaponClass);
            bw.Write(CooldownTime);
            bw.Write(HideModel);
            bw.Write(HideEffects);
            bw.Write(PauseSounds);
            bw.Write(Resistances!.Length);
            for (int i = 0; i < Resistances.Length; i++)
            {
                bw.Write((int)Resistances[i].Element);
                bw.Write(Resistances[i].Multiplier);
                bw.Write(Resistances[i].Modifier);
                bw.Write(Resistances[i].StatusImmunity);
            }
            bw.Write((byte)PassiveAbilityType);
            bw.Write(PassiveAbilityStrength);
            bw.Write(Effects!.Length);
            for (int i = 0; i < Effects.Length; i++)
            {
                bw.Write(Effects[i]);
            }
            bw.Write(Lights!.Length);
            for (int i = 0; i < Lights.Length; i++)
            {
                bw.Write(Lights[i].Radius);
                for (int x = 0; x < 3; x++)
                {
                    bw.Write(Lights[i].DiffuseColor[x]);
                }
                for (int x = 0; x < 3; x++)
                {
                    bw.Write(Lights[i].AmbientColor[x]);
                }
                bw.Write(Lights[i].SpecularAmount);
                bw.Write((byte)Lights[i].LightVariationType);
                bw.Write(Lights[i].VariationAmount);
                bw.Write(Lights[i].VariationSpeed);
            }
            bw.Write(HasSpecialAbility);
            if (HasSpecialAbility)
            {
                bw.Write(SpecialAbilityCooldown);
                bw.Write(SpecialAbility!.Type!);
                bw.Write(SpecialAbility.Animation!);
                bw.Write(SpecialAbility.Hash!);
                for (int x = 0; x < SpecialAbility!.Elements!.Length; x++)
                {
                    bw.Write(SpecialAbility.Elements[x]);
                }
            }
            bw.Write(MeleeRange);
            bw.Write(MeleeMultihit);
            bw.Write(MeleeConditions!.Length);
            for (int i = 0; i < MeleeConditions.Length; i++)
            {
                bw.Write((byte)MeleeConditions[i].ConditionType);
                bw.Write(MeleeConditions[i].Hitpoints);
                bw.Write((int)MeleeConditions[i].Element);
                bw.Write(MeleeConditions[i].Threshold);
                bw.Write(MeleeConditions[i].Time);
                bw.Write(MeleeConditions[i].Repeat);
                bw.Write(MeleeConditions[i].Events!.Length);
                for (int x = 0; x < MeleeConditions[i].Events?.Length; x++)
                {
                    MeleeConditions[i].Events![x].Write(bw);
                }
            }
            bw.Write(RangedRange);
            bw.Write(Facing);
            bw.Write(Homing);
            bw.Write(RangedElevation);
            bw.Write(RangedDanger);
            bw.Write(GunRange);
            bw.Write(GunClip);
            bw.Write(GunRate);
            bw.Write(GunAccuracy);
            bw.Write(GunSound!);
            bw.Write(GunMuzzleEffect!);
            bw.Write(GunShellEffect!);
            bw.Write(GunTracerVelocity);
            bw.Write(GunNonTracer!);
            bw.Write(GunTracer!);
            bw.Write(GunConditions!.Length);
            for (int i = 0; i < GunConditions.Length; i++)
            {
                bw.Write((byte)GunConditions[i].ConditionType);
                bw.Write(GunConditions[i].Hitpoints);
                bw.Write((int)GunConditions[i].Element);
                bw.Write(GunConditions[i].Threshold);
                bw.Write(GunConditions[i].Time);
                bw.Write(GunConditions[i].Repeat);
                bw.Write(GunConditions[i].Events!.Length);
                for (int x = 0; x < GunConditions[i].Events?.Length; x++)
                {
                    GunConditions[i].Events![x].Write(bw);
                }
            }
            bw.Write(ProjectileModel!);
            bw.Write(RangedConditions!.Length);
            for (int i = 0; i < RangedConditions.Length; i++)
            {
                bw.Write((byte)RangedConditions[i].ConditionType);
                bw.Write(RangedConditions[i].Hitpoints);
                bw.Write((int)RangedConditions[i].Element);
                bw.Write(RangedConditions[i].Threshold);
                bw.Write(RangedConditions[i].Time);
                bw.Write(RangedConditions[i].Repeat);
                bw.Write(RangedConditions[i].Events!.Length);
                for (int x = 0; x < RangedConditions[i].Events?.Length; x++)
                {
                    RangedConditions[i].Events![x].Write(bw);
                }
            }
            bw.Write(Scale);
            bw.Write(Model!);
            bw.Write(Auras!.Length);
            for (int i = 0; i < Auras.Length; i++)
            {
                Auras[i].Write(bw);
            }
            bw.Close();
        }
        public static void WriteToJson(string outputPath, Item item)
        {
            StreamWriter sw = new (outputPath);
            sw.Write(JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true, }));
            sw.Close();
        }
        public static Item LoadFromJson(string inputPath)
        {
            StreamReader sr = new (inputPath);
            string json = sr.ReadToEnd();
            sr.Close();
            return JsonSerializer.Deserialize<Item>(json)!;
        }

        public Item XNBToItem(string inputPath)
        {
            BinaryReader br = new (File.Open(inputPath, FileMode.Open));
            br.ReadBytes(XNB_HEADER.Length);

            Name = br.ReadString();
            LocalizedName = br.ReadString();
            LocalizedDescription = br.ReadString();

            int x = br.ReadInt32();
            Sounds = new Sound[x];
            for (int i = 0; i < x; i++)
            {
                Sounds[i].Cue = br.ReadString();
                Sounds[i].Bank = (Banks)br.ReadInt32();
            }
            Grabbable = br.ReadBoolean();
            Bound = br.ReadBoolean();
            BlockStrength = br.ReadInt32();
            WeaponClass = (WeaponClass)br.ReadByte();
            CooldownTime = br.ReadSingle();
            HideModel = br.ReadBoolean();
            HideEffects = br.ReadBoolean();
            PauseSounds = br.ReadBoolean();

            Resistances = new Resistance[br.ReadInt32()];
            for (int i = 0; i < Resistances.Length; i++)
            {
                Resistances[i].Element = (Elements)br.ReadInt32();
                Resistances[i].Multiplier = br.ReadSingle();
                Resistances[i].Modifier = br.ReadSingle();
                Resistances[i].StatusImmunity = br.ReadBoolean();
            }
            PassiveAbilityType = (PassiveAbilities)br.ReadByte();
            PassiveAbilityStrength = br.ReadSingle();

            Effects = new string[br.ReadInt32()];
            for (int i = 0; i < Effects.Length; i++)
            {
                Effects[i] = br.ReadString();
            }

            Lights = new Light[br.ReadInt32()];
            for (int i = 0; i < Lights.Length; i++)
            {
                Lights[i].Radius = br.ReadSingle();

                Lights[i].DiffuseColor = new float[3];
                Lights[i].AmbientColor = new float[3];

                for (int y = 0; y < 3; y++)
                {
                    Lights[i].DiffuseColor[y] = br.ReadSingle();
                }
                for (int y = 0; y < 3; y++)
                {
                    Lights[i].AmbientColor[y] = br.ReadSingle();
                }
                Lights[i].SpecularAmount = br.ReadSingle();
                Lights[i].LightVariationType = (LightVariationType)br.ReadByte();
                Lights[i].VariationAmount = br.ReadSingle();
                Lights[i].VariationSpeed = br.ReadSingle();
            }

            HasSpecialAbility = br.ReadBoolean();
            if (HasSpecialAbility)
            {
                SpecialAbilityCooldown = br.ReadSingle();
                SpecialAbility specialAbility = new ();
                specialAbility.Type = br.ReadString();
                specialAbility.Animation = br.ReadString();
                specialAbility.Hash = br.ReadString();
                specialAbility.Elements = new int[br.ReadInt32()];
                for (int i = 0; i < specialAbility.Elements.Length; i++)
                {
                    specialAbility.Elements[i] = br.ReadInt32();
                }
                SpecialAbility = specialAbility;
            }
            MeleeRange = br.ReadSingle();
            MeleeMultihit = br.ReadBoolean();
            MeleeConditions = new ConditionCollection[br.ReadInt32()];
            for (int i = 0; i < MeleeConditions.Length; i++)
            {
                MeleeConditions[i] = new ConditionCollection(br);
            }
            RangedRange = br.ReadSingle();
            Facing = br.ReadBoolean();
            Homing = br.ReadSingle();
            RangedElevation = br.ReadSingle();
            RangedDanger = br.ReadSingle();
            GunRange = br.ReadSingle();
            GunClip = br.ReadInt32();
            GunRate = br.ReadInt32();
            GunAccuracy = br.ReadSingle();
            GunSound = br.ReadString();
            GunMuzzleEffect = br.ReadString();
            GunShellEffect = br.ReadString();
            GunTracerVelocity = br.ReadSingle();
            GunNonTracer = br.ReadString();
            GunTracer = br.ReadString();
            GunConditions = new ConditionCollection[br.ReadInt32()];
            for (int i = 0; i < GunConditions.Length; i++)
            {
                GunConditions[i] = new ConditionCollection(br);
            }
            ProjectileModel = br.ReadString();
            RangedConditions = new ConditionCollection[br.ReadInt32()];
            for (int i = 0; i < RangedConditions.Length; i++)
            {
                RangedConditions[i] = new ConditionCollection(br);
            }
            Scale = br.ReadSingle();
            Model = br.ReadString();
            Auras = new Aura[br.ReadInt32()];
            for (int i = 0; i < Auras.Length; i++)
            {
                AuraTarget target = (AuraTarget)br.ReadByte();
                AuraType auraType = (AuraType)br.ReadByte();
                VisualCategory visualCategory = (VisualCategory)br.ReadByte();
                float[] color = new float[3];
                for (int y = 0; y < 3; y++)
                {
                    color[y] = br.ReadSingle();
                }
                string effect = br.ReadString();
                float duration = br.ReadSingle();
                float Radius = br.ReadSingle();
                string Types = br.ReadString();
                Factions Faction = (Factions)br.ReadInt32();

                switch (auraType)
                {
                    case (AuraType.Buff):
                        {
                            Auras[i] = new BuffAura()
                            {
                                Buff = Buff.GetBuff(br)
                            };
                        }
                        break;
                    case (AuraType.Deflect):
                        {
                            Auras[i] = new DeflectAura()
                            {
                                DeflectStrength = br.ReadSingle(),
                            };
                        }
                        break;
                    case (AuraType.Boost):
                        {
                            Auras[i] = new BoostAura()
                            {
                                BoostStrength = br.ReadSingle(),
                            };
                        }
                        break;
                    case (AuraType.LifeSteal):
                        {
                            Auras[i] = new LifeStealAura()
                            {
                                LifeStealAmount = br.ReadSingle(),
                            };
                        }
                        break;
                    case (AuraType.Love):
                        {
                            Auras[i] = new LoveAura()
                            {
                                CharmRadius = br.ReadSingle(),
                                CharmDuration = br.ReadSingle(),
                            };
                        }
                        break;
                };

                Auras[i].AuraTarget = target;
                Auras[i].VisualCategory = visualCategory;
                Auras[i].Color = color;
                Auras[i].Effect = effect;
                Auras[i].Duration = duration;
                Auras[i].Radius = Radius;
                Auras[i].Types = Types;
                Auras[i].Faction = Faction;
            }

            br.Close();
            return this;
        }
    }

}
