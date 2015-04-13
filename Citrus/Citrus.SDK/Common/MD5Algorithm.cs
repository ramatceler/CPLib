﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MD5Algorithm.cs" company="Citrus Payment Solutions Pvt. Ltd.">
//   Copyright 2015 Citrus Payment Solutions Pvt. Ltd.
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <summary>
//   MD5 alogrithm
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Common
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    ///     The custom implementation of MD5 algorithm.
    /// </summary>
    public class MD5 : IDisposable
    {
        #region Constants

        /// <summary>
        /// </summary>
        private const byte S11 = 7;

        /// <summary>
        /// </summary>
        private const byte S12 = 12;

        /// <summary>
        /// </summary>
        private const byte S13 = 17;

        /// <summary>
        /// </summary>
        private const byte S14 = 22;

        /// <summary>
        /// </summary>
        private const byte S21 = 5;

        /// <summary>
        /// </summary>
        private const byte S22 = 9;

        /// <summary>
        /// </summary>
        private const byte S23 = 14;

        /// <summary>
        /// </summary>
        private const byte S24 = 20;

        /// <summary>
        /// </summary>
        private const byte S31 = 4;

        /// <summary>
        /// </summary>
        private const byte S32 = 11;

        /// <summary>
        /// </summary>
        private const byte S33 = 16;

        /// <summary>
        /// </summary>
        private const byte S34 = 23;

        /// <summary>
        /// </summary>
        private const byte S41 = 6;

        /// <summary>
        /// </summary>
        private const byte S42 = 10;

        /// <summary>
        /// </summary>
        private const byte S43 = 15;

        /// <summary>
        /// </summary>
        private const byte S44 = 21;

        #endregion

        #region Static Fields

        /// <summary>
        /// </summary>
        private static byte[] PADDING =
            {
                0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        protected int HashSizeValue = 128;

        /// <summary>
        /// </summary>
        protected byte[] HashValue;

        /// <summary>
        /// </summary>
        protected int State;

        /// <summary>
        ///     input buffer
        /// </summary>
        private byte[] buffer = new byte[64];

        /// <summary>
        ///     number of bits, modulo 2^64 (lsb first)
        /// </summary>
        private uint[] count = new uint[2];

        /// <summary>
        ///     state (ABCD)
        /// </summary>
        private uint[] state = new uint[4];

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        internal MD5()
        {
            this.Initialize();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public virtual bool CanReuseTransform
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// </summary>
        public virtual bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public virtual byte[] Hash
        {
            get
            {
                if (this.State != 0)
                {
                    throw new InvalidOperationException();
                }

                return (byte[])this.HashValue.Clone();
            }
        }

        /// <summary>
        /// </summary>
        public virtual int HashSize
        {
            get
            {
                return this.HashSizeValue;
            }
        }

        /// <summary>
        /// </summary>
        public virtual int InputBlockSize
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// </summary>
        public virtual int OutputBlockSize
        {
            get
            {
                return 1;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create an instance of MD5 provider.
        /// </summary>
        /// <param name="hashName">
        /// The hash type name.
        /// </param>
        /// <returns>
        /// The <see cref="MD5"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public static MD5 Create(string hashName)
        {
            if (hashName == "MD5")
            {
                return new MD5();
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Create a new instance for MD5
        /// </summary>
        /// <returns>
        /// </returns>
        public static MD5 Create()
        {
            return new MD5();
        }

        /// <summary>
        /// Gets MD5 String value.
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetMd5String(string source)
        {
            MD5 md = MD5CryptoServiceProvider.Create();
            byte[] hash;

            // Create a new instance of ASCIIEncoding to
            // convert the string into an array of Unicode bytes.
            var enc = new UTF8Encoding();

            // ASCIIEncoding enc = new ASCIIEncoding();

            // Convert the string into an array of bytes.
            byte[] buffer = enc.GetBytes(source);

            // Create the hash value from the array of bytes.
            hash = md.ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        public byte[] ComputeHash(byte[] buffer)
        {
            return this.ComputeHash(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <param name="offset">
        /// </param>
        /// <param name="count">
        /// </param>
        /// <returns>
        /// </returns>
        public byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            this.Initialize();
            this.HashCore(buffer, offset, count);
            this.HashValue = this.HashFinal();
            return (byte[])this.HashValue.Clone();
        }

        /// <summary>
        /// </summary>
        /// <param name="inputStream">
        /// </param>
        /// <returns>
        /// </returns>
        public byte[] ComputeHash(Stream inputStream)
        {
            this.Initialize();
            int count;
            var buffer = new byte[4096];
            while (0 < (count = inputStream.Read(buffer, 0, 4096)))
            {
                this.HashCore(buffer, 0, count);
            }

            this.HashValue = this.HashFinal();
            return (byte[])this.HashValue.Clone();
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        ///     MD5 initialization. Begins an MD5 operation, writing a new context.
        /// </summary>
        /// <remarks>
        ///     The RFC named it "MD5Init"
        /// </remarks>
        public virtual void Initialize()
        {
            this.count[0] = this.count[1] = 0;

            // Load magic initialization constants.
            this.state[0] = 0x67452301;
            this.state[1] = 0xefcdab89;
            this.state[2] = 0x98badcfe;
            this.state[3] = 0x10325476;
        }

        /// <summary>
        /// </summary>
        /// <param name="inputBuffer">
        /// </param>
        /// <param name="inputOffset">
        /// </param>
        /// <param name="inputCount">
        /// </param>
        /// <param name="outputBuffer">
        /// </param>
        /// <param name="outputOffset">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public int TransformBlock(
            byte[] inputBuffer, 
            int inputOffset, 
            int inputCount, 
            byte[] outputBuffer, 
            int outputOffset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }

            if ((inputCount < 0) || (inputCount > inputBuffer.Length))
            {
                throw new ArgumentException("inputCount");
            }

            if ((inputBuffer.Length - inputCount) < inputOffset)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }

            if (this.State == 0)
            {
                this.Initialize();
                this.State = 1;
            }

            this.HashCore(inputBuffer, inputOffset, inputCount);
            if ((inputBuffer != outputBuffer) || (inputOffset != outputOffset))
            {
                Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            }

            return inputCount;
        }

        /// <summary>
        /// </summary>
        /// <param name="inputBuffer">
        /// </param>
        /// <param name="inputOffset">
        /// </param>
        /// <param name="inputCount">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }

            if ((inputCount < 0) || (inputCount > inputBuffer.Length))
            {
                throw new ArgumentException("inputCount");
            }

            if ((inputBuffer.Length - inputCount) < inputOffset)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }

            if (this.State == 0)
            {
                this.Initialize();
            }

            this.HashCore(inputBuffer, inputOffset, inputCount);
            this.HashValue = this.HashFinal();
            var buffer = new byte[inputCount];
            Buffer.BlockCopy(inputBuffer, inputOffset, buffer, 0, inputCount);
            this.State = 0;
            return buffer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose the current instance.
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                this.Initialize();
            }
        }

        /// <summary>
        /// MD5 block update operation. Continues an MD5 message-digest
        ///     operation, processing another message block, and updating the
        ///     context.
        /// </summary>
        /// <param name="input">
        /// </param>
        /// <param name="offset">
        /// </param>
        /// <param name="count">
        /// </param>
        /// <remarks>
        /// The RFC Named it MD5Update
        /// </remarks>
        protected virtual void HashCore(byte[] input, int offset, int count)
        {
            int i;
            int index;
            int partLen;

            // Compute number of bytes mod 64
            index = (int)((this.count[0] >> 3) & 0x3F);

            // Update number of bits
            if ((this.count[0] += (uint)count << 3) < ((uint)count << 3))
            {
                this.count[1]++;
            }

            this.count[1] += (uint)count >> 29;

            partLen = 64 - index;

            // Transform as many times as possible.
            if (count >= partLen)
            {
                Buffer.BlockCopy(input, offset, this.buffer, index, partLen);
                this.Transform(this.buffer, 0);

                for (i = partLen; i + 63 < count; i += 64)
                {
                    this.Transform(input, offset + i);
                }

                index = 0;
            }
            else
            {
                i = 0;
            }

            // Buffer remaining input
            Buffer.BlockCopy(input, offset + i, this.buffer, index, count - i);
        }

        /// <summary>
        ///     MD5 finalization. Ends an MD5 message-digest operation, writing the
        ///     the message digest and zeroizing the context.
        /// </summary>
        /// <returns>message digest</returns>
        /// <remarks>The RFC named it MD5Final</remarks>
        protected virtual byte[] HashFinal()
        {
            var digest = new byte[16];
            var bits = new byte[8];
            int index, padLen;

            // Save number of bits
            Encode(bits, 0, this.count, 0, 8);

            // Pad out to 56 mod 64.
            index = (int)(this.count[0] >> 3 & 0x3f);
            padLen = (index < 56) ? (56 - index) : (120 - index);
            this.HashCore(PADDING, 0, padLen);

            // Append length (before padding)
            this.HashCore(bits, 0, 8);

            // Store state in digest
            Encode(digest, 0, this.state, 0, 16);

            // Zeroize sensitive information.
            this.count[0] = this.count[1] = 0;
            this.state[0] = 0;
            this.state[1] = 0;
            this.state[2] = 0;
            this.state[3] = 0;

            // initialize again, to be ready to use
            this.Initialize();

            return digest;
        }

        /// <summary>
        /// Decodes input (byte) into output (uint). Assumes len is a multiple of 4.
        /// </summary>
        /// <param name="output">
        /// </param>
        /// <param name="outputOffset">
        /// </param>
        /// <param name="input">
        /// </param>
        /// <param name="inputOffset">
        /// </param>
        /// <param name="count">
        /// </param>
        private static void Decode(uint[] output, int outputOffset, byte[] input, int inputOffset, int count)
        {
            int i, j;
            int end = inputOffset + count;
            for (i = outputOffset, j = inputOffset; j < end; i++, j += 4)
            {
                output[i] = input[j] | (((uint)input[j + 1]) << 8) | (((uint)input[j + 2]) << 16)
                            | (((uint)input[j + 3]) << 24);
            }
        }

        /// <summary>
        /// Encodes input (uint) into output (byte). Assumes len is  multiple of 4.
        /// </summary>
        /// <param name="output">
        /// </param>
        /// <param name="outputOffset">
        /// </param>
        /// <param name="input">
        /// </param>
        /// <param name="inputOffset">
        /// </param>
        /// <param name="count">
        /// </param>
        private static void Encode(byte[] output, int outputOffset, uint[] input, int inputOffset, int count)
        {
            int i, j;
            int end = outputOffset + count;
            for (i = inputOffset, j = outputOffset; j < end; i++, j += 4)
            {
                output[j] = (byte)(input[i] & 0xff);
                output[j + 1] = (byte)((input[i] >> 8) & 0xff);
                output[j + 2] = (byte)((input[i] >> 16) & 0xff);
                output[j + 3] = (byte)((input[i] >> 24) & 0xff);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static uint F(uint x, uint y, uint z)
        {
            return ((x) & (y)) | ((~x) & (z));
        }

        /// <summary>
        /// </summary>
        /// <param name="a">
        /// </param>
        /// <param name="b">
        /// </param>
        /// <param name="c">
        /// </param>
        /// <param name="d">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="s">
        /// </param>
        /// <param name="ac">
        /// </param>
        /// FF, GG, HH, and II transformations
        /// for rounds 1, 2, 3, and 4.
        /// Rotation is separate from addition to prevent recomputation.
        private static void FF(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += F((b), (c), (d)) + (x) + ac;
            a = ROTATE_LEFT((a), (s));
            a += (b);
        }

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static uint G(uint x, uint y, uint z)
        {
            return ((x) & (z)) | ((y) & (~z));
        }

        /// <summary>
        /// </summary>
        /// <param name="a">
        /// </param>
        /// <param name="b">
        /// </param>
        /// <param name="c">
        /// </param>
        /// <param name="d">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="s">
        /// </param>
        /// <param name="ac">
        /// </param>
        private static void GG(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += G((b), (c), (d)) + (x) + ac;
            a = ROTATE_LEFT((a), (s));
            a += (b);
        }

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static uint H(uint x, uint y, uint z)
        {
            return (x) ^ (y) ^ (z);
        }

        /// <summary>
        /// </summary>
        /// <param name="a">
        /// </param>
        /// <param name="b">
        /// </param>
        /// <param name="c">
        /// </param>
        /// <param name="d">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="s">
        /// </param>
        /// <param name="ac">
        /// </param>
        private static void HH(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += H((b), (c), (d)) + (x) + ac;
            a = ROTATE_LEFT((a), (s));
            a += (b);
        }

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static uint I(uint x, uint y, uint z)
        {
            return (y) ^ ((x) | (~z));
        }

        /// <summary>
        /// </summary>
        /// <param name="a">
        /// </param>
        /// <param name="b">
        /// </param>
        /// <param name="c">
        /// </param>
        /// <param name="d">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="s">
        /// </param>
        /// <param name="ac">
        /// </param>
        private static void II(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += I((b), (c), (d)) + (x) + ac;
            a = ROTATE_LEFT((a), (s));
            a += (b);
        }

        /// <summary>
        /// rotates x left n bits.
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="n">
        /// </param>
        /// <returns>
        /// </returns>
        private static uint ROTATE_LEFT(uint x, byte n)
        {
            return ((x) << (n)) | ((x) >> (32 - (n)));
        }

        /// <summary>
        /// MD5 basic transformation. Transforms state based on 64 bytes block.
        /// </summary>
        /// <param name="block">
        /// </param>
        /// <param name="offset">
        /// </param>
        private void Transform(byte[] block, int offset)
        {
            uint a = this.state[0], b = this.state[1], c = this.state[2], d = this.state[3];
            var x = new uint[16];
            Decode(x, 0, block, offset, 64);

            // Round 1
            FF(ref a, b, c, d, x[0], S11, 0xd76aa478); /* 1 */
            FF(ref d, a, b, c, x[1], S12, 0xe8c7b756); /* 2 */
            FF(ref c, d, a, b, x[2], S13, 0x242070db); /* 3 */
            FF(ref b, c, d, a, x[3], S14, 0xc1bdceee); /* 4 */
            FF(ref a, b, c, d, x[4], S11, 0xf57c0faf); /* 5 */
            FF(ref d, a, b, c, x[5], S12, 0x4787c62a); /* 6 */
            FF(ref c, d, a, b, x[6], S13, 0xa8304613); /* 7 */
            FF(ref b, c, d, a, x[7], S14, 0xfd469501); /* 8 */
            FF(ref a, b, c, d, x[8], S11, 0x698098d8); /* 9 */
            FF(ref d, a, b, c, x[9], S12, 0x8b44f7af); /* 10 */
            FF(ref c, d, a, b, x[10], S13, 0xffff5bb1); /* 11 */
            FF(ref b, c, d, a, x[11], S14, 0x895cd7be); /* 12 */
            FF(ref a, b, c, d, x[12], S11, 0x6b901122); /* 13 */
            FF(ref d, a, b, c, x[13], S12, 0xfd987193); /* 14 */
            FF(ref c, d, a, b, x[14], S13, 0xa679438e); /* 15 */
            FF(ref b, c, d, a, x[15], S14, 0x49b40821); /* 16 */

            // Round 2
            GG(ref a, b, c, d, x[1], S21, 0xf61e2562); /* 17 */
            GG(ref d, a, b, c, x[6], S22, 0xc040b340); /* 18 */
            GG(ref c, d, a, b, x[11], S23, 0x265e5a51); /* 19 */
            GG(ref b, c, d, a, x[0], S24, 0xe9b6c7aa); /* 20 */
            GG(ref a, b, c, d, x[5], S21, 0xd62f105d); /* 21 */
            GG(ref d, a, b, c, x[10], S22, 0x2441453); /* 22 */
            GG(ref c, d, a, b, x[15], S23, 0xd8a1e681); /* 23 */
            GG(ref b, c, d, a, x[4], S24, 0xe7d3fbc8); /* 24 */
            GG(ref a, b, c, d, x[9], S21, 0x21e1cde6); /* 25 */
            GG(ref d, a, b, c, x[14], S22, 0xc33707d6); /* 26 */
            GG(ref c, d, a, b, x[3], S23, 0xf4d50d87); /* 27 */
            GG(ref b, c, d, a, x[8], S24, 0x455a14ed); /* 28 */
            GG(ref a, b, c, d, x[13], S21, 0xa9e3e905); /* 29 */
            GG(ref d, a, b, c, x[2], S22, 0xfcefa3f8); /* 30 */
            GG(ref c, d, a, b, x[7], S23, 0x676f02d9); /* 31 */
            GG(ref b, c, d, a, x[12], S24, 0x8d2a4c8a); /* 32 */

            // Round 3
            HH(ref a, b, c, d, x[5], S31, 0xfffa3942); /* 33 */
            HH(ref d, a, b, c, x[8], S32, 0x8771f681); /* 34 */
            HH(ref c, d, a, b, x[11], S33, 0x6d9d6122); /* 35 */
            HH(ref b, c, d, a, x[14], S34, 0xfde5380c); /* 36 */
            HH(ref a, b, c, d, x[1], S31, 0xa4beea44); /* 37 */
            HH(ref d, a, b, c, x[4], S32, 0x4bdecfa9); /* 38 */
            HH(ref c, d, a, b, x[7], S33, 0xf6bb4b60); /* 39 */
            HH(ref b, c, d, a, x[10], S34, 0xbebfbc70); /* 40 */
            HH(ref a, b, c, d, x[13], S31, 0x289b7ec6); /* 41 */
            HH(ref d, a, b, c, x[0], S32, 0xeaa127fa); /* 42 */
            HH(ref c, d, a, b, x[3], S33, 0xd4ef3085); /* 43 */
            HH(ref b, c, d, a, x[6], S34, 0x4881d05); /* 44 */
            HH(ref a, b, c, d, x[9], S31, 0xd9d4d039); /* 45 */
            HH(ref d, a, b, c, x[12], S32, 0xe6db99e5); /* 46 */
            HH(ref c, d, a, b, x[15], S33, 0x1fa27cf8); /* 47 */
            HH(ref b, c, d, a, x[2], S34, 0xc4ac5665); /* 48 */

            // Round 4
            II(ref a, b, c, d, x[0], S41, 0xf4292244); /* 49 */
            II(ref d, a, b, c, x[7], S42, 0x432aff97); /* 50 */
            II(ref c, d, a, b, x[14], S43, 0xab9423a7); /* 51 */
            II(ref b, c, d, a, x[5], S44, 0xfc93a039); /* 52 */
            II(ref a, b, c, d, x[12], S41, 0x655b59c3); /* 53 */
            II(ref d, a, b, c, x[3], S42, 0x8f0ccc92); /* 54 */
            II(ref c, d, a, b, x[10], S43, 0xffeff47d); /* 55 */
            II(ref b, c, d, a, x[1], S44, 0x85845dd1); /* 56 */
            II(ref a, b, c, d, x[8], S41, 0x6fa87e4f); /* 57 */
            II(ref d, a, b, c, x[15], S42, 0xfe2ce6e0); /* 58 */
            II(ref c, d, a, b, x[6], S43, 0xa3014314); /* 59 */
            II(ref b, c, d, a, x[13], S44, 0x4e0811a1); /* 60 */
            II(ref a, b, c, d, x[4], S41, 0xf7537e82); /* 61 */
            II(ref d, a, b, c, x[11], S42, 0xbd3af235); /* 62 */
            II(ref c, d, a, b, x[2], S43, 0x2ad7d2bb); /* 63 */
            II(ref b, c, d, a, x[9], S44, 0xeb86d391); /* 64 */

            this.state[0] += a;
            this.state[1] += b;
            this.state[2] += c;
            this.state[3] += d;

            // Zeroize sensitive information.
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = 0;
            }
        }

        #endregion
    }
}