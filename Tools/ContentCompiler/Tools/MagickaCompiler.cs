using ContentCompiler.Data;
using ContentCompiler.Data.Languages;
using ContentCompiler.Misc;
using ContentCompiler.Settings;
using MagickaForge.Pipeline.Json;
using MagickaForge.Pipeline.Json.Characters;
using MagickaForge.Pipeline.Json.Items;

namespace ContentCompiler.Tools
{
    internal class MagickaCompiler
    {
        private readonly XNBVerifier _verifier;

        public MagickaCompiler()
        {
            _verifier = new XNBVerifier();
        }

        private readonly static EnumerationOptions _options = new EnumerationOptions()
        {
            MatchCasing = MatchCasing.CaseInsensitive,
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
            IgnoreInaccessible = true,
            MaxRecursionDepth = 16
        };


        public void Compile(string inputPath, bool useModernCompilation)
        {
            var successes = 0;
            var attempts = 0;

            var languageFile = new LanguageFile();
            var languageFilePath = Path.Combine(Configuration.Instance.Settings.LocalizationPath, $"{Configuration.Instance.Settings.LanguageFileName}.loctable.xml");

            if (Configuration.Instance.Settings.GenerateLanguageFiles)
            {
                if (File.Exists(languageFilePath))
                {
                    languageFile.LoadFromFile(languageFilePath);
                    Logger.WriteSuccess($"Found language file, loading {languageFilePath}\n");
                }
                else
                {
                    languageFile.LoadDefault();
                    Logger.WriteSuccess($"No language file found, creating new one at {languageFilePath}\nMake sure to restart your game to reload the files!\n");
                }
            }

            var isDirectory = CompilingHelper.IsDirectory(inputPath);

            if (isDirectory)
            {
                foreach (string filePath in Directory.GetFiles(inputPath, "*.json", _options))
                {
                    BeginCompile(languageFile, filePath, useModernCompilation, ref attempts, ref successes);
                }
            }
            else
            {
                BeginCompile(languageFile, inputPath, useModernCompilation, ref attempts, ref successes);
            }

            if (languageFile.IsDirty && Configuration.Instance.Settings.GenerateLanguageFiles)
            {
                languageFile.Export(languageFilePath);
                Logger.WriteResult($"\nLanguage file modifed, writing to {languageFilePath}");
            }

            Logger.WriteResult($"\n\nCompilation complete! {successes}/{attempts} successful compilations.");
        }

        private void BeginCompile(LanguageFile languageFile, string filePath, bool useModern, ref int attempts, ref int successes)
        {
            attempts++;
            var pipelineItem = LoadWithModernPreferences(filePath, useModern);

            if (TryCompilation(pipelineItem, languageFile, filePath))
            {
                successes++;
            }
        }

        private bool TryCompilation(PipelineJsonObject pipelineObject, LanguageFile languageFile, string inputPath)
        {
            var outputPath = Path.ChangeExtension(inputPath, FileExtensions.XNBExtension);
            var verifyResult = new VerifyResult();

            if (Configuration.Instance.Settings.GenerateLanguageFiles)
            {
                if (pipelineObject is Character character && HasCustomText(character.LocalizedName))
                {
                    var code = $"#mod_{character.Name}";
                    languageFile.RegisterEntry(character.LocalizedName, code);
                    character.LocalizedName = code;
                }
                else if (pipelineObject is Item item)
                {
                    if (HasCustomText(item.LocalizedName))
                    {
                        var code = $"#mod_{item.Name}";
                        languageFile.RegisterEntry(item.LocalizedName, code);
                        item.LocalizedName = code;
                    }
                    if (HasCustomText(item.LocalizedDescription))
                    {
                        var code = $"#mod_{item.Name}_d";
                        languageFile.RegisterEntry(item.LocalizedDescription, code);
                        item.LocalizedDescription = code;
                    }
                }
            }

            if (Configuration.Instance.Settings.VerifyXNBs)
            {
                verifyResult = _verifier.VerifyXNB(pipelineObject, Path.GetDirectoryName(outputPath));
            }

            if (!verifyResult.IsValidCompilation)
            {
                PrintFailMessage(inputPath, verifyResult);
                return false;
            }

            pipelineObject.Export(outputPath);
            PrintSuccessMessage(inputPath, verifyResult);
            return true;
        }

        private PipelineJsonObject LoadWithModernPreferences(string filePath, bool useModern)
        {
            var pipelineObject = PipelineJsonObject.Load(filePath);

            if (pipelineObject is Character character)
            {
                character.CompileForModernMagicka = useModern;
            }

            return pipelineObject;
        }

        private bool HasCustomText(string target)
        {
            return !string.IsNullOrEmpty(target) && target[0] != '#';
        }

        private void PrintSuccessMessage(string inputPath, VerifyResult verifyResult)
        {
            if (Configuration.Instance.Settings.VerifyXNBs)
            {
                Logger.WriteSuccess($"Successfully compiled {Path.GetFileName(inputPath)} with {verifyResult.WarningCount} warnings");
                verifyResult.Print();
                return;
            }

            Logger.WriteSuccess($"Successfully compiled {Path.GetFileName(inputPath)}");
        }

        private void PrintFailMessage(string inputPath, VerifyResult verifyResult)
        {
            Logger.WriteError($"Failed to compile {Path.GetFileName(inputPath)} [{verifyResult.ErrorCount} errors / {verifyResult.WarningCount} warnings]");
            verifyResult.Print();
        }
    }
}
