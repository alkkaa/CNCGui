using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using keba.CNC.Afl;

namespace CNCGui.Programs
{
    class ControllerComms
    {
        public static string strControllerIPAddress = "192.168.71.3";  // default target IP
        static int channel = 0;                  // channel 0
        static uint lcid = 0x7;                // language german

        // CNC service
        static LibAfl ctx = null;
        static CNCService cncsvc = null;

        // States
        static bool kernelStopped = false;


        // Connect to CNC (throws AflCTXException in case of failure)
        public static void ConnectToCNC()
        {
            try
            {
                ctx = LibAfl.GetInstance(0, strControllerIPAddress, 0x7, IntPtr.Zero);
                cncsvc = ctx.GetCNCService();
                cncsvc.OperationModeChanged += (object sender, EventArgs<CNCOperationMode> e) => {
                    if (e.Value == CNCOperationMode.Terminated)
                        kernelStopped = true;
                };
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "CNC Controller Connection Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Yes);
            }
            

            cncsvc.SubscribeCNCData(CNCServiceSubscription.OperationMode);
            kernelStopped = false;
            Console.WriteLine("done");
        }

        // Disconnect from CNC
        static void DisconnectFromCNC()
        {
            if (ctx != null)
            {
                ctx.ReleaseCNCService();
                ctx.Dispose();
                ctx = null;
            }
        }

        // Check for keyboard input
        //static bool NeedToQuit => Console.KeyAvailable;

        // Entry point
        public static void CommsMain() //string[] args
        {
            Console.Write("Waiting for CNC kernel... ");
            bool stopRunning = false;

            while (!stopRunning)
            {
                try
                {
                    // Setup connection to CNC
                    ConnectToCNC();
                    Console.WriteLine("done");

                    while (!stopRunning)
                    {
                        Thread.Sleep(100);

                        if (ctx.ConnectionTimeout || ctx.LastError == AflRetCode.CNCChannelNotFound)
                        {
                            Console.Write("Lost connection to CNC kernel... reconnecting... ");
                            DisconnectFromCNC();
                            break;

                        }
                        else if (kernelStopped)
                        {
                            Console.Write("CNC kernel has terminated... reconnecting... ");
                            DisconnectFromCNC();
                            break;

                        }
                        else
                        {
                            //stopRunning = NeedToQuit;
                            if (stopRunning)
                            {
                                Console.WriteLine("Disconnecting!");
                                break;
                            }
                        }
                    }

                }
                catch (AflCTXException)
                {
                }
                finally
                {
                    if (!stopRunning)
                    {
                        Thread.Sleep(1000);

                        //stopRunning = NeedToQuit;
                        if (stopRunning)
                        {
                            Console.WriteLine("astalavista !");
                        }
                    }
                }
            }

            DisconnectFromCNC();
            return;
        }
    }
}
