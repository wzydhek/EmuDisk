using System;
using System.Text;

namespace EmuDisk
{
    public class OS9DirectoryEntry
    {
        #region Private Properties

        byte[] entry;

        #endregion

        #region Constructors

        public OS9DirectoryEntry()
        {
            entry = new byte[0x20];
        }

        public OS9DirectoryEntry(string filename, int lsn) : this()
        {
            if (filename.Length > 28)
                filename = filename.Substring(0, 28);
            byte[] namebytes = Encoding.ASCII.GetBytes(filename);
            Array.Copy(namebytes, 0, entry, 0, namebytes.Length);

            entry[0x1F] = (byte)(lsn & 0xFF);
            entry[0x1E] = (byte)((lsn >> 8) & 0xFF);
            entry[0x1D] = (byte)((lsn >> 16) & 0xFF);
        }

        public OS9DirectoryEntry(byte[] buffer) : this()
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

        public String Filename
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 28; i++)
                {
                    if (entry[i] == 0x00)
                        break;
                    sb.Append(Encoding.ASCII.GetString(new byte[] { entry[i] }));
                }
                string name = sb.ToString();
                if (string.IsNullOrEmpty(name))
                    return null;
                return name;
            }
            set
            {
                if (value.Length > 28)
                    value = value.Substring(0, 28);
                byte[] namebytes = Encoding.ASCII.GetBytes(value);
                Array.Copy(namebytes, 0, entry, 0, namebytes.Length);
            }
        }

        public int LSN
        {
            get
            {
                return ((entry[0x1D] << 16) + (entry[0x1E] << 8) + entry[0x1F]);
            }
            set
            {
                entry[0x1F] = (byte)(value & 0xFF);
                entry[0x1E] = (byte)((value >> 8) & 0xFF);
                entry[0x1D] = (byte)((value >> 16) & 0xFF);
            }
        }

        public byte[] Bytes
        {
            get { return entry; }
            set
            {
                if (value.Length < 0x20)
                {
                    entry = new byte[0x20];
                    Array.Copy(value, 0, entry, 0, value.Length);
                }
                else
                    Array.Copy(value, 0, entry, 0, 0x20);
            }
        }

        #endregion
    }
}
