using ContentCompiler.Misc;
using MagickaForge.Pipeline;
using MagickaForge.Pipeline.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContentCompiler.Tools
{
    internal class MagickaDecompiler
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            WriteIndented = true,
        };

        private static readonly EnumerationOptions _enumerationOptions = new()
        {
            MatchCasing = MatchCasing.CaseInsensitive,
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
            IgnoreInaccessible = true,
            MaxRecursionDepth = 16
        };

        public void DirectoryDecompile(string instructionPath, ForgeType forgeType, bool modern)
        {
            foreach (string filePath in Directory.GetFiles(instructionPath, "*.xnb", _enumerationOptions))
            {
                Decompile(forgeType, filePath, modern);
            }
        }

        public void Decompile(ForgeType forgeType, string inputPath, bool modern)
        {
            PipelineJsonObject pipelineObject = PipelineJsonObject.ForgeTypeToInstance(forgeType, modern);

            pipelineObject.Import(inputPath);
            var outputPath = Path.ChangeExtension(inputPath, FileExtensions.JsonExtension);

            PipelineJsonObject.Save(outputPath, pipelineObject, _serializerOptions);

            PrintSuccessMessage(inputPath);
        }

        private void PrintSuccessMessage(string inputPath)
        {
            Logger.WriteSuccess($"Succesfully decompiled {inputPath}");
        }
    }
}
