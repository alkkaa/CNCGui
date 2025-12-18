using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using keba.CNC.Afl;

namespace CNCGui.Pages
{
    /// <summary>
    /// Interaction logic for AxisData.xaml
    /// </summary>
    public partial class ATFSData : Page
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public NCProgramMonitor pgNCProgramMonitor = null;
        int iOldLineNumber;
        static string ip = "192.168.71.6";    // default target IP
        static int channel = 0;                    // channel 0
        static uint lcid = 0x7;                  // language german

        // CNC service
        LibAfl ctx = null;
        static CNCService cncsvc = null;

        // Coordinate systems
        static PositionIDs[] ids = new PositionIDs[]
        {
            PositionIDs.Machine,
            PositionIDs.Prog,
            PositionIDs.TogoProgBlockEnd,
            0
        };

        public ATFSData()
        {
            InitializeComponent();
            /*
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
            */
            new Task(ExecuteCNCEvents).Start();
            Task.Delay(500);
            Console.WriteLine("ATFS Data CNC Events executed...");
        }

        public void ExecuteCNCEvents()
        {
            ctx = LibAfl.GetInstance(channel, Programs.ControllerComms.strControllerIPAddress, lcid, IntPtr.Zero);
            cncsvc = ctx.GetCNCService();
            cncsvc.OperationModeChanged += cncsvc_CNCOperationModeChanged;
            cncsvc.PositionChanged += cncsvc_PositionChanged;
            cncsvc.GCodeDataChanged += cncsvc_GCodeDataChanged;
            cncsvc.RegisterCoordinateSystems(ids);
            cncsvc.SubscribeCNCData(CNCServiceSubscription.Positions);
            cncsvc.SubscribeCNCData(CNCServiceSubscription.OperationMode);
            cncsvc.SubscribeCNCData(CNCServiceSubscription.GCodeData);
        }

        public void ReleaseCNCService()
        {
            LibAfl ctx = null;
            ctx = LibAfl.GetInstance(channel, ip, lcid, IntPtr.Zero);
            ctx.ReleaseCNCService();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            /*
            LibAfl ctx = null;
            ctx = LibAfl.GetInstance(channel, ip, lcid, IntPtr.Zero);
            cncsvc = ctx.GetCNCService();

            PositionData posMachine = cncsvc.GetPositions(PositionIDs.Machine);
            PositionData posBlockEnd = cncsvc.GetPositions(PositionIDs.TogoProgBlockEnd);
            NCData ncdataMachine = cncsvc.GetNCData(2);

            // Machine pos
            if ((posMachine.valid & posMachine.update) != 0)
            {
                lblXPosition.Content = posMachine.pos[1].ToString();
                lblYPosition.Content = posMachine.pos[3].ToString();
                lblZPosition.Content = posMachine.pos[2].ToString();
                lblAPosition.Content = posMachine.pos[0].ToString();
                lblBPosition.Content = posMachine.pos[4].ToString();
                lblCPosition.Content = posMachine.pos[5].ToString();
            }

            
            // BlockEnd pos
            if ((posBlockEnd.valid & posBlockEnd.update) != 0)
            {
                lblXDistToGo.Content = posBlockEnd.pos[1].ToString();
                lblYDistToGo.Content = posBlockEnd.pos[3].ToString();
                lblZDistToGo.Content = posBlockEnd.pos[2].ToString();
                lblADistToGo.Content = posBlockEnd.pos[0].ToString();
                lblBDistToGo.Content = posBlockEnd.pos[4].ToString();
                lblCDistToGo.Content = posBlockEnd.pos[5].ToString();
            }
            */

        }

        // Convert enum to uint
        public static uint ToUInt(AxisBit bit)
        {
            return (uint)bit;
        }

        /* Operation mode degisimi algilama */
        private static void cncsvc_CNCOperationModeChanged(object sender, EventArgs<CNCOperationMode> e)
        {

        }

        /* Position degerlerini alma */
        private void cncsvc_PositionChanged(object sender, PositionEventArgs e)
        {
            if ((e.AkwUpDate & e.AkwValid) != 0)
            {
                PositionData pos = cncsvc.GetPositions(e.PositionID);

                // X-Axis
                if ((pos.valid & pos.update & ToUInt(AxisBit.X)) != 0)
                {
                    if (e.PositionID == PositionIDs.Machine)
                    {
                        lblXPosition.Dispatcher.Invoke(() =>
                        { 
                            lblXPosition.Content = pos.pos[1].ToString(); 
                        });
                    }
                    else if (e.PositionID == PositionIDs.TogoProgBlockEnd)
                    {
                        lblXDistToGo.Dispatcher.Invoke(() =>
                        {
                            lblXDistToGo.Content = pos.pos[1].ToString();
                        });
                    }
                }

                // Y-Axis
                if ((pos.valid & pos.update & ToUInt(AxisBit.Y)) != 0)
                {
                    if (e.PositionID == PositionIDs.Machine)
                    {
                        lblYPosition.Dispatcher.Invoke(() =>
                        {
                            lblYPosition.Content = pos.pos[3].ToString();
                        });
                    }
                    else if (e.PositionID == PositionIDs.TogoProgBlockEnd)
                    {
                        lblYDistToGo.Dispatcher.Invoke(() =>
                        {
                            lblYDistToGo.Content = pos.pos[3].ToString();
                        });
                    }
                }

                // Z-Axis
                if ((pos.valid & pos.update & ToUInt(AxisBit.Z)) != 0)
                {
                    if (e.PositionID == PositionIDs.Machine)
                    {
                        lblZPosition.Dispatcher.Invoke(() =>
                        {
                            lblZPosition.Content = pos.pos[2].ToString();
                        });
                    }
                    else if (e.PositionID == PositionIDs.TogoProgBlockEnd)
                    {
                        lblZDistToGo.Dispatcher.Invoke(() =>
                        {
                            lblZDistToGo.Content = pos.pos[2].ToString();
                        });
                    }
                }
            }
        }

        /* NC Data degerlerini alma */
        private void cncsvc_GCodeDataChanged(object sender, GMDataEventArgs e)
        {
            if (e.Process == 0)
            {
                NCData data = e.MSTE;

                /*
                 LineNumber 0, LineNumberStartProgram 1, FeedrateNominal 2, FeedrateActual 3, NestingLevel 4, M_Function 5, M_Address 6, S_Function 7, S_Address 8,
                 T_Function 9, T_Address 10, D_Function 11, D_Address 12, E_Function 13, E_Address 14, EstimatedProgamRunningTime 15, NoofElements 16
                */

                if (data.updateBits != 0 && data.validBits != 0)
                {
                    lblFeedrateSetVal.Dispatcher.Invoke(() =>
                    {
                        lblFeedrateSetVal.Content = data.values[2].ToString();
                    });
                    lblFeedrateActVal.Dispatcher.Invoke(() =>
                    {
                        lblFeedrateActVal.Content = data.values[3].ToString();
                    });
                    
                    if (data.values[2] != 0 && data.values[3] != 0)
                    {
                        lblFeedratePercentage.Dispatcher.Invoke(() =>
                        {
                            lblFeedratePercentage.Content = (100 / ((float)data.values[2] / (float)data.values[3])).ToString();
                        });
                    }
                    else if (data.values[3] == 0)
                    {
                        lblFeedratePercentage.Dispatcher.Invoke(() =>
                        {
                            lblFeedratePercentage.Content = 0;
                        });
                    }
                }
            }
        }

    }
}
