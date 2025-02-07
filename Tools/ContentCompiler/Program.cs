using ContentCompiler.Tools;
using System.Diagnostics;
using System.Reflection;
namespace ContentCompiler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.Title = $"Magicka Forge v{versionInfo.FileVersion}";

            var path = string.Empty;
            if (args.Length > 0)
            {
                path = args[0];
            }

            var factory = new MagickaFactory(path);
            factory.BeginProcess();

        }
    }
}
