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
            block = new byte[5];
        }

        public OS9FileSegment(int lsn, int sectors) : this()
        {
            block[0] = (byte)(lsn >> 16);
            block[1] = (byte)(lsn >> 8);
            block[2] = (byte)lsn;
            block[4] = (byte)(sectors >> 8);
            block[5] = (byte)sectors;
        }

        public OS9FileSegment(byte[] buffer): this()
        {
            if (buffer.Length < 5)
            {
                block = new byte[5];
                Array.Copy(buffer, 0, block, 0, buffer.Length);
            }
            else
                Array.Copy(buffer, 0, block, 0, 5);
        }

        #endregion

        #region Public Properties

        public int LSN
        {
            get { return (block[0] << 16) + (block[1] << 8) + block[2]; }
            set { block[0] = (byte)(value >> 16); block[1] = (byte)(value >> 8); block[2] = (byte)value; }
        }

        public int Sectors
        {
            get { return (block[3] << 8) + block[4]; }
            set { block[3] = (byte)(value >> 8); block[4] = (byte)value; }
        }

        public byte[] Bytes
        {
            get { return block; }
            set
            {
                if (value.Length < 5)
                {
                    block = new byte[5];
                    Array.Copy(value, 0, block, 0, value.Length);
                }
                else
                    Array.Copy(value, 0, block, 0, 5);
            }
        }

        #endregion
    }
}
