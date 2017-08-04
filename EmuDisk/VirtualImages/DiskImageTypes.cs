using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmuDisk
{
    /// <summary>
    /// Disk Image Type
    /// </summary>
    public enum DiskImageTypes
    {
        /// <summary>
        /// DMK Image support
        /// </summary>
        DMKImage,

        /// <summary>
        /// JVC Image support
        /// </summary>
        JVCImage,

        /// <summary>
        /// OS9 Image support
        /// </summary>
        RAWImage,

        /// <summary>
        /// VDK Image support
        /// </summary>
        VDKImage,

        /// <summary>
        /// VHD Image support
        /// </summary>
        VHDImage,

        /// <summary>
        /// Physical Floppy support
        /// </summary>
        PhysicalDisk,

        Unknown = -1
    }
}
