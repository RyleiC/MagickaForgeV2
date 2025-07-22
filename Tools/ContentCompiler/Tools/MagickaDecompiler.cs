using ContentCompiler.Data;
using ContentCompiler.Misc;
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

        public void Decompile(string inputPath, bool useModernCompilation)
        {
            var isDirectory = CompilingHelper.IsDirectory(inputPath);

            if (isDirectory)
            {
                foreach (string filePath in Directory.GetFiles(inputPath, "*.xnb", _enumerationOptions))
                {
                    BeginDecompile(filePath, useModernCompilation);
                }
            }
            else
            {
                BeginDecompile(inputPath, useModernCompilation);
            }

            Logger.WriteResult($"\n\nDecompilation complete!");
        }

        private void BeginDecompile(string inputPath, bool modern)
        {
            var content = PipelineJsonObject.AutoImportXNB(inputPath);
            if (content == null)
            {
                Logger.WriteWarning($"{inputPath}'s XNB format could not be determined, skipping.");
                return;
            }

            var outputPath = Path.ChangeExtension(inputPath, FileExtensions.JsonExtension);
            PipelineJsonObject.Save(outputPath, content, _serializerOptions);

            Logger.WriteSuccess($"Succesfully decompiled {inputPath}");
        }
    }
}
