using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterSpellChecker
{
    /// <summary>
    /// Implementation of bloom filter.
    /// His Error-Probability can be calculated with
    ///                 1
    /// P = ( 1 - [ 1 - - ]^kn ) ^k
    ///                 m
    /// Where <c>m</c> is the size of the array
    /// Where <c>k</c> is the number of hashFunctions 
    /// Where <c>n</c> is the number of excpected element to be inserted into the bitArray
    /// 
    /// The HashFunction used in the class is MurmurHash
    /// </summary>
    public partial class Filter<T>
    {
        /// <summary>
        /// That's where the bits 0 or 1 can be stored, representing this results in a set of (TRUE and FALSE) where TRUE = 1 and FALSE = 0
        /// </summary>
        public readonly BitArray hashBits;

        /// <summary>
        /// The best k iteration for hashing found by K
        /// </summary>
        private readonly int hashFunctionCount;

        /// <summary>
        /// delegate for fast using and splitting usage for method in case of String and Int
        /// </summary>
        private readonly HashFunction getHash;

        /// <summary>
        /// Creating Delegate
        /// </summary>
        /// <param name="input"></param>
        /// <param name="seed"></param>
        /// <returns>The value where 0 or 1 need to be stored</returns>
        public delegate int HashFunction(string input, uint seed);



        public Filter(int capacity)
            : this(capacity, null)
        {

        }
        
        public Filter(int capacity, HashFunction? hashFunction)
            : this(capacity, BestErrorRate(capacity), hashFunction)
        {

        }

        public Filter(int capacity, float? errorRate, HashFunction? hashFunction)
            : this(capacity, errorRate, hashFunction, BestM(capacity, errorRate), BestK(capacity, errorRate))
        {

        }

        public Filter(int capacity, float? errorRate, HashFunction? hashFunction, int m, int k)
        {
            if(capacity < 1)
            {
                throw new ArgumentOutOfRangeException("Capacity must be > 0");
            }

            if(errorRate >= 1 || errorRate <= 0)
            {
                throw new ArgumentOutOfRangeException("Error Must be between 0 and 1");
            }

            if(m < 1)
            {
                throw new ArgumentOutOfRangeException("The provided capacity and errorRate would result in an array length > int.MaxValue");
            }

            if(hashFunction == null)
            {
                if (typeof(T) == typeof(string))
                    this.getHash = Hash;
            }
            else
            {
                this.getHash = hashFunction;
            }

            this.hashFunctionCount = k;
            this.hashBits = new BitArray(m);
        }

        public void add(string item)
        {
            int primaryHash = item.GetHashCode();
            int secondaryHash = this.getHash(item, (uint)hashFunctionCount);
            for(int i = 0; i < this.hashFunctionCount; i++)
            {
                int hash = this.ComputeHash(primaryHash, secondaryHash, i);
                this.hashBits[hash] = true;
            }
        }

        /// <summary>
        /// Check if hashBits contains the word or number <see cref="string"/> <c>item</c>
        /// </summary>
        /// <param name="item"></param>
        /// <returns <see cref="bool"/>></returns>
        public bool Contains(string item)
        {
            int primaryHash = item.GetHashCode();
            int secondaryHash = this.getHash(item, (uint)hashFunctionCount);
            for(int i = 0; i < this.hashFunctionCount; i++)
            {
                int hash = this.ComputeHash(primaryHash, secondaryHash, i);
                if (this.hashBits[hash] == false)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// If the expected number of elements n is known and desired probability is p, we can calculate the size of the bitArray as :
        /// 
        ///      -(n log(P))
        ///  m = ------------
        ///        (ln2)^2
        ///        
        /// this give us the best array size for reducing false positives, that can be caused by index position value overloading.
        /// </summary>
        private static int BestM(int capacity, float? errorRate)
        {
            return (int)Math.Ceiling(capacity * Math.Log((double)errorRate, 1.0 / Math.Pow(2, Math.Log(2.0))));
        }


        /// <summary>
        /// K is the best number of hash functions it can be calculated as :
        ///     m
        /// k = - ln(2)
        ///     n
        /// </summary>
        private static int BestK(int capacity, float? errorRate)
        {
            return (int)Math.Round(Math.Log(2.0) * BestM(capacity, errorRate) / capacity);
        }


        /// <summary>
        /// Finds the error Rate that is beetween 0 and 1 
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        private static float BestErrorRate(int capacity)
        {
            float c = (float)(1.0 / capacity);
            if(c != 0) return c;

            return (float)Math.Pow(0.6185, int.MaxValue / capacity);
        }


        /// <summary>
        /// Performs Dillinger and Manolis Double Hashing
        /// </summary>
        /// <param name="primaryHash"></param>
        /// <param name="secondaryHash"></param>
        /// <param name="i"></param>
        /// <returns><see cref="int"/> Hashed Values</returns>
        private int ComputeHash(int primaryHash, int secondaryHash, int i)
        {
            int resultingHash = (primaryHash + (i * secondaryHash)) % this.hashBits.Count;
            return Math.Abs((int)resultingHash);
        }
    }
}
