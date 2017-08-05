﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuDisk
{
    internal class FixedLengthByteProvider : IByteProvider
    {
        /// <summary>
        /// Contains information about changes.
        /// </summary>
        bool _hasChanges;
        /// <summary>
        /// Contains a byte collection.
        /// </summary>
        List<byte> _bytes;

        public FixedLengthByteProvider(byte[] data)
            : this(new List<Byte>(data))
        {
        }

        /// <summary>
        /// Initializes a new instance of the DynamicByteProvider class.
        /// </summary>
        /// <param name="bytes"></param>
        public FixedLengthByteProvider(List<Byte> bytes)
        {
            _bytes = bytes;
        }

        /// <summary>
        /// Raises the Changed event.
        /// </summary>
        void OnChanged(EventArgs e)
        {
            _hasChanges = true;

            if (Changed != null)
                Changed(this, e);
        }

        /// <summary>
        /// Gets the byte collection.
        /// </summary>
        public List<Byte> Bytes
        {
            get { return _bytes; }
        }

        public byte[] ByteArray
        {
            get
            {
                return _bytes.ToArray();
            }
        }

        #region IByteProvider Members

        /// <summary>
        /// True, when changes are done.
        /// </summary>
        public bool HasChanges()
        {
            return _hasChanges;
        }

        /// <summary>
        /// Applies changes.
        /// </summary>
        public void ApplyChanges()
        {
            _hasChanges = false;
        }

        /// <summary>
        /// Occurs, when the write buffer contains new changes.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Occurs, when InsertBytes or DeleteBytes method is called.
        /// </summary>
        public event EventHandler LengthChanged;

        /// <summary>
        /// Reads a byte from the byte collection.
        /// </summary>
        /// <param name="index">the index of the byte to read</param>
        /// <returns>the byte</returns>
        public byte ReadByte(long index)
        { return _bytes[(int)index]; }

        /// <summary>
        /// Write a byte into the byte collection.
        /// </summary>
        /// <param name="index">the index of the byte to write.</param>
        /// <param name="value">the byte</param>
        public void WriteByte(long index, byte value)
        {
            _bytes[(int)index] = value;
            OnChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Deletes bytes from the byte collection.
        /// </summary>
        /// <param name="index">the start index of the bytes to delete.</param>
        /// <param name="length">the length of bytes to delete.</param>
        public void DeleteBytes(long index, long length)
        {

        }

        /// <summary>
        /// Inserts byte into the byte collection.
        /// </summary>
        /// <param name="index">the start index of the bytes in the byte collection</param>
        /// <param name="bs">the byte array to insert</param>
        public void InsertBytes(long index, byte[] bs)
        {
        }

        /// <summary>
        /// Gets the length of the bytes in the byte collection.
        /// </summary>
        public long Length
        {
            get
            {
                return _bytes.Count;
            }
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public bool SupportsWriteByte()
        {
            return true;
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public bool SupportsInsertBytes()
        {
            return false;
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public bool SupportsDeleteBytes()
        {
            return false;
        }

        #endregion

    }
}
