using MagickaForge.Pipeline.Json;
using MagickaForge.Pipeline.Json.Characters;

namespace MagickaToolSuite.Tools
{
    internal class MagickaCompiler
    {

        public void DirectoryCompile(string instructionPath, bool modern)
        {
            var searchOptions = new EnumerationOptions()
            {
                RecurseSubdirectories = true,
                AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
                IgnoreInaccessible = true,
                MaxRecursionDepth = 16
            };

            foreach (string filePath in Directory.GetFiles(instructionPath, "*.json", searchOptions))
            {
                var pipelineItem = PipelineJsonObject.Load(filePath);
                Compile(pipelineItem, filePath, modern);
            }
        }

        public void Compile(PipelineJsonObject pipelineObject, string inputPath, bool modern)
        {
            if (pipelineObject is Character)
            {
                (pipelineObject as Character)!.CompileForModernMagicka = modern;
            }

            Compile(pipelineObject, inputPath);
        }

        public void Compile(PipelineJsonObject pipelineObject, string inputPath)
        {
            var outputPath = Path.ChangeExtension(inputPath, FileExtensions.XNBExtension);

            pipelineObject.Export(outputPath);
            PrintSuccessMessage(inputPath);
        }

        private void PrintSuccessMessage(string inputPath)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Succesfully compiled {inputPath}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
