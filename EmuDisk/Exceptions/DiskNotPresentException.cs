using System;
using System.Runtime.Serialization;

namespace EmuDisk
{
    [Serializable]
    public class DiskNotPresentException : Exception
    {
        public DiskNotPresentException()
        {
        }

        public DiskNotPresentException(string message) : base(message)
        {
        }

        public DiskNotPresentException(string key, bool globalized)
        {
            if (globalized)
            {
                throw new DiskNotPresentException(key);
            }
            else
            {
                throw new DiskNotPresentException(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo));
            }
        }

        public DiskNotPresentException(string key, object[] paramlist)
        {
            throw new DiskNotPresentException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        public DiskNotPresentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DiskNotPresentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
