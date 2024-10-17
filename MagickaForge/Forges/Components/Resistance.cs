using MagickaForge.Utils;
using System.Text.Json.Serialization;

namespace MagickaForge.Forges.Components
{
    public struct Resistance
    {
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Multiplier { get; set; }
        public float Modifier { get; set; }
        public bool StatusImmunity { get; set; }
    }
}
