using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class Util
    {
        private static bool _designMode;

        /// <summary>
        /// <para>
        /// Truncate a path to fit within a certain number of characters 
        /// by replacing path components with ellipses.
        /// </para>
        /// <para>
        /// This solution is provided by CodeProject and GotDotNet C# expert
        /// Richard Deeming.
        /// </para>
        /// </summary>
        /// <param name="longName">Long file name</param>
        /// <param name="maxLen">Maximum length</param>
        /// <returns>Truncated file name</returns>
        public static string GetShortDisplayName(string longName, int maxLen)
        {
            StringBuilder pszOut = new StringBuilder(maxLen + maxLen + 2);  // for safety

            if (NativeMethods.PathCompactPathEx(pszOut, longName, maxLen, 0))
            {
                return pszOut.ToString();
            }
            else
            {
                return longName;
            }
        }

        /// <summary>
        /// Gets a high-bit terminated string from an array of bytes
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>String value</returns>
        public static string GetHighBitString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((bytes[i] & 0x80) == 0x80)
                {
                    sb.Append((char)(bytes[i] & 0x7f));
                    break;
                }

                sb.Append((char)bytes[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert an 5 byte array to a DateTime
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>DateTime structure</returns>
        public static DateTime ModifiedDate(byte[] bytes)
        {
            DateTime date = new DateTime();
            if (bytes == null || bytes.Length != 5)
            {
                return date;
            }

            try
            {
                date = new DateTime(1900 + bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], 0);
                if (date.Year < 1970)
                {
                    date.AddYears(100);
                }
            }
            catch
            {
                if (bytes[0] == 0 || bytes[1] == 0)
                {
                    date = DateTime.Now;
                }
                else
                {
                    try
                    {
                        date = new DateTime(2000 + bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], 0);
                    }
                    catch
                    {
                        date = DateTime.Now;
                    }
                }
            }

            return date;
        }

        /// <summary>
        /// Convert a DateTime structure to a 5 byte array
        /// </summary>
        /// <param name="date">DateTime structure</param>
        /// <returns>5-Byte array</returns>
        public static byte[] ModifiedDateBytes(DateTime date)
        {
            return new byte[] { (byte)(date.Year - 1900), (byte)date.Month, (byte)date.Day, (byte)date.Hour, (byte)date.Minute };
        }

        /// <summary>
        /// Convert an array of bytes to a 16 bit unsigned integer
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>New 16 bit unsigned integer</returns>
        public static ushort UInt16(byte[] bytes)
        {
            ushort value = 0;
            if (bytes.Length > 0)
            {
                int pos = bytes.Length - 1;
                int count = 0;
                while (pos >= 0 && count < 2)
                {
                    value |= (ushort)(bytes[pos--] << (count++ * 8));
                }
            }

            return value;
        }

        /// <summary>
        /// Convert Array of bytes to a 24 bit integer
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>24 bit integer</returns>
        public static int Int24(byte[] bytes)
        {
            int value = 0;
            if (bytes.Length > 0)
            {
                int pos = bytes.Length - 1;
                int count = 0;
                while (pos >= 0 && count < 3)
                {
                    value |= bytes[pos--] << (count++ * 8);
                }
            }

            return value;
        }

        public static bool DesignMode
        {
            get
            {
                return _designMode;
            }
        }
    }
}
