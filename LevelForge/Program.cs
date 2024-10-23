using MagickaForge.Forges.Items;
using MagickaForge.Forges.Levels;
using System.Diagnostics;

namespace LevelForge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("= Magicka Level Forge by Rylei. C =");
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

            Console.WriteLine("= Process Starting... =\n");

            var stopWatch = Stopwatch.StartNew();

            Level level = new Level();
            level.XNBToLevel(instructionPath);

            stopWatch.Stop();
            Console.WriteLine($"= Process completed in {stopWatch.ElapsedMilliseconds} ms =");

            Console.ReadKey();
        }
    }
}
