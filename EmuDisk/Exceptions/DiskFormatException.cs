using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace EmuDisk
{
    /// <summary>
    /// Exception thrown when virtual disk format could not be determined
    /// </summary>
    [Serializable]
    public class DiskFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiskFormatException"/> class.
        /// This is the default constructor.
        /// </summary>
        public DiskFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskFormatException"/> class
        /// with a custom exception message. 
        /// </summary>
        /// <param name="message">Custom exception message</param>
        public DiskFormatException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskFormatException"/> class
        /// with a custom localized exception message.
        /// </summary>
        /// <param name="key">Localized resources text key or custom text</param>
        /// <param name="globalized">Use localized string</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskFormatException"/> class
        /// with a custom localized exception message that contains variable place holders.
        /// </summary>
        /// <param name="key">Localized resources text key</param>
        /// <param name="paramlist">Addition parameters to use in Localized string format</param>
        public DiskFormatException(string key, object[] paramlist)
        {
            throw new DiskFormatException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskFormatException"/> class
        /// with a custom message and an inner exception.
        /// </summary>
        /// <param name="message">Custom exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DiskFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskFormatException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">Serialization information</param>
        /// <param name="context">Streaming context</param>
        protected DiskFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
