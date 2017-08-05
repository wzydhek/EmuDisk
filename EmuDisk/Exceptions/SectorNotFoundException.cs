using System;
using System.Runtime.Serialization;

namespace EmuDisk
{
    [Serializable]
    public class SectorNotFoundException : Exception
    {
        public SectorNotFoundException()
        {
        }

        public SectorNotFoundException(string message) : base(message)
        {
        }

        public SectorNotFoundException(string key, bool globalized)
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

        public SectorNotFoundException(string key, object[] paramlist)
        {
            throw new DiskFormatException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        public SectorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SectorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
