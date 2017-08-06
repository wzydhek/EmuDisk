using System;

namespace EmuDisk
{
    public class DragonContinuationBlock
    {
        #region Private Properties

        byte[] block;

        #endregion

        #region Constructors

        public DragonContinuationBlock()
        {
            block = new byte[0x17];
        }

        public DragonContinuationBlock(byte[] buffer) : this()
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

        public DragonSectorAllocationBlock SectorAllocationBlock(int index)
        {
            if (index > 6)
                return null;
            return new DragonSectorAllocationBlock(block.Subset(index * 3, 3));
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
