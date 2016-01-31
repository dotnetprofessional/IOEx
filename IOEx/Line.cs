using System;
using System.Text;

namespace IOEx
{
    public class Line
    {
        private int _startPosition;
        private int _endPosition;


        public Line(char[] charBuffer)
        {
            this.CharBuffer = charBuffer;
        }

        internal char[] CharBuffer { get; set; }

        internal string BigAssString { get; set; }
        /// <summary>
        /// The byte position within the file where this line begins
        /// </summary>
        public int StartPosition
        {
            get { return _startPosition; }
            set
            {
                _startPosition = value;
                this.StartBufferPosition = value - this.FilePosition;
            }
        }

        /// <summary>
        /// The byte position within the file where this line ends
        /// </summary>
        public int EndPosition
        {
            get { return _endPosition; }
            set
            {
                _endPosition = value;
                this.Length = value - this.StartPosition;
            }
        }

        /// <summary>
        /// The byte position within the file where the buffer starts
        /// </summary>
        internal int FilePosition { get; set; }

        internal int StartBufferPosition { get; set; }

        public int Length { get; set; }

        public string Text
        {
            get
            {
                //charLen = this.Decoder.GetChars(this.Buffer, this.StartBufferPosition, this.Length, this.Buffer,0);

                //return this.BigAssString.Substring(this.StartBufferPosition, this.Length);
                return new String(this.CharBuffer, this.StartBufferPosition, this.Length);
                //return Encoding.ASCII.GetString(this.Buffer, this.StartBufferPosition, this.Length);
                //return Encoding.UTF8.GetString(this.Buffer, this.StartBufferPosition, this.Length);
            }
        }
    }
}
