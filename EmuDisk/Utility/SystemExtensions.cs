using System;

namespace EmuDisk
{
    internal static class SystemExtensions
    {
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

        public static byte[] Initialize(this byte[] array, byte defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }

            return array;
        }

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
