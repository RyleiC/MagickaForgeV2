using MagickaForge.Pipeline.Characters;
using MagickaForge.Pipeline.Items;
using MagickaForge.Pipeline.Levels;
using MagickaForge.Pipeline.Models;
using MagickaForge.Pipeline.Textures;
using MagickaForge.Utils.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagickaForge.Pipeline
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$ForgeType")]
    [JsonDerivedType(typeof(Item), typeDiscriminator: "Item")]
    [JsonDerivedType(typeof(Level), typeDiscriminator: "Level")]
    [JsonDerivedType(typeof(Character), typeDiscriminator: "Character")]
    [JsonDerivedType(typeof(NonEmbeddedModel), typeDiscriminator: "Model")]
    [JsonDerivedType(typeof(Texture), typeDiscriminator: "Texture")]
    public class PipelineObject
    {
        private bool _modernMagicka;

        public virtual void WriteToXNB(string outputPath)
        {

        }

        public virtual void ReadFromXNB(string inputPath)
        {

        }

        public static void WriteToJson(string outputPath, PipelineObject pipelineObject)
        {
            StreamWriter sw = new(outputPath);
            sw.Write(JsonSerializer.Serialize(pipelineObject, JsonSettings.SerializerSettings));
            sw.Close();
        }

        public static PipelineObject LoadFromJson(string inputPath)
        {
            string json = File.ReadAllText(inputPath);
            return JsonSerializer.Deserialize<PipelineObject>(json)!;
        }

        [JsonIgnore]
        public bool CompileForModernMagicka
        {
            get
            {
                return _modernMagicka;
            }
            set
            {
                _modernMagicka = value;
            }
        }
    }
}
