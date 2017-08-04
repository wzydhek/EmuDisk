using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    /// <summary>
    /// Common interface for All Disk Formats.
    /// </summary>
    public interface IDiskFormat
    {
        /// <summary>
        /// Gets or sets the underlying disk image
        /// </summary>
        IDiskImage DiskImage { get; set; }

        /// <summary>
        /// Gets an enum value of which disk format this class supports
        /// </summary>
        DiskFormatTypes DiskFormat { get; }

        /// <summary>
        /// Gets a value indicating whether the disk represented is in a valid format for this class
        /// </summary>
        bool IsValidFormat { get; }

        /// <summary>
        /// Gets the logical number of cylinders
        /// </summary>
        int LogicalTracks { get; }

        /// <summary>
        /// Gets the logical number of sides
        /// </summary>
        int LogicalHeads { get; }

        /// <summary>
        /// Gets the logical number of sectors
        /// </summary>
        int LogicalSectors { get; }

        /// <summary>
        /// Gets the logical size of sectors in bytes
        /// </summary>
        int LogicalSectorSize { get; }

        int FreeSpace { get; }
        int TotalSpace { get; }

        string ToString();
    }
}
