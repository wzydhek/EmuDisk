using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    public class LSN0
    {
        #region Private Fields

        private byte[] lsn0;

        #endregion

        #region Constructors

        public LSN0()
        {
            lsn0 = new byte[256];
        }

        public LSN0(byte[] buffer)
        {
            lsn0 = buffer;

            //this.TotalSectors = Util.Int24(buffer.Subset(0x00, 3));
            //this.TrackSize = buffer[0x03];
            //this.MapBytes = Util.UInt16(buffer.Subset(0x04, 2));
            //this.ClusterSize = Util.UInt16(buffer.Subset(0x06, 2));
            //this.RootDirectory = Util.Int24(buffer.Subset(0x08, 3));
            //this.Owner = Util.UInt16(buffer.Subset(0x0b, 2));
            //this.Attributes = buffer[0x0d];
            //this.DiskID = Util.UInt16(buffer.Subset(0x0e, 2));
            //this.DiskFormat = buffer[0x10];
            //this.SectorsPerTrack = Util.UInt16(buffer.Subset(0x11, 2));
            //this.Reserved = Util.UInt16(buffer.Subset(0x13, 2));
            //this.BootStrap = Util.Int24(buffer.Subset(0x15, 3));
            //this.BootStrapSize = Util.UInt16(buffer.Subset(0x18, 2));
            //this.PackedCreationDate = buffer.Subset(0x1a, 5);
            //this.VolumeNameBytes = buffer.Subset(0x1f, 32);
            //this.PathDescriptor = new PathDescriptor(buffer.Subset(0x3f, 19));
            //this.SectorSize = buffer[0x68];
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Number of sectors on disk
        /// DD.TOT - offset $00-$02
        /// </summary>
        public int TotalSectors
        {
            get { return (lsn0[0x00] << 16) + (lsn0[0x01] << 8) + lsn0[0x02]; }
            set {
                lsn0[0x02] = (byte)(value & 0xff);
                lsn0[0x01] = (byte)((value >> 8) & 0xff);
                lsn0[0x00] = (byte)(value >> 16);
            }
        }

        /// <summary>
        /// Gets or sets Track size (in sectors)
        /// DD.TKS - offset $03
        /// </summary>
        public int TrackSize
        {
            get { return lsn0[0x03]; }
            set { lsn0[0x03] = (byte)value; SectorsPerTrack = value; }
        }

        /// <summary>
        /// Gets or sets Number of bytes in the allocation bit map
        /// DD.MAP - offset $04-$05
        /// </summary>
        public int MapBytes
        {
            get { return (lsn0[0x04] << 8) + lsn0[0x05]; }
            set
            {
                lsn0[0x05] = (byte)(value & 0xff);
                lsn0[0x04] = (byte)((value >> 8) & 0xff);
            }
        }
        /// <summary>
        /// Gets or sets Number of sectors per cluster
        /// DD.BIT - offset $06-$07
        /// </summary>
        public int ClusterSize
        {
            get { return (lsn0[0x06] << 8) + lsn0[0x07]; }
            set
            {
                lsn0[0x07] = (byte)(value & 0xff);
                lsn0[0x06] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Gets or sets Starting sector of the root directory
        /// DD.DIR - offset $08-$0A
        /// </summary>
        public int RootDirectory
        {
            get { return (lsn0[0x08] << 16) + (lsn0[0x09] << 8) + lsn0[0x0A]; }
            set
            {
                lsn0[0x0A] = (byte)(value & 0xff);
                lsn0[0x09] = (byte)((value >> 8) & 0xff);
                lsn0[0x08] = (byte)(value >> 16);
            }
        }

        /// <summary>
        /// Gets or sets Owner’s user number
        /// DD.OWN - offset $0B-$0C
        /// </summary>
        public int Owner
        {
            get { return (lsn0[0x0B] << 8) + lsn0[0x0C]; }
            set
            {
                lsn0[0x0C] = (byte)(value & 0xff);
                lsn0[0x0B] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Gets or sets Disk attributes
        /// DD.ATT - offset $0D
        /// </summary>
        public int Attributes
        {
            get { return lsn0[0x0D]; }
            set { lsn0[0x0D] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets Disk identification (for internal use)
        /// DD.DSK - offset $0E-$0F
        /// </summary>
        public int DiskID
        {
            get { return (lsn0[0x0E] << 8) + lsn0[0x0F]; }
            set
            {
                lsn0[0x0F] = (byte)(value & 0xff);
                lsn0[0x0E] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Gets or sets Disk format, density, number of sides
        /// DD.FMT - offset $10
        /// </summary>
        public int DiskFormat
        {
            get { return lsn0[0x10]; }
            set { lsn0[0x10] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets Number of sectors per track
        /// DD.SPT - offset $11-$12
        /// </summary>
        public int SectorsPerTrack
        {
            get { return (lsn0[0x11] << 8) + lsn0[0x12]; }
            set
            {
                lsn0[0x12] = (byte)(value & 0xff);
                lsn0[0x11] = (byte)((value >> 8) & 0xff);
                TrackSize = value;
            }
        }

        /// <summary>
        /// Gets or sets Reserved for future use
        /// DD.RES - offset $13-$14
        /// </summary>
        public int Reserved
        {
            get { return (lsn0[0x13] << 8) + lsn0[0x14]; }
            set
            {
                lsn0[0x14] = (byte)(value & 0xff);
                lsn0[0x13] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Gets or sets Starting sector of the bootstrap file
        /// DD.BT  - offset $15-$17
        /// </summary>
        public int BootStrap
        {
            get { return (lsn0[0x15] << 16) + (lsn0[0x16] << 8) + lsn0[0x17]; }
            set
            {
                lsn0[0x17] = (byte)(value & 0xff);
                lsn0[0x16] = (byte)((value >> 8) & 0xff);
                lsn0[0x15] = (byte)(value >> 16);
            }
        }

        /// <summary>
        /// Gets or sets Size of the bootstrap file (in bytes)
        /// DD.BSZ - offset $18-$19
        /// </summary>
        public int BootStrapSize
        {
            get { return (lsn0[0x18] << 8) + lsn0[0x19]; }
            set
            {
                lsn0[0x19] = (byte)(value & 0xff);
                lsn0[0x18] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Gets or sets Time of creation (Y:M:D:H:M)
        /// offset $1A-$1E
        /// </summary>
        public DateTime CreatedDate
        {
            get
            {
                int year = lsn0[0x1A];
                int month = lsn0[0x1B];
                int day = lsn0[0x1C];
                int hour = lsn0[0x1D];
                int minute = lsn0[0x1E];

                if (year == 0 && month == 0 && day == 0)
                {
                    year = 1970;
                    month = 1;
                    day = 1;
                }
                else if (year == 0 || month == 0 || day == 0)
                {
                    if (year == 0)
                        year = 2000;
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
                    lsn0[0x1A] = (byte)(value.Year - 2000);
                else
                    lsn0[0x1A] = (byte)(value.Year - 1900);

                lsn0[0x1B] = (byte)value.Month;
                lsn0[0x1C] = (byte)value.Day;
                lsn0[0x1D] = (byte)value.Hour;
                lsn0[0x1E] = (byte)value.Minute;
            }
        }

        /// <summary>
        /// Gets or sets Volume name in which the last character has the most significant bit set
        /// DD.NAM - offset $1F-$3E
        /// </summary>
        public string VolumeName
        {
            get
            {
                byte[] name = new byte[0x20];
                Array.Copy(lsn0, 0x1F, name, 0, 0x20);
                return Util.GetHighBitString(name);
            }

            set
            {
                byte[] name = new byte[0x20];
                if (value.Length > 32)
                {
                    value = value.Substring(0, 32);
                }

                byte[] namebytes = Encoding.ASCII.GetBytes(value);
                namebytes[namebytes.Length - 1] |= 0x80;
                Array.Copy(namebytes, 0, name, 0, namebytes.Length);
                Array.Copy(name, 0, lsn0, 0x1F, 0x20);
            }
        }

        /// <summary>
        /// Gets or sets Path Descriptor
        /// DD.OPT bytes $3F-$5E
        /// </summary>
        public Byte[] PathDescriptor
        {
            get
            {
                byte[] pd = new byte[0x20];
                Array.Copy(lsn0, 0x3F, pd, 0, 0x20);
                return pd;
            }
            set
            {
                byte[] pd = new byte[0x20];
                int l = value.Length;
                if (l > 0x20)
                    l = 0x20;
                Array.Copy(value, 0, pd, 0, l);
                Array.Copy(pd, 0, lsn0, 0x3F, 0x20);
            }
        }

        /// <summary>
        /// Reserved
        ///   - offset $5F
        /// </summary>
        public int Reserved2
        {
            get { return lsn0[0x10]; }
            set { lsn0[0x10] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets Media Integrity Code
        /// DD.SYNC  - offset $60-$63
        /// </summary>
        public int MediaIntegrity
        {
            get { return (lsn0[0x60] << 24) + (lsn0[0x61] << 16) + (lsn0[0x62] << 8) + lsn0[0x63]; }
            set
            {
                lsn0[0x63] = (byte)(value & 0xff);
                lsn0[0x62] = (byte)((value >> 8) & 0xff);
                lsn0[0x61] = (byte)((value >> 16) & 0xff);
                lsn0[0x60] = (byte)(value >> 24);
            }
        }

        /// <summary>
        /// Gets or sets Bitmap starting sector number
        /// 0 = LSN 1
        /// DD.MapLSN  - offset $64-$67
        /// </summary>
        public int MapLSN
        {
            get { return (lsn0[0x64] << 24) + (lsn0[0x65] << 16) + (lsn0[0x66] << 8) + lsn0[0x67]; }
            set
            {
                lsn0[0x67] = (byte)(value & 0xff);
                lsn0[0x66] = (byte)((value >> 8) & 0xff);
                lsn0[0x65] = (byte)((value >> 16) & 0xff);
                lsn0[0x64] = (byte)(value >> 24);
            }
        }

        /// <summary>
        /// Gets or sets the Logical Sector Size
        /// DD.LSNSize - offset $68-$69
        /// </summary>
        public int SectorSize
        {
            get { return (lsn0[0x68] << 8) + lsn0[0x69]; }
            set
            {
                lsn0[0x69] = (byte)(value & 0xff);
                lsn0[0x68] = (byte)((value >> 8) & 0xff);
            }
        }

        public byte[] Bytes
        {
            get { return lsn0; }
            set { lsn0 = value; }
        }

        #endregion
    }
}
