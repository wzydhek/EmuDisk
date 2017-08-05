using System;
using System.Text;

namespace EmuDisk
{
    internal class Util
    {
        private static bool _designMode;

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

        public static byte[] ModifiedDateBytes(DateTime date)
        {
            return new byte[] { (byte)(date.Year - 1900), (byte)date.Month, (byte)date.Day, (byte)date.Hour, (byte)date.Minute };
        }

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
