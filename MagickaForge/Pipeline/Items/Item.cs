﻿using MagickaForge.Components;
using MagickaForge.Components.Auras;
using MagickaForge.Components.Events;
using MagickaForge.Components.Lights;
using MagickaForge.Utils.Data;
using MagickaForge.Utils.Data.Abilities;
using MagickaForge.Utils.Data.Graphics;
using MagickaForge.Utils.Helpers;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;

namespace MagickaForge.Pipeline.Items
{
    public class Item : PipelineObject
    {
        private const int MaxLights = 1;

        private readonly byte[] ReaderHeader =
        [
            0x01, 0x4C, 0x4D, 0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2E, 0x43, 0x6F,
            0x6E, 0x74, 0x65, 0x6E, 0x74, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x73,
            0x2E, 0x49, 0x74, 0x65, 0x6D, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C,
            0x20, 0x4D, 0x61, 0x67, 0x69, 0x63, 0x6B, 0x61, 0x2C, 0x20, 0x56, 0x65,
            0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x2E,
            0x30, 0x2C, 0x20, 0x43, 0x75, 0x6C, 0x74, 0x75, 0x72, 0x65, 0x3D, 0x6E,
            0x65, 0x75, 0x74, 0x72, 0x61, 0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01
        ];

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

        public override void WriteToXNB(string outputPath)
        {
            BinaryWriter binaryWriter = new(File.Create(outputPath));
            binaryWriter.Write(XNBHelper.XNBHeader);
            binaryWriter.Write(0); //File size placeholder, we go back after the file is written and put in the file size.
            binaryWriter.Write(ReaderHeader);

            binaryWriter.Write(Name!);
            binaryWriter.Write(LocalizedName!);
            binaryWriter.Write(LocalizedDescription!);
            binaryWriter.Write(Sounds!.Length);
            foreach (Sound sound in Sounds)
            {
                sound.Write(binaryWriter);
            }
            binaryWriter.Write(CanBePickedUp);
            binaryWriter.Write(Bound);
            binaryWriter.Write(BlockStrength);
            binaryWriter.Write((byte)WeaponClass);
            binaryWriter.Write(CooldownTime);
            binaryWriter.Write(HideModel);
            binaryWriter.Write(HideEffects);
            binaryWriter.Write(PauseSounds);
            binaryWriter.Write(Resistances!.Length);
            for (var i = 0; i < Resistances.Length; i++)
            {
                binaryWriter.Write((int)Resistances[i].Element);
                binaryWriter.Write(Resistances[i].Multiplier);
                binaryWriter.Write(Resistances[i].Modifier);
                binaryWriter.Write(Resistances[i].StatusImmunity);
            }
            binaryWriter.Write((byte)PassiveAbilityType);
            binaryWriter.Write(PassiveAbilityStrength);
            binaryWriter.Write(Effects!.Length);
            for (var i = 0; i < Effects.Length; i++)
            {
                binaryWriter.Write(Effects[i]);
            }

            if (Lights!.Length > MaxLights)
            {
                throw new CantLoadInMagickaException("Items may only have up to 1 light!");
            }

            binaryWriter.Write(Lights!.Length);
            for (var i = 0; i < Lights.Length; i++)
            {
                binaryWriter.Write(Lights[i].Radius);
                Lights[i].DiffuseColor.Write(binaryWriter);
                Lights[i].AmbientColor.Write(binaryWriter);
                binaryWriter.Write(Lights[i].SpecularAmount);
                binaryWriter.Write((byte)Lights[i].LightVariationType);
                binaryWriter.Write(Lights[i].VariationAmount);
                binaryWriter.Write(Lights[i].VariationSpeed);
            }
            binaryWriter.Write(HasSpecialAbility);
            if (HasSpecialAbility)
            {
                binaryWriter.Write(SpecialAbilityCooldown);
                binaryWriter.Write(SpecialAbility!.Type!);
                binaryWriter.Write(SpecialAbility.Animation!);
                binaryWriter.Write(SpecialAbility.Hash!);
                binaryWriter.Write(SpecialAbility.Elements!.Length);
                for (var x = 0; x < SpecialAbility!.Elements!.Length; x++)
                {
                    binaryWriter.Write((int)SpecialAbility.Elements[x]);
                }
            }
            binaryWriter.Write(MeleeRange);
            binaryWriter.Write(MeleeMultihit);
            binaryWriter.Write(MeleeConditions!.Length);
            for (var i = 0; i < MeleeConditions.Length; i++)
            {
                MeleeConditions[i].Write(binaryWriter);
            }
            binaryWriter.Write(RangedRange);
            binaryWriter.Write(Facing);
            binaryWriter.Write(HomingStrength);
            binaryWriter.Write(RangedElevation);
            binaryWriter.Write(RangedDanger);
            binaryWriter.Write(GunRange);
            binaryWriter.Write(GunClip);
            binaryWriter.Write(GunRate);
            binaryWriter.Write(GunAccuracy);
            binaryWriter.Write(GunSoundCue!);
            binaryWriter.Write(GunMuzzleEffect!);
            binaryWriter.Write(GunShellEffect!);
            binaryWriter.Write(GunTracerVelocity);
            binaryWriter.Write(GunNonTracer!);
            binaryWriter.Write(GunTracer!);
            binaryWriter.Write(GunConditions!.Length);
            for (var i = 0; i < GunConditions.Length; i++)
            {
                GunConditions[i].Write(binaryWriter);
            }
            binaryWriter.Write(ProjectileModel!);
            binaryWriter.Write(RangedConditions!.Length);
            for (var i = 0; i < RangedConditions.Length; i++)
            {
                RangedConditions[i].Write(binaryWriter);
            }
            binaryWriter.Write(Scale);
            binaryWriter.Write(Model!);
            binaryWriter.Write(Auras!.Length);
            for (var i = 0; i < Auras.Length; i++)
            {
                Auras[i].Write(binaryWriter);
            }

            XNBHelper.WriteFileSize(binaryWriter);
            binaryWriter.Close();
        }

