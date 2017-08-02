using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuDisk
{
    /// <summary>
    /// Interface which should be implemented by owner form
    /// to use MRUManager.
    /// </summary>
    public interface IMRUClient
    {
        /// <summary>
        /// Method called to handle opening a selected MRU file
        /// </summary>
        /// <param name="fileName">Filename of selected MRU file</param>
        void OpenMRUFile(string fileName);
    }
}
