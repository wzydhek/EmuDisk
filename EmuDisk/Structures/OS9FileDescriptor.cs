using System;

namespace EmuDisk
{
    public class OS9FileDescriptor
    {
        #region Private Properties

        byte[] desc;

        #endregion

        #region Constructors

        public OS9FileDescriptor()
        {
            desc = new byte[256];
        }

        public OS9FileDescriptor(byte[] buffer) : this()
        {
            if (buffer.Length < 256)
                Array.Copy(buffer, 0, desc, 0, buffer.Length);
            else
                Array.Copy(buffer, 0, desc, 0, 256);
        }

        #endregion

        #region Public Properties

        public int FD_ATT
        {
            get
            {
                return desc[0x00];
            }
            set
            {
                desc[0x00] = (byte)value;
            }
        }

        public int FD_OWN
        {
            get
            {
                return (desc[0x01] << 8) + desc[0x02];
            }
            set
            {
                desc[0x01] = (byte)((value >> 8) & 0xFF);
                desc[0x02] = (byte)(value & 0xFF);
            }
        }

        public DateTime FD_DAT
        {
            get
            {
                int year = desc[0x03];
                int month = desc[0x04];
                int day = desc[0x05];
                int hour = desc[0x06];
                int minute = desc[0x07];

                if (year == 0 && month == 0 && day == 0)
                {
                    year = 70;
                    month = 1;
                    day = 1;
                }
                else if (year == 0 || month == 0 || day == 0)
                {
                    if (year == 0)
                        year = 00;
                    if (month == 0)
                        month = 1;
                    if (day == 0)
                        day = 1;
                }
                if (year < 70)
                    year += 2000;
                else
                    year += 1900;

                return new DateTime(year, month, day, hour, minute, 0);
            }

            set
            {
                if (value.Year > 1999)
                    desc[0x03] = (byte)(value.Year - 2000);
                else
                    desc[0x03] = (byte)(value.Year - 1900);

                desc[0x04] = (byte)value.Month;
                desc[0x05] = (byte)value.Day;
                desc[0x06] = (byte)value.Hour;
                desc[0x07] = (byte)value.Minute;
            }
        }

        public int FD_LNK
        {
            get
            {
                return desc[0x08];
            }

            set
            {
                desc[0x08] = (byte)value;
            }
        }

        public int FD_SIZ
        {
            get
            {
                return (desc[0x09] << 24) + (desc[0x0A] << 16) + (desc[0x0B] << 8) + desc[0x0C];
            }
            set
            {
                desc[0x09] = (byte)(value >> 24);
                desc[0x0A] = (byte)(value >> 16);
                desc[0x0B] = (byte)(value >> 8);
                desc[0x0C] = (byte)value;
            }
        }

        public DateTime FD_CREAT
        {
            get
            {
                int year = desc[0x0D];
                int month = desc[0x0E];
                int day = desc[0x0F];

                if (year == 0 && month == 0 && day == 0)
                {
                    year = 70;
                    month = 1;
                    day = 1;
                }
                else if (year == 0 || month == 0 || day == 0)
                {
                    if (year == 0)
                        year = 00;
                    if (month == 0)
                        month = 1;
                    if (day == 0)
                        day = 1;
                }
                if (year < 70)
                    year += 2000;
                else
                    year += 1900;

                return new DateTime(year, month, day, 0, 0, 0);
            }

            set
            {
                if (value.Year > 1999)
                    desc[0x0D] = (byte)(value.Year - 2000);
                else
                    desc[0x0D] = (byte)(value.Year - 1900);

                desc[0x0E] = (byte)value.Month;
                desc[0x0F] = (byte)value.Day;
            }
        }

        public OS9FileSegment[] FD_SEG
        {
            get 
            {
                OS9FileSegment[] segs = new OS9FileSegment[48];
                for (int i = 0; i<48; i++)
                {
                    segs[i] = new OS9FileSegment(desc.Subset(0x10 + (i * 5), 5));
                }
                return segs;
            }
            set
            {
                for (int i = 0; i < 240; i++)
                    desc[0X10 + i] = 0x00;

                for (int i=0; i<value.Length; i++)
                {
                    Array.Copy(value[i].Bytes, 0, desc, 0x10 + (i * 5), 5);
                }
            }
        }
        
        #endregion
    }
}
