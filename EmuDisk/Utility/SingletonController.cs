using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace EmuDisk
{
    /// <summary>
    /// Singleton Controller to allow only a single instance of the application
    /// </summary>
    [Serializable]
    internal class SingletonController : MarshalByRefObject
    {
        /// <summary>
        /// Receiver Delegate
        /// </summary>
        private static ReceiveDelegate receive = null;

        /// <summary>
        /// TCP Channel
        /// </summary>
        private static TcpChannel tcpChannel = null;

        /// <summary>
        /// Threading Mutex
        /// </summary>
        private static Mutex mutex = null;

        /// <summary>
        /// Receiver Delegate
        /// </summary>
        /// <param name="args">Arguments passed to application</param>
        public delegate void ReceiveDelegate(string[] args);

        /// <summary>
        /// Gets or sets the receiver delegate
        /// </summary>
        public static ReceiveDelegate Receiver
        {
            get
            {
                return receive;
            }

            set
            {
                receive = value;
            }
        }

        /// <summary>
        /// Checks to see if this is the first call to the application
        /// </summary>
        /// <param name="r">Receiver Delegate</param>
        /// <returns>True if this is the first call</returns>
        public static bool IamFirst(ReceiveDelegate r)
        {
            if (IamFirst())
            {
                Receiver += r;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks to see if this is the first call to the application
        /// </summary>
        /// <returns>True if this is the first call</returns>
        public static bool IamFirst()
        {
            string uniqueIdentifier;
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).CodeBase;
            uniqueIdentifier = assemblyName.Replace("\\", "_");

            mutex = new Mutex(false, uniqueIdentifier);

            if (mutex.WaitOne(1, true))
            {
                // We locked it! We are the first instance!!!    
                CreateInstanceChannel();
                return true;
            }
            else
            {
                // Not the first instance!!!
                mutex.Close();
                mutex = null;
                return false;
            }
        }

        /// <summary>
        /// Cleanup after call
        /// </summary>
        public static void Cleanup()
        {
            if (mutex != null)
            {
                mutex.Close();
            }

            if (tcpChannel != null)
            {
                tcpChannel.StopListening(null);
            }

            mutex = null;
            tcpChannel = null;
        }

        /// <summary>
        /// Send passed arguments to existing instance
        /// </summary>
        /// <param name="s">String parameters to pass</param>
        public static void Send(string[] s)
        {
            SingletonController ctrl;
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
            try
            {
                ctrl = (SingletonController)Activator.GetObject(typeof(SingletonController), "tcp://localhost:1234/SingletonController");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                throw;
            }

            ctrl.Receive(s);
        }

        /// <summary>
        /// Received passed arguments
        /// </summary>
        /// <param name="s">String parameters that were passed</param>
        public void Receive(string[] s)
        {
            if (receive != null)
            {
                receive(s);
            }
        }

        /// <summary>
        /// Create a Singleton Instance channel
        /// </summary>
        private static void CreateInstanceChannel()
        {
            tcpChannel = new TcpChannel(6309);
            ChannelServices.RegisterChannel(tcpChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                Type.GetType("EmuDisk.SingletonController"),
                "SingletonController",
                WellKnownObjectMode.SingleCall);
        }
    }
}
