using System;
using System.Collections.Generic;
using System.Linq;

namespace BitStream
{
    public struct BitNum
    {
        public static readonly BitNum MaxValue = new BitNum(8, true);
        public static readonly BitNum MinValue = new BitNum(1, true);

        private readonly byte value;

        /// <summary>
        /// Creates a new instance of the <see cref="BitNum"/> struct with the given value.
        /// <para/>
        /// Value will be truncated to the MaxValue if it's larger or rised to the MinValue.
        /// </summary>
        public BitNum(byte value)
        {
            this.value = Math.Min(MaxValue, Math.Max(MinValue, value));
        }

        private BitNum(byte value, bool overide)
        {
            if (!overide)
                this = new BitNum(value);
            else
                this.value = value;
        }

        public byte GetBitPos()
        {
            return (byte)(1 << (value - 1));
        }

        #region Casts

        #region From this

        public static implicit operator ushort(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator sbyte(BitNum @this)
        {
            return (sbyte)@this.value;
        }

        public static implicit operator ulong(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator int(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator byte(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator double(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator decimal(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator short(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator uint(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator long(BitNum @this)
        {
            return @this.value;
        }

        public static implicit operator float(BitNum @this)
        {
            return @this.value;
        }

        #endregion From this

        #region To this

        public static explicit operator BitNum(sbyte that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(byte that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(short that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(ushort that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(int that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(uint that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(long that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(ulong that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(decimal that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(float that)
        {
            return new BitNum((byte)that);
        }

        public static explicit operator BitNum(double that)
        {
            return new BitNum((byte)that);
        }

        #endregion To this

        #endregion Casts
    }
}