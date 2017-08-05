using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuDisk
{
    /// <summary>
    /// Represents a position in the HexBox control
    /// </summary>
    internal struct BytePositionInfo
    {
        public BytePositionInfo(long index, int characterPosition)
        {
            _index = index;
            _characterPosition = characterPosition;
        }

        public int CharacterPosition
        {
            get { return _characterPosition; }
        }
        int _characterPosition;

        public long Index
        {
            get { return _index; }
        }
        long _index;
    }
}
