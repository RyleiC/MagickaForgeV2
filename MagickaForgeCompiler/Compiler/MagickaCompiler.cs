using MagickaForge.Pipeline;
using MagickaForge.Pipeline.Characters;
using MagickaForge.Pipeline.Items;
using MagickaForge.Pipeline.Levels;
using MagickaForge.Pipeline.Models;
using MagickaForgeCompiler.Data;
using System.Diagnostics;

namespace MagickaForgeCompiler.Compiler
{
    internal class MagickaCompiler
    {
        private bool _modern;
        private ForgeTypes _forgeType;

        public MagickaCompiler()
        {
        }

        public void StartPrompts()
        {
            Console.WriteLine("= Magicka Forge Compiler by Rylei. C =");
            Console.WriteLine(@"Input the path to a JSON instruction or XNB file\directory:");

            string instructionPath = Console.ReadLine()!.Trim('\"');
            Console.Clear();

            Console.WriteLine("Would you like to compile to XNB or decompile to Json?\n\"0\" : Compile\n\"1\" : Decompile");
            var mode = (CompilationMode)int.Parse(Console.ReadLine()!);
            Console.Clear();

            if (mode == CompilationMode.Decompile)
            {
                Console.WriteLine("What type of content are you attempting to decompile? \n\"0\" : Character\n\"1\" : Item\n\"2\" : Level\n\"3\" : Model");
                _forgeType = (ForgeTypes)int.Parse(Console.ReadLine()!);
                Console.Clear();

                if (_forgeType == ForgeTypes.Character)
                {
                    PromptForModern();
                }
            }
            else
            {
                PromptForModern();
            }


            Console.WriteLine("= Process Starting... =\n");

            var stopWatch = Stopwatch.StartNew();

            if (mode == CompilationMode.Decompile)
            {
                ReadXNB(instructionPath);
            }
            else
            {
                CompileXNB(instructionPath);
            }

            stopWatch.Stop();
            Console.WriteLine($"= Process completed in {stopWatch.ElapsedMilliseconds} ms =");

            Console.ReadKey();
        }

        private void PromptForModern()
        {
            Console.WriteLine("Are you creating/reading content from an older version of Magicka? [Eg. 1.5.1.0]\n\"0\" : No\n\"1\" : Yes");
            _modern = int.Parse(Console.ReadLine()!) == 0;
            Console.Clear();
        }

        private void CompileXNB(string instructionPath)
        {
            FileAttributes fileAttributes = File.GetAttributes(instructionPath);
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                foreach (string file in Directory.GetFiles(instructionPath, "*.json"))
                {
                    CompileXNB(file);
                }
                return;
            }
            else
            {
                var item = PipelineObject.LoadFromJson(instructionPath);
                item.CompileForModernMagicka = _modern;
                item.WriteToXNB(instructionPath.Replace(".json", ".xnb"));
                Console.WriteLine($"Succesfully compiled {instructionPath}");
            }
        }

        private void ReadXNB(string instructionPath)
        {
            FileAttributes fileAttributes = File.GetAttributes(instructionPath);
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                foreach (string file in Directory.GetFiles(instructionPath, "*.xnb"))
                {
                    ReadXNB(file);
                }
                return;
            }
            else
            {
                PipelineObject pipelineObject = GetType(_forgeType);
                pipelineObject.ReadFromXNB(instructionPath);
                PipelineObject.WriteToJson(instructionPath.Replace(".xnb", ".json"), pipelineObject);
                Console.WriteLine($"Succesfully decompiled {instructionPath}");
            }
        }

        private PipelineObject GetType(ForgeTypes forgeType)
        {
            return forgeType switch
            {
                (ForgeTypes.Character) => new Character() { CompileForModernMagicka = _modern },
                (ForgeTypes.Item) => new Item(),
                (ForgeTypes.Level) => new Level(),
                (ForgeTypes.Model) => new NonEmbeddedModel(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
