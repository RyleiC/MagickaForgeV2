using ContentCompiler.Misc;
using ContentCompiler.Settings;
using ContentCompiler.Tools;
using System.Diagnostics;
using System.Reflection;
namespace ContentCompiler
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                SetTitle();

                string path = null;
                if (args.Length > 0)
                {
                    path = args[0];
                }

                var config = Configuration.Instance;
                var factory = new MagickaFactory(path);
                factory.StartFactory();
            }
            catch (Exception ex)
            {
                Console.Clear();

                Logger.WriteError("Magicka Forge threw an exception !\n\n");
                Logger.WriteError(ex.ToString());
                Console.WriteLine("\nPress any key to exit program...");

                _ = Console.ReadKey();
            }
        }

        private static void SetTitle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.Title = $"Magicka Forge v{versionInfo.FileVersion}";
        }
    }
}
