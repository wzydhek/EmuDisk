using System;
using System.Text;

namespace EmuDisk
{
    public class LSN0
    {
        /*
            DD.TOT         RMB       3                   Total number of sectors
            DD.TKS         RMB       1                   Track size in sectors
            DD.MAP         RMB       2                   Number of bytes in allocation bit map
            DD.BIT         RMB       2                   Number of sectors/bit
            DD.DIR         RMB       3                   Address of root directory fd
            DD.OWN         RMB       2                   Owner
            DD.ATT         RMB       1                   Attributes
            DD.DSK         RMB       2                   Disk ID
            DD.FMT         RMB       1                   Disk format; density/sides
            DD.SPT         RMB       2                   Sectors/track
            DD.RES         RMB       2                   Reserved for future use
            DD.SIZ         EQU       .                   Device descriptor minimum size
            DD.BT          RMB       3                   System bootstrap sector
            DD.BSZ         RMB       2                   Size of system bootstrap
            DD.DAT         RMB       5                   Creation date
            DD.NAM         RMB       32                  Volume name
            DD.OPT         RMB       32                  Option area
        */

        /*
                           RMB       1                   Device type
            PD.DRV         RMB       1                   Drive number
            PD.STP         RMB       1                   Step rate
            PD.TYP         RMB       1                   Disk device type (5" 8" other)
            PD.DNS         RMB       1                   Density capability
            PD.CYL         RMB       2                   Number of cylinders
            PD.SID         RMB       1                   Number of surfaces
            PD.VFY         RMB       1                   0=verify disk writes
            PD.SCT         RMB       2                   Default sectors/track
            PD.T0S         RMB       2                   Default sectors/track tr00,s0
            PD.ILV         RMB       1                   Sector interleave offset
            PD.SAS         RMB       1                   Segment allocation size
            PD.TFM         RMB       1                   DMA Transfer Mode
            PD.Exten       RMB       2                   Path Extension (PE) for record locking
            PD.SToff       RMB       1                   Sector/Track offsets (for "foreign" disk formats)
            PD.ATT         RMB       1                   File attributes
            PD.FD          RMB       3                   File descriptor psn
            PD.DFD         RMB       3                   Directory file descriptor psn
            PD.DCP         RMB       4                   File directory entry ptr
            PD.DVT         RMB       2                   User readable dev tbl ptr
        */
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
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Number of sectors on disk
        /// DD.TOT - offset $00-$02
        /// </summary>
        public int DD_TOT
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
        public int DD_TKS
        {
            get { return lsn0[0x03]; }
            set { lsn0[0x03] = (byte)value; DD_SPT = value; }
        }

