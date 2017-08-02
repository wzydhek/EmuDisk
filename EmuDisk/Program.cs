using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmuDisk
{
    static class Program
    {
        /// <summary>
        /// MainForm instance
        /// </summary>
        private static MainForm form;

        /// <summary>
        /// Singleton Form Delegate
        /// </summary>
        /// <param name="arg">String arguments passed to application</param>
        public delegate void FormDelegate(string arg);

        /// <summary>
        /// Singleton Receiver
        /// </summary>
        /// <param name="args">String arguments passed to application</param>
        public static void MyReceive(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                form.BeginInvoke(new FormDelegate(DelegateMethod), args[i]);
            }
        }

        /// <summary>
        /// Singleton handler
        /// </summary>
        /// <param name="arg">String arguments passed to application</param>
        public static void DelegateMethod(string arg)
        {
            form.OpenDiskView(arg);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (SingletonController.IamFirst(new SingletonController.ReceiveDelegate(MyReceive)))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (args != null)
                {
                    form = new MainForm(args);
                    Application.Run(form);
                }
                else
                {
                    form = new MainForm();
                    Application.Run(form);
                }
            }
            else
            {
                // send command line arguments to running application, then terminate
                SingletonController.Send(args);
            }

            SingletonController.Cleanup();
        }
    }
}
