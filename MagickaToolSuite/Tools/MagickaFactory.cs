using MagickaForge.Pipeline.Json;
using MagickaForge.Pipeline.Json.Characters;
using MagickaToolSuite.Data;
using System.Diagnostics;

namespace MagickaToolSuite.Tools
{
    internal class MagickaFactory
    {
        private bool _modern;
        private string? _path;
        private bool _pathIsDirectory;
        private CompilationMode _toolMode;
        private ForgeType _forgeType;

        public void BeginProcess()
        {
            Console.WriteLine("= Magicka Forge by Rylei. C =");
            Console.WriteLine(@"Input the path to a JSON instruction or XNB file\directory:");

            _path = Console.ReadLine()!.Trim('\"');

            FileAttributes fileAttributes = File.GetAttributes(_path);
            _pathIsDirectory = fileAttributes.HasFlag(FileAttributes.Directory);

            if (!_pathIsDirectory)
            {
                string fileExtension = Path.GetExtension(_path);

                if (fileExtension == FileExtensions.JsonExtension)
                {
                    _toolMode = CompilationMode.Compile;
                }
                else if (fileExtension == FileExtensions.XNBExtension)
                {
                    _toolMode = CompilationMode.Decompile;
                }
                else
                {
                    throw new ArgumentException("Your file is neither a XNB nor a JSON!");
                }
            }
            else
            {
                PromptToolMode();
            }

            if (_toolMode == CompilationMode.Decompile)
            {
                PromptForgeType();

                if (_forgeType == ForgeType.Character)
                {
                    PromptIsModern();
                }
            }

            else if (_pathIsDirectory)
            {
                PromptIsModern();
            }

            else if (_toolMode == CompilationMode.Compile)
            {
                var pipelineObject = PipelineJsonObject.Load(_path);
                if (pipelineObject is Character)
                {
                    PromptIsModern();
                }
            }

            GenerateContent(_toolMode, _path, _pathIsDirectory);
        }

        private void ResetFactory()
        {
            Console.Clear();
            BeginProcess();
        }

        private void GenerateContent(CompilationMode processMode, string path, bool isPathDirectory)
        {
            Console.Clear();
            Console.WriteLine("= Process Starting... =\n");

            var stopWatch = Stopwatch.StartNew();

            if (processMode == CompilationMode.Decompile)
            {
                CreateDecompiler();
            }
            else
            {
                CreateCompiler();
            }

            stopWatch.Stop();

            Console.WriteLine($"\n= Process completed in {stopWatch.ElapsedMilliseconds} ms =");

            PromptEnd();
        }

        private void CreateDecompiler()
        {
            var decompiler = new MagickaDecompiler();
            if (!_pathIsDirectory)
            {
                decompiler.Decompile(_forgeType, _path!, _modern);
            }
            else
            {
                decompiler.DirectoryDecompile(_path!, _forgeType, _modern);
            }
        }

        private void CreateCompiler()
        {
            var compiler = new MagickaCompiler();
            if (!_pathIsDirectory)
            {
                var pipelineObject = PipelineJsonObject.Load(_path!);
                if (pipelineObject is Character)
                {
                    (pipelineObject as Character)!.CompileForModernMagicka = _modern;
                }
                compiler.Compile(pipelineObject, _path!.Replace(FileExtensions.JsonExtension, FileExtensions.XNBExtension));
            }
            else
            {
                compiler.DirectoryCompile(_path!, _modern);
            }
        }

        private void ReadLastKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            var key = keyInfo.Key;

            switch (key)
            {
                case (ConsoleKey.R):
                    GenerateContent(_toolMode, _path!, _pathIsDirectory);
                    break;

                case (ConsoleKey.N):
                    ResetFactory();
                    break;

                case (ConsoleKey.Escape):
                    return;

                default:
                    ReadLastKey();
                    break;
            }
        }
        private void PromptEnd()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nPlease input a key.\n- 'R' to repeat the last process\n- 'N' to begin a new compilation/decompilation\n- 'ESC' to exit program");
            Console.ForegroundColor = ConsoleColor.White;
            ReadLastKey();
        }

        private void PromptIsModern()
        {
            Console.Clear();
            Console.WriteLine("Are you creating/reading content from an older version of Magicka? [Eg. 1.5.1.0]\n\"0\" : No\n\"1\" : Yes");
            _modern = int.Parse(Console.ReadLine()!) == 0; //Client has said yes
            Console.Clear();
        }

        private void PromptForgeType()
        {
            Console.Clear();
            Console.WriteLine("What type of content are you attempting to decompile? \n\"0\" : Character\n\"1\" : Item\n\"2\" : Level\n\"3\" : Model");
            _forgeType = (ForgeType)int.Parse(Console.ReadLine()!);
            Console.Clear();
        }

        private void PromptToolMode()
        {
            Console.Clear();
            Console.WriteLine("Would you like to compile to XNB or decompile to Json?\n\"0\" : Compile\n\"1\" : Decompile");
            _toolMode = (CompilationMode)int.Parse(Console.ReadLine()!);
            Console.Clear();
        }
    }
}
