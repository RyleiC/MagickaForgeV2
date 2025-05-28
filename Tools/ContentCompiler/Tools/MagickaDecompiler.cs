using ContentCompiler.Data;
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

        public void Decompile(ForgeType type, string inputPath, bool useModernCompilation)
        {
            var isDirectory = CompilingHelper.IsDirectory(inputPath);

            if (isDirectory)
            {
                foreach (string filePath in Directory.GetFiles(inputPath, "*.json", _enumerationOptions))
                {
                    BeginDecompile(type, filePath, useModernCompilation);
                }
            }
            else
            {
                BeginDecompile(type, inputPath, useModernCompilation);
            }

            Logger.WriteResult($"\n\nDecompilation complete!");
        }

        public void BeginDecompile(ForgeType forgeType, string inputPath, bool modern)
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
