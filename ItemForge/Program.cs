using MagickaForgeCompiler.Compiler;
namespace MagickaForgeCompiler
{
    public class Program
    {
        public static void Main()
        {
            var compiler = new MagickaCompiler();
            compiler.StartPrompts();
        }
    }
}
