﻿using System.Numerics;

namespace MagickaForge.Components.Graphics.Effects
{
    public class RenderDeferredLiquidEffect : ShaderEffect
    {
        private string reflectionMap;
        private float waveHeight;
        private Vector2 waveSpeedA;
        private Vector2 waveSpeedB;
        private float waterReflectiveness;
        private Vector3 bottomColor;
        private Vector3 deepColor;
        private float waterEmissive;
        private float waterSpecAmount;
        private float waterSpecPower;
        private string bottomTexture;
        private string waterNormalMap;
        private float iceReflectiveness;
        private Vector3 iceColor;
        private float iceEmissiveAmount;
        private float iceSpecAmount;
        private float iceSpecPower;
        private string iceDiffuseMap;
        private string iceNormalMap;
        
        public RenderDeferredLiquidEffect(BinaryReader binaryReader)
        {
            reflectionMap = binaryReader.ReadString();
            waveHeight = binaryReader.ReadSingle();
            waveSpeedA = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            waveSpeedB = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            waterReflectiveness = binaryReader.ReadSingle();
            bottomColor = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            deepColor = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            waterEmissive = binaryReader.ReadSingle();
            waterSpecAmount = binaryReader.ReadSingle();
            waterSpecPower = binaryReader.ReadSingle();
            bottomTexture = binaryReader.ReadString();
            waterNormalMap = binaryReader.ReadString();
            iceReflectiveness = binaryReader.ReadSingle();
            iceColor = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            iceEmissiveAmount = binaryReader.ReadSingle();
            iceSpecAmount = binaryReader.ReadSingle();
            iceSpecPower = binaryReader.ReadSingle();
            iceDiffuseMap = binaryReader.ReadString();
            iceNormalMap = binaryReader.ReadString();
        }
    }
}
