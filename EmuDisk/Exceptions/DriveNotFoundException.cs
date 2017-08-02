using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace EmuDisk
{
    /// <summary>
    /// Exception thrown when attempting to use a non-existent physical drive
    /// </summary>
    [Serializable]
    public class DriveNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriveNotFoundException"/> class.
        /// This is the default constructor.
        /// </summary>
        public DriveNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveNotFoundException"/> class
        /// with a custom exception message. 
        /// </summary>
        /// <param name="message">Custom exception message</param>
        public DriveNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveNotFoundException"/> class
        /// with a custom localized exception message.
        /// </summary>
        /// <param name="key">Localized resources text key or custom text</param>
        /// <param name="globalized">Use localized string</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveNotFoundException"/> class
        /// with a custom localized exception message that contains variable place holders.
        /// </summary>
        /// <param name="key">Localized resources text key</param>
        /// <param name="paramlist">Addition parameters to use in Localized string format</param>
        public DriveNotFoundException(string key, object[] paramlist)
        {
            throw new DriveNotFoundException(string.Format(MainForm.ResourceManager.GetString(key, MainForm.CultureInfo), paramlist));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveNotFoundException"/> class
        /// with a custom message and an inner exception.
        /// </summary>
        /// <param name="message">Custom exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DriveNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveNotFoundException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">Serialization information</param>
        /// <param name="context">Streaming context</param>
        protected DriveNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
