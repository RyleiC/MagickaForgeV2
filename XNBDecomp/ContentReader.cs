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
        public int filePlatform;
        public int fileVersion;
        public bool compressed;
        public int fileSize;
        public int graphicsProfile;

        private const char PlatformWindows = 'w';
        private const char PlatformXbox = 'x';
        private const char PlatformMobile = 'm';
        private const byte XnbVersion_30 = 3;
        private const byte XnbVersion_31 = 4;
        private const byte XnbVersion_40 = 5;
        private const int XnbCompressedPrologueSize = 14;
        private const int XnbPrologueSize = 10;
        private const byte XnbProfileMask = 0x7f;
        private const byte XnbCompressedMask = 0x80;

        private ContentReader(Stream input, int filePlatform, int fileVersion, int graphicsProfile, bool compressed, int fileSize)
            : base(input)
        {
            this.filePlatform = filePlatform;
            this.fileVersion = fileVersion;
            this.graphicsProfile = graphicsProfile;
            this.compressed = compressed;
            this.fileSize = fileSize;
        }

        public static ContentReader Create(Stream input, int diskFileSize)
        {
            var reader = new BinaryReader(input);
            if (((reader.ReadByte() != 'X') || (reader.ReadByte() != 'N')) || (reader.ReadByte() != 'B'))
            {
                throw new InvalidOperationException("Bad magic.");
            }

            int filePlatform = reader.ReadByte();
            if (filePlatform != PlatformWindows && filePlatform != PlatformXbox)
            {
                throw new InvalidOperationException("Bad platform.");
            }

            int fileVersion = reader.ReadByte();
            if (fileVersion > XnbVersion_40)
            {
                throw new InvalidOperationException("Bad version.");
            }

            int num = reader.ReadByte();
            bool compressed = false;
            int graphicsProfile = 0;
            if (fileVersion >= XnbVersion_40)
            {
                graphicsProfile = num & XnbProfileMask;
            }
            if (fileVersion >= XnbVersion_30)
            {
                compressed = (num & XnbCompressedMask) == XnbCompressedMask;
            }

            var fileSize = reader.ReadInt32();
            if (compressed)
            {
                int compressedTodo = fileSize - XnbCompressedPrologueSize;
                fileSize = reader.ReadInt32();
                input = DecompressStream.getStream(input, compressedTodo, fileSize);
            }
            else
            {

                //BACKWARDS COMPATIBIILITY WITH OLDER VERSIONS OF MAGICKA FORGE
                if (fileSize < diskFileSize)
                {
                    fileSize = diskFileSize;
                }
                //it won't have a wrong file size if it is compressed as this tool doesn't compress produced XNBs
                fileSize = fileSize - XnbPrologueSize;
            }

            return new ContentReader(input, filePlatform, fileVersion, graphicsProfile, compressed, fileSize);
        }

        public static ContentReader Create(string filename)
        {
            return Create(File.Open(filename, FileMode.Open), (int)new FileInfo(filename).Length);
        }
    }
}
