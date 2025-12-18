using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using keba.CNC.Afl;
using keba.CNC.Afm;
using keba.CNC.NCConverter;
using CNCGui.Pages;

namespace CNCGui.Programs
{
    class CNCServiceProcess
    {
        /* Variables */
        static string ip = "192.168.71.6";    // default target IP
        static int channel = 0;                    // channel 0
        static uint lcid = 0x7;                  // language german

        // CNC service
        static LibAfl ctx = null;
        static CNCService cncsvc = null;

        int iOldLineNumber;

        // Coordinate systems
        static PositionIDs[] ids = new PositionIDs[] {
            PositionIDs.Machine,
            PositionIDs.Prog,
            0
        };


        // Convert enum to uint
        public static uint ToUInt(AxisBit bit)
        {
            return (uint)bit;
        }

        // Entry point
        public static void Startup()
        {
            LibAfl ctx = null;

            try
            {
                ctx = LibAfl.GetInstance(channel, ip, lcid, IntPtr.Zero);
            }
            catch (AflCTXException e)
            {
                Console.WriteLine("Application can't be initialized (Target: {0}, Channel: {1}, Error code: {2})", ip, channel, e.RetCode);
                return;
            }

            cncsvc = ctx.GetCNCService();
            cncsvc.OperationModeChanged += cncsvc_CNCOperationModeChanged;
            cncsvc.PositionChanged += cncsvc_PositionChanged;
            cncsvc.GCodeDataChanged += cncsvc_GCodeDataChanged;
            cncsvc.RegisterCoordinateSystems(ids);
            cncsvc.SubscribeCNCData(CNCServiceSubscription.Positions);
            cncsvc.SubscribeCNCData(CNCServiceSubscription.OperationMode);
            cncsvc.SubscribeCNCData(CNCServiceSubscription.GCodeData);
        }

        /* Operation mode degisimi algilama */
        private static void cncsvc_CNCOperationModeChanged(object sender, EventArgs<CNCOperationMode> e)
        {

        }

        /* Position degerlerini alma */
        private static void cncsvc_PositionChanged(object sender, PositionEventArgs e)
        {
            //NCProgramMonitor pgNCProgMon = new NCProgramMonitor();
            Application.Current.Dispatcher.Invoke((Action)delegate 
            {
                ATFSData pgATFSData = new ATFSData();

                if ((e.AkwUpDate & e.AkwValid) != 0)
                {
                    PositionData pos = cncsvc.GetPositions(e.PositionID);

                    // X-Axis
                    if ((pos.valid & pos.update & ToUInt(AxisBit.X)) != 0)
                    {
                        if (e.PositionID == PositionIDs.Machine)
                        {
                            pgATFSData.lblXPosition.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblXPosition.Content = pos.pos[1].ToString();
                            });
                        }
                        else if (e.PositionID == PositionIDs.TogoProgBlockEnd)
                        {
                            pgATFSData.lblXDistToGo.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblXDistToGo.Content = pos.pos[1].ToString();
                            });
                        }
                    }

