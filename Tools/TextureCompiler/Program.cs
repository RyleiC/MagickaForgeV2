namespace TextureCompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input the path to a texture");
            var path = Console.ReadLine();
            Texture.WriteImageToXNB(path);
        }
    }
}
