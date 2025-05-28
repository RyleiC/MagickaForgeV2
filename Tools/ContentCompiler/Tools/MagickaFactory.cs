using ContentCompiler.Data;
using ContentCompiler.Misc;
using ContentCompiler.Settings;
using MagickaForge.Pipeline;
using MagickaForge.Pipeline.Json;
using MagickaForge.Pipeline.Json.Characters;
using System.Diagnostics;

namespace ContentCompiler.Tools
{
    public class MagickaFactory
    {
        private bool _modern;
        private bool _pathIsDirectory;
        private string _path;
        private ToolMode _toolMode;
        private ForgeType _forgeType;

        public MagickaFactory(string path)
        {
            if (path != null)
            {
                _path = path.Trim('\"');
            }
        }

        public MagickaFactory() : this(null)
        {

        }

        public void StartFactory()
        {
            Logger.WriteTitle("= Magicka Forge by Rylei. C =");

            if (_path == null)
            {
                PromptForPath();
            }

            var pathStatus = VerifyPath();
            while (pathStatus != PathStatus.Valid)
            {
                PrintPathErrors(pathStatus);
                PromptForPath();

                pathStatus = VerifyPath();
            }

            InferToolMode();

            if (_toolMode == ToolMode.Decompile)
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
            else if (_toolMode == ToolMode.Compile)
            {
                var pipelineObject = PipelineJsonObject.Load(_path);

                if (pipelineObject is Character)
                {
                    PromptIsModern();
                }
            }

            GenerateContent(_toolMode);
        }

        private PathStatus VerifyPath()
        {
            if (_path == null)
            {
                return PathStatus.PathIsNull;
            }
            else if (!PathExists)
            {
                return PathStatus.PathDoesNotExist;
            }

            _pathIsDirectory = CompilingHelper.IsDirectory(_path);

            if (!_pathIsDirectory)
            {
                var extension = Path.GetExtension(_path);

                if (extension != FileExtensions.JsonExtension && extension != FileExtensions.XNBExtension)
                {
                    return PathStatus.InvalidExtension;
                }
            }

            return PathStatus.Valid;
        }

        private void InferToolMode()
        {
            if (_pathIsDirectory)
            {
                PromptToolMode();
                return;
            }

            string fileExtension = Path.GetExtension(_path);
            if (fileExtension == FileExtensions.JsonExtension)
            {
                _toolMode = ToolMode.Compile;
            }
            else
            {
                _toolMode = ToolMode.Decompile;
            }
        }

        private void GenerateContent(ToolMode processMode)
        {
            Console.Clear();

            Logger.WriteInfo("= Process Starting... =\n");

            var stopWatch = Stopwatch.StartNew();

            if (processMode == ToolMode.Decompile)
            {
                CreateDecompiler();
            }
            else
            {
                CreateCompiler();
            }

            stopWatch.Stop();
            Logger.WriteInfo($"\n= Process completed in {stopWatch.ElapsedMilliseconds} ms =");

            PromptForHotkeys();
        }

        private void CreateDecompiler()
        {
            var decompiler = new MagickaDecompiler();
            decompiler.Decompile(_forgeType, _path, _modern);
        }

        private void CreateCompiler()
        {
            var compiler = new MagickaCompiler();
            compiler.Compile(_path!, _modern);
        }

        private void ReverseProcess()
        {
            if (_toolMode == ToolMode.Compile)
            {
                _toolMode = ToolMode.Decompile;
                if (!_pathIsDirectory)
                {
                    _path = Path.ChangeExtension(_path, FileExtensions.XNBExtension);
                }
            }
            else
            {
                _toolMode = ToolMode.Compile;
                if (!_pathIsDirectory)
                {
                    _path = Path.ChangeExtension(_path, FileExtensions.JsonExtension);
                }
            }

            GenerateContent(_toolMode);
        }

        private void ResetFactory()
        {
            Console.Clear();
            _path = null;
            StartFactory();
        }

        private void ReadLastKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            var key = keyInfo.Key;

            switch (key)
            {
                case (ConsoleKey.R):
                    GenerateContent(_toolMode);
                    break;

                case (ConsoleKey.N):
                    ResetFactory();
                    break;

                case (ConsoleKey.P):
                    ReverseProcess();
                    break;

                case (ConsoleKey.Escape):
                    return;

                case (ConsoleKey.Q):
                    Configuration.Instance.ReloadSettings();
                    ResetFactory();
                    break;

                default:
                    ReadLastKey();
                    break;
            }
        }

        private void PromptForPath()
        {
            var pathStatus = VerifyPath();
            if (PathStatus.Valid == pathStatus)
            {
                return;
            }

            while (pathStatus != PathStatus.Valid)
            {
                if (pathStatus != PathStatus.PathIsNull)
                {
                    PrintPathErrors(pathStatus);
                }

                Logger.WritePrompt("Input the path to a JSON instruction or XNB file\\directory:");
                _path = Console.ReadLine().Trim('\"');

                pathStatus = VerifyPath();
            }
        }

        private void PromptForHotkeys()
        {
            Logger.WritePrompt("\nPlease input a key.\n- 'R' to repeat the last process\n- 'P' to reverse the last process\n- 'N' to begin a new compilation/decompilation\n- 'Q' to reload the config file\n- 'ESC' to exit program");
            ReadLastKey();
        }

        private void PromptIsModern()
        {
            Console.Clear();
            Logger.WritePrompt("Are you creating/reading content from an older version of Magicka? [Eg. 1.5.1.0]\n\"0\" : No\n\"1\" : Yes");
            _modern = int.Parse(Console.ReadLine()) == 0; //Client has said yes
            Console.Clear();
        }

        private void PromptForgeType()
        {
            Console.Clear();
            Logger.WritePrompt("What type of content are you attempting to decompile? \n\"0\" : Character\n\"1\" : Item\n\"2\" : Level\n\"3\" : Model\n\"4\" : Skinned Model");
            _forgeType = Enum.Parse<ForgeType>(Console.ReadLine());
            Console.Clear();
        }

        private void PromptToolMode()
        {
            Console.Clear();
            Logger.WritePrompt("Would you like to compile to XNB or decompile to Json?\n\"0\" : Compile\n\"1\" : Decompile");
            _toolMode = Enum.Parse<ToolMode>(Console.ReadLine());
            Console.Clear();
        }

        private void PrintPathErrors(PathStatus status)
        {
            Console.Clear();

            switch (status)
            {
                case PathStatus.Valid:
                    Logger.WriteResult("The path is valid!");
                    break;
                case PathStatus.PathIsNull:
                    Logger.WriteError("The path is null! Please provide a valid path to a file or directory.");
                    break;
                case PathStatus.PathDoesNotExist:
                    Logger.WriteError($"No file nor directory exists on the inputted path! Ensure that your file exists!\nUser Input: {_path}");
                    break;
                case PathStatus.InvalidExtension:
                    Logger.WriteError($"The file extension is not valid! Please provide a JSON or XNB file.\nUser Input: {_path}");
                    break;
            }

            Console.WriteLine();
        }

        private bool PathExists => File.Exists(_path) || Directory.Exists(_path);
    }
}
