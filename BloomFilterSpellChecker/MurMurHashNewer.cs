using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterSpellChecker
{
    public partial class Filter<T>
    {
        private static uint c1 = 0xcc9e2d51;
        private static uint c2 = 0x1b873593;
        private static uint h1 = 0;
        private static uint k1 = 0;
        private static uint streamLength = 0;

        public static int NewerHash(string toHash, uint seed)
        {
            h1 = seed;



            int maxUtf8SizeInBytes = Encoding.UTF8.GetMaxByteCount(toHash.Length);
            byte[] buffer = ArrayPool<byte>.Shared.Rent(maxUtf8SizeInBytes);

            int utf8SizeInBytes = Encoding.UTF8.GetBytes(toHash, buffer);

            ReadOnlySpan<byte> utf8Text = buffer.AsSpan(0, utf8SizeInBytes);

            ReadOnlySpan<byte> remainingBytes = utf8Text;

            while (remainingBytes.Length > 0)
            {
                int nextSliceSize = Math.Min(4, remainingBytes.Length);

                ReadOnlySpan<byte> chunk = remainingBytes.Slice(nextSliceSize);

                //PROCESS THe Chunk
                
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

                remainingBytes = remainingBytes.Slice(nextSliceSize);
            }
            ArrayPool<byte>.Shared.Return(buffer);

            h1 ^= streamLength;
            h1 = fmix(h1);
            unchecked { return (int)h1; }

        }
    }
}



