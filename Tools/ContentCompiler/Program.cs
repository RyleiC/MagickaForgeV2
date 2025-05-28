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

        private static void SetTitle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.Title = $"Magicka Forge v{versionInfo.FileVersion}";
        }
    }
}
