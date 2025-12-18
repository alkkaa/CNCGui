using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CNCGui.Programs;
using CNCGui.Pages;
using System.Windows.Media.Imaging;

namespace CNCGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            // Application is running
            ControllerComms.ConnectToCNC();
            //CNCServiceProcess.Startup();
        }
    }
}
