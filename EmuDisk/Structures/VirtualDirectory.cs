using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace EmuDisk
{
    public class VirtualDirectory : ICollection
    {
        #region Private Properties

        List<VirtualFile> files;
        int parentlsn;

        #endregion;

        #region Constructors

        public VirtualDirectory()
        {
            files = new List<VirtualFile>();
        }

        #endregion

        #region ICollection Properties

        public int Count
        {
            get { return files.Count; }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ICollection Methods

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return files.GetEnumerator();
        }

        #endregion

        #region Public Properties

        public VirtualFile this[int key]
        {
            get
            {
                return files[key];
            }
        }

        #endregion

        #region Public Methods

        public void Add(VirtualFile file)
        {
            files.Add(file);
        }

        public bool Contains(string filename, string extension)
        {
            foreach(VirtualFile file in files)
            {
                if (file.LSN == 0)
                {
                    if (file.Filename.Equals(filename, StringComparison.OrdinalIgnoreCase) && file.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                else
                {
                    if (file.Filename.Equals(filename, StringComparison.Ordinal) && file.Extension.Equals(extension, StringComparison.Ordinal))
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}
