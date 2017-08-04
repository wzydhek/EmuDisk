using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    /// <summary>
    /// Common interface for All Disk Images.
    /// </summary>
    public interface IDiskImage
    {
        DiskImageTypes ImageType { get; }
        string Filename { get; }
        bool IsValidImage { get; }
        bool IsPartitioned { get; }

        int HeaderLength { get; }
        int PhysicalTracks { get; }
        int PhysicalHeads { get; }
        int PhysicalSectors { get; }
        int PhysicalSectorSize { get; }
        int Partitions { get; set; }
        Geometry DiskGeometry { get; }
        string DiskLabel { get; set; }

        void SetPartition(int partition);
        byte[] ReadBytes(int offset, int length);
        void WriteBytes(int offset, byte[] data);

        byte[] ReadSector(int track, int head, int sector);
        void WriteSector(int track, int head, int sector, byte[] data);
        void FormatSector(int track, int head, int sector, byte filldata);

        void CreateDisk(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata);

        void Close();

        string ToString();
    }
}
