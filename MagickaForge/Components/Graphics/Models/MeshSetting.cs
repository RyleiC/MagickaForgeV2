namespace MagickaForge.Components.Graphics.Models
{
    public struct MeshSetting
    {
        public string Key { get; set; }
        public bool Value { get; set; }
        public bool Highlight { get; set; }

        public MeshSetting(BinaryReader binaryReader)
        {
            Key = binaryReader.ReadString();
            Value = binaryReader.ReadBoolean();
            Highlight = binaryReader.ReadBoolean();
        }
    }
}
