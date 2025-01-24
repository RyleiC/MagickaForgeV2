using MagickaForge.Pipeline;
using MagickaForge.Pipeline.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagickaToolSuite.Tools
{
    internal class MagickaDecompiler
    {
        private readonly JsonSerializerOptions _options;

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
            var searchOptions = new EnumerationOptions()
            {
                RecurseSubdirectories = true,
                AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
                IgnoreInaccessible = true,
                MaxRecursionDepth = 16
            };

            foreach (string filePath in Directory.GetFiles(instructionPath, "*.xnb", searchOptions))
            {
                Decompile(forgeType, filePath, modern);
            }
        }

        public void Decompile(ForgeType forgeType, string inputPath, bool modern)
        {
            PipelineJsonObject pipelineObject = PipelineJsonObject.ForgeTypeToInstance(forgeType, modern);
            pipelineObject.Import(inputPath);

            var outputPath = Path.ChangeExtension(inputPath, FileExtensions.JsonExtension);

            PipelineJsonObject.Save(outputPath, pipelineObject, _options);

            PrintSuccessMessage(inputPath);
        }

        private void PrintSuccessMessage(string inputPath)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Succesfully decompiled {inputPath}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
