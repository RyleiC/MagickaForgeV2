#if GLTFEnabled

using System.Text.Json.Serialization;

namespace MagickaForge.Experimental.GLTF
{
    public class BufferViewNode
    {
        [JsonPropertyName("buffer")]
        public int Buffer { get; set; }
        [JsonPropertyName("byteLength")]
        public int ByteLength { get; set; }
        [JsonPropertyName("byteOffset")]
        public int ByteOffset { get; set; }
        [JsonPropertyName("target")]
        public int Target { get; set; }
    }
}

#endif