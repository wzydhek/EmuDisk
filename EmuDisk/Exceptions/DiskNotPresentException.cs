using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EmuDisk
{
    /// <summary>
    /// Exception thrown when a physical disk is not in the drive
    /// </summary>
    [Serializable]
    public class DiskNotPresentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiskNotPresentException"/> class.
        /// This is the default constructor.
        /// </summary>
        public DiskNotPresentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskNotPresentException"/> class
        /// with a custom exception message. 
        /// </summary>
        /// <param name="message">Custom exception message</param>
        public DiskNotPresentException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskNotPresentException"/> class
        /// with a custom localized exception message.
        /// </summary>
        /// <param name="key">Localized resources text key or custom text</param>
        /// <param name="globalized">Use localized string</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskNotPresentException"/> class
        /// with a custom localized exception message that contains variable place holders.
        /// </summary>
        /// <param name="key">Localized resources text key</param>
        /// <param name="paramlist">Addition parameters to use in Localized string format</param>
        public DiskNotPresentException(string key, object[] paramlist)
        {
            throw new DiskNotPresentException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskNotPresentException"/> class
        /// with a custom message and an inner exception.
        /// </summary>
        /// <param name="message">Custom exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DiskNotPresentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskNotPresentException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">Serialization information</param>
        /// <param name="context">Streaming context</param>
        protected DiskNotPresentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
