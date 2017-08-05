namespace EmuDisk
{
    public interface IDiskFormat
    {
        IDiskImage DiskImage { get; set; }

        DiskFormatTypes DiskFormat { get; }

        bool IsValidFormat { get; }

        int LogicalTracks { get; }

        int LogicalHeads { get; }

        int LogicalSectors { get; }

        int LogicalSectorSize { get; }

        int FreeSpace { get; }

        int TotalSpace { get; }

        string VolumeLabel { get; set; }

        string ToString();
    }
}
