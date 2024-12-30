using MagickaForge.Pipeline.Json;
using MagickaForge.Pipeline.Json.Characters;

namespace MagickaToolSuite.Tools
{
    internal class MagickaCompiler
    {
        public void DirectoryCompile(string instructionPath, bool modern)
        {
            foreach (string filePath in Directory.GetFiles(instructionPath, "*.json"))
            {
                var pipelineItem = PipelineJsonObject.Load(filePath);
                Compile(pipelineItem, filePath.Replace(FileExtensions.JsonExtension, FileExtensions.XNBExtension), modern);
            }
        }

        public void Compile(PipelineJsonObject pipelineObject, string outputPath, bool modern)
        {
            if (pipelineObject is Character)
            {
                (pipelineObject as Character)!.CompileForModernMagicka = modern;
            }

            Compile(pipelineObject, outputPath);
        }

        public void Compile(PipelineJsonObject pipelineObject, string outputPath)
        {
            pipelineObject.Export(outputPath);
            Console.WriteLine($"Succesfully compiled {outputPath}");
        }
    }
}
