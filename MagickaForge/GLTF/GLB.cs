namespace MagickaForge.GLTF
{
    public class GLB
    {
        private Buffer buffer;
        public BufferView[] bufferViews { get; set; }
        public GLB()
        {

        }

        public void WriteModelData(string path)
        {
            BinaryReader binaryReader = new BinaryReader(File.OpenRead(path.Replace(".gltf", ".bin")));
            buffer = new Buffer();
            buffer.Read(binaryReader, bufferViews);
            buffer.ToVertexBuffer(path.Replace(".gltf", ".vtx"));
            buffer.ToIndexBuffer(path.Replace(".gltf", ".idx"));
            binaryReader.Close();
        }
    }
}
