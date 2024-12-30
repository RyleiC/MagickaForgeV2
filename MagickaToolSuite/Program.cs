using MagickaToolSuite.Tools;
using System.Diagnostics;
using System.Reflection;
namespace MagickaToolSuite
{
    internal class Program
    {
        public static void Main()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Console.Title = $"Magicka Forge v{versionInfo.FileVersion}";

            var compiler = new MagickaFactory();
            compiler.BeginProcess();
        }
    }
}
