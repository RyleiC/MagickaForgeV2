using System;
using System.IO;

namespace XNBDecomp
{
    internal static class DecompressStream
    {
        public static Stream getStream(Stream baseStream, int compressedTodo, int decompressedTodo)
        {
            byte[] inBuf = new byte[0x10000];
            byte[] outBuf = new byte[0x10000];

            LzxDecoder dec = new LzxDecoder(16);

            MemoryStream decompressedStream = new MemoryStream(decompressedTodo);

            int decodedBytes = 0;
            int pos = 0;

            long origin = baseStream.Position;

            while (pos < compressedTodo)
            {
                int flag, hi, lo, frame_size, block_size;
                flag = (byte)baseStream.ReadByte();
                if (flag == 0xFF)
                {
                    hi = (byte)baseStream.ReadByte();
                    lo = (byte)baseStream.ReadByte();
                    frame_size = (hi << 8) | lo;
                    hi = (byte)baseStream.ReadByte();
                    lo = (byte)baseStream.ReadByte();
                    block_size = (hi << 8) | lo;
                    pos += 5;
                }
                else
                {
                    hi = flag;
                    lo = (byte)baseStream.ReadByte();
                    block_size = (hi << 8) | lo;
                    frame_size = 0x8000;
                    pos += 2;
                }

                if (block_size == 0 || frame_size == 0)
                {
                    break;
                }

                if (block_size > 0x10000 || frame_size > 0x10000)
                {
                    throw new InvalidOperationException("Error decompressing content data.");
                }

                baseStream.Read(inBuf, 0, block_size);
                dec.Decompress(outBuf, frame_size, inBuf, block_size);
                decompressedStream.Write(outBuf, 0, frame_size);

                pos += block_size;
                decodedBytes += frame_size;
            }

            decompressedStream.Seek(0, SeekOrigin.Begin);

            return decompressedStream;
        }
    }
}
