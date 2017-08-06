using System;

namespace EmuDisk
{
    public class VirtualFile
    {
        #region Private Properties

        private string filename;
        private string extension;
        private int filesize;

        //OS9
        private DateTime created;
        private DateTime modified;
        private int attr;
        private int lsn;
        private int parentlsn;

        //RSDOS
        private RSDosFileTypes filetype;
        private bool ascii;

        #endregion

        #region Constructors

        public VirtualFile()
        {

        }

        #endregion

        #region Public Properties

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        public int Filesize
        {
            get { return filesize; }
            set { filesize = value; }
        }
        
        public bool IsDirectory
        {
            get { return ((attr & 0x80) > 0); }
            set { if (value) attr |= 0x80; else attr &= 0x7F; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public DateTime Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        public int Attr
        {
            get { return attr; }
            set { attr = value; }
        }

        public int LSN
        {
            get { return lsn; }
            set { lsn = value; }
        }

        public int ParentLSN
        {
            get { return parentlsn; }
            set { parentlsn = value; }
        }

        public RSDosFileTypes FileType
        {
            get { return filetype; }
            set { filetype = value; }
        }

        public bool ASCII
        {
            get { return ascii; }
            set { ascii = value; }
        }

        #endregion
    }
}
