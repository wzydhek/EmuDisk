using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace EmuDisk
{
    /* -----------------------------------------------------------------------
     * VDK file format
     *
     * Used by Paul Burgin's PC-Dragon emulator
     *
     *  Offset  Bytes   Field           Description
     *  ------  -----   ------------    -----------
     *       0      1   magic1          Signature byte 1 ('d')
     *       1      1   magic2          Signature byte 2 ('k')
     *       2      2   header_len      Total header length (little endian)
     *       4      1   ver_actual      Version of the VDK format (0x10)
     *       5      1   ver_compat      Backwards compatibility version (0x10)
     *       6      1   source_id       Identify of the file source
     *       7      1   source_ver      Version of the file source
     *       8      1   tracks          Number of tracks
     *       9      1   sides           Number of sides (1-2)
     *      10      1   flags           Various flags
     *                                      bit 0: Write protect
     *                                      bit 1: A Lock
     *                                      bit 2: F Lock
     *                                      bit 3: Disk set
     *      11      1   compression     Compression flags (bits 0-2) and name length
     * ----------------------------------------------------------------------- */

    /*
    Dragon Emulator Virtual Disk (VDK) Format
    Paul Burgin / v1.0 / April 1999

    The new virtual disk format used by PC-Dragon v2.05 has at least 12 header bytes as follows:

    Offset	Size	Item						Notes
    0		2		Signature (ÒdkÓ)			MSB first. Note lower case to differentiate 
                                            a VDK from a DOS ROM file.
    2		2		Header length				LSB first. Total length of the header (equal 
                                            to the offset to the start of disk data). 
                                            Intended to allow the header to be extended 
                                            easily.
    4		1		VDK version - actual		Indicates the version of the VDK format 
                                            used to write the file. Currently $10 (VDK 
                                            v1.0).
    5		1		VDK version - compatibility	For backwards compatibility, this indicates 
                                            the minimum version of the VDK format 
                                            that can be used to read the file. Effectively 
                                            it says to the emulator ÒIf you can 
                                            understand VDK version X then you can 
                                            understand this fileÉÓ. Usually this byte 
                                            will be the same as the previous one, but if 
                                            a minor addition is made to the header then 
                                            it becomes useful.
    6		1		Source id					Indicates how the file was created. 
                                            Essentially this is for information only, 
                                            but may be useful for debugging. The 
                                            following values are defined:
                                                    0 = created by hand
                                                    1 = header stub
                                                    2 = mkdsk.exe
                                                    3 = other tools
                                                    ÔPÕ = PC-Dragon
                                                    ÔTÕ = T3
                                                >$7F = other emulator
    7		1		Source version				Version information for the source 
                                            identified above. E.g. $25=v2.05
    8		1		Number of tracks			40 or 80
    9		1		Number of sides				1 or 2
    10		1		Flags						bit 0 = write protect [0=off, 1=on]
                                            bit 1 = lock (advisory) [0=off, 1=on]
                                            bit 2 = lock (mandatory) [0=off, 1=on]
                                            bit 3 = disk-set [0=last disk, 1=not last 
                                            disk]
                                            bits 4-7 = unused in VDK v1.0
    11		1		Compression & Name length	bits 0-2 = compression [0=off, >0=TBD]
                                            bits 3-7 = disk name length [min 0, max 
                                            31]
    12		0-31	Disk name					Optional ASCII name for the virtual disk. 
                                            Not zero terminated.
    (min 12)	0+								Compression variables
                                            TBD

    Some of the above needs a little more explanation. The write protect
    ability is included as a bit in the header so that it can survive
    circumstances which file attributes might not, if necessary (e.g. public
    file servers, e-mail attachments). This also allows easy change from
    within an emulator, and might be useful for disk-sets (see below).
    Support for write protection by file attributes is at the option of each
    individual emulator.

    The lock bits were added as a possible approach for preventing a disk
    being loaded more than once. It is typically not required for a single
    instance of an emulator, but may be used across multiple simultaneous
    emulators, multiple instances of the same emulator, or in multi-user
    environments. The difference between bit1 and bit2 is that the user is
    asked whether or not they wish to ignore the bit1 lock but are not
    allowed to override the bit2 lock. A well behaved emulator should at
    least obey the locks upon opening the VDK, but for the current v1.0 of
    the file format need not set the locks unless it wishes to. PC-Dragon
    v2.05 does not set either of the locks (I felt a little uncomfortable
    about modifying the disk without the userÕs consent) but they may be
    used in the future.

    The disk-set bit allows more than one virtual disk to be stored in a
    single VDK file. This is intended for software supplied on >1 disk, or
    for a collection of similar disks. Emulators may allow disk-sets to be
    created/modified/loaded at their option. PC-Dragon v2.05 supports
    loading of disk-sets, but the user interface is rather basic and thereÕs
    no facility for creating disk-sets.

    Virtual disk compression has been anticipated by the format, but is left
    TBD for the moment due to the complexity of randomly accessing a
    compressed file. The disk name is optional and isnÕt ever displayed by
    PC-Dragon v2.05. Any data associated with compression is assumed to
    follow the disk name.
    */

    internal class VDKImage : DiskImageBase, IDiskImage
    {
        #region Private Properties

        private byte[] header;

        #endregion

        #region Constructors

        public VDKImage(string filename) : base(filename)
        {
            this.GetDiskInfo();
        }

        public VDKImage(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
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
                return DiskImageTypes.VDKImage;
            }
        }

        public override Geometry DiskGeometry
        {
            get
            {
                return new Geometry(Partitions, PhysicalTracks, PhysicalHeads, PhysicalSectors, PhysicalSectorSize, ((header[10] & 0x01) > 0));
            }
        }

        public override string DiskLabel
        {
            get
            {
                int namelength = header[11] >> 3;
                if (namelength == 0)
                    return null;
                byte[] namebytes = new byte[namelength];
                Array.Copy(header, 12, namebytes, 0, namelength);
                string name = Encoding.ASCII.GetString(namebytes);
                return name;
            }
            set
            {
                if (value == "" || value == null)
                {
                    byte[] newheader = new byte[12];
                    Array.Copy(header, 0, newheader, 0, 12);
                    header[11] = (byte)(header[11] & (0x3));
                }
                else
                {
                    if (value.Length > 32)
                        value = value.Substring(0, 32);
                    byte[] namebytes = Encoding.ASCII.GetBytes(value);
                    int namelength = namebytes.Length;
                    byte[] newheader = new byte[namelength + 12];
                    Array.Copy(header, 0, newheader, 0, 12);
                    Array.Copy(namebytes, 0, newheader, 12, namelength);
                    newheader[11] = (byte)((namelength << 3) + (header[11] & (0x3)));
                    newheader[2] = (byte)(newheader.Length & 0xff);
                    newheader[3] = (byte)(newheader.Length >> 8);
                    header = newheader;
                }

                string ext = Path.GetExtension(this.Filename);
                string tmpfile = Path.GetDirectoryName(this.Filename) + "\\" + Path.GetFileNameWithoutExtension(this.Filename) + ".tmp";
                Stream newfile = File.Open(tmpfile, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                newfile.Write(header, 0, header.Length);
                this.baseStream.Seek(HeaderLength, SeekOrigin.Begin);
                byte[] olddisk = new byte[(int)this.baseStream.Length - HeaderLength];
                this.baseStream.Read(olddisk, 0, olddisk.Length);
                newfile.Write(olddisk, 0, olddisk.Length);
                this.baseStream.Close();
                newfile.Close();
                File.Delete(this.Filename);
                File.Move(tmpfile, this.Filename);
                this.baseStream = File.Open(this.filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                headerLength = header.Length;
            }
        }

        #endregion

        #region Public Methods

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

                byte[] header = new byte[12] { (byte)'d', (byte)'k', 12, 00, 0x10, 0x10, 0x03, 0x10, (byte)PhysicalTracks, (byte)PhysicalHeads, 0, 0 };
                headerLength = 12;

                byte[] data = new byte[HeaderLength + (PhysicalTracks * PhysicalHeads * PhysicalSectors * (PhysicalSectorSize))].Initialize(filldata);
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

        public override string ToString()
        {
            return "VDK";
        }

        #endregion

        #region Private Methods

        private void GetDiskInfo()
        {
            this.physicalTracks = 40;
            this.physicalHeads = 1;
            this.physicalSectors = 18;
            this.physicalSectorSize = 256;

            this.headerLength = (int)this.Length % 256;
            if (this.HeaderLength == 0)
                goto NotValid;

            this.Seek(0);
            this.header = this.ReadBytes(0, HeaderLength);

            if (this.header[0] != 'd' || this.header[1] != 'k')
                goto NotValid;

            if (this.HeaderLength != (this.header[2] | (this.header[3] << 8)))
                goto NotValid;

            if (this.header[4] != 0x10 || this.header[5] != 0x10)
                goto NotValid;

            this.physicalTracks = header[8];
            if (this.PhysicalTracks != 40 && this.PhysicalTracks != 80)
                goto NotValid;

            this.physicalHeads = header[9];
            if (this.PhysicalHeads != 1 && this.PhysicalHeads != 2)
                goto NotValid;

            if (this.Length != HeaderLength + (this.PhysicalTracks * this.PhysicalHeads * this.PhysicalSectors * this.physicalSectorSize))
                goto NotValid;

            this.isValidImage = true;
            return;

        NotValid:
            this.isValidImage = false;
            this.Close();

        }

        #endregion
    }
}
