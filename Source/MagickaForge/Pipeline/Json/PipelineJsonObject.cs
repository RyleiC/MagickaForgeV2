using MagickaForge.Pipeline.Json.Characters;
using MagickaForge.Pipeline.Json.Items;
using MagickaForge.Pipeline.Json.Levels;
using MagickaForge.Pipeline.Json.Models;
using MagickaForge.Pipeline.Json.PhysicsEntities;
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
    [JsonDerivedType(typeof(PhysicsEntity), typeDiscriminator: "PhysicsEntity")]

    public abstract class PipelineJsonObject
    {
        private const int ReaderPeekOffset = 25;

        /*
         * Context on these:
            To quickly determine the file type, I've observed that all file types from magicka (specifically) have a unique character at a specific position.
            Thus, as a quick way to sniff out what type of file it is, it'll import the file, peek the byte and then use that to determine what type of XNB it is.
            Probably would suck if there was a lot of different types but magicka hasn't been updated in a long while.
         */

        private const byte ItemReaderStart = 0x49;
        private const byte CharacterReaderStart = 0x43;
        private const byte LevelReaderStart = 0x4C;
        private const byte SkinnedModelStart = 0x69;
        private const byte PhysicsObjectStart = 0x50;

        private const byte XNAContentStart = 0x2E;
        private const byte ModelStart = 0x4D;

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

        public static PipelineJsonObject AutoImportXNB(string inputPath)
        {
            using (var binaryReader = new BinaryReader(XNBHelper.DecompressXNB(inputPath)))
            {
                var oldPosition = binaryReader.BaseStream.Position;

                binaryReader.BaseStream.Position += ReaderPeekOffset;
                var readerStart = binaryReader.ReadByte();

                if (readerStart == XNAContentStart) //This is shared with textures so we peek another byte to make sure it's a model.
                {
                    var nextByte = binaryReader.ReadByte();

                    if (nextByte != ModelStart)
                    {
                        return null; //It's some other XNA format
                    }
                }

                binaryReader.BaseStream.Position = oldPosition;

                switch (readerStart)
                {
                    case CharacterReaderStart:
                        return GetImportedContent(new Character(), binaryReader);
                    case ItemReaderStart:
                        return GetImportedContent(new Item(), binaryReader);
                    case LevelReaderStart:
                        return GetImportedContent(new Level(), binaryReader);
                    case XNAContentStart:
                        return GetImportedContent(new NonEmbeddedModel(), binaryReader);
                    case SkinnedModelStart:
                        return GetImportedContent(new NonEmbeddedSkinnedModel(), binaryReader);
                    case PhysicsObjectStart:
                        return GetImportedContent(new PhysicsEntity(), binaryReader);
                    default:
                        return null;
                }
            }
        }

        private static PipelineJsonObject GetImportedContent(PipelineJsonObject content, BinaryReader binaryReader)
        {
            content.MidImport(binaryReader);
            return content;
        }

        //In case you wanna do something manually?
        public void Import(string inputPath)
        {
            using var binaryReader = new BinaryReader(XNBHelper.DecompressXNB(inputPath));
            MidImport(binaryReader);
        }

        public static void Save(string outputPath, PipelineJsonObject pipelineObject, JsonSerializerOptions options)
        {
            using StreamWriter streamWriter = new(outputPath);
            streamWriter.Write(JsonSerializer.Serialize(pipelineObject, options));
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
