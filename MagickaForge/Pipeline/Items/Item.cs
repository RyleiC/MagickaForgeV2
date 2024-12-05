using MagickaForge.Components;
using MagickaForge.Components.Auras;
using MagickaForge.Components.Events;
using MagickaForge.Components.Lights;
using MagickaForge.Utils;
using MagickaForge.Utils.Definitions;
using MagickaForge.Utils.Definitions.Abilities;
using MagickaForge.Utils.Definitions.Graphics;
using MagickaForge.Utils.Structures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagickaForge.Pipeline.Items
{
    public class Item
    {
        private const int MaxLights = 1;

        private readonly byte[] ReaderHeader =
        {
            0x01, 0x4C, 0x4D, 0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2E, 0x43, 0x6F,
            0x6E, 0x74, 0x65, 0x6E, 0x74, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x73,
            0x2E, 0x49, 0x74, 0x65, 0x6D, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C,
            0x20, 0x4D, 0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2C, 0x20, 0x56, 0x65,
            0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x2E,
            0x30, 0x2C, 0x20, 0x43, 0x75, 0x6C, 0x74, 0x75, 0x72, 0x65, 0x3D, 0x6E,
            0x65, 0x75, 0x74, 0x72, 0x61, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01
        };

        public string? Name { get; set; }
        public string? LocalizedName { get; set; }
        public string? LocalizedDescription { get; set; }
        public Sound[]? Sounds { get; set; }
        public bool CanBePickedUp { get; set; }
        public bool Bound { get; set; }
        public int BlockStrength { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<WeaponClass>))]
        public WeaponClass WeaponClass { get; set; }
        public float CooldownTime { get; set; }
        public bool HideModel { get; set; }
        public bool HideEffects { get; set; }
        public bool PauseSounds { get; set; }
        public Resistance[]? Resistances { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<PassiveAbility>))]
        public PassiveAbility PassiveAbilityType { get; set; }
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
        public float HomingStrength { get; set; }
        public float RangedElevation { get; set; }
        public float RangedDanger { get; set; }
        public float GunRange { get; set; }
        public int GunClip { get; set; }
        public int GunRate { get; set; }
        public float GunAccuracy { get; set; }
        public string? GunSoundCue { get; set; }
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
            BinaryWriter bw = new(File.Create(outputPath));
            bw.Write(XNBHelper.XNBHeader);
            bw.Write(0); //File size placeholder, we go back after the file is written and put in the file size.
            bw.Write(ReaderHeader);

            bw.Write(Name!);
            bw.Write(LocalizedName!);
            bw.Write(LocalizedDescription!);
            bw.Write(Sounds!.Length);
            foreach (Sound sound in Sounds)
            {
                sound.Write(bw);
            }
            bw.Write(CanBePickedUp);
            bw.Write(Bound);
            bw.Write(BlockStrength);
            bw.Write((byte)WeaponClass);
            bw.Write(CooldownTime);
            bw.Write(HideModel);
            bw.Write(HideEffects);
            bw.Write(PauseSounds);
            bw.Write(Resistances!.Length);
            for (var i = 0; i < Resistances.Length; i++)
            {
                bw.Write((int)Resistances[i].Element);
                bw.Write(Resistances[i].Multiplier);
                bw.Write(Resistances[i].Modifier);
                bw.Write(Resistances[i].StatusImmunity);
            }
            bw.Write((byte)PassiveAbilityType);
            bw.Write(PassiveAbilityStrength);
            bw.Write(Effects!.Length);
            for (var i = 0; i < Effects.Length; i++)
            {
                bw.Write(Effects[i]);
            }

            if (Lights!.Length > MaxLights)
            {
                throw new ArgumentOutOfRangeException("Items may only have up to 1 light!");
            }

            bw.Write(Lights!.Length);
            for (var i = 0; i < Lights.Length; i++)
            {
                bw.Write(Lights[i].Radius);
                Lights[i].DiffuseColor.Write(bw);
                Lights[i].AmbientColor.Write(bw);
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
                bw.Write(SpecialAbility.Elements!.Length);
                for (var x = 0; x < SpecialAbility!.Elements!.Length; x++)
                {
                    bw.Write((int)SpecialAbility.Elements[x]);
                }
            }
            bw.Write(MeleeRange);
            bw.Write(MeleeMultihit);
            bw.Write(MeleeConditions!.Length);
            for (var i = 0; i < MeleeConditions.Length; i++)
            {
                MeleeConditions[i].Write(bw);
            }
            bw.Write(RangedRange);
            bw.Write(Facing);
            bw.Write(HomingStrength);
            bw.Write(RangedElevation);
            bw.Write(RangedDanger);
            bw.Write(GunRange);
            bw.Write(GunClip);
            bw.Write(GunRate);
            bw.Write(GunAccuracy);
            bw.Write(GunSoundCue!);
            bw.Write(GunMuzzleEffect!);
            bw.Write(GunShellEffect!);
            bw.Write(GunTracerVelocity);
            bw.Write(GunNonTracer!);
            bw.Write(GunTracer!);
            bw.Write(GunConditions!.Length);
            for (var i = 0; i < GunConditions.Length; i++)
            {
                GunConditions[i].Write(bw);
            }
            bw.Write(ProjectileModel!);
            bw.Write(RangedConditions!.Length);
            for (var i = 0; i < RangedConditions.Length; i++)
            {
                RangedConditions[i].Write(bw);
            }
            bw.Write(Scale);
            bw.Write(Model!);
            bw.Write(Auras!.Length);
            for (var i = 0; i < Auras.Length; i++)
            {
                Auras[i].Write(bw);
            }

            XNBHelper.WriteFileSize(bw);

            bw.Close();
        }
        public static void WriteToJson(string outputPath, Item item)
        {
            StreamWriter sw = new(outputPath);
            sw.Write(JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true, }));
            sw.Close();
        }
        public static Item LoadFromJson(string inputPath)
        {
            string json = File.ReadAllText(inputPath);
            return JsonSerializer.Deserialize<Item>(json)!;
        }

        public void XNBToItem(string inputPath)
        {
            BinaryReader br = new(XNBDecompressor.DecompressXNB(inputPath));
            br.BaseStream.Position += XNBHelper.XNBHeader.Length + ReaderHeader.Length + 4; //Ingores the entire header length, including the XNB header, fileSize, and reader header.

            Name = br.ReadString();
            LocalizedName = br.ReadString();
            LocalizedDescription = br.ReadString();

            Sounds = new Sound[br.ReadInt32()];
            for (var i = 0; i < Sounds.Length; i++)
            {
                Sounds[i].Cue = br.ReadString();
                Sounds[i].Bank = (Banks)br.ReadInt32();
            }

            CanBePickedUp = br.ReadBoolean();
            Bound = br.ReadBoolean();
            BlockStrength = br.ReadInt32();
            WeaponClass = (WeaponClass)br.ReadByte();
            CooldownTime = br.ReadSingle();
            HideModel = br.ReadBoolean();
            HideEffects = br.ReadBoolean();
            PauseSounds = br.ReadBoolean();

            Resistances = new Resistance[br.ReadInt32()];
            for (var i = 0; i < Resistances.Length; i++)
            {
                Resistances[i].Element = (Elements)br.ReadInt32();
                Resistances[i].Multiplier = br.ReadSingle();
                Resistances[i].Modifier = br.ReadSingle();
                Resistances[i].StatusImmunity = br.ReadBoolean();
            }

            PassiveAbilityType = (PassiveAbility)br.ReadByte();
            PassiveAbilityStrength = br.ReadSingle();

            Effects = new string[br.ReadInt32()];
            for (var i = 0; i < Effects.Length; i++)
            {
                Effects[i] = br.ReadString();
            }

            Lights = new Light[br.ReadInt32()];
            for (var i = 0; i < Lights.Length; i++)
            {
                Lights[i].Radius = br.ReadSingle();
                Lights[i].DiffuseColor = new Color(br);
                Lights[i].AmbientColor = new Color(br);
                Lights[i].SpecularAmount = br.ReadSingle();
                Lights[i].LightVariationType = (LightVariationType)br.ReadByte();
                Lights[i].VariationAmount = br.ReadSingle();
                Lights[i].VariationSpeed = br.ReadSingle();
            }

            HasSpecialAbility = br.ReadBoolean();
            if (HasSpecialAbility)
            {
                SpecialAbilityCooldown = br.ReadSingle();
                SpecialAbility specialAbility = new()
                {
                    Type = br.ReadString(),
                    Animation = br.ReadString(),
                    Hash = br.ReadString(),
                    Elements = new Elements[br.ReadInt32()]
                };
                for (var i = 0; i < specialAbility.Elements.Length; i++)
                {
                    specialAbility.Elements[i] = (Elements)br.ReadInt32();
                }
                SpecialAbility = specialAbility;
            }

            MeleeRange = br.ReadSingle();
            MeleeMultihit = br.ReadBoolean();
            MeleeConditions = new ConditionCollection[br.ReadInt32()];
            for (var i = 0; i < MeleeConditions.Length; i++)
            {
                MeleeConditions[i] = new ConditionCollection(br);
            }

            RangedRange = br.ReadSingle();
            Facing = br.ReadBoolean();
            HomingStrength = br.ReadSingle();
            RangedElevation = br.ReadSingle();
            RangedDanger = br.ReadSingle();
            GunRange = br.ReadSingle();
            GunClip = br.ReadInt32();
            GunRate = br.ReadInt32();
            GunAccuracy = br.ReadSingle();
            GunSoundCue = br.ReadString();
            GunMuzzleEffect = br.ReadString();
            GunShellEffect = br.ReadString();
            GunTracerVelocity = br.ReadSingle();
            GunNonTracer = br.ReadString();
            GunTracer = br.ReadString();
            GunConditions = new ConditionCollection[br.ReadInt32()];
            for (var i = 0; i < GunConditions.Length; i++)
            {
                GunConditions[i] = new ConditionCollection(br);
            }

            ProjectileModel = br.ReadString();
            RangedConditions = new ConditionCollection[br.ReadInt32()];
            for (var i = 0; i < RangedConditions.Length; i++)
            {
                RangedConditions[i] = new ConditionCollection(br);
            }

            Scale = br.ReadSingle();
            Model = br.ReadString();
            Auras = new Aura[br.ReadInt32()];
            for (var i = 0; i < Auras.Length; i++)
            {
                Auras[i] = Aura.GetAura(br);
            }

            br.Close();
        }
    }

}
