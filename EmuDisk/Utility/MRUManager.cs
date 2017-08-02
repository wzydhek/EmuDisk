// MRI list manager.
// <copyright file="MRUManager.cs" company="Alex Farber">
//     Copyright (c) Alex Farber. All rights reserved.
// </copyright>
// alexm@cmt.co.il

/*******************************************************************************

Using:

1) Add menu item Recent Files (or any name you want) to main application menu.
   This item is used by MRUManager as popup menu for MRU list.

2) Implement IMRUClient inteface in the form class:

public class Form1 : System.Windows.Forms.Form, IMRUClient
{
     public void OpenMRUFile(string fileName)
     {
         // open file here
     }
 
     // ...    
}

3) Add MRUManager member to the form class and initialize it:

     private MRUManager mruManager;

     private void Form1_Load(object sender, System.EventArgs e)
     {
         mruManager = new MRUManager();
         mruManager.Initialize(
             this,                              // owner form
             mnuFileMRU,                        // Recent Files menu item
             "Software\\MyCompany\\MyProgram"); // Registry path to keep MRU list

        // Optional. Call these functions to change default values:

        mruManager.CurrentDir = ".....";           // default is current directory
        mruManager.MaxMRULength = ...;             // default is 10
        mruMamager.MaxDisplayNameLength = ...;     // default is 40
     }

     NOTES:
     - If Registry path is, for example, "Software\MyCompany\MyProgram",
       MRU list is kept in
       HKEY_CURRENT_USER\Software\MyCompany\MyProgram\MRU Registry entry.

     - CurrentDir is used to show file names in the menu. If file is in
       this directory, only file name is shown.

4) Call MRUManager Add and Remove functions when necessary:

       mruManager.Add(fileName);          // when file is successfully opened

       mruManager.Remove(fileName);       // when Open File operation failed

*******************************************************************************/

