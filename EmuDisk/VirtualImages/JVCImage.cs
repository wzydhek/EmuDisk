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
 * JVC (Jeff Vavasour CoCo) format
 *
 * Used by Jeff Vavasour's CoCo Emulators
 *
 *  Documentation taken from Tim Linder's web site:
 *      http://home.netcom.com/~tlindner/JVC.html
 *
 *  A. Header length
 *      The header length is determined by the file length modulo 256:
 *          headerSize = fileLength % 256;
 *      This means that the header is variable length and the minimum size
 *      is zero bytes, and the maximum size of 255 bytes.
 *
 * B. Header
 *      Here is a description of the header bytes:
 *          Byte Offset     Description            Default
 *          -----------     -----------------      -------
 *                    0     Sectors per track      18
 *                    1     Side count              1
 *                    2     Sector size code        1
 *                    3     First sector ID         1
 *                    4     Sector attribute flag   0
 *
 *  If the sector attribute flag is zero then the track count is determined
 *  by the formula:
 *
 *      (fileLength - headerSize) / (sectorsPerTrack * (128 <<
 *          sectorSizeCode)) / sideCount
 *
 *  If the sector attribute flag is non zero then the track count is
 *  determined by the more complex formula:
 *
 *      (fileLength - headerSize) / (sectorsPerTrack * ((128 <<
 *          sectorSizeCode) + 1) ) / sideCount
 *
 *  If the length of the header is to short to contain the geometry desired,
 *  then the default values are assumed. If the header length is zero the all
 *  of the geometry is assumed. When creating disk images it is desirable to
 *  make the header length as short as possible. The header should only be
 *  used to deviate from the default values.
 *
 *  The sector data begins immediately after the header. If the header length
 *  is zero then the sector data is at the beginning file.
 *
 *  C. Sectors per track
 *      This is the number of sectors per track (ones based). A value of 18
 *      means there are 18 sectors per track
 *
 *  D. Side Count
 *      This is the number of sides in the disk image. Values of 1 or 2 are
 *      acceptable. If there are two sides then the tracks are interleaved.
 *      The first track in the image file is track zero side 1, the second
 *      track in the image file is track zero side 2.
 *
 *  E. Sector size
 *      The is the same value that is stored in the wd179x ID field to
 *      determine sector size:
 *
 *          0x00         128 bytes
 *          0x01         256 bytes
 *          0x02         512 bytes
 *          0x03        1024 bytes
 *
 *  Other values are undefined. Every sector in the disk image must be the
 *  same size.
 *
 *  F. First sector ID
 *      This determines the first sector ID for each track. Each successive
 *      sector adds one to the previous ID. If the first sector ID is 1, then
 *      the second sector has an ID of 2, and the third has an ID of 3.
 *
 *  G. Sector Attribute Flag
 *      If this byte is non zero, then each sector contains an additional
 *      byte prepended to the sector data. If the attribute flag is zero then
 *      there are no extra bytes in front of the sector data.
 *
 *  H. Sector attribute byte
 *      This byte is put at the beginning of every sector if the header flag
 *      is turned on. The information this byte contains is the same as the
 *      status register (of the wd179x) would contain when a 'Read Sector'
 *      command was issued. The bit fields are defined as:
 *
 *      Bit position:
 *      ---------------
 *      7 6 5 4 3 2 1 0
 *      | | | | | | | |
 *      | | | | | | | +--- Not used. Set to zero.
 *      | | | | | | +----- Not used. Set to zero.
 *      | | | | | +------- Not used. Set to zero.
 *      | | | | +--------- Set on CRC error.
 *      | | | +----------- Set if sector not found.
 *      | | +------------- Record type: 1 - Deleted Data Mark, 0 - Data Mark.
 *      | +--------------- Not Used. Set to zero.
 *      +----------------- Not Used. Set to zero.
 *
 * ----------------------------------------------------------------------- */

    /// <summary>
    /// JVC Disk Image support
    /// </summary>
    internal class JVCImage : DiskImageBase, IDiskImage
    {
        #region Private Properties

        private byte firstSectorID;
        private byte sectorAttributeFlag;

        #endregion

        #region Constructors

        public JVCImage(string filename) : base(filename)
        {
            this.GetDiskInfo();
        }

        public JVCImage(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
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
                return DiskImageTypes.JVCImage;
            }
        }

        public byte FirstSectorID
        {
            get { return firstSectorID; }
            set { firstSectorID = value; }
        }

        public byte SectorAttributeFlag
        {
            get { return sectorAttributeFlag; }
            set { sectorAttributeFlag = value; }
        }

        #endregion

        #region Public Methods

        public override byte[] ReadSector(int track, int head, int sector)
        {
            int offset = this.HeaderLength + (track * this.PhysicalHeads * this.PhysicalSectors * (this.PhysicalSectorSize + SectorAttributeFlag)) + (head * this.PhysicalSectors * (this.PhysicalSectorSize + SectorAttributeFlag)) + ((sector - 1) * (this.PhysicalSectorSize + SectorAttributeFlag));
            return this.ReadBytes(offset, this.PhysicalSectorSize);
        }

        public override void WriteSector(int track, int head, int sector, byte[] data)
        {
            int offset = this.HeaderLength + (track * this.PhysicalHeads * this.PhysicalSectors * (this.PhysicalSectorSize + SectorAttributeFlag)) + (head * this.PhysicalSectors * (this.PhysicalSectorSize + SectorAttributeFlag)) + ((sector - 1) * (this.PhysicalSectorSize + SectorAttributeFlag));
            this.WriteBytes(offset, data);
        }

        public override void FormatSector(int track, int head, int sector, byte filldata)
        {
            byte[] data = new byte[PhysicalSectorSize].Initialize(filldata);
            WriteSector(track, head, sector, data);
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

                byte[] header = new byte[5] { (byte)PhysicalSectors, (byte)PhysicalHeads, (byte)((PhysicalSectorSize / 128) >> 1), firstSectorID, sectorAttributeFlag };
                
                headerLength = 0;
                if (sectorAttributeFlag != 0)
                    headerLength = 5;
                else if (firstSectorID != 1)
                    headerLength = 4;
                else if (PhysicalSectorSize != 256)
                    headerLength = 3;
                else if (PhysicalHeads != 1)
                    headerLength = 2;
                else if (physicalSectors != 18)
                    headerLength = 1;

                byte[] data = new byte[HeaderLength + (PhysicalTracks * PhysicalHeads * PhysicalSectors * (PhysicalSectorSize + SectorAttributeFlag))].Initialize(filldata);
                if (HeaderLength != 0)
                    Array.Copy(header, 0, data, 0, HeaderLength);

                this.baseStream.Write(data, 0, data.Length);
            }
            catch (IOException)
            {
                MessageBox.Show(string.Format(MainForm.ResourceManager.GetString("DiskImageBase_FileOpenError", MainForm.CultureInfo), this.filename), MainForm.ResourceManager.GetString("DiskImageBase_FileOpenErrorCaption", MainForm.CultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.filename = string.Empty;
            }
        }

        #endregion

        #region Private Methods

        private void GetDiskInfo()
        {
            this.physicalTracks = 35;
            this.physicalHeads = 1;
            this.physicalSectors = 18;
            this.physicalSectorSize = 256;
            this.firstSectorID = 1;
            this.sectorAttributeFlag = 0;

            this.headerLength = (int)this.Length % 256;
            this.Seek(0);
            if (this.HeaderLength > 0)
            {
                for (int i = 0; i < this.HeaderLength; i++)
                {
                    switch (i)
                    {
                        case 0:
                            this.physicalSectors = this.ReadByte();
                            if (this.PhysicalSectors < 1)
                            {
                                goto NotValid;
                            }

                            break;
                        case 1:
                            this.physicalHeads = this.ReadByte();
                            if (this.PhysicalHeads < 1)
                            {
                                goto NotValid;
                            }

                            break;
                        case 2:
                            this.physicalSectorSize = 128 << this.ReadByte();
                            if (this.PhysicalSectorSize < 0)
                            {
                                goto NotValid;
                            }

                            break;
                        case 3:
                            this.firstSectorID = (byte)this.ReadByte();
                            break;
                        case 4:
                            this.sectorAttributeFlag = (byte)this.ReadByte();
                            break;
                        default:
                            goto NotValid;
                    }
                }
            }

            this.physicalTracks = ((int)this.Length - this.HeaderLength) / (this.PhysicalSectors * this.PhysicalSectorSize) / this.PhysicalHeads;
            if (this.PhysicalTracks < 1)
            {
                goto NotValid;
            }

            if ((this.Length - this.HeaderLength) % this.PhysicalSectorSize != 0)
            {
                goto NotValid;
            }

            // Fix for non-full size images - Minimum 35 tracks
            if (this.PhysicalTracks < 35)
            {
                this.physicalTracks = 35;
                this.SetLength(this.HeaderLength + (this.PhysicalTracks * this.PhysicalHeads * this.PhysicalSectors * this.PhysicalSectorSize));
            }

            this.isValidImage = true;
            return;

        NotValid:
            this.isValidImage = false;
            this.Close();

        }

        #endregion
    }
}