                    // Y-Axis
                    if ((pos.valid & pos.update & ToUInt(AxisBit.Y)) != 0)
                    {
                        if (e.PositionID == PositionIDs.Machine)
                        {
                            pgATFSData.lblYPosition.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblYPosition.Content = pos.pos[3].ToString();
                            });
                        }
                        else if (e.PositionID == PositionIDs.TogoProgBlockEnd)
                        {
                            pgATFSData.lblYDistToGo.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblYDistToGo.Content = pos.pos[3].ToString();
                            });
                        }
                    }

                    // Z-Axis
                    if ((pos.valid & pos.update & ToUInt(AxisBit.Z)) != 0)
                    {
                        if (e.PositionID == PositionIDs.Machine)
                        {
                            pgATFSData.lblZPosition.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblZPosition.Content = pos.pos[2].ToString();
                            });
                        }
                        else if (e.PositionID == PositionIDs.TogoProgBlockEnd)
                        {
                            pgATFSData.lblZDistToGo.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblZDistToGo.Content = pos.pos[2].ToString();
                            });
                        }
                    }
                }
            });
            
        }

        /* NC Data degerlerini alma */
        private static void cncsvc_GCodeDataChanged(object sender, GMDataEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                NCProgramMonitor pgNCProgMon = new NCProgramMonitor();
                ATFSData pgATFSData = new ATFSData();
                if (e.Process == 0)
                {
                    NCData data = e.MSTE;

                    /*
                        LineNumber 0, LineNumberStartProgram 1, FeedrateNominal 2, FeedrateActual 3, NestingLevel 4, M_Function 5, M_Address 6, S_Function 7, S_Address 8,
                        T_Function 9, T_Address 10, D_Function 11, D_Address 12, E_Function 13, E_Address 14, EstimatedProgamRunningTime 15, NoofElements 16
                    */

                    /* LineNumber */
                    if (data.values[0] != pgNCProgMon.iOldLineNumber)
                    {
                        int iLineNum = data.values[0] - pgNCProgMon.iOldLineNumber;
                        while (iLineNum > 0)
                        {
                            //pgNCProgMon.LineDown();
                            iLineNum--;
                        }
                        pgNCProgMon.iOldLineNumber = data.values[0];
                    }

                    if (data.updateBits != 0 && data.validBits != 0)
                    {
                        pgATFSData.lblFeedrateSetVal.Dispatcher.Invoke(() =>
                        {
                            pgATFSData.lblFeedrateSetVal.Content = data.values[2].ToString();
                        });
                        pgATFSData.lblFeedrateActVal.Dispatcher.Invoke(() =>
                        {
                            pgATFSData.lblFeedrateActVal.Content = data.values[3].ToString();
                        });

                        if (data.values[2] != 0 && data.values[3] != 0)
                        {
                            pgATFSData.lblFeedratePercentage.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblFeedratePercentage.Content = (100 / ((float)data.values[2] / (float)data.values[3])).ToString();
                            });
                        }
                        else if (data.values[3] == 0)
                        {
                            pgATFSData.lblFeedratePercentage.Dispatcher.Invoke(() =>
                            {
                                pgATFSData.lblFeedratePercentage.Content = 0;
                            });
                        }
                    }
                }
            });
        }

        public static void LoadProgramToController()
        {
            /*
            string ip = "192.168.71.6";    // default target IP
            int channel = 0;                    // channel 0
            uint lcid = 0x7;
            LibAfl ctx = null;
            */
            String fileName = "kasa deneme1.nc";

            try
            {
                ctx = LibAfl.GetInstance(channel, ip, lcid, IntPtr.Zero);
            }
            catch (AflCTXException e)
            {
                Console.WriteLine("Application can't be initialized (Target: {0}, Channel: {1}, Error code: {2})", ip, channel, e.RetCode);
                //return -1;
            }

            // Get user path 1
            Repository repo = Repository.GetInstance();
            PathInfo pi = repo.GetPathInfo(1, false);
            if (pi == null)
            {
                Console.WriteLine("User path 1 not found !");
                ctx.Dispose();
                //return -1;
            }

            // Get file path of source and destination files
            String srcFile = System.IO.Path.Combine(pi.GetFilePath(FileType.GRP_NCS), fileName);
            String ipdFile = System.IO.Path.Combine(pi.GetFilePath(FileType.GRP_NCO), fileName);

            if (!File.Exists(srcFile))
            {
                Console.WriteLine("File {0} doesn't exist !", srcFile);
                ctx.Dispose();
                //return -1;
            }


            // Get CNC service
            CNCService cncsvc = ctx.GetCNCService();

            // Get G-Code converter
            GMCodeConverterResults gmconvResult = null;
            GMCodeConverter gmconv = null;
            bool failed = false;

            try
            {
                gmconv = new GMCodeConverter();
                gmconv.ReportError += gmconv_ReportError;

                // Compile G-Code file
                gmconvResult = gmconv.CompileNCFile(srcFile, ipdFile);
            }
            catch (GMCodeConverterException exc)
            {
                Console.WriteLine("G-Code converter exception: {0}", exc.Message);
                failed = true;
            }

            if (!failed && gmconvResult.Errors == 0)
            {
                Console.WriteLine("G-Code file converted with no errors");

                AflRetCode rCode = cncsvc.LoadDBFile(false, 1, FileType.GRP_NCO, fileName);
                if (rCode == AflRetCode.NoError)
                {
                    Console.WriteLine("G-Code file loaded");
                }
                else
                {
                    Console.WriteLine("Error loading G-Code file {0}", rCode);
                }
            }

            gmconv?.Dispose(false);

            ctx.ReleaseCNCService();
            ctx.Dispose();
            //return 0;
        }

        private static void gmconv_ReportError(object sender, ReportErrorEventArgs e)
        {
            switch (e.NotificationCode)
            {
                case GMErrorType.Error:
                case GMErrorType.FatalError:
                case GMErrorType.Warning:
                    Console.WriteLine("Error in line {0}: {1}", e.LineNumber, GMCodeConverter.GetMessageText(e.ErrorCode, false));
                    break;
                default:
                    break;
            }
        }
    }
}
