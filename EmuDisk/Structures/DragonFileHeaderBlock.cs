using System;
using System.Text;

namespace EmuDisk
{
    public class DragonFileHeaderBlock
    {
        #region Private Properties

        byte[] block;

        #endregion

        #region Constructors

        public DragonFileHeaderBlock()
        {
            block = new byte[0x17];
        }

        public DragonFileHeaderBlock(byte[] buffer) : this()
        {
            if (buffer.Length < 0x17)
            {
                Array.Copy(buffer, 0, block, 0, buffer.Length);
            }
            else
                Array.Copy(buffer, 0, block, 0, 0x17);
        }

        #endregion

        #region Public Properties

        public string Filename
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i=1; i<9; i++)
                {
                    if (block[i] == 0)
                        break;
                    sb.Append(Encoding.ASCII.GetString(new byte[] { block[i] }));
                }

                string filename = sb.ToString();
                if (string.IsNullOrEmpty(filename))
                    return null;

                return filename;
            }
            set
            {
                if (value.Length > 8)
                    value = value.Substring(0, 8);

                byte[] filename = new byte[8];
                byte[] bytes = Encoding.ASCII.GetBytes(value);
                Array.Copy(bytes, 0, filename, 0, bytes.Length);
                Array.Copy(filename, 0, block, 1, 8);
            }
        }

        public string Extension
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 9; i < 0x0C; i++)
                {
                    if (block[i] == 0)
                        break;
                    sb.Append(Encoding.ASCII.GetString(new byte[] { block[i] }));
                }

                string extension = sb.ToString();
                if (string.IsNullOrEmpty(extension))
                    return null;

                return extension;
            }
            set
            {
                if (value.Length > 3)
                    value = value.Substring(0, 3);

                byte[] filename = new byte[3];
                byte[] bytes = Encoding.ASCII.GetBytes(value);
                Array.Copy(bytes, 0, filename, 0, bytes.Length);
                Array.Copy(filename, 0, block, 1, 3);
            }
        }

        public DragonSectorAllocationBlock SectorAllocationBlock(int index)
        {
            if (index > 3)
                return null;
            return new DragonSectorAllocationBlock(block.Subset(0x0C + (index * 3), 3));
        }

        public byte[] Bytes
        {
            get { return block; }
            set
            {
                block = new byte[0x17];
                if (Bytes.Length < 0x17)
                    Array.Copy(value, 0, block, 0, value.Length);
                else
                    Array.Copy(value, 0, block, 0, 0x17);
            }
        }

        #endregion
    }
}
