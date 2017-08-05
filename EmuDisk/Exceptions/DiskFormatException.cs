using System;
using System.Runtime.Serialization;

namespace EmuDisk
{
    [Serializable]
    public class DiskFormatException : Exception
    {
        public DiskFormatException()
        {
        }

        public DiskFormatException(string message) : base(message)
        {
        }

        public DiskFormatException(string key, bool globalized)
        {
            if (globalized)
            {
                throw new DiskFormatException(key);
            }
            else
            {
                throw new DiskFormatException(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo));
            }
        }

        public DiskFormatException(string key, object[] paramlist)
        {
            throw new DiskFormatException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        public DiskFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DiskFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
