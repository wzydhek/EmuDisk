using System;
using System.Windows.Forms;

namespace EmuDisk
{
    static class Program
    {
        public delegate void FormDelegate(string arg);
        private static MainForm form = null;

        static void myReceive(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                form.BeginInvoke(new FormDelegate(DelegateMethod), args[i]);
            }
        }
        static void DelegateMethod(string arg)
        {
            form.OpenDiskView(arg);
        }

        [STAThread]
        static void Main(string[] args)
        {
            if (SingletonController.IamFirst(new SingletonController.ReceiveDelegate(myReceive)))
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
