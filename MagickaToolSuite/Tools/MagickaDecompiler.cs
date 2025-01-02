using MagickaForge.Pipeline.Json;
using MagickaToolSuite.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagickaToolSuite.Tools
{
    internal class MagickaDecompiler
    {
        private JsonSerializerOptions _options;

        public MagickaDecompiler()
        {
            _options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                WriteIndented = true,
            };
        }

        public void DirectoryDecompile(string instructionPath, ForgeType forgeType, bool modern)
        {
            foreach (string filePath in Directory.GetFiles(instructionPath, "*.xnb"))
            {
                Decompile(forgeType, filePath, modern);
            }
        }

        public void Decompile(ForgeType forgeType, string inputPath, bool modern)
        {
            PipelineJsonObject pipelineObject = PipelineJsonObject.GetPipelineInstance(forgeType, modern);
            pipelineObject.Import(inputPath);
            PipelineJsonObject.Save(inputPath.Replace(FileExtensions.XNBExtension, FileExtensions.JsonExtension), pipelineObject, _options);
            Console.WriteLine($"Succesfully decompiled {inputPath}");
        }
    }
}
