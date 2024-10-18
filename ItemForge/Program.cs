using MagickaForge.Forges.Characters;
using MagickaForge.Forges.Items;
using System.Diagnostics;
using System.Text.Json.Nodes;
namespace CharacterForge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("= Magicka Item Forge by Rylei. C =");
            string instructionPath;

            if (args.Length < 1)
            {
                Console.WriteLine(@"Input the path to a JSON instruction file\directory:");
                instructionPath = Console.ReadLine()!;
            }
            else
            {
                instructionPath = args[1];
            }
            Console.WriteLine("Would you like to compile to XNB or decompile to Json?\n\"0\" : Compile\n\"1\" : Decompile");
            int mode = int.Parse(Console.ReadLine()!);

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
                Item item = Item.LoadFromJson(InstructionPath);
                item.ItemToXNB(InstructionPath.Replace(".json", ".xnb"));
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
                Item item = new();
                item = item.XNBToItem(InstructionPath);
                Item.WriteToJson(InstructionPath.Replace(".xnb", ".json"), item);
                Console.WriteLine($"Succesfully decompiled {InstructionPath}");
            }
        }
    }
}
