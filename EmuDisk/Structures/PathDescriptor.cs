using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuDisk
{
    /// <summary>
    /// OS9 Path Descriptor
    /// </summary>
    public class PathDescriptor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PathDescriptor"/> class
        /// </summary>
        public PathDescriptor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathDescriptor"/> class
        /// </summary>
        /// <param name="buffer">Array of bytes representing a Path Descriptor</param>
        public PathDescriptor(byte[] buffer)
        {
            this.DeviceClass = buffer[0x00];
            this.DriveNumber = buffer[0x01];
            this.StepRate = buffer[0x02];
            this.DeviceType = buffer[0x03];
            this.Density = buffer[0x04];
            this.Cylinders = Util.UInt16(buffer.Subset(0x05, 2));
            this.Sides = buffer[0x07];
            this.Verify = buffer[0x08];
            this.SectorsPerTrack = Util.UInt16(buffer.Subset(0x09, 2));
            this.SectorsPerTrack0 = Util.UInt16(buffer.Subset(0x0b, 2));
            this.Interleave = buffer[0x0d];
            this.SegmentAllocationSize = buffer[0x0e];
            this.DMATransferMode = buffer[0x0f];
            this.Extension = Util.UInt16(buffer.Subset(0x10, 2));
            this.Offsets = buffer[0x12];
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Device Class - 0=SCF, 1=RBF, 2=PIPE, 3=SBF
        /// PD.DPT - offset $20 ($00)
        /// </summary>
        public byte DeviceClass { get; set; }

        /// <summary>
        /// Gets or sets Drive number (0..n)
        /// PD.DRV - offset $21 ($01)
        /// </summary>
        public byte DriveNumber { get; set; }

        /// <summary>
        /// Gets or sets Step rate
        /// PD.STP - offset $22 ($02)
        /// </summary>
        public byte StepRate { get; set; }

        /// <summary>
        /// Gets or sets Device Type
        /// PD.TYP - offset $23 ($03)
        /// </summary>
        public byte DeviceType { get; set; }

        /// <summary>
        /// Gets or sets Density capability
        /// PD.DNS - offset $24 ($04)
        /// </summary>
        public byte Density { get; set; }

        /// <summary>
        /// Gets or sets Number of cylinders (tracks)
        /// PD.CYL - offset $25-$26 ($05-$06)
        /// </summary>
        public ushort Cylinders { get; set; }

        /// <summary>
        /// Gets or sets Number of sides (surfaces)
        /// PD.SID - offset $27 ($07)
        /// </summary>
        public byte Sides { get; set; }

        /// <summary>
        /// Gets or sets value indicating verify writes. 0 = verify writes
        /// PD.VFY - offset $28 ($08)
        /// </summary>
        public byte Verify { get; set; }

        /// <summary>
        /// Gets or sets Default number of sectors per track
        /// PD.SCT - offset $29-$2A ($09-$0A)
        /// </summary>
        public ushort SectorsPerTrack { get; set; }

        /// <summary>
        /// Gets or sets Default number of sectors per track (Track 0)
        /// PD.T0S - offset $2B-$2C ($0B-$0C)
        /// </summary>
        public ushort SectorsPerTrack0 { get; set; }

        /// <summary>
        /// Gets or sets Sector interleave factor
        /// PD.ILV - offset $2D ($0D)
        /// </summary>
        public byte Interleave { get; set; }

        /// <summary>
        /// Gets or sets Segment allocation size
        /// PD.SAS - offset $2E ($0E)
        /// </summary>
        public byte SegmentAllocationSize { get; set; }

        /// <summary>
        /// Gets or sets DMA transfer mode
        /// PD.TFM - offset $2F ($0F)
        /// </summary>
        public byte DMATransferMode { get; set; }

        /// <summary>
        /// Gets or sets Path extension for record locking
        /// PD.EXTEN - offset $30-$31 ($10-$11)
        /// </summary>
        public ushort Extension { get; set; }

        /// <summary>
        /// Gets or sets Sector/track offsets
        /// PD.STOFF - offset $32 ($12)
        /// </summary>
        public byte Offsets { get; set; }

        #endregion
    }
}
