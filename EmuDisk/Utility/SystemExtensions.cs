using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    /// <summary>
    /// Extensions to built-in system methods or value types
    /// </summary>
    internal static class SystemExtensions
    {
        /// <summary>
        /// Return a subset of an array
        /// </summary>
        /// <param name="array">Source array of bytes</param>
        /// <param name="startIndex">Source's Beginning index</param>
        /// <param name="length">Maximum Number of bytes to return</param>
        /// <returns>Subset of source array</returns>
        public static byte[] Subset(this byte[] array, int startIndex, int length)
        {
            byte[] data;
            if (startIndex + length > array.Length)
            {
                data = new byte[array.Length - startIndex];
            }
            else
            {
                data = new byte[length];
            }

            Array.Copy(array, startIndex, data, 0, data.Length);
            return data;
        }

        /// <summary>
        /// Initialize an array of bytes to a specified value
        /// </summary>
        /// <param name="array">Array of bytes</param>
        /// <param name="defaultValue">New value to initialize to</param>
        /// <returns>Initialized array of bytes</returns>
        public static byte[] Initialize(this byte[] array, byte defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }

            return array;
        }

        /// <summary>
        /// Validates that all bytes in the array are an ASCII value
        /// Optionally skip Zero bytes
        /// </summary>
        /// <param name="array">Array of bytes to validate</param>
        /// <param name="ignoreZero">Skip Zero bytes?</param>
        /// <returns>True if all bytes are ASCII</returns>
        public static bool IsASCII(this byte[] array, bool ignoreZero)
        {
            if (array.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < array.Length; i++)
            {
                if ((array[i] < 0x20 || array[i] > 0x7e) && array[i] != 0x0d && array[i] != 0x0a && array[i] != 0x09)
                {
                    if (!ignoreZero)
                    {
                        if (array[i] == 0)
                        {
                            for (int j = i; j < array.Length; j++)
                            {
                                if (array[j] != 0x00)
                                {
                                    return false;
                                }
                            }

                            return true;
                        }
                    }

                    return false;
                }
            }

            return true;
        }

    }
}
