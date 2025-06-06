﻿using XNBDecomp;

namespace MagickaForge.Utils.Helpers
{
    public static class XNBHelper
    {
        public static readonly byte[] XNBHeader =
        [
            //Magic
            0x58, //X 
            0x4E, //N
            0x42, //B
           
            //Platform
            0x77, //w [Windows]
            
            //Version
            0x04, //3.1

            //Compression Flag
            0x00 //Uncompressed Flag
        ];

        public static void WriteFileSize(BinaryWriter binaryWriter)
        {
            var fileSize = (int)binaryWriter.BaseStream.Position;
            binaryWriter.BaseStream.Position = XNBHeader.Length;
            binaryWriter.Write(fileSize);
        }

        public static Stream DecompressXNB(string path)
        {
            var contentReader = ContentReader.Create(path);

            var stream = contentReader.BaseStream;
            if (contentReader.Compressed)
            {
                contentReader.BaseStream.Position = 0;
            }
            else
            {
                contentReader.BaseStream.Position = XNBHeader.Length + 4;
            }

            return stream;
        }
    }
}
