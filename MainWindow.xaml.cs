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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CNCGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void butMainPage_Click(object sender, RoutedEventArgs e)
        {

        }

        /*
        private void txtbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                IInputElement focusedControl = Keyboard.FocusedElement;
                var foc = focusedControl as TextBox;
                //foc.Text += 'A';
            }
            catch (Exception)
            {

            }
        }*/
    }
}
