using System;

namespace EmuDisk
{
    public class DragonDirectoryEntry
    {
        #region Private Properties

        byte[] entry;

        #endregion

        #region Constructors

        public DragonDirectoryEntry()
        {
            entry = new byte[0x19];
        }

        public DragonDirectoryEntry(byte[] buffer) : this()
        {
            if (buffer.Length < 0x19)
            {
                Array.Copy(buffer, 0, entry, 0, buffer.Length);
            }
            else
                Array.Copy(buffer, 0, entry, 0, 0x19);
        }

        #endregion

        #region Public Properties

        public int FlagByte
        {
            get { return entry[0]; }
            set { entry[0] = (byte)value; }
        }

        public bool Deleted
        {
            get { return (entry[0] & 0x80) > 0; }
            set { if (value) entry[0] |= 0x80; else entry[0] &= 0x7F; }
        }

        public bool Continued
        {
            get { return (entry[0] & 0x20) > 0; }
            set { if (value) entry[0] |= 0x20; else entry[0] &= 0xDF; }
        }

        public bool EoD
        {
            get { return (entry[0] & 0x08) > 0; }
            set { if (value) entry[0] |= 0x08; else entry[0] &= 0xF7; }
        }

        public bool Protected
        {
            get { return (entry[0] & 0x02) > 0; }
            set { if (value) entry[0] |= 0x02; else entry[0] &= 0xFD; }
        }

        public bool Continuation
        {
            get { return (entry[0] & 0x01) > 0; }
            set { if (value) entry[0] |= 0x01; else entry[0] &= 0xFE; }
        }

        public DragonFileHeaderBlock FileHeaderBlock
        {
            get
            {
                if (!Continuation)
                    return null;
                return new DragonFileHeaderBlock(entry.Subset(1, 0x17));
            }
            set
            {
                Continuation = false;
                Array.Copy(value.Bytes, 0, entry, 1, 0x17);
            }
        }

        public DragonContinuationBlock ContinuationBlock
        {
            get
            {
                if (Continuation)
                    return null;
                return new DragonContinuationBlock(entry.Subset(1, 0x17));
            }
            set
            {
                Continuation = true;
                Array.Copy(value.Bytes, 0, entry, 1, 0x17);
            }
        }

        public int Next
        {
            get { return entry[0x18]; }
            set { entry[0x18] = (byte)(value); }
        }

        public byte[] Bytes
        {
            get { return entry; }
            set
            {
                entry = new byte[0x19];
                if (Bytes.Length < 0x19)
                    Array.Copy(value, 0, entry, 0, value.Length);
                else
                    Array.Copy(value, 0, entry, 0, 0x19);
            }
        }

        #endregion
    }
}
