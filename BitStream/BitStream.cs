using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BitStream
{
    /// <summary>
    /// Represents a
    /// </summary>
    public sealed class BitStream : Stream
    {
        private readonly Stream stream;

        public Stream UnderlayingStream
        {
            get { return stream; }
        }

        public override bool CanRead
        {
            get { return stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return stream.CanWrite; }
        }

        public override void Flush()
        {
            stream.Flush();
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

        public BitNum BitPosition { get; set; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (BitPosition == BitNum.MaxValue)
                return stream.Read(buffer, offset, count);

            return (int)(BitRead(buffer, offset, (uint)count * BitNum.MaxValue) / BitNum.MaxValue);
        }

        public ulong BitRead(byte[] buffer, int offset, ulong count)
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
            if (BitRead(out lastByte, bits))
                bitsRead += bits;

            buffer[offset] = lastByte;

            return bitsRead;
        }

        public override int ReadByte()
        {
            byte buffer;
            return BitRead(out buffer, BitNum.MaxValue) ? buffer : -1;
        }

        private byte currentByte;

        public bool BitRead(out byte buffer, BitNum bits)
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

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (BitPosition == BitNum.MaxValue)
                stream.Write(buffer, offset, count);

            BitWrite(buffer, offset, (uint)count * 8);
        }

        private void BitWrite(byte[] buffer, int offset, ulong p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BitStream"/> class with the given underlaying stream.
        /// </summary>
        /// <param name="underlayingStream">The underlaying stream to work on.</param>
        public BitStream(Stream underlayingStream)
        {
            BitPosition = BitNum.MaxValue;
            stream = underlayingStream;
        }
    }
}