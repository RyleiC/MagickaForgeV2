using System.Text.Json;

namespace MagickaForge.GLTF
{
    public class ModelNode
    {
        private string _path;
        public Buffer Buffer { get; private set; }
        public BufferViewNode[] bufferViews { get; set; } //Must be named this to be deserialized
        public ModelNode()
        {
            Buffer = new Buffer();
        }

        public static ModelNode LoadGLTFModel(string inputPath)
        {
            string json = File.ReadAllText(inputPath);

            var model = JsonSerializer.Deserialize<ModelNode>(json);
            model._path = inputPath;

            return model;
        }
        public void InitializeBuffer()
        {
            if (bufferViews is null)
            {
                throw new NullReferenceException("You have not deserialized any buffer views!");
            }
            var binaryReader = new BinaryReader(File.OpenRead(_path.Replace(".gltf", ".bin")));
            Buffer.Read(binaryReader, bufferViews);
            binaryReader.Close();
        }
    }
}
