using System;
using System.Text;

namespace Citrus.SDK.Common
{
    public class RandomPasswordGenerator 
    {
        public String Generate(String email, String mobile)
        {
            PseudoRandomNumberGenerator ran = new PseudoRandomNumberGenerator(GenerateSeed(email));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                builder.Append(ran.NextLetter());
            }
            return builder.ToString();
        }

        /// <summary>
        /// Generates a non-random positive number from a string. 
	    /// @param data the string to generate seed from.
        /// @return the value of the 3 highest bytes of the MD5 sum of data.
        /// </summary>
        public int GenerateSeed(String data)
        {
            var hash = Encoding.UTF8.GetBytes(Md5Algorithm.MD5.GetMd5String(data));
            hash = rangeCopy(hash, hash.Length - 3, hash.Length);
            return new BigMath.BigInteger(1, hash).IntValue;
        }

        /// <summary>
        /// Copy the range of values from byte array.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static byte[] rangeCopy(byte[] original, int from, int to)
        {
            int lengthNew = to - from;
            if (lengthNew < 0)
            {
                //throw new IllegalArgumentException(from + " > " + to);
            }
            byte[] hash = new byte[lengthNew];
            System.Array.Copy(original, from, hash, 0,
                Math.Min(original.Length - from, lengthNew));
            return hash;

        }
    }

    /// <summary>
    /// A pseudo random integer generator.
    /// </summary>
    public class PseudoRandomNumberGenerator
    {
        private int state;

        public PseudoRandomNumberGenerator()
        {

        }

        /// <summary>
        /// Creates a new pseudo random number generator. param seed the seed to start the generation from.
        /// </summary>
        public PseudoRandomNumberGenerator(int seed)
        {
            this.state = seed;
        }

        /// <summary>
        /// Generates the next pseudo-random integer. The number is positive in the [0-max] interval.
        /// @param max the exclusive upper boundary of the interval to generate the pseudo-random in.
        /// @return the next number in the pseudo-random suite modulo max.
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public int NextInt(int max)
        {
            state = 7*state%3001;
            return (state - 1)%max;
        }

        /// <summary>
        /// Generates the next pseudo-random alphabetical character. The letter is in the [a-zA-Z] range. 
        /// @return the next letter in the pseudo-random suite.
        /// </summary>
        /// <returns></returns>
        public char NextLetter()
        {
            int n = NextInt(52);
            return (char) (n + ((n < 26) ? 'A' : 'a' - 26));
        }
    }
}
