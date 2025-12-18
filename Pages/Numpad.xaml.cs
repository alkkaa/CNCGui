using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using WindowsInput;
using CNCGui;

namespace CNCGui.Pages
{
    /// <summary>
    /// Interaction logic for Numpad.xaml
    /// </summary>
    public partial class Numpad : Page
    {
        public Numpad()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        public void SendKey(UIElement sourceElement, Key keyToSend)
        {

            KeyEventArgs args = new KeyEventArgs(InputManager.Current.PrimaryKeyboardDevice, PresentationSource.FromVisual(sourceElement), 0, keyToSend);

            args.RoutedEvent = Keyboard.KeyDownEvent;
            InputManager.Current.ProcessInput(args);

        }

        private void butZeroClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD0);
        }

        private void butOneClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD1);
        }

        private void butTwoClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD2);
        }

        private void butThreeClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD3);
        }

        private void butFourClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD4);
        }

        private void butFiveClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD5);
        }

        private void butSixClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD6);
        }

        private void butSevenClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD7);
        }

        private void butEightClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD8);
        }

        private void butNineClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NUMPAD9);
        }

        private void butMinusClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.OEM_MINUS);
        }

        private void butPlusClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ADD);
        }

        private void butDivClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.DIVIDE);
        }

        private void butMulClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MULTIPLY);
        }

        private void butPointClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.OEM_PERIOD);
        }

    }
}
