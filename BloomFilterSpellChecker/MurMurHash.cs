﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterSpellChecker
{
    /// <summary>
    /// Implementation of public hashing non-encryption alghorithm
    /// </summary>
    public partial class Filter<T>
    {

        private static Encoding encoding = new UTF8Encoding();
        public static int Hash(string stream, uint seed)
        {
            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;

            byte[] input = encoding.GetBytes(stream);

            uint h1 = seed;
            uint k1 = 0;
            uint streamLength = 0;


            using (MemoryStream ms = new MemoryStream(input))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    byte[] chunk = reader.ReadBytes(4);
                    while (chunk.Length > 0)
                    {
                        streamLength += (uint)chunk.Length;
                        switch (chunk.Length)
                        {
                            case 4:
                                k1 = (uint)(
                                      chunk[0]
                                    | chunk[1] << 8
                                    | chunk[2] << 16
                                    | chunk[3] << 24);
                                k1 *= c1;
                                k1 = rotl132(k1, 15);
                                k1 *= c2;

                                h1 ^= k1;
                                h1 = rotl132(h1, 13);
                                h1 = h1 * 5 + 0xe6546b64;
                                break;
                            case 3:
                                k1 = (uint)
                                   (chunk[0]
                                  | chunk[1] << 8
                                  | chunk[2] << 16);
                                k1 *= c1;
                                k1 = rotl132(k1, 15);
                                k1 *= c2;
                                h1 ^= k1;
                                break;
                            case 2:
                                k1 = (uint)
                                   (chunk[0]
                                  | chunk[1] << 8);
                                k1 *= c1;
                                k1 = rotl132(k1, 15);
                                k1 *= c2;
                                h1 ^= k1;
                                break;
                            case 1:
                                k1 = (uint)(chunk[0]);
                                k1 *= c1;
                                k1 = rotl132(k1, 15);
                                k1 *= c2;
                                h1 ^= k1;
                                break;
                        }
                        chunk = reader.ReadBytes(4);
                    }
                }
                h1 ^= streamLength;
                h1 = fmix(h1);

                unchecked
                {
                    return (int)h1;
                }
            }
        }

        private static uint rotl132(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static uint fmix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }
    }
}
