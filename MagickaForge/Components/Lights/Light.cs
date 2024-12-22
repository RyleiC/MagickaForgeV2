using MagickaForge.Utils.Data.Graphics;
using MagickaForge.Utils.Structures;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Lights
{
    public struct Light
    {
        public float Radius { get; set; }
        public Color DiffuseColor { get; set; }
        public Color AmbientColor { get; set; }
        public float SpecularAmount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<LightVariationType>))]
        public LightVariationType LightVariationType { get; set; }
        public float VariationAmount { get; set; }
        public float VariationSpeed { get; set; }
    }

}