        /// <summary>
        /// Gets or sets Number of bytes in the allocation bit map
        /// DD.MAP - offset $04-$05
        /// </summary>
        public int DD_MAP
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
        public int DD_BIT
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
        public int DD_DIR
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
        public int DD_OWN
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
        public int DD_ATT
        {
            get { return lsn0[0x0D]; }
            set { lsn0[0x0D] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets Disk identification (for internal use)
        /// DD.DSK - offset $0E-$0F
        /// </summary>
        public int DD_DSK
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
        public int DD_FMT
        {
            get { return lsn0[0x10]; }
            set { lsn0[0x10] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets Number of sectors per track
        /// DD.SPT - offset $11-$12
        /// </summary>
        public int DD_SPT
        {
            get { return (lsn0[0x11] << 8) + lsn0[0x12]; }
            set
            {
                lsn0[0x12] = (byte)(value & 0xff);
                lsn0[0x11] = (byte)((value >> 8) & 0xff);
                DD_TKS = value;
            }
        }

        /// <summary>
        /// Gets or sets Reserved for future use
        /// DD.RES - offset $13-$14
        /// </summary>
        public int DD_RES
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
        public int DD_BT
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
        public int DD_BSZ
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
        /// DD.DAT - offset $1A-$1E
        /// </summary>
        public DateTime DD_DAT
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
        public string DD_NAM
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
        public Byte[] DD_OPT
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
        /// Device Type
        /// PD.DTYP - offset $3F
        /// </summary>
        public int PD_DTYP
        {
            get { return lsn0[0x3F]; }
            set { lsn0[0x3F] = (byte)value; }
        }

        /// <summary>
        /// Drive Number
        /// PD.DRV - offset $40
        /// </summary>
        public int PD_DRV
        {
            get { return lsn0[0x40]; }
            set { lsn0[0x40] = (byte)value; }
        }

        /// <summary>
        /// Step Rate
        /// PD.STP - offset $41
        /// </summary>
        public int PD_STP
        {
            get { return lsn0[0x41]; }
            set { lsn0[0x41] = (byte)value; }
        }

        /// <summary>
        /// Disk device type (5" 8" other)
        /// PD.TYP - offset $42
        /// </summary>
        public int PD_TYP
        {
            get { return lsn0[0x42]; }
            set { lsn0[0x42] = (byte)value; }
        }

        /// <summary>
        /// Density Capability
        /// PD.DNS - offset $43
        /// </summary>
        public int PD_DNS
        {
            get { return lsn0[0x43]; }
            set { lsn0[0x43] = (byte)value; }
        }

        /// <summary>
        /// Number of Cylinders
        /// PD.CYL - offset $44-$45
        /// </summary>
        public int PD_CYL
        {
            get { return (lsn0[0x44] << 8) + lsn0[0x45]; }
            set
            {
                lsn0[0x45] = (byte)(value & 0xff);
                lsn0[0x44] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Number of Surfaces
        /// PD.SID - offset $46
        /// </summary>
        public int PD_SID
        {
            get { return lsn0[0x46]; }
            set { lsn0[0x46] = (byte)value; }
        }

        /// <summary>
        /// Verify Disk Writes (0 = verify on)
        /// PD.VFY - offset $47
        /// </summary>
        public int PD_VFY
        {
            get { return lsn0[0x47]; }
            set { lsn0[0x47] = (byte)value; }
        }

        /// <summary>
        /// Default Sectors/Track
        /// PD.SCT - offset $48-$49
        /// </summary>
        public int PD_SCT
        {
            get { return (lsn0[0x48] << 8) + lsn0[0x49]; }
            set
            {
                lsn0[0x49] = (byte)(value & 0xff);
                lsn0[0x48] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Default Sectors/Track LSN0
        /// PD.T0S - offset $4A-$4B
        /// </summary>
        public int PD_T0S
        {
            get { return (lsn0[0x4A] << 8) + lsn0[0x4B]; }
            set
            {
                lsn0[0x4B] = (byte)(value & 0xff);
                lsn0[0x4A] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Sector Interleave
        /// PD.ILV - offset $4C
        /// </summary>
        public int PD_ILV
        {
            get { return lsn0[0x4C]; }
            set { lsn0[0x4C] = (byte)value; }
        }

        /// <summary>
        /// Segment Allocation Size
        /// PD.SAS - offset $4D
        /// </summary>
        public int PD_SAS
        {
            get { return lsn0[0x4D]; }
            set { lsn0[0x4D] = (byte)value; }
        }

        /// <summary>
        /// DMA Transfer Mode
        /// PD.TFM - offset $4E
        /// </summary>
        public int PD_TFM
        {
            get { return lsn0[0x4E]; }
            set { lsn0[0x4E] = (byte)value; }
        }

        /// <summary>
        /// Path Extension (PE) for record locking
        /// PD.Exten - offset $4F-$50
        /// </summary>
        public int PD_Exten
        {
            get { return (lsn0[0x4F] << 8) + lsn0[0x50]; }
            set
            {
                lsn0[0x50] = (byte)(value & 0xff);
                lsn0[0x4F] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Sectors per Track offsets (for "foreign" disk formats)
        /// PD.SToff - offset $51
        /// </summary>
        public int PD_SToff
        {
            get { return lsn0[0x51]; }
            set { lsn0[0x51] = (byte)value; }
        }

        /// <summary>
        /// File Attributes
        /// PD.ATT - offset $52
        /// </summary>
        public int PD_ATT
        {
            get { return lsn0[0x52]; }
            set { lsn0[0x52] = (byte)value; }
        }

        /// <summary>
        /// File Descriptor PSN
        /// PD.FD - offset $53-$55
        /// </summary>
        public int PD_FD
        {
            get { return (lsn0[0x53] << 16) + (lsn0[0x54] << 8) + lsn0[0x55]; }
            set
            {
                lsn0[0x55] = (byte)(value & 0xff);
                lsn0[0x54] = (byte)((value >> 8) & 0xff);
                lsn0[0x53] = (byte)((value >> 16) & 0xff);
            }
        }

        /// <summary>
        /// Directory File Descriptor PSN
        /// PD.DFD - offset $56-$58
        /// </summary>
        public int PD_DFD
        {
            get { return (lsn0[0x56] << 16) + (lsn0[0x57] << 8) + lsn0[0x58]; }
            set
            {
                lsn0[0x58] = (byte)(value & 0xff);
                lsn0[0x57] = (byte)((value >> 8) & 0xff);
                lsn0[0x56] = (byte)((value >> 16) & 0xff);
            }
        }

        /// <summary>
        /// File Directory Entry ptr
        /// PD.DCP - offset $59-$5C
        /// </summary>
        public int PD_DCP
        {
            get { return (lsn0[0x59] << 24) + (lsn0[0x5A] << 16) + (lsn0[0x5B] << 8) + lsn0[0x5C]; }
            set
            {
                lsn0[0x5C] = (byte)(value & 0xff);
                lsn0[0x5B] = (byte)((value >> 8) & 0xff);
                lsn0[0x5A] = (byte)((value >> 16) & 0xff);
                lsn0[0x59] = (byte)((value >> 24) & 0xff);
            }
        }

        /// <summary>
        /// User readable drv tbl ptr
        /// PD.DVT - offset $5D-$5E
        /// </summary>
        public int PD_DVT
        {
            get { return (lsn0[0x5D] << 8) + lsn0[0x5E]; }
            set
            {
                lsn0[0x5E] = (byte)(value & 0xff);
                lsn0[0x5D] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Reserved
        /// DD.RES2  - offset $5F
        /// </summary>
        public int DD_RES2
        {
            get { return lsn0[0x10]; }
            set { lsn0[0x10] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets Media Integrity Code
        /// DD.SYNC  - offset $60-$63
        /// </summary>
        public int DD_SYNC
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
        public int DD_MapLSN
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
        public int DD_LSNSize
        {
            get { return (lsn0[0x68] << 8) + lsn0[0x69]; }
            set
            {
                lsn0[0x69] = (byte)(value & 0xff);
                lsn0[0x68] = (byte)((value >> 8) & 0xff);
            }
        }

        /// <summary>
        /// Gets or sets the Logical Sector Size
        /// DD.VersID - offset $6A-$6B
        /// </summary>
        public int DD_VersID
        {
            get { return (lsn0[0x6A] << 8) + lsn0[0x6B]; }
            set
            {
                lsn0[0x6B] = (byte)(value & 0xff);
                lsn0[0x6A] = (byte)((value >> 8) & 0xff);
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
