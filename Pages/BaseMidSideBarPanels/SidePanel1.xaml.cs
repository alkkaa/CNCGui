using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CNCGui.Pages.BaseMidSideBarPanels
{
    /// <summary>
    /// Interaction logic for BaseMidSidePanelButton.xaml
    /// </summary>
    public partial class SidePanel1 : Page
    {
        public SidePanel1()
        {
            InitializeComponent();
        }

        private void butGFunctionsClick(object sender, RoutedEventArgs e)
        {

        }

        private void butSidePanel1NextClick(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["SidePanelLeft"] as Storyboard;
            sb.Begin(gridSidePanelBase);
        }

        private void butSidePanel1BackClick(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["SidePanelRight"] as Storyboard;
            sb.Begin(gridSidePanelBase);
        }
    }
}
