using XNBDecomp;
using MagickaForge;

public static class XNBDecompressor
{
    public static string DecompressXNB(string path)
    {
        ContentReader contentReader = ContentReader.Create(path);
        var outPath = path.Replace(".xnb", ".dxnb");
        ContentWriter contentWriter = new ContentWriter(File.Create(outPath), false, contentReader.fileVersion, contentReader.graphicsProfile);
        try
        {
            for (int i = 0; i < contentReader.fileSize; i++)
            {
                contentWriter.Write(contentReader.ReadByte());
            }
        }
        catch
        {
            Console.WriteLine("Decompression failed!");
        }

        contentReader.Close();
        contentWriter.FlushOutput();

        return outPath;
    }
}