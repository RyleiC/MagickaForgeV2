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
    public class ContentReader : BinaryReader
    {
        public int FilePlatform { get; private set; }
        public int FileVersion { get; private set; }
        public bool Compressed { get; private set; }
        public int FileSize { get; private set; }
        public int GraphicsProfile { get; private set; }

        private const char PlatformWindows = 'w';
        private const char PlatformXbox = 'x';
        private const byte XnbVersion_31 = 4;
        private const int XnbCompressedPrologueSize = 14;
        private const int XnbPrologueSize = 10;
        private const byte XnbCompressedMask = 0x80;

        private ContentReader(Stream input, int filePlatform, int fileVersion, int graphicsProfile, bool compressed, int fileSize)
            : base(input)
        {
            FilePlatform = filePlatform;
            FileVersion = fileVersion;
            GraphicsProfile = graphicsProfile;
            Compressed = compressed;
            FileSize = fileSize;
        }

        public static ContentReader Create(Stream input, int diskFileSize)
        {
            var reader = new BinaryReader(input);
            if (((reader.ReadByte() != 'X') || (reader.ReadByte() != 'N')) || (reader.ReadByte() != 'B'))
            {
                throw new InvalidOperationException("Bad magic.");
            }
            var filePlatform = reader.ReadByte();
            if (filePlatform != PlatformWindows && filePlatform != PlatformXbox)
            {
                throw new InvalidOperationException("Bad platform.");
            }
            var fileVersion = reader.ReadByte();
            if (fileVersion != XnbVersion_31)
            {
                throw new Exception("MagickaForge only supports XNBs from XNA 3.1");
            }

            var num = reader.ReadByte();
            var graphicsProfile = 0;
            bool compressed = (num & XnbCompressedMask) == XnbCompressedMask;

            var fileSize = reader.ReadInt32();
            if (compressed)
            {
                var compressedTodo = fileSize - XnbCompressedPrologueSize;
                fileSize = reader.ReadInt32();
                input = DecompressStream.getStream(input, compressedTodo, fileSize);
            }
            else
            {
                //backwards compatibility with older versions which set 0 as the file size
                //ignore on compressed files as magicka forge cannot produce them
                if (fileSize < diskFileSize)
                {
                    fileSize = diskFileSize;
                }
                fileSize -= XnbPrologueSize;
            }

            return new ContentReader(input, filePlatform, fileVersion, graphicsProfile, compressed, fileSize);
        }

        public static ContentReader Create(string filename)
        {
            return Create(File.OpenRead(filename), (int)new FileInfo(filename).Length);
        }
    }
}
