using MagickaForge.Utils.Data.AI;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Characters
{
    public struct Movement
    {
        [JsonConverter(typeof(JsonStringEnumConverter<MovementProperties>))]
        public MovementProperties MoveProperty { get; set; }
        public string[] Animations { get; set; }
    }
}