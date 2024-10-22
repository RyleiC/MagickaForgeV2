using MagickaForge.Forges.Characters;
using System.Diagnostics;
namespace CharacterForge
{
    internal class Program
    {
        static bool LegacyMagicka;
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
                instructionPath = args[1];
            }
            Console.WriteLine("Would you like to compile to XNB or decompile to Json?\n\"0\" : Compile\n\"1\" : Decompile");
            int mode = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Do you want to compile to an older version of Magicka? [Eg. 1.5.1.0]\n\"0\" : No\n\"1\" : Yes");
            LegacyMagicka = int.Parse(Console.ReadLine()!) == 1;
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
                character.CharacterToXNB(InstructionPath.Replace(".json", ".xnb"), LegacyMagicka);
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
                character = character.XNBToCharacter(InstructionPath, LegacyMagicka);
                Character.WriteToJson(InstructionPath.Replace(".xnb", ".json"), character);
                Console.WriteLine($"Succesfully decompiled {InstructionPath}");
            }
        }
    }
}
