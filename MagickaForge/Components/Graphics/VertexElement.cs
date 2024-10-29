namespace MagickaForge.Components.Graphics
{
    public struct VertexElement
    {
        public short Stream { get; set; }
        public short Offset { get; set; }

        public byte Format { get; set; }
        public byte Method { get; set; }
        public byte Usage { get; set; }
        public byte UsageIndex { get; set; }
    }
}
