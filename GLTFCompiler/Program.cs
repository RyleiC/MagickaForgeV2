using System.Diagnostics;
using GLTFCompiler.Compilers;
using MagickaForge.GLTF;
using MagickaForge.Pipeline.Levels;
namespace LevelForge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("= Magicka glTF Model Compiler by Rylei. C =");
            Console.WriteLine(@"Input the glTF file path:");
            string instructionPath = Console.ReadLine()!.Trim('\"');

            Console.WriteLine("= Process Starting... =\n");

            var stopWatch = Stopwatch.StartNew();
            var compiler = new LevelCompiler();
            compiler.Compile(instructionPath);
            stopWatch.Stop();

            Console.WriteLine($"= Process completed in {stopWatch.ElapsedMilliseconds} ms =");

            Console.ReadKey();
        }

        private static void Compile(string instructionPath)
        {
            
            GLTFModel model;
            model = GLTFModel.LoadGLTFModel(instructionPath);
        }
 
    }
}
