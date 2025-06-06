﻿using MagickaForge.Pipeline.Json.Characters;
using MagickaForge.Pipeline.Json.Items;
using MagickaForge.Pipeline.Json.Levels;
using MagickaForge.Pipeline.Json.Models;
using MagickaForge.Utils.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagickaForge.Pipeline.Json
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$ForgeType")]
    [JsonDerivedType(typeof(Item), typeDiscriminator: "Item")]
    [JsonDerivedType(typeof(Level), typeDiscriminator: "Level")]
    [JsonDerivedType(typeof(Character), typeDiscriminator: "Character")]
    [JsonDerivedType(typeof(NonEmbeddedModel), typeDiscriminator: "Model")]
    [JsonDerivedType(typeof(NonEmbeddedSkinnedModel), typeDiscriminator: "SkinnedModel")]

    public abstract class PipelineJsonObject
    {
        protected abstract void MidExport(BinaryWriter binaryWriter);
        protected abstract void MidImport(BinaryReader binaryReader);

        public void Export(string outputPath)
        {
            using (var binaryWriter = new BinaryWriter(File.Create(outputPath)))
            {
                binaryWriter.Write(XNBHelper.XNBHeader);
                binaryWriter.Write(-1);
                MidExport(binaryWriter);
                XNBHelper.WriteFileSize(binaryWriter);
            }
        }

        public void Import(string inputPath)
        {
            using (var binaryReader = new BinaryReader(XNBHelper.DecompressXNB(inputPath)))
            {
                MidImport(binaryReader);
            }
        }

        public static void Save(string outputPath, PipelineJsonObject pipelineObject, JsonSerializerOptions options)
        {
            using (StreamWriter sw = new(outputPath))
            {
                sw.Write(JsonSerializer.Serialize(pipelineObject, options));
            }
        }

        public static PipelineJsonObject Load(string inputPath)
        {
            string json = File.ReadAllText(inputPath);
            return JsonSerializer.Deserialize<PipelineJsonObject>(json)!;
        }

        public static PipelineJsonObject ForgeTypeToInstance(ForgeType forgeType, bool modern)
        {
            return forgeType switch
            {
                (ForgeType.Character) => new Character() { CompileForModernMagicka = modern },
                (ForgeType.Item) => new Item(),
                (ForgeType.Level) => new Level(),
                (ForgeType.Model) => new NonEmbeddedModel(),
                (ForgeType.SkinnedModel) => new NonEmbeddedSkinnedModel(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
