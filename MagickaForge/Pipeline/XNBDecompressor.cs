using XNBDecomp;

public static class XNBDecompressor
{
    public static Stream DecompressXNB(string path)
    {
        var stream = new MemoryStream();
        var contentReader = ContentReader.Create(path);

        ContentWriter contentWriter = new ContentWriter(stream, false, contentReader.fileVersion, contentReader.graphicsProfile);
        try
        {
            for (int i = 0; i < contentReader.fileSize; i++)
            {
                contentWriter.Write(contentReader.ReadByte());
            }
        }
        catch
        {
            throw new InvalidDataException("Decompression failed!");
        }

        contentReader.Close();
        contentWriter.FlushOutput();

        stream.Position = 0;
        return stream;
    }
}