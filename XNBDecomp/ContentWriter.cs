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
    public class ContentWriter : BinaryWriter
    {
        private char filePlatform;
        private byte fileVersion;
        private byte graphicsProfile;
        private bool compressContent;
        private Stream finalOutput;
        private MemoryStream contentData;

        private const char PlatformWindows = 'w';
        private const char PlatformXbox = 'x';
        private const char PlatformMobile = 'm';
        private const byte XnbVersion_30 = 3;
        private const byte XnbVersion_31 = 4;
        private const byte XnbVersion_40 = 5;
        private const int XnbFileSizeOffset = 6;
        private const int XnbPrologueSize = 10;
        private const int XnbVersionOffset = 4;
        private const byte XnbProfileMask = 0x7f;
        private const byte XnbCompressedMask = 0x80;

        public ContentWriter(Stream output, bool compressContent = false, int filePlatform = PlatformWindows, int fileVersion = XnbVersion_40, int graphicsProfile = 0)
        {
            finalOutput = output;
            contentData = new MemoryStream();

            this.filePlatform = (char)filePlatform;

            this.fileVersion = (byte)fileVersion;

            if (fileVersion >= XnbVersion_40)
            {
                this.graphicsProfile = (byte)graphicsProfile;
            }
            else
            {
                this.graphicsProfile = 0;
            }

            this.compressContent = false;

            base.OutStream = contentData;
        }

        public ContentWriter(string filename, bool compressContent = false, int filePlatform = PlatformWindows, int fileVersion = XnbVersion_40, int graphicsProfile = 0)
            : this(File.Open(filename, FileMode.Create), compressContent, filePlatform, fileVersion, graphicsProfile)
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    contentData.Dispose();
                }
            }
            finally
            {
                base.OutStream = contentData;
                base.Dispose(disposing);
            }
        }

        public void FlushOutput()
        {
            WriteFinalOutput();
        }

        private void WriteFinalOutput()
        {
            base.OutStream = finalOutput;
            Write('X');
            Write('N');
            Write('B');

            Write(filePlatform);

            WriteUncompressedOutput();
            Close();
        }

        private void WriteUncompressedOutput()
        {
            WriteVersionNumber();

            int contentLength = (int)contentData.Length;
            Write((int)(XnbPrologueSize + contentLength));

            base.OutStream.Write(contentData.GetBuffer(), 0, contentLength);
        }

        private void WriteVersionNumber()
        {
            Write(fileVersion);

            byte profile = (byte)((graphicsProfile & XnbProfileMask) | (compressContent ? XnbCompressedMask : 0));
            Write(profile);
        }
    }
}
