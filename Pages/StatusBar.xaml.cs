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
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    public partial class StatusBar : Page
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        // CNC service
        static LibAfl ctx = null;
        static CNCService cncsvc = null;
        static string strControllerIPAddress = "192.168.71.3";    // default target IP
        static int channel = 0;                    // channel 0
        static uint lcid = 0x7;                  // language german

        public StatusBar()
        {
            InitializeComponent();
            LibAfl ctx = null;
            ctx = LibAfl.GetInstance(channel, Programs.ControllerComms.strControllerIPAddress, lcid, IntPtr.Zero);
            cncsvc = ctx.GetCNCService();
            cncsvc.OperationModeChanged += cncsvc_CNCOperationModeChanged;
            cncsvc.SubscribeCNCData(CNCServiceSubscription.Positions | CNCServiceSubscription.OperationMode);
            /*
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            */
        }

        /* Operation mode degisimi algilama */
        private static void cncsvc_CNCOperationModeChanged(object sender, EventArgs<CNCOperationMode> e)
        {

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ctx = LibAfl.GetInstance(0, strControllerIPAddress, 0x7, IntPtr.Zero);
            cncsvc = ctx.GetCNCService();
            CNCOperationMode cncOpMode = cncsvc.GetOperationMode();

            switch (cncOpMode)
            {
                case CNCOperationMode.Automatic:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;
                case CNCOperationMode.AutomaticBreak:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.AutomaticError:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.AutomaticRunning:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.EmergencyStop:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.EventProgramRunning:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.Manual:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.ManualInAutomaticBreak:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.MDI:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.MDIInAutomaticBreak:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.Ready:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsActive.png"));
                    break;

                case CNCOperationMode.Terminated:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsWait.png"));
                    break;

                case CNCOperationMode.Undefined:
                    imgStatus1.Source = new BitmapImage(new Uri("pack://application:,,,/CNCGui;component/Resources/Icons/iconCommsWait.png"));
                    break;
            }

        }
    }
}
