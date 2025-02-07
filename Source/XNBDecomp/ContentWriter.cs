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
        private readonly char _filePlatform;
        private readonly byte _fileVersion;
        private readonly byte _graphicsProfile;
        private readonly bool _compressContent;
        private readonly Stream _finalOutput;
        private readonly MemoryStream _contentData;

        private const char PlatformWindows = 'w';
        private const byte XnbVersion_31 = 4;
        private const int XnbPrologueSize = 10;
        private const byte XnbProfileMask = 0x7f;
        private const byte XnbCompressedMask = 0x80;

        public ContentWriter(Stream output, int filePlatform = PlatformWindows, int fileVersion = XnbVersion_31)
        {
            _finalOutput = output;
            _contentData = new MemoryStream();
            _filePlatform = (char)filePlatform;
            _fileVersion = (byte)fileVersion;
            _compressContent = false;
            OutStream = _contentData;
        }

        public ContentWriter(string filename, int filePlatform = PlatformWindows, int fileVersion = XnbVersion_31)
            : this(File.Open(filename, FileMode.Create), filePlatform, fileVersion)
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _contentData.Dispose();
                }
            }
            finally
            {
                OutStream = _contentData;
                base.Dispose(disposing);
            }
        }

        public void FlushOutput()
        {
            WriteFinalOutput();
        }

        private void WriteFinalOutput()
        {
            OutStream = _finalOutput;
            Write('X');
            Write('N');
            Write('B');

            Write(_filePlatform);

            WriteUncompressedOutput();
            Close();
        }

        private void WriteUncompressedOutput()
        {
            WriteVersionNumber();

            var contentLength = (int)_contentData.Length;
            Write(XnbPrologueSize + contentLength);

            OutStream.Write(_contentData.GetBuffer(), 0, contentLength);
        }

        private void WriteVersionNumber()
        {
            Write(_fileVersion);

            byte profile = (byte)((_graphicsProfile & XnbProfileMask) | (_compressContent ? XnbCompressedMask : 0));
            Write(profile);
        }
    }
}
