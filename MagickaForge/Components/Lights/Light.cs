using MagickaForge.Utils;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Lights
{
    public struct Light
    {
        public float Radius { get; set; }
        public float[] DiffuseColor { get; set; }
        public float[] AmbientColor { get; set; }
        public float SpecularAmount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<LightVariationType>))]
        public LightVariationType LightVariationType { get; set; }
        public float VariationAmount { get; set; }
        public float VariationSpeed { get; set; }
    }

}
