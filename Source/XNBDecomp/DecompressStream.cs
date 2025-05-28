/*
 The MIT License (MIT)

Copyright (c) 2011-2014 Andrew McRae

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace XNBDecomp
{
    internal static class DecompressStream
    {
        public static Stream GetStream(Stream baseStream, int compressedTodo, int decompressedTodo)
        {
            byte[] inBuf = new byte[0x10000];
            byte[] outBuf = new byte[0x10000];

            var dec = new LzxDecoder(16);

            var decompressedStream = new MemoryStream(decompressedTodo);

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
