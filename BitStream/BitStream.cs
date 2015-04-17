using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BitStream
{
    /// <summary>
    /// Wrapper for <see cref="Stream"/>s that allows bit-level reads and writes.
    /// </summary>
    public sealed class BitStream : Stream
    {
        private readonly Stream stream;

        private byte currentByte;

        public BitNum BitPosition { get; set; }

        #region Proxy Properties

        public override bool CanRead
        {
            get { return stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return stream.CanSeek; }
        }

        public override bool CanTimeout
        {
            get { return stream.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return stream.CanWrite; }
        }

        public override long Length
        {
            get { return stream.Length; }
        }

        public override long Position
        {
            get { return stream.Position; }
            set { stream.Position = value; }
        }

        public override int ReadTimeout
        {
            get { return stream.ReadTimeout; }
            set { stream.ReadTimeout = value; }
        }

        public Stream UnderlayingStream
        {
            get { return stream; }
        }

        public override int WriteTimeout
        {
            get { return stream.WriteTimeout; }
            set { stream.WriteTimeout = value; }
        }

        #endregion Proxy Properties

        /// <summary>
        /// Creates a new instance of the <see cref="BitStream"/> class with the given underlaying stream.
        /// </summary>
        /// <param name="underlayingStream">The underlaying stream to work on.</param>
        public BitStream(Stream underlayingStream)
        {
            BitPosition = BitNum.MaxValue;
            stream = underlayingStream;
        }

        #region Proxy Methods

        public override bool Equals(object obj)
        {
            return stream.Equals(obj);
        }

        public override void Flush()
        {
            stream.Flush();
        }

        public override int GetHashCode()
        {
            return stream.GetHashCode();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override string ToString()
        {
            return stream.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            stream.Dispose();
        }

        #endregion Proxy Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (BitPosition == BitNum.MaxValue)
                return stream.Read(buffer, offset, count);

            return (int)(ReadBits(buffer, offset, (uint)count * BitNum.MaxValue) / BitNum.MaxValue);
        }

        public bool ReadBits(out byte buffer, BitNum bits)
        {
            if (BitPosition == BitNum.MaxValue && bits == BitNum.MaxValue)
            {
                var readByte = stream.ReadByte();
                buffer = (byte)(readByte < 0 ? 0 : readByte);
                return !(readByte < 0);
            }

            buffer = 0;
            for (byte i = 0; i < bits; ++i)
            {
                if (BitPosition == BitNum.MaxValue)
                {
                    var readByte = stream.ReadByte();

                    if (readByte < 0)
                        return i > 0;

                    currentByte = (byte)readByte;
                    BitPosition = BitNum.MinValue;
                }

                buffer |= (byte)(currentByte & BitPosition.GetBitPos());
                BitPosition = new BitNum((byte)(BitPosition + 1));
            }

            return true;
        }

        public ulong ReadBits(byte[] buffer, int offset, ulong count)
        {
            ulong bitsRead = 0;
            while (count / BitNum.MaxValue > 0)
            {
                var nextByte = ReadByte();

                if (nextByte < 0)
                    buffer[offset] = 0;
                else
                {
                    buffer[offset] = (byte)nextByte;
                    bitsRead += BitNum.MaxValue;
                }

                ++offset;
            }

            byte lastByte;
            var bits = (BitNum)(count % BitNum.MaxValue);
            if (ReadBits(out lastByte, bits))
                bitsRead += bits;

            buffer[offset] = lastByte;

            return bitsRead;
        }

        public override int ReadByte()
        {
            byte buffer;
            return ReadBits(out buffer, BitNum.MaxValue) ? buffer : -1;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (BitPosition == BitNum.MaxValue)
                stream.Write(buffer, offset, count);

            WriteBits(buffer, offset, (uint)count * 8);
        }

        public void WriteBits(byte[] buffer, int offset, ulong p)
        {
            throw new NotImplementedException();
        }

        public void WriteBits(byte buffer, BitNum bits)
        {
            throw new NotImplementedException();
        }

        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }
    }
}