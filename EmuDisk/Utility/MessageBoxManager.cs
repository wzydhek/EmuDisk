using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EmuDisk.Utility
{
    /// <summary>
    /// Message Box Manager
    /// </summary>
    internal class MessageBoxManager
    {
        #region Private Static Fields

        /// <summary>
        /// OK Text
        /// </summary>
        private static string ok = "&OK";

        /// <summary>
        /// Cancel Text
        /// </summary>
        private static string cancel = "&Cancel";

        /// <summary>
        /// Abort Text
        /// </summary>
        private static string abort = "&Abort";

        /// <summary>
        /// Retry Text
        /// </summary>
        private static string retry = "&Retry";

        /// <summary>
        /// Ignore Text
        /// </summary>
        private static string ignore = "&Ignore";

        /// <summary>
        /// Yes Text
        /// </summary>
        private static string yes = "&Yes";

        /// <summary>
        /// No Text
        /// </summary>
        private static string no = "&No";

        /// <summary>
        /// Hook Proc Delegate
        /// </summary>
        private static NativeMethods.HookProc hookProc;

        /// <summary>
        /// EnumChildWindows callback function
        /// </summary>
        private static NativeMethods.EnumChildProc enumProc;

        /// <summary>
        /// Windows Hook
        /// </summary>
        [ThreadStatic]
        private static IntPtr hHook;

        /// <summary>
        /// Dialog Button Number
        /// </summary>
        [ThreadStatic]
        private static int nButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="MessageBoxManager"/> class.
        /// </summary>
        static MessageBoxManager()
        {
            hookProc = new NativeMethods.HookProc(MessageBoxHookProc);
            enumProc = new NativeMethods.EnumChildProc(MessageBoxEnumProc);
            hHook = IntPtr.Zero;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the OK Text
        /// </summary>
        public static string OK
        {
            get
            {
                return ok;
            }

            set
            {
                ok = value;
            }
        }

        /// <summary>
        /// Gets or sets the Cancel Text
        /// </summary>
        public static string Cancel
        {
            get
            {
                return cancel;
            }

            set
            {
                cancel = value;
            }
        }

        /// <summary>
        /// Gets or sets the Abort Text
        /// </summary>
        public static string Abort
        {
            get
            {
                return abort;
            }

            set
            {
                abort = value;
            }
        }

        /// <summary>
        /// Gets or sets the Retry Text
        /// </summary>
        public static string Retry
        {
            get
            {
                return retry;
            }

            set
            {
                retry = value;
            }
        }

        /// <summary>
        /// Gets or sets the Ignore Text
        /// </summary>
        public static string Ignore
        {
            get
            {
                return ignore;
            }

            set
            {
                ignore = value;
            }
        }

        /// <summary>
        /// Gets or sets the Yes Text
        /// </summary>
        public static string Yes
        {
            get
            {
                return yes;
            }

            set
            {
                yes = value;
            }
        }

        /// <summary>
        /// Gets or sets the No Text
        /// </summary>
        public static string No
        {
            get
            {
                return no;
            }

            set
            {
                no = value;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handle the message
        /// </summary>
        /// <param name="nCode">Specifies the hook code passed to the current hook procedure.</param>
        /// <param name="wParam">Specifies the wParam value passed to the current hook procedure.</param>
        /// <param name="lParam">Specifies the lParam value passed to the current hook procedure.</param>
        /// <returns>This value is returned by the next hook procedure in the chain.</returns>
        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return NativeMethods.CallNextHookEx(hHook, nCode, wParam, lParam);
            }

            NativeMethods.CWPRETSTRUCT msg = (NativeMethods.CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.CWPRETSTRUCT));
            IntPtr hook = hHook;

            if (msg.message == NativeMethods.WM_INITDIALOG)
            {
                int nLength = NativeMethods.GetWindowTextLength(msg.hwnd);
                StringBuilder className = new StringBuilder(10);
                NativeMethods.GetClassName(msg.hwnd, className, className.Capacity);
                if (className.ToString() == "#32770")
                {
                    nButton = 0;
                    NativeMethods.EnumChildWindows(msg.hwnd, enumProc, IntPtr.Zero);
                    if (nButton == 1)
                    {
                        IntPtr hButton = NativeMethods.GetDlgItem(msg.hwnd, NativeMethods.MBCancel);
                        if (hButton != IntPtr.Zero)
                        {
                            NativeMethods.SetWindowText(hButton, OK);
                        }
                    }
                }
            }

            return NativeMethods.CallNextHookEx(hook, nCode, wParam, lParam);
        }

        /// <summary>
        /// Process the message
        /// </summary>
        /// <param name="hWnd">Handle to the MessageBox</param>
        /// <param name="lParam">Specifies the lParam value passed to the current hook procedure.</param>
        /// <returns>The message was handled</returns>
        private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder className = new StringBuilder(10);
            NativeMethods.GetClassName(hWnd, className, className.Capacity);

            if (className.ToString() == "Button")
            {
                int ctlId = NativeMethods.GetDlgCtrlID(hWnd);
                switch (ctlId)
                {
                    case NativeMethods.MBOK:
                        NativeMethods.SetWindowText(hWnd, OK);
                        break;
                    case NativeMethods.MBCancel:
                        NativeMethods.SetWindowText(hWnd, Cancel);
                        break;
                    case NativeMethods.MBAbort:
                        NativeMethods.SetWindowText(hWnd, Abort);
                        break;
                    case NativeMethods.MBRetry:
                        NativeMethods.SetWindowText(hWnd, Retry);
                        break;
                    case NativeMethods.MBIgnore:
                        NativeMethods.SetWindowText(hWnd, Ignore);
                        break;
                    case NativeMethods.MBYes:
                        NativeMethods.SetWindowText(hWnd, Yes);
                        break;
                    case NativeMethods.MBNo:
                        NativeMethods.SetWindowText(hWnd, No);
                        break;
                }

                nButton++;
            }

            return true;
        }

        #endregion

    }
}
