using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        /*This section of code includes necessary using statements for the rest of the code 
         * and declares the ReadonlyBytes class,
         * which implements the IEnumerable<byte> interface.*/
        private readonly byte[] values;
        private int hash;
        public int Length => values.Length;
        /*The values field is a private,
         * readonly array of bytes that stores the actual data contained within the ReadonlyBytes object.
         * The hash field is an integer that is used to store the hash code of the object.
         * The Length property returns the length of the values array*/


        public override int GetHashCode()
        {
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((ReadonlyBytes)obj);
        }

        private bool Equals(ReadonlyBytes obj)
        {
            if (obj.Length != Length)
            {
                return false;
            }

            for (var i = 0; i < Length; i++)
            {
                if (obj[i] != values[i])
                {
                    return false;
                }
            }

            return true;
        }
        /*These methods override the GetHashCode and Equals methods defined by the base Object class
         * . The GetHashCode method simply returns the pre-calculated hash code stored in the hash field
         * . The Equals method checks if the given object is null or not an instance of ReadonlyBytes
         * , and then calls the Equals(ReadonlyBytes) method to compare the objects.*/


        public ReadonlyBytes(params byte[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException();
            }

            this.values = values;
            FNVHash();
        }
        /*This is the constructor for the ReadonlyBytes class.
         * It takes in a variable number of byte parameters,
         * which are used to initialize the values array*/


        private void FNVHash()
        {
            const int fnvPrime = 4999;
            foreach (var value in values)
            {
                unchecked
                {
                    hash *= fnvPrime;
                    hash ^= value;
                }
            }
        }
        /*he method iterates over each byte in the values array and uses it to update the hash field.
         * The unchecked keyword is used to avoid an overflow exception when calculating the hash code.*/
        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /*This method uses the Fowler-Noll-Vo (FNV) hash algorithm to calculate
         * the hash code for the ReadonlyBytes object.
         * The fnvPrime constant is a prime number used by the FNV algorithm*/
        public byte this[int index]
        {
            get
            {
                if (index >= values.Length)
                {
                    throw new IndexOutOfRangeException();
                }

                return values[index];
            }
        }

        public override string ToString() => "[" + string.Join(", ", values) + "]";

        public static bool operator ==(ReadonlyBytes left, ReadonlyBytes right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(ReadonlyBytes left, ReadonlyBytes right)
        {
            return !(left == right);
        }
    }
}
