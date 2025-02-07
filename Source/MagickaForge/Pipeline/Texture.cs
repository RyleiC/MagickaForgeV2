namespace MagickaForge.Pipeline
{
    public static class Texture
    {
        /*
        private static readonly byte[] ReaderHeader =
        [
            0x01, 0x2F, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74,
            0x2E, 0x58, 0x6E, 0x61, 0x2E, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F,
            0x72, 0x6B, 0x2E, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2E, 0x54,
            0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x32, 0x44, 0x52, 0x65, 0x61, 0x64,
            0x65, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01
        ];
        private const int RGBA4Code = 1;
        private const int MipMapLayers = 1;*/

        public static void WritePngToXNB(string inputPath)
        {
            /*    Png png = Png.Open(inputPath);
                using (BinaryWriter bw = new BinaryWriter(File.Open(inputPath.Replace(".png", ".xnb"), FileMode.Create)))
                {
                    bw.Write(XNBHelper.XNBHeader);
                    bw.Write(0); //Placeholder
                    bw.Write(ReaderHeader);
                    bw.Write(RGBA4Code);
                    bw.Write(png.Width);
                    bw.Write(png.Height);
                    bw.Write(MipMapLayers);
                    bw.Write(png.Width * png.Height * 4);
                    for (int x = 0; x < png.Height; x++)
                    {
                        for (int y = 0; y < png.Width; y++)
                        {
                            Pixel pixel = png.GetPixel(y, x);
                            bw.Write(pixel.B);
                            bw.Write(pixel.G);
                            bw.Write(pixel.R);
                            bw.Write(pixel.A);
                        }
                    }
                    XNBHelper.WriteFileSize(bw);
                };*/
            throw new NotImplementedException();
        }
    }
}
