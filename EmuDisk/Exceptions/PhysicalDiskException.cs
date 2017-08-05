using System;
using System.Runtime.Serialization;

namespace EmuDisk
{
    [Serializable]
    public class PhsycialDiskException : Exception
    {
        public PhsycialDiskException()
        {
        }

        public PhsycialDiskException(string message)
            : base(message)
        {
        }

        public PhsycialDiskException(string key, bool globalized)
        {
            if (globalized)
            {
                throw new PhsycialDiskException(key);
            }
            else
            {
                throw new PhsycialDiskException(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo));
            }
        }

        public PhsycialDiskException(string key, object[] paramlist)
        {
            throw new PhsycialDiskException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        public PhsycialDiskException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PhsycialDiskException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
