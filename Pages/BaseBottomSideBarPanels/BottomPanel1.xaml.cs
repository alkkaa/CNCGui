using System;
using System.IO;
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
using keba.CNC.Afm;
using keba.CNC.NCConverter;
using CNCGui.Pages;

namespace CNCGui.Pages.BaseBottomSideBarPanels
{
    /// <summary>
    /// Interaction logic for BaseMidSidePanelButton.xaml
    /// </summary>
    public partial class BottomPanel1 : Page
    {
        public ATFSData pgATFSData = null;
        int iHighlightLineNumber = 1;
        int iActGCodeLineNumber = 1;
        long iTotalGCodeLineNumber = 0;
        bool bScroolSetDone = false;
        public int iOldLineNumber = 0;
        public bool bProgramLoaded;
        int bGCodeLoaded;

        string fileName = "kasa deneme1.nc";

        static string ip = "192.168.71.6";    // default target IP
        static int channel = 0;                    // channel 0
        static uint lcid = 0x7;                  // language german

        // CNC service
        LibAfl ctx = null;
        static CNCService cncsvc = null;

        public BottomPanel1()
        {
            InitializeComponent();
        }

        private void butLoadNCFileClick(object sender, RoutedEventArgs e)
        {
            NCProgramMonitor wNCProgramMon = new NCProgramMonitor();
            wNCProgramMon.LoadNCFile();
        }

        private void butUnloadNCFileClick(object sender, RoutedEventArgs e)
        {
            NCProgramMonitor wNCProgramMon = new NCProgramMonitor();
            wNCProgramMon.UnloadNCFile();
            //wNCProgramMon = null;
        }
    }
}
