using MagickaForge.Utils.Data.Graphics;
using System.Text.Json.Serialization;

namespace MagickaForge.Components.Graphics
{
    public struct VertexElement
    {
        public short Stream { get; set; }
        public short Offset { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<VertexElementFormat>))]
        public VertexElementFormat Format { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<VertexElementMethod>))]
        public VertexElementMethod Method { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<VertexElementUsage>))]
        public VertexElementUsage Usage { get; set; }
        public byte UsageIndex { get; set; }
    }
}
