using MagickaForge.Utils;
using System.Text.Json.Serialization;

namespace MagickaForge.Forges.Components
{
    public struct Damage
    {
        [JsonConverter(typeof(JsonStringEnumConverter<AttackProperties>))]
        public AttackProperties AttackProperty { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Elements>))]
        public Elements Element { get; set; }
        public float Amount { get; set; }
        public float Magnitude { get; set; }
    }
}