namespace MagickaForge.GLTF
{
    public class BufferView
    {
        /*
         * These have to be named this to be deserialized
         */
        public int buffer { get; set; }
        public int byteLength { get; set; }
        public int byteOffset { get; set; }
        public int target { get; set; }
    }
}