        public override void ReadFromXNB(string inputPath)
        {
            BinaryReader binaryReader = new(XNBHelper.DecompressXNB(inputPath));
            binaryReader.BaseStream.Position += XNBHelper.XNBHeader.Length + ReaderHeader.Length + 4; //Ingores the entire header length, including the XNB header, fileSize, and reader header.

            Name = binaryReader.ReadString();
            LocalizedName = binaryReader.ReadString();
            LocalizedDescription = binaryReader.ReadString();

            Sounds = new Sound[binaryReader.ReadInt32()];
            for (var i = 0; i < Sounds.Length; i++)
            {
                Sounds[i].Cue = binaryReader.ReadString();
                Sounds[i].Bank = (Banks)binaryReader.ReadInt32();
            }

            CanBePickedUp = binaryReader.ReadBoolean();
            Bound = binaryReader.ReadBoolean();
            BlockStrength = binaryReader.ReadInt32();
            WeaponClass = (WeaponClass)binaryReader.ReadByte();
            CooldownTime = binaryReader.ReadSingle();
            HideModel = binaryReader.ReadBoolean();
            HideEffects = binaryReader.ReadBoolean();
            PauseSounds = binaryReader.ReadBoolean();

            Resistances = new Resistance[binaryReader.ReadInt32()];
            for (var i = 0; i < Resistances.Length; i++)
            {
                Resistances[i].Element = (Elements)binaryReader.ReadInt32();
                Resistances[i].Multiplier = binaryReader.ReadSingle();
                Resistances[i].Modifier = binaryReader.ReadSingle();
                Resistances[i].StatusImmunity = binaryReader.ReadBoolean();
            }

            PassiveAbilityType = (PassiveAbility)binaryReader.ReadByte();
            PassiveAbilityStrength = binaryReader.ReadSingle();

            Effects = new string[binaryReader.ReadInt32()];
            for (var i = 0; i < Effects.Length; i++)
            {
                Effects[i] = binaryReader.ReadString();
            }

            Lights = new Light[binaryReader.ReadInt32()];
            for (var i = 0; i < Lights.Length; i++)
            {
                Lights[i].Radius = binaryReader.ReadSingle();
                Lights[i].DiffuseColor = new Color(binaryReader);
                Lights[i].AmbientColor = new Color(binaryReader);
                Lights[i].SpecularAmount = binaryReader.ReadSingle();
                Lights[i].LightVariationType = (LightVariationType)binaryReader.ReadByte();
                Lights[i].VariationAmount = binaryReader.ReadSingle();
                Lights[i].VariationSpeed = binaryReader.ReadSingle();
            }

            HasSpecialAbility = binaryReader.ReadBoolean();
            if (HasSpecialAbility)
            {
                SpecialAbilityCooldown = binaryReader.ReadSingle();
                SpecialAbility specialAbility = new()
                {
                    Type = binaryReader.ReadString(),
                    Animation = binaryReader.ReadString(),
                    Hash = binaryReader.ReadString(),
                    Elements = new Elements[binaryReader.ReadInt32()]
                };
                for (var i = 0; i < specialAbility.Elements.Length; i++)
                {
                    specialAbility.Elements[i] = (Elements)binaryReader.ReadInt32();
                }
                SpecialAbility = specialAbility;
            }

            MeleeRange = binaryReader.ReadSingle();
            MeleeMultihit = binaryReader.ReadBoolean();
            MeleeConditions = new ConditionCollection[binaryReader.ReadInt32()];
            for (var i = 0; i < MeleeConditions.Length; i++)
            {
                MeleeConditions[i] = new ConditionCollection(binaryReader);
            }

            RangedRange = binaryReader.ReadSingle();
            Facing = binaryReader.ReadBoolean();
            HomingStrength = binaryReader.ReadSingle();
            RangedElevation = binaryReader.ReadSingle();
            RangedDanger = binaryReader.ReadSingle();
            GunRange = binaryReader.ReadSingle();
            GunClip = binaryReader.ReadInt32();
            GunRate = binaryReader.ReadInt32();
            GunAccuracy = binaryReader.ReadSingle();
            GunSoundCue = binaryReader.ReadString();
            GunMuzzleEffect = binaryReader.ReadString();
            GunShellEffect = binaryReader.ReadString();
            GunTracerVelocity = binaryReader.ReadSingle();
            GunNonTracer = binaryReader.ReadString();
            GunTracer = binaryReader.ReadString();
            GunConditions = new ConditionCollection[binaryReader.ReadInt32()];
            for (var i = 0; i < GunConditions.Length; i++)
            {
                GunConditions[i] = new ConditionCollection(binaryReader);
            }

            ProjectileModel = binaryReader.ReadString();
            RangedConditions = new ConditionCollection[binaryReader.ReadInt32()];
            for (var i = 0; i < RangedConditions.Length; i++)
            {
                RangedConditions[i] = new ConditionCollection(binaryReader);
            }

            Scale = binaryReader.ReadSingle();
            Model = binaryReader.ReadString();
            Auras = new Aura[binaryReader.ReadInt32()];
            for (var i = 0; i < Auras.Length; i++)
            {
                Auras[i] = Aura.GetAura(binaryReader);
            }

            binaryReader.Close();
        }
    }

}
