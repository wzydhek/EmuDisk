using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace EmuDisk
{
    /// <summary>
    /// Disk Image Base
    /// </summary>
    internal class DiskImageBase : Stream
    {

        #region Private Fields

        internal FileStream baseStream;
        internal string filename;
        internal int headerLength = 0;
        internal int physicalTracks = 35;
        internal int physicalHeads = 1;
        internal int physicalSectors = 18;
        internal int physicalSectorSize = 256;
        internal bool isValidImage = false;

        #endregion

        #region Constructors

        public DiskImageBase()
        {
        }

        public DiskImageBase(string filename)
        {
            this.filename = filename;

            try
            {
                this.baseStream = File.Open(this.filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch (IOException)
            {
                MessageBox.Show(string.Format(MainForm.ResourceManager.GetString("DiskImageBase_FileOpenError", MainForm.CultureInfo), this.filename), MainForm.ResourceManager.GetString("DiskImageBase_FileOpenErrorCaption", MainForm.CultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.filename = string.Empty;
            }
        }

        #endregion

        #region IDiskImage Public Properties

        public virtual DiskImageTypes ImageType
        {
            get { return DiskImageTypes.Unknown; }
        }

        public string Filename
        {
            get { return this.filename; }
        }

        public virtual bool IsValidImage
        {
            get
            {
                return isValidImage;
            }
        }

        public virtual bool IsPartitioned
        {
            get
            {
                return false;
            }
        }

        public int HeaderLength
        {
            get
            {
                return headerLength;
            }
        }

        public int PhysicalTracks
        {
            get
            {
                return physicalTracks;
            }
        }

        public int PhysicalHeads
        {
            get
            {
                return physicalHeads;
            }
        }

        public int PhysicalSectors
        {
            get
            {
                return physicalSectors;
            }
        }

        public int PhysicalSectorSize
        {
            get
            {
                return physicalSectorSize;
            }
        }

        public virtual int Partitions
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        #endregion

        #region IDiskImage Public Methods

        public virtual void SetPartition(int partition)
        {

        }

        public byte ReadByte(int offset)
        {
            byte[] data = ReadBytes(offset, 1);
            return data[0];
        }

        public void WriteByte(int offset, byte data)
        {
            WriteBytes(offset, new byte[] { data });
        }

        public virtual byte[] ReadBytes(int offset, int length)
        {
            byte[] buffer = new byte[length];
            if (this.CanRead && this.CanSeek)
            {
                this.Seek(offset);
                this.Read(buffer, 0, length);
                return buffer;
            }

            return null;
        }

        public virtual void WriteBytes(int offset, byte[] data)
        {
            if (this.CanWrite && this.CanSeek)
            {
                this.Seek(offset);
                this.Write(data, 0, data.Length);
            }
        }

        public virtual byte[] ReadSector(int track, int head, int sector)
        {
            int offset = this.HeaderLength + (track * this.PhysicalHeads * this.PhysicalSectors * this.PhysicalSectorSize) + (head * this.PhysicalSectors * this.PhysicalSectorSize) + ((sector - 1) * this.PhysicalSectorSize);
            return this.ReadBytes(offset, this.PhysicalSectorSize);
        }

        public virtual void WriteSector(int track, int head, int sector, byte[] data)
        {
            int offset = this.HeaderLength + (track * this.PhysicalHeads * this.PhysicalSectors * this.PhysicalSectorSize) + (head * this.PhysicalSectors * this.PhysicalSectorSize) + ((sector - 1) * this.PhysicalSectorSize);
            this.WriteBytes(offset, data);
        }

        public virtual void FormatSector(int track, int head, int sector, byte filldata)
        {
            byte[] data = new byte[PhysicalSectorSize].Initialize(filldata);
            WriteSector(track, head, sector, data);
        }

        public virtual void CreateDisk(string filename, int tracks, int heads, int sectors, int sectorsize, byte filldata)
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
                headerLength = 0;
                physicalTracks = tracks;
                physicalHeads = heads;
                physicalSectors = sectors;
                physicalSectorSize = sectorsize;
                byte[] data = new byte[PhysicalTracks * PhysicalHeads * PhysicalSectors * PhysicalSectorSize].Initialize(filldata);
                this.baseStream.Write(data, 0, data.Length);
            }
            catch (IOException)
            {
                MessageBox.Show(string.Format(MainForm.ResourceManager.GetString("DiskImageBase_FileOpenError", MainForm.CultureInfo), this.filename), MainForm.ResourceManager.GetString("DiskImageBase_FileOpenErrorCaption", MainForm.CultureInfo), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.filename = string.Empty;
            }
        }

        #endregion

        #region Stream Public Properties

        public override bool CanRead
        {
            get
            {
                if (this.baseStream != null && this.baseStream.CanRead)
                {
                    return true;
                }

                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                if (this.baseStream != null && this.baseStream.CanSeek)
                {
                    return true;
                }

                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                if (this.baseStream != null && this.baseStream.CanWrite)
                {
                    return true;
                }

                return false;
            }
        }

        public override long Length
        {
            get
            {
                if (this.baseStream != null)
                {
                    return this.baseStream.Length;
                }

                return 0;
            }
        }

        public override long Position
        {
            get
            {
                if (this.baseStream != null)
                {
                    return this.baseStream.Position;
                }

                return 0;
            }

            set
            {
                if (this.baseStream != null)
                {
                    this.baseStream.Position = value;
                }
            }
        }

        #endregion

        #region Stream Public Methods

        public override void Flush()
        {
            if (this.baseStream != null)
            {
                this.baseStream.Flush();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (this.baseStream != null && this.CanSeek)
            {
                return this.baseStream.Seek(offset, origin);
            }

            return -1;
        }

        public long Seek(long offset)
        {
            return this.Seek(offset, SeekOrigin.Begin);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.baseStream != null && this.CanRead)
            {
                return this.baseStream.Read(buffer, offset, count);
            }

            return -1;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.baseStream != null && this.CanWrite)
            {
                this.baseStream.Write(buffer, offset, count);
            }
        }

        #endregion

    }
}
