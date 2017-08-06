using System;

namespace EmuDisk
{
    public class OS9FileSegment
    {
        #region Private Properties

        byte[] block;

        #endregion

        #region Constructors

        public OS9FileSegment()
        {
            block = new byte[3];
        }

        public OS9FileSegment(int lsn, int sectors) : this()
        {
            block[0] = (byte)(lsn >> 8);
            block[1] = (byte)(lsn & 0xFF);
            block[2] = (byte)sectors;
        }

        public OS9FileSegment(byte[] buffer): this()
        {
            if (buffer.Length < 3)
            {
                block = new byte[3];
                Array.Copy(buffer, 0, block, 0, buffer.Length);
            }
            else
                Array.Copy(buffer, 0, block, 0, 3);
        }

        #endregion

        #region Public Properties

        public int LSN
        {
            get { return (block[0] << 8) + block[1]; }
            set { block[0] = (byte)(value >> 8); block[1] = (byte)(LSN & 0xFF); }
        }

        public int Sectors
        {
            get { return block[2]; }
            set { block[2] = (byte)value; }
        }

        public byte[] Bytes
        {
            get { return block; }
            set
            {
                if (value.Length < 3)
                {
                    block = new byte[3];
                    Array.Copy(value, 0, block, 0, value.Length);
                }
                else
                    Array.Copy(value, 0, block, 0, 3);
            }
        }

        #endregion
    }
}
