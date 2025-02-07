using MagickaForge.Utils.Data;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Common
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