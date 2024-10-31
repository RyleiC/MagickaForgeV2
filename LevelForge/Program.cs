using MagickaForge.Pipeline.Levels;
using System.Diagnostics;

namespace LevelForge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("= Magicka *Experimental* Level Forge by Rylei. C =");
            Console.WriteLine(@"Input the path to a JSON instruction or XNB file\directory:");
            string instructionPath = Console.ReadLine()!.Trim('\"');
            Console.WriteLine("Would you like to compile to XNB or decompile to Json?\n\"0\" : Compile\n\"1\" : Decompile");
            var mode = int.Parse(Console.ReadLine()!);

            Console.WriteLine("= Process Starting... =\n");

            var stopWatch = Stopwatch.StartNew();

            if (mode == 0)
            {
                GenerateXNB(instructionPath);
            }
            else
            {
                GenerateJson(instructionPath);
            }

            stopWatch.Stop();
            Console.WriteLine($"= Process completed in {stopWatch.ElapsedMilliseconds} ms =");

            Console.ReadKey();
        }

        private static void GenerateXNB(string InstructionPath)
        {
            FileAttributes fileAttributes = File.GetAttributes(InstructionPath);
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                foreach (string file in Directory.GetFiles(InstructionPath, "*.json"))
                {
                    GenerateXNB(file);
                }
                return;
            }
            else
            {
                if (!File.Exists(InstructionPath))
                {
                    throw new FileNotFoundException(InstructionPath);
                }
                Level level = Level.LoadFromJson(InstructionPath);
                level.LevelToXNB(InstructionPath.Replace(".json", ".xnb"));
                Console.WriteLine($"Succesfully compiled {InstructionPath}");
            }
        }
        private static void GenerateJson(string InstructionPath)
        {
            FileAttributes fileAttributes = File.GetAttributes(InstructionPath);
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                foreach (string file in Directory.GetFiles(InstructionPath, "*.xnb"))
                {
                    GenerateJson(file);
                }
                return;
            }
            else
            {
                if (!File.Exists(InstructionPath))
                {
                    throw new FileNotFoundException(InstructionPath);
                }
                Level level = new();
                level.XNBToLevel(InstructionPath);
                Level.WriteToJson(InstructionPath.Replace(".xnb", ".json"), level);
                Console.WriteLine($"Succesfully decompiled {InstructionPath}");
            }
        }
    }
}
