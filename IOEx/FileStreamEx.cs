using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IOEx
{
    public class FileStreamEx : IDisposable
    {
        public string Filename { get; set; }

        public Encoding Encoding { get; set; }

        protected FileStream InternalReader { get; set; }

        protected byte[] Buffer { get; set; }

        public int LineCount { get; set; }

        internal Line InternalLine { get; set; }

        public FileStreamEx(string filename)
        {
            Filename = filename;
            this.BufferSize = 1024 * 1024;
            //this.BufferSize = 50;
            this.Buffer = new byte[this.BufferSize];
            this.Encoding = Encoding.UTF8;

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

        public int BufferSize { get; set; }

        protected async Task InitializeStream()
        {
            this.InternalLine = new Line(this.Buffer);

            this.InternalReader = new FileStream(this.Filename, FileMode.Open, FileAccess.Read, FileShare.Read, this.BufferSize, FileOptions.SequentialScan);
            await this.ReadBufferAsync().ConfigureAwait(false);
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

        public async Task<Line> ReadLineAsync()
        {
            if (this.InternalLine == null)
                await this.InitializeStream().ConfigureAwait(false);

            // check if there's any buffer remaining
            if (this.BufferPosition == this.BufferAvailable)
                await this.ReadBufferAsync().ConfigureAwait(false);

            if (this.BufferAvailable == 0)
                // No data was read from the file so must be finished
                return null;

            var lineFound = this.NextLineBreakPosition();
            if (!lineFound)
            {
                // a line break wasn't found check if the line exceeds available buffer or data is in the next file chunk
                if (this.BufferAvailable == 0)
                    throw new ArgumentException("Line length exceeds available buffer.");

                // Transfer unused buffer to beginning then load next chunk into buffer
                //var buffer = this.InternalLine.Buffer;
                //var bufferOffset = 0;
                //for (int i = this.BufferPosition; i < this.BufferAvailable; i++)
                //{
                //    buffer[bufferOffset] = buffer[i];
                //    bufferOffset++;
                //}
                this.BufferReadOffset = this.BufferAvailable - this.BufferPosition;
                System.Buffer.BlockCopy(this.Buffer, this.BufferPosition, this.Buffer, 0, this.BufferReadOffset);
                // Read next chunk 
                await ReadBufferAsync().ConfigureAwait(false);

                return await this.ReadLineAsync().ConfigureAwait(false);
            }

            return this.InternalLine;
        }

        bool NextLineBreakPosition()
        {
            var buffer = this.InternalLine.Buffer;
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

        protected async Task ReadBufferAsync()
        {
            this.InternalLine.FilePosition += this.BufferAvailable;
            // Read the next chunk of data from file, preserving any buffer data before the BufferReadOffset
            var bytesRead = await this.InternalReader.ReadAsync(this.InternalLine.Buffer, this.BufferReadOffset, this.BufferSize - this.BufferReadOffset).ConfigureAwait(false);
            this.BufferPosition = 0;
            this.BufferReadOffset = 0;
            this.BufferAvailable = bytesRead + this.BufferReadOffset;
        }

        public void Dispose()
        {
            this.InternalReader?.Dispose();
        }
    }
}
