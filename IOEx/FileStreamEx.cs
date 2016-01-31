using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOEx
{
    public class FileStreamEx : IDisposable
    {
        private object lockRef = new object();

        public string Filename { get; set; }

        public Encoding Encoding { get; set; }

        protected FileStream InternalReader { get; set; }

        protected byte[] ByteBuffer { get; set; }
        protected char[] Buffer { get; set; }

        public int LineCount { get; set; }

        internal Line InternalLine { get; set; }

        internal bool DecodingBuffer { get; set; }

        public FileStreamEx(string filename) : this(filename, 1024 * 1024)
        {

        }

        public FileStreamEx(string filename, int bufferSize)
        {
            Filename = filename;
            this.BufferSize = bufferSize;
            this.ByteBuffer = new byte[this.BufferSize];
            this.Buffer = new char[this.BufferSize * 2];
            this.Encoding = Encoding.UTF8;
            this.Decoder = Encoding.GetDecoder();

            // Don't open a Stream before checking for invalid arguments,
            // or we'll create a FileStream on disk and we won't close it until
            // the finalizer runs, causing problems for applications.
            if (this.Filename == null || this.Encoding == null)
                throw new ArgumentNullException((this.Filename == null ? "filename" : "encoding"));
            if (this.Filename.Length == 0)
                throw new ArgumentException("Unknown filename.");
            //if (this.BufferSize <= 1024)
            //    throw new ArgumentOutOfRangeException("BufferSize", "Buffer must be greater than 1024 bytes");
        }

        internal Decoder Decoder { get; set; }

        public int BufferSize { get; set; }

        protected void InitializeStream()
        {
            this.InternalLine = new Line(this.Buffer);

            this.InternalReader = new FileStream(this.Filename, FileMode.Open, FileAccess.Read, FileShare.Read, this.BufferSize, FileOptions.SequentialScan);
            this.ReadBuffer();
        }

        internal int BufferReadOffset { get; set; }

        /// <summary>
        /// The byte count of consumed data from the buffer
        /// </summary>
        internal int BufferPosition { get; set; }

        /// <summary>
        /// The byte count of read data in buffer
        /// </summary>
        internal int BufferAvailable { get; set; }

        public Line ReadLine()
        {
            if (this.InternalLine == null)
                this.InitializeStream();

            // check if there's any buffer remaining
            if (this.BufferPosition == this.BufferAvailable)
            {
                if (this.DecodingBuffer)
                {
                    this.DelayCount++;
                    Task.Delay(5); // Wait 5 ms and try again
                    Console.WriteLine("Delay " + this.DelayCount);
                    return this.ReadLine();
                }

                this.DelayCount = 0;
                this.ReadBuffer();
            }

            if (this.BufferAvailable == 0)
                // No data was read from the file so must be finished
                return null;

            var lineFound = this.NextLineBreakPosition();
            if (!lineFound)
            {
                if (this.DecodingBuffer)
                {
                    this.DelayCount++;
                    Task.Delay(5); // Wait 5 ms and try again
                    Console.WriteLine("Delay " + this.DelayCount);
                    return this.ReadLine();
                }
                this.DelayCount = 0;
                // a line break wasn't found check if the line exceeds available buffer or data is in the next file chunk
                if (this.BufferAvailable == 0)
                    throw new ArgumentException("Line length exceeds available buffer.");

                // Transfer unused buffer to beginning then load next chunk into buffer

                var buffer = this.Buffer;
                var bufferOffset = 0;
                for (int i = this.BufferPosition; i < this.BufferAvailable; i++)
                {
                    buffer[bufferOffset] = buffer[i];
                    bufferOffset++;
                }
                this.BufferReadOffset = bufferOffset;

                //System.Buffer.BlockCopy(this.ByteBuffer, this.BufferPosition, this.ByteBuffer, 0, this.BufferReadOffset);
                // Read next chunk 
                ReadBuffer();

                return this.ReadLine();
            }

            return this.InternalLine;
        }

        public int DelayCount { get; set; }

        bool NextLineBreakPosition()
        {
            var buffer = this.Buffer;
            for (int i = this.BufferPosition; i < this.BufferAvailable; i++)
            {
                if (buffer[i] == '\n' || buffer[i] == '\r')
                {
                    this.InternalLine.StartPosition = this.InternalLine.FilePosition + this.BufferPosition;
                    this.InternalLine.EndPosition = i + this.InternalLine.FilePosition;

                    if (this.BufferAvailable > i + 1)
                    {
                        if (buffer[i] == '\r' && buffer[i + 1] == '\n')
                            i++;

                    }
                    i++; // consumes the control character without outputting it

                    this.BufferPosition = i;

                    return true;
                }
            }

            return false;
        }

        protected void ReadBuffer()
        {
            if (this.DecodingBuffer)
                return;

            this.InternalLine.FilePosition += this.BufferAvailable;
            // Read the next chunk of data from file, preserving any buffer data before the BufferReadOffset
            var bytesRead = this.InternalReader.Read(this.ByteBuffer, 0, this.BufferSize);
            this.DecodeBuffer(bytesRead);
        }

        void DecodeBuffer(int bytesRead)
        {
            if (this.DecodingBuffer)
                throw new Exception("Decoding already true!!!");

            if(this.BufferReadOffset > this.BufferSize)
                throw new ArgumentException($"Lines in this file exceed the buffer size of {this.BufferSize}, try using a larger buffer for this file.");

            // Now process the buffer array into decoded chars that can be consumed
            int firstPassBytesToConsume = Math.Min(bytesRead, this.BufferSize) / 2;
            var charLen = this.Decoder.GetChars(this.ByteBuffer, 0, firstPassBytesToConsume, this.Buffer,
                this.BufferReadOffset);

            this.BufferPosition = 0;
            this.BufferAvailable = charLen + this.BufferReadOffset;
            this.BufferReadOffset = 0;

            this.DecodingBuffer = true;
            Task.Run(() =>
            {
                // Now decode the second part of the byteBuffer
                charLen = this.Decoder.GetChars(this.ByteBuffer, firstPassBytesToConsume, bytesRead - firstPassBytesToConsume, this.Buffer, this.BufferAvailable);
                this.BufferAvailable = charLen + this.BufferAvailable;
                this.DecodingBuffer = false;
            });
        }

        public void Dispose()
        {
            this.InternalReader?.Dispose();
        }
    }
}
