using System;
using System.Text;

namespace EmuDisk
{
    public enum RSDosFileTypes
    {
        Basic,
        BasicData,
        ML,
        Text
    }

    public class RSDosDirectoryEntry
    {
        #region Private Properties

        byte[] entry;

        #endregion

        #region Constructors

        public RSDosDirectorEntry()
        {
            entry = new byte[0x20];
        }

        public RSDosDirectorEntry(byte[] buffer) : this()
        {
            if (buffer.Length < 0x20)
            {
                Array.Copy(buffer, 0, entry, 0, buffer.Length);
            }
            else
                Array.Copy(buffer, 0, entry, 0, 0x20);
        }

        #endregion

        #region Public Properties

        public string Filename
        {
            get
            {
                return Encoding.ASCII.GetString(entry.Subset(0, 8)).Trim();
            }
            set
            {
                value += "        ";
                if (value.Length > 8)
                    value = value.Substring(0, 8);
                Array.Copy(Encoding.ASCII.GetBytes(value), 0, entry, 0, 8);
            }
        }

        public string Extension
        {
            get
            {
                return Encoding.ASCII.GetString(entry.Subset(8, 3)).Trim();
            }
            set
            {
                value += "   ";
                if (value.Length > 3)
                    value = value.Substring(0, 3);
                Array.Copy(Encoding.ASCII.GetBytes(value), 0, entry, 8, 3);
            }
        }

        public RSDosFileTypes FileType
        {
            get
            {
                return (RSDosFileTypes)entry[11];
            }
            set
            {
                entry[11] = (byte)value;
            }
        }

        public bool ASCII
        {
            get
            {
                if (entry[12] == 0xFF)
                    return true;
                else
                    return false;
            }
            set
            {
                entry[12] = (byte)(value ? 0xFF : 0x00);
            }
        }

        public int FirstGranule
        {
            get { return entry[13]; }
            set { entry[13] = (byte)value; }
        }

        public int LastSectorBytes
        {
            get { return (entry[14] << 8) + entry[15]; }
            set
            {
                entry[14] = (byte)(value >> 8);
                entry[15] = (byte)(value & 0xFF);
            }
        }

        public byte[] Bytes
        {
            get { return entry; }
            set
            {
                entry = new byte[0x20];
                if (Bytes.Length < 0x20)
                    Array.Copy(value, 0, entry, 0, value.Length);
                else
                    Array.Copy(value, 0, entry, 0, 0x20);
            }
        }

        #endregion
    }
}
