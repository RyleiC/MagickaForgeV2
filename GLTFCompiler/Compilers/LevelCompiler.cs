using MagickaForge.Components.Levels;
using MagickaForge.GLTF;
using System.Text.Json;

namespace GLTFCompiler.Compilers
{
    internal class LevelCompiler
    {
        private string jsonBase = @"{""_visible"":true,""_castShadows"":true,""_sway"":0,""_entityInfluence"":0,""_groundLevel"":0,""_vertexCount"":4018,""_vertexStride"":44,""_vertexDeclaration"":{""_vertexElements"":[{""Stream"":0,""Offset"":0,""Format"":""Vector3"",""Method"":""Default"",""Usage"":""Position"",""UsageIndex"":0},{""Stream"":0,""Offset"":12,""Format"":""Vector3"",""Method"":""Default"",""Usage"":""Normal"",""UsageIndex"":0},{""Stream"":0,""Offset"":24,""Format"":""Vector2"",""Method"":""Default"",""Usage"":""TextureCoordinate"",""UsageIndex"":0},{""Stream"":0,""Offset"":32,""Format"":""Vector3"",""Method"":""Default"",""Usage"":""Tangent"",""UsageIndex"":0},{""Stream"":0,""Offset"":24,""Format"":""Vector2"",""Method"":""Default"",""Usage"":""TextureCoordinate"",""UsageIndex"":1},{""Stream"":0,""Offset"":32,""Format"":""Vector3"",""Method"":""Default"",""Usage"":""Tangent"",""UsageIndex"":1}],""readerIndex"":3},""_vertexBuffer"":{""readerIndex"":4,""_data"":null},""_indexBuffer"":{""readerIndex"":5,""_is16Bit"":true,""_data"":null},""_effect"":{""_EffectType"":""DeferredRender"",""_alpha"":0.4,""_sharpness"":1,""_vertexColorEnabled"":false,""_useTextureAsReflectiveness"":false,""_reflectionMap"":"""",""_materialA"":{""DiffuseNoAlpha"":true,""AlphaMaskEnabled"":false,""DiffuseColor"":{""R"":1,""G"":1,""B"":1},""SpecularAmount"":0,""SpecularPower"":20,""EmissiveAmount"":0,""NormalPower"":1,""Reflectiveness"":0,""DiffuseTexture"":""..\\Textures\\Surface\\Structure\\Floor\\stonetile00_0"",""MaterialTexture"":"""",""NormalTexture"":""""},""_materialB"":null,""readerType"":6},""_primativeCount"":2036,""_startIndex"":0,""_minBounding"":{""X"":-15,""Y"":-15,""Z"":-15},""_maxBounding"":{""X"":15,""Y"":15,""Z"":15},""_childA"":null,""_childB"":null}";
        public LevelCompiler()
        {

        }
        public void Compile(string instructionPath)
        {
            BinTreeRoot binTreeRoot = JsonSerializer.Deserialize<BinTreeRoot>(jsonBase);
            ModelNode model;

            int vertexCounter, indexCounter;
            vertexCounter = binTreeRoot._vertexBuffer.readerIndex;
            indexCounter = binTreeRoot._indexBuffer.readerIndex;

            model = ModelNode.LoadGLTFModel(instructionPath);
            model.InitializeBuffer();

            binTreeRoot._indexBuffer = model.Buffer.ToIndexBuffer();
            binTreeRoot._indexBuffer.readerIndex = indexCounter;

            binTreeRoot._vertexBuffer = model.Buffer.ToVertexBuffer();
            binTreeRoot._vertexBuffer.readerIndex = vertexCounter;

            binTreeRoot._primativeCount = model.Buffer.IndiceCount / 3;
            binTreeRoot._vertexCount = model.Buffer.VertexCount;

            TriangleMesh triangleMesh = model.Buffer.ToTriangleMesh();
            triangleMesh.readerType = 10;

            var bw = new BinaryWriter(File.Create(instructionPath.Replace(".gltf", ".biRoot")));
            bw.Write(JsonSerializer.Serialize(binTreeRoot, new JsonSerializerOptions() { WriteIndented = true }));
            bw.Close();

            bw = new BinaryWriter(File.Create(instructionPath.Replace(".gltf", ".collMesh")));
            bw.Write(JsonSerializer.Serialize(triangleMesh, new JsonSerializerOptions() { WriteIndented = true }));
            bw.Close();
        }

    }
}
