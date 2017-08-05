namespace EmuDisk
{
    /// <summary>
    /// <para>
    /// Dragon Dos virtual disk support
    /// </para>
    /// <para>
    /// 3.  Disk Layout
    /// ===============
    /// </para>
    /// <para>
    ///     MFM Double Density
    ///      40 or 80 tracks (0 - 39 or 0 - 79)
    ///       1 or  2 sides
    ///      18 sectors per track (1 - 18)
    ///     256 bytes per sector
    ///     2:1 interleave
    /// </para>
    /// <para>
    /// Drive numbering: Dragon DOS refers to drives 1-4 (physical units 0-3),
    /// drive 0 can also be used to refer to drive 1.
    /// </para>
    /// <para>
    /// Double sided disks are usually considered as having 36 sectors per
    /// track rather than referring to side numbers.
    /// </para>
    /// <para>
    /// Logical Sector Numbers (LSNs) are used to refer to a sector relative to the
    /// first sector on the disk.  LSN 0x000 == Track 0, Side 0, Sector 1.
    /// </para>
    /// <para>
    /// 40 Tracks, 1 Side,  18 Sectors:
    /// </para>
    /// <para>
    ///   LSN 0x000  Track 0 Side 0 Sector 1
    ///   LSN 0x012  Track 1 Side 0 Sector 1
    /// </para>
    /// <para>
    /// 40 Tracks, 2 Sides, 36 Sectors:
    /// </para>
    /// <para>
    ///   LSN 0x000  Track 0 Side 0 Dragon Sector  1 Physical Sector 1
    ///   LSN 0x012  Track 0 Side 1 Dragon Sector 19 Physical Sector 1
    ///   LSN 0x024  Track 1 Side 0 Dragon Sector  1 Physical Sector 1
    /// </para>
    /// <para>
    /// 4.  Directory Track
    /// ===================
    /// </para>
    /// <para>
    /// Track 20 contains the Directory information
    /// </para>
    /// <para>
    /// Sectors 1 and 2 hold the sector bitmap
    /// </para>
    /// <para>
    /// Track 20 Sector 1 identifies the disk format as follows:
    /// Offset
    /// 0xfc    Tracks on disk
    /// 0xfd    Sectors per track (36 indicates double sided / 18 secs per track)
    /// 0xfe    One's complement of offset 0xfc
    /// 0xff    One's complement of offset 0xfd
    /// </para>
    /// <para>
    /// Sectors 3 - 18 hold directory entries.
    /// Each directory entry is 25 bytes long - 160 entries max numbered 0 - 159.
    /// Each sector contains 10 directory entries as follows:
    /// 0x00 - 0x18 Directory Entry  1
    /// 0x19 - 0x31 Directory Entry  2
    /// 0x32 - 0x4a Directory Entry  3
    /// 0x4b - 0x63 Directory Entry  4
    /// 0x64 - 0x7c Directory Entry  5
    /// 0x7d - 0x95 Directory Entry  6
    /// 0x96 - 0xae Directory Entry  7
    /// 0xaf - 0xc7 Directory Entry  8
    /// 0xc8 - 0xe0 Directory Entry  9
    /// 0xe1 - 0xf9 Directory Entry 10
    /// 0xfa - 0xff 6 unused bytes - usually 0x00 - used by some programs to
    ///         store long disk labels
    /// </para>
    /// <para>
    /// 4.1  Format of Sector Bitmap
    /// ----------------------------
    /// </para>
    /// <para>
    /// The sector bitmap is split across sectors 1 and 2 of track 20 
    /// Sector 1:
    /// 0x00 - 0xb3 Bitmap for LSNs 0x000 - 0x59f
    /// 0xb4 - 0xfb Unused - 0x00 - used by DosPlus for label and something else
    /// 0xfc - 0xff Disk format information (see above)
    /// </para>
    /// <para>
    /// Sector 2:
    /// 0x00 - 0xb3 Bitmap for LSNs 0x5a0 - 0xb3f (80 Track, DS only)
    /// 0xb4 - 0xff Unused - 0x00
    /// </para>
    /// <para>
    /// Each bit in the sector bitmap represents a single logical sector number -
    /// 0 = used, 1 = free
    /// </para>
    /// <para>
    /// LSN
    /// 0x000   Sector 1 Byte 0x00 Bit 0
    /// 0x007   Sector 1 Byte 0x00 Bit 7
    /// 0x008   Sector 1 Byte 0x01 Bit 0
    /// 0x59f   Sector 1 Byte 0xb3 Bit 7
    /// 0x5a0   Sector 2 Byte 0x00 Bit 0
    /// 0xb3f   Sector 2 Byte 0xb3 Bit 7
    /// </para>
    /// <para>
    /// 4.2  Directory entry format
    /// ---------------------------
    /// </para>
    /// <para>
    /// 0x00        flag byte
    ///     bit 7   Deleted - this entry may be reused
    ///     bit 6   Unused
    ///     bit 5   Continued - byte at offset 0x18 gives next entry number
    ///     bit 4   Unused
    ///     bit 3   End of Directory - no further entries need to be scanned
    ///     bit 2   Unused
    ///     bit 1   Protect Flag - file should not be overwritten
    ///     bit 0   Continuation Entry - this entry is a Continuation Block
    /// </para>
    /// <para>
    /// 0x01 - 0x17 File Header Block or Continuation Block
    /// </para>
    /// <para>
    /// 0x18    [flag byte bit 5 == 0]
    ///         Bytes used in last sector (0x00 == 256 bytes)
    ///         [flag byte bit 5 == 1]
    ///         Next directory entry num (0-159)
    /// </para>
    /// <para>
    /// File Header block:  (flag byte bit 0 == 0)
    /// </para>
    /// <para>
    /// 0x01 - 0x08 filename (padded with 0x00)
    /// 0x09 - 0x0b extension (padded with 0x00)
    /// 0x0c - 0x0e Sector Allocation Block 1
    /// 0x0f - 0x11 Sector Allocation Block 2
    /// 0x12 - 0x14 Sector Allocation Block 3
    /// 0x15 - 0x17 Sector Allocation Block 4
    /// </para>
    /// <para>
    /// Continuation block:  (flag byte bit 0 == 1)
    /// </para>
    /// <para>
    /// 0x01 - 0x03 Sector Allocation Block 1
    /// 0x04 - 0x06 Sector Allocation Block 2
    /// 0x07 - 0x09 Sector Allocation Block 3
    /// 0x0a - 0x0c Sector Allocation Block 4
    /// 0x0d - 0x0f Sector Allocation Block 5
    /// 0x10 - 0x12 Sector Allocation Block 6
    /// 0x13 - 0x15 Sector Allocation Block 7
    /// 0x16 : 0x17 Unused
    /// </para>
    /// <para>
    /// Sector Allocation Block format:
    /// </para>
    /// <para>
    /// 0x00 : 0x01 Logical Sector Number of first sector in this block
    /// 0x02        Count of contiguous sectors in this block
    /// </para>
    /// <para>
    /// 5.  Bootable disks
    /// ==================
    /// </para>
    /// <para>
    /// The characters 'OS' at the offsets 0 and 1 of LSN 2 (Track 0, sector 3)
    /// signify the disk is bootable.
    /// </para>
    /// <para>
    /// The DragonDos BOOT command checks for this signature returning BT error
    /// if not found.  Otherwise, Sectors 3-18 of track 0 are loaded into memory
    /// at address $2600 and execution begins at $2602.
    /// </para>
    /// <para>
    /// Note that the operation of BOOT under the original DragonDos does not set the
    /// default drive number to that passed to the BOOT command but DosPlus does.
    /// </para>
    /// </summary>
    internal class DragonDosFormat : BaseDiskFormat, IDiskFormat
    {
        #region Constructors

        public DragonDosFormat(IDiskImage diskImage)
        {
            this.DiskImage = diskImage;

            this.LogicalTracks = 40;
            this.LogicalHeads = 1;
            this.LogicalSectors = 18;
            this.LogicalSectorSize = 256;

            if (!(this.DiskImage.IsPartitioned))
            {
                this.LogicalTracks = DiskImage.PhysicalTracks;
                this.LogicalHeads = DiskImage.PhysicalHeads;
                this.LogicalSectors = DiskImage.PhysicalSectors;
                this.LogicalSectorSize = DiskImage.PhysicalSectorSize;
            }

            this.ValidateDragonDos();

        }

        #endregion

        #region IDiskFormat Members

        #region Public Properties

        public DiskFormatTypes DiskFormat
        {
            get
            {
                return DiskFormatTypes.DragonDosFormat;
            }
        }

        public bool IsValidFormat
        {
            get
            {
                return this.ValidateDragonDos();
            }
        }

        public override int FreeSpace
        {
            get
            {
                return this.GetFreeSectors(this.DiskImage.ReadSector(20, 0, 1), this.DiskImage.ReadSector(20, 1, 1)) * this.LogicalSectorSize;
            }
        }

        public override int TotalSpace
        {
            get
            {
                byte[] bitmap0 = this.DiskImage.ReadSector(20, 0, 1);
                return bitmap0[0xfc] * bitmap0[0xfd] * this.LogicalSectorSize;
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return "DRAGON";
        }

        #endregion

        #endregion

        #region Private Methods

        private bool ValidateDragonDos()
        {
            byte[] bitmap0 = this.DiskImage.ReadSector(20, 0, 1);
            this.LogicalTracks = bitmap0[0xfc];
            this.LogicalSectors = bitmap0[0xfd];
            this.LogicalHeads = 1;
            this.LogicalSectorSize = 256;
            if (this.LogicalSectors == 36)
            {
                this.LogicalSectors = 18;
                this.LogicalHeads = 2;
            }

            if (bitmap0[0xfe] != (~bitmap0[0xfc] & 0xff) || bitmap0[0xff] != (~bitmap0[0xfd] & 0xff))
            {
                return false;
            }

            if (this.LogicalTracks != 40 && this.LogicalTracks != 80)
            {
                return false;
            }

            if (this.LogicalSectors != 18)
            {
                return false;
            }

            return true;
        }

        private int GetFreeSectors(byte[] bitmap0, byte[] bitmap1)
        {
            int freeLSNs = 0;

            for (int i = 0; i < 40 * this.LogicalHeads * this.LogicalSectors; i++)
            {
                byte b = bitmap0[i / 8];
                b >>= i % 8;
                freeLSNs += b & 1;
            }

            if (this.LogicalTracks == 80)
            {
                for (int i = 0; i < 40 * this.LogicalHeads * this.LogicalSectors; i++)
                {
                    byte b = bitmap1[i / 8];
                    b >>= i % 8;
                    freeLSNs += b & 1;
                }
            }

            return freeLSNs;
        }

        #endregion
    }
}
