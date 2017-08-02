using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace EmuDisk
{
    /* -----------------------------------------------------------------------
    * DMK file format
    *
    * David M. Keil's disk image format is aptly called an 'on disk' image
    * format. This means that whatever written to the disk is enocded into
    * the image file. IDAMS, sector headers, traling CRCs, and intra sector
    * spacing.                                                         *
    *
    *  HEADER DESCRIPTION:
    *
    *  Offset  Bytes   Field           Description
    *  ------  -----   ------------    -----------
    *       0      1   write_prot      0xff = Writed Protected, 0x00 = R/W
    *       1      1   tracks          Number of tracks
    *       2      2   track_length    Bytes per track (little endian)
    *       4      1   disk_options    Miscellaneous flags
    *                                      bit 0-3:  Unused
    *                                      bit   4:  1=single sided 0=dbl
    *                                      bit   5:  Unused
    *                                      bit   6:  Single density?
    *                                      bit   6:  Ignore density flags?
    *       5      7   reserved        Reserved for future use
    *      12      4   real_disk_code  If this is 0x12345678 (little endian)
    *                                  then access a real disk drive
    *                                  (unsupported)
    *
    * Each track begins with a track TOC, consisting of 64 little endian 16-bit
    * integers.  Each integer has the following format:
    *      bit 0-13:   Offset from beginning of track to 'FE' byte of IDAM
    *                  Note these are always sorted from first to last. All empty
    *                  entries are 0x00
    *      bit 14:     Undefined (reserved)
    *      bit 15:     Sector double density (0=SD 1=DD)
    *      
    * Track Layout:
    *      128 bytes   IDAM table (max 64 2-byte offsets to FEh byte of IDAM - LSB/MSB)
    *                  Bit 15 set if double density
    *                  Bit 14 undefined
    *       32 bytes   Gap IV (0x4E)
    *       
    * Sector Layout:
    *      338 bytes   Sector Information
    *      
    *        8 bytes   Sync Field (0x00)
    *        3 bytes   (0xA1)
    *        1 byte    ID Address Mark (0xFE)
    *    
    *    Sector ID
    *        1 byte    Track
    *        1 byte    Head
    *        1 byte    Sector
    *        1 byte    Sector Size
    *        2 bytes   Sector ID CRC16 (0xCDB4)
    *        
    *       22 bytes   Gap II (0x4E)
    *       
    *       12 bytes   Sync Field (0x00)
    *        3 bytes   (0xA1)
    *        1 byte    ID Address Mark (0xFB)
    *        
    *        x bytes   Sector Data
    *        
    *        2 bytes   Sector Data CRC16 (0xE295)
    *        
    *       24 bytes   Gap III (0x4E)
    *        
    * End of Track:
    *      156 bytes   (0x4E)
    * ----------------------------------------------------------------------- */

    /// <summary>
    /// DMK Disk Image support
    /// </summary>
    internal class DMKImage : DiskImageBase, IDiskImage
    {
        #region Private Properties

        private const int DMK_HEADER_LEN = 16;
        private const int DMK_TOK_LEN = 64;
        private const int DMK_IDAM_LENGTH = 7;
        private const int DMK_DATA_GAP = 80;
        private const int DMK_LEAD_IN = 32;
        private const int DMK_EXTRA_TRACK_LENGTH = 156;
        private const int DMK_DEFAULT_TRACK_LENGTH = 0x1900;

        private byte[] header;
        private int trackLength;
        private int interleave = 4;

        #endregion

        #region Constructors

        public DMKImage(string filename) : base(filename)
        {
            this.GetDiskInfo();
        }

        public DMKImage(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
        {
            this.CreateDisk(filename, tracks, heads, sectors, sectorsize, filldata);
            this.GetDiskInfo();
        }

        #endregion

        #region Public Properties

        public override DiskImageTypes ImageType
        {
            get
            {
                return DiskImageTypes.DMKImage;
            }
        }

        #endregion

        #region Public Methods

        public override byte[] ReadSector(int track, int head, int sector)
        {
            byte[] buffer = null;
            int sectorLength = 0;
            int offset = GetSectorOffset(track, head, sector, ref sectorLength);

            if (offset != 0 && sectorLength != 0)
                buffer = this.ReadBytes(offset, sectorLength);

            return buffer;
        }

        public override void WriteSector(int track, int head, int sector, byte[] buffer)
        {
            int sectorLength = 0;
            int offset = GetSectorOffset(track, head, sector, ref sectorLength);
            if (buffer.Length != sectorLength)
                throw new IOException();

            if (offset != 0 && sectorLength != 0)
            {
                byte[] crc = Crc16.ComputeChecksumBytes(0xE295, buffer);
                WriteBytes(offset, buffer);
                WriteByte(offset + buffer.Length, crc[1]);
                WriteByte(offset + buffer.Length + 1, crc[0]);
            }
        }

        public override void CreateDisk(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
        {
            if (this.baseStream != null)
            {
                this.baseStream.Close();
                this.baseStream = null;
            }

            this.filename = filename;

            try
            {
                this.baseStream = File.Open(this.filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                physicalTracks = tracks;
                physicalHeads = heads;
                physicalSectors = sectors;
                physicalSectorSize = sectorsize;
                trackLength = DMK_DEFAULT_TRACK_LENGTH;
                
                byte[] sectorData = new byte[this.PhysicalSectorSize].Initialize(filldata);
                byte[] sectorCRC = Crc16.ComputeChecksumBytes(0xE295, sectorData);

                byte[] header = new byte[] { 0, (byte)this.PhysicalTracks, (byte)(this.trackLength & 0xff), (byte)(this.trackLength >> 8), (byte)(((this.PhysicalHeads > 1) ? 0 : 1) << 4), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                headerLength = header.Length;

                this.baseStream.Write(header, 0, HeaderLength);

                for (int t = 0; t < PhysicalTracks; t++)
                {
                    for (int h = 0; h < PhysicalHeads; h++)
                    {
                        byte[] trackData = new byte[trackLength].Initialize(0x4E);
                        for (int i = 0; i < 128; i++)
                            trackData[i] = 0;
                        int[] sPointers = new int[PhysicalSectors];

                        for (int s = 0, i = 0x80AB; s < PhysicalSectors; s++, i += 338)
                        {
                            sPointers[s] = i;
                            trackData[2 * s] = (byte)(i & 0xff);
                            trackData[2 * s + 1] = (byte)(i >> 8);
                        }

                        int SectorID = 1;

                        for (int s = 0; s < PhysicalSectors; s++)
                        {
                            int sectorOffset = (sPointers[SectorID - 1] & 0x3fff) - 11;
                            for (int i = 0; i < 8; i++)
                                trackData[sectorOffset++] = 0;
                            for (int i = 0; i < 3; i++)
                                trackData[sectorOffset++] = 0xA1;

                            byte[] sectorControl = new byte[] {0xFE, (byte)t, (byte)h, (byte)(s + 1), (byte)((PhysicalSectorSize / 128) >> 1) };
                            byte[] sectorControlCRC = Crc16.ComputeChecksumBytes(0xCDB4, sectorControl);
                            Array.Copy(sectorControl, 0, trackData, sectorOffset, 5);
                            sectorOffset += 5;
                            trackData[sectorOffset++] = sectorControlCRC[1];
                            trackData[sectorOffset++] = sectorControlCRC[0];

                            sectorOffset += 22;
                            for (int i = 0; i < 12; i++)
                                trackData[sectorOffset++] = 0;
                            for (int i = 0; i < 3; i++)
                                trackData[sectorOffset++] = 0xA1;
                            trackData[sectorOffset++] = 0xFB;

                            Array.Copy(sectorData, 0, trackData, sectorOffset, PhysicalSectorSize);
                            sectorOffset += PhysicalSectorSize;
                            trackData[sectorOffset++] = sectorCRC[1];
                            trackData[sectorOffset++] = sectorCRC[0];

                            if (s < PhysicalSectors - 1)
                            {
                                SectorID++;
                                SectorID += interleave;
                                if (SectorID > PhysicalSectors)
                                    SectorID -= PhysicalSectors;

                                while (trackData[sPointers[SectorID - 1] & 0x3FFF] == 0xFE)
                                    SectorID++;

                                if (SectorID > PhysicalSectors)
                                    throw new IOException();
                            }
                        }

                        this.baseStream.Write(trackData, 0, trackData.Length);
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show(string.Format(MainForm.ResourceManager.GetString("DiskImageBase_FileOpenError", MainForm.CultureInfo), this.filename), MainForm.ResourceManager.GetString("DiskImageBase_FileOpenErrorCaption", MainForm.CultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.filename = string.Empty;
            }
        }

        public override string ToString()
        {
            return "DMK";
        }

        #endregion

        #region Private Methods

        private void GetDiskInfo()
        {
            this.physicalTracks = 35;
            this.physicalHeads = 1;
            this.physicalSectors = 18;
            this.physicalSectorSize = 256;

            this.headerLength = 16;

            this.Seek(0);
            header = this.ReadBytes(0, 16);


            this.physicalTracks = header[1];
            this.physicalHeads = ((header[4] >> 4) == 0) ? 2 : 1;
            this.trackLength = (header[3] << 8) + header[2];

            if (this.Length != this.HeaderLength + (this.PhysicalTracks * this.PhysicalHeads * this.trackLength))
                goto NotValid;

            this.isValidImage = true;
            return;

        NotValid:
            this.isValidImage = false;
            this.Close();

        }

        private int GetSectorOffset(int track, int head, int sector, ref int sectorsize)
        {
            int offset = 0;
            sectorsize = 0;

            int trackoffset = this.HeaderLength + (track * this.PhysicalHeads * this.trackLength) + (head * this.trackLength);
            int idamOffset = 0;
            int i = 0;
            byte[] idam = new byte[0];

            // Search for sector
            for (i = 0; i < DMK_TOK_LEN; i++)
            {
                idamOffset = ((this.ReadByte(trackoffset + (i * 2) + 1) << 8) + this.ReadByte(trackoffset + (i * 2))) & 0x3fff;
                if (idamOffset == 0)
                {
                    i = DMK_TOK_LEN;
                    break;
                }

                if ((idamOffset + DMK_IDAM_LENGTH) > trackLength)
                    continue;

                int idamCRC = (this.ReadByte(trackoffset + idamOffset + 5) << 8) + this.ReadByte(trackoffset + idamOffset + 6);
                idam = this.ReadBytes(trackoffset + idamOffset, DMK_IDAM_LENGTH - 2);
                int calcCRC = Crc16.ComputeChecksum(0xCDB4, idam);
                if (idamCRC == calcCRC)
                {
                    if (sector == idam[3] && track == idam[1] && head == idam[2])
                        break;
                }
            }

            if (i >= DMK_TOK_LEN)
                throw new SectorNotFoundException();

            int state = 0;
            int offs = idamOffset + DMK_IDAM_LENGTH;

            for (i = 0; i < DMK_DATA_GAP; i++)
            {
                if ((i + offs) > trackLength)
                    throw new SectorNotFoundException();

                if (this.ReadByte(trackoffset + offs + i) == 0xA1)
                    state++;
                else if ((this.ReadByte(trackoffset + offs + i) == 0xFB) && state > 0)
                    break;
                else
                    state = 0;
            }

            if (i >= DMK_DATA_GAP)
                throw new SectorNotFoundException();

            offs += i + 1;
            int sec_len = 128 << idam[4];

            if ((offs + sec_len) > trackLength)
                throw new SectorNotFoundException();

            offset = trackoffset + offs;
            sectorsize = sec_len;
            return offset;
        }
        
        #endregion
    }
}
