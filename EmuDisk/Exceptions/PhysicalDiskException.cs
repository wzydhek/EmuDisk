using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace EmuDisk
{
    /// <summary>
    /// Exception thrown when general error occurs when accessing a physical disk
    /// </summary>
    [Serializable]
    public class PhsycialDiskException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhsycialDiskException"/> class.
        /// This is the default constructor.
        /// </summary>
        public PhsycialDiskException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhsycialDiskException"/> class
        /// with a custom exception message. 
        /// </summary>
        /// <param name="message">Custom exception message</param>
        public PhsycialDiskException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhsycialDiskException"/> class
        /// with a custom localized exception message.
        /// </summary>
        /// <param name="key">Localized resources text key or custom text</param>
        /// <param name="globalized">Use localized string</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PhsycialDiskException"/> class
        /// with a custom localized exception message that contains variable place holders.
        /// </summary>
        /// <param name="key">Localized resources text key</param>
        /// <param name="paramlist">Addition parameters to use in Localized string format</param>
        public PhsycialDiskException(string key, object[] paramlist)
        {
            throw new PhsycialDiskException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhsycialDiskException"/> class
        /// with a custom message and an inner exception.
        /// </summary>
        /// <param name="message">Custom exception message</param>
        /// <param name="innerException">Inner exception</param>
        public PhsycialDiskException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhsycialDiskException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">Serialization information</param>
        /// <param name="context">Streaming context</param>
        protected PhsycialDiskException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
