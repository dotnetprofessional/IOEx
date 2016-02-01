using System;
using System.Collections.Generic;
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
                if (this.StartBufferPosition < 0)
                    this.StartBufferPosition = 0;
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
                return new String(this.CharBuffer, this.StartBufferPosition, this.Length);
            }
        }

        public List<Line> Split(char s)
        {
            var parts = new List<Line>();
            var line = new Line(this.CharBuffer);
            // Set the first part details
            line.StartPosition = this.StartBufferPosition;

            for (int i = this.StartBufferPosition; i < this.StartBufferPosition + this.Length; i++)
            {
                var c = this.CharBuffer[i];
                if (c == s)
                {
                    line.EndPosition = i;
                    parts.Add(line);
                    line = new Line(this.CharBuffer);
                    line.StartPosition = i + 1;
                }
            }
            parts.Add(line);
            line.EndPosition = this.EndPosition;
            return parts;
        }

    }
}
