using System.Text;

namespace IOEx
{
    public class Line
    {
        private int _startPosition;
        private int _endPosition;

        public Line(byte[] buffer)
        {
            Buffer = buffer;
        }

        internal byte[] Buffer { get; set; }

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

        public string Text => Encoding.UTF8.GetString(this.Buffer, this.StartBufferPosition, this.Length);

        ///// <summary>
        ///// The number of bytes that can be read given the current
        ///// </summary>
        //internal int ReadCapacity { get; set; }
    }
}
