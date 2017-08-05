using System;
using System.Runtime.Serialization;

namespace EmuDisk
{
    [Serializable]
    public class DriveNotFoundException : Exception
    {
        public DriveNotFoundException()
        {
        }

        public DriveNotFoundException(string message)
            : base(message)
        {
        }

        public DriveNotFoundException(string key, bool globalized)
        {
            if (globalized)
            {
                throw new DriveNotFoundException(key);
            }
            else
            {
                throw new DriveNotFoundException(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo));
            }
        }

        public DriveNotFoundException(string key, object[] paramlist)
        {
            throw new DriveNotFoundException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        public DriveNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DriveNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