//// Implementation details:
////
//// MRUManager loads MRU list from Registry in Initialize function.
//// List is saved in Registry when owner form is closed.
////
//// MRU list in the menu is updated when parent menu is poped-up.
////
//// Owner form OpenMRUFile function is called when user selects file
//// from MRU list.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace EmuDisk
{
    /// <summary>
    /// MRU manager - manages Most Recently Used Files list
    /// for Windows Form application.
    /// </summary>
    public class MRUManager
    {
        #region Members

        /// <summary>
        /// entry name to keep MRU (file0, file1...)
        /// </summary>
        private const string RegEntryName = "file";

        /// <summary>
        /// owner form
        /// </summary>
        private Form ownerForm;

        /// <summary>
        /// Recent Files menu item
        /// </summary>
        private ToolStripMenuItem menuItemMRU;

        /// <summary>
        /// Recent Files menu item parent
        /// </summary>
        private ToolStripMenuItem menuItemParent;

        /// <summary>
        /// Registry path to keep MRU list
        /// </summary>
        private string registryPath;

        /// <summary>
        /// maximum number of files in MRU list
        /// </summary>
        private int maxNumberOfFiles = 10;

        /// <summary>
        /// maximum length of file name for display
        /// </summary>
        private int maxDisplayLength = 40;

        /// <summary>
        /// current directory
        /// </summary>
        private string currentDirectory;

        /// <summary>
        /// MRU list (file names)
        /// </summary>
        private ArrayList mruList;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MRUManager"/> class
        /// </summary>
        public MRUManager()
        {
            this.mruList = new ArrayList();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Maximum length of displayed file name in menu (default is 40).
        /// </summary>
        public int MaxDisplayNameLength
        {
            get
            {
                return this.maxDisplayLength;
            }

            set
            {
                this.maxDisplayLength = value;

                if (this.maxDisplayLength < 10)
                {
                    this.maxDisplayLength = 10;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Maximum length of MRU list (default is 10).
        /// </summary>
        public int MaxMRULength
        {
            get
            {
                return this.maxNumberOfFiles;
            }

            set
            {
                this.maxNumberOfFiles = value;

                if (this.maxNumberOfFiles < 1)
                {
                    this.maxNumberOfFiles = 1;
                }

                if (this.mruList.Count > this.maxNumberOfFiles)
                {
                    this.mruList.RemoveRange(this.maxNumberOfFiles - 1, this.mruList.Count - this.maxNumberOfFiles);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current directory.
        /// Default value is program current directory which is set when
        /// Initialize function is called.
        /// Set this property to change default value (optional)
        /// after call to Initialize.
        /// </summary>
        public string CurrentDir
        {
            get
            {
                return this.currentDirectory;
            }

            set
            {
                this.currentDirectory = value;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Initialization. Call this function in form Load handler.
        /// </summary>
        /// <param name="owner">Owner form</param>
        /// <param name="mruItem">Recent Files menu item</param>
        /// <param name="regPath">Registry Path to keep MRU list</param>
        public void Initialize(Form owner, ToolStripMenuItem mruItem, string regPath)
        {
            // keep reference to owner form
            this.ownerForm = owner;

            // check if owner form implements IMRUClient interface
            if (!(owner is IMRUClient))
            {
                throw new Exception(
                    "MRUManager: Owner form doesn't implement IMRUClient interface");
            }

            // keep reference to MRU menu item
            this.menuItemMRU = mruItem;

            // keep reference to MRU menu item parent
            try
            {
                this.menuItemParent = (ToolStripMenuItem)this.menuItemMRU.OwnerItem;
            }
            catch
            {
            }

            if (this.menuItemParent == null)
            {
                throw new Exception(
                    "MRUManager: Cannot find parent of MRU menu item");
            }

            // keep Registry path adding MRU key to it
            this.registryPath = regPath;
            if (this.registryPath.EndsWith("\\"))
            {
                this.registryPath += "MRU";
            }
            else
            {
                this.registryPath += "\\MRU";
            }

            // keep current directory in the time of initialization
            this.currentDirectory = System.IO.Directory.GetCurrentDirectory();

            // subscribe to MRU parent Popup event
            this.menuItemParent.DropDownOpened += new EventHandler(this.OnMRUParentPopup);

            // subscribe to owner form Closing event
            this.ownerForm.Closing += new System.ComponentModel.CancelEventHandler(this.OnOwnerClosing);

            // load MRU list from Registry
            this.LoadMRU();
        }

        /// <summary>
        /// Add file name to MRU list.
        /// Call this function when file is opened successfully.
        /// If file already exists in the list, it is moved to the first place.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Add(string file)
        {
            this.Remove(file);

            // if array has maximum length, remove last element
            if (this.mruList.Count == this.maxNumberOfFiles)
            {
                this.mruList.RemoveAt(this.maxNumberOfFiles - 1);
            }

            // add new file name to the start of array
            this.mruList.Insert(0, file);
        }

        /// <summary>
        /// Remove file name from MRU list.
        /// Call this function when File - Open operation failed.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Remove(string file)
        {
            int i = 0;

            IEnumerator myEnumerator = this.mruList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if ((string)myEnumerator.Current == file)
                {
                    this.mruList.RemoveAt(i);
                    return;
                }

                i++;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Update MRU list when MRU menu item parent is opened
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Arguments</param>
        private void OnMRUParentPopup(object sender, EventArgs e)
        {
            // remove all children
            if (this.menuItemMRU.DropDownItems.Count > 0)
            {
                this.menuItemMRU.DropDownItems.Clear();
            }

            // Disable menu item if MRU list is empty
            if (this.mruList.Count == 0)
            {
                this.menuItemMRU.Enabled = false;
                return;
            }

            // enable menu item and add child items
            this.menuItemMRU.Enabled = true;

            ToolStripMenuItem item;

            IEnumerator myEnumerator = this.mruList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                item = new ToolStripMenuItem(this.GetDisplayName((string)myEnumerator.Current));
                item.Tag = (string)myEnumerator.Current;

                // subscribe to item's Click event
                item.Click += new EventHandler(this.OnMRUClicked);

                this.menuItemMRU.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// MRU menu item is clicked - call owner's OpenMRUFile function
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Arguments</param>
        private void OnMRUClicked(object sender, EventArgs e)
        {
            string s = string.Empty;

            try
            {
                // cast sender object to ToolStripMenuItem
                ToolStripMenuItem item = (ToolStripMenuItem)sender;

                if (item != null)
                {
                    // Get file name from list using item index
                    s = (string)item.Tag; // (string)mruList[item.Index];

                    // call owner's OpenMRUFile function
                    if (s.Length > 0)
                    {
                        ((IMRUClient)this.ownerForm).OpenMRUFile(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception in OnMRUClicked: " + ex.Message);
            }
        }

        /// <summary>
        /// Save MRU list in Registry when owner form is closing
        /// </summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event Arguments</param>
        private void OnOwnerClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int i, n;

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(this.registryPath);

                if (key != null)
                {
                    n = this.mruList.Count;

                    for (i = 0; i < this.maxNumberOfFiles; i++)
                    {
                        key.DeleteValue(RegEntryName + i.ToString(), false);
                    }

                    for (i = 0; i < n; i++)
                    {
                        key.SetValue(RegEntryName + i.ToString(), this.mruList[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Saving MRU to Registry failed: " + ex.Message);
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Load MRU list from Registry.
        /// Called from Initialize.
        /// </summary>
        private void LoadMRU()
        {
            string sKey, s;

            try
            {
                this.mruList.Clear();

                RegistryKey key = Registry.CurrentUser.OpenSubKey(this.registryPath);

                if (key != null)
                {
                    for (int i = 0; i < this.maxNumberOfFiles; i++)
                    {
                        sKey = RegEntryName + i.ToString();

                        s = (string)key.GetValue(sKey, string.Empty);

                        if (s.Length == 0)
                        {
                            break;
                        }

                        this.mruList.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Loading MRU from Registry failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Get display file name from full name.
        /// </summary>
        /// <param name="fullName">Full file name</param>
        /// <returns>Short display name</returns>
        private string GetDisplayName(string fullName)
        {
            // if file is in current directory, show only file name
            FileInfo fileInfo = new FileInfo(fullName);

            if (fileInfo.DirectoryName == this.currentDirectory)
            {
                return Util.GetShortDisplayName(fileInfo.Name, this.maxDisplayLength);
            }

            return Util.GetShortDisplayName(fullName, this.maxDisplayLength);
        }

        #endregion

    }
}
