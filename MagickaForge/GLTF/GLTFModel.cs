using System.Text.Json;

namespace MagickaForge.GLTF
{
    public class GLTFModel
    {
        private Buffer buffer;
        public BufferView[] bufferViews { get; set; } //Must be named this to be deserialized
        public GLTFModel()
        {

        }

        public static GLTFModel LoadGLTFModel(string inputPath)
        {
            string json = File.ReadAllText(inputPath);
            var model = JsonSerializer.Deserialize<GLTFModel>(json);
            return model;
        }

        public void WriteModelData(string path)
        {
            BinaryReader binaryReader = new BinaryReader(File.OpenRead(path.Replace(".gltf", ".bin")));
            buffer = new Buffer();
            buffer.Read(binaryReader, bufferViews);
            binaryReader.Close();
        }
    }
}
