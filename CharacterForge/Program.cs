using MagickaForge.Forges.Characters;
using MagickaForge.Forges.Items;
using System.Diagnostics;
using System.Text.Json.Nodes;
namespace CharacterForge
{
    internal class Program
    {
        static bool ModernMagicka;
        static void Main(string[] args)
        {
            Console.WriteLine("= Magicka Character Forge by Rylei. C =");
            string instructionPath;

            if (args.Length < 1)
            {
                Console.WriteLine(@"Input the path to a JSON instruction file\directory:");
                instructionPath = Console.ReadLine()!;
            }
            else
            {
                instructionPath = args[0];
            }
            Console.WriteLine("Would you like to compile to XNB or decompile to Json?\n\"0\" : Compile\n\"1\" : Decompile");
            int mode = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Do you want to compile to an older version of Magicka? [Eg. 1.5.1.0]\n\"y\" : Yes\n\"n\" : No");
            char usesModern = char.Parse(Console.ReadLine()!.ToLower());
            ModernMagicka = usesModern == 'y';
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
                Character character = Character.LoadFromJson(InstructionPath);
                character.CharacterToXNB(InstructionPath.Replace(".json", ".xnb"), ModernMagicka);
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
                Character character = new();
                character = character.XNBToCharacter(InstructionPath, ModernMagicka);
                Character.WriteToJson(InstructionPath.Replace(".xnb", ".json"), character);
                Console.WriteLine($"Succesfully decompiled {InstructionPath}");
            }
        }
    }
}
