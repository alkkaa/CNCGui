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
    /// Interaction logic for KeyboardFunc.xaml
    /// </summary>
    public partial class KeyboardFunc : Page
    {
        public KeyboardFunc()
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

        private void butNextWindowClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void butUpClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.UP);
        }

        private void butPageUpClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.PRIOR);
        }

        private void butLeftClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.LEFT);
        }

        private void butOClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Mouse.LeftButtonClick();
        }

        private void butRightClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RIGHT);
        }

        private void butEndClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.END);
        }

        private void butDownClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.DOWN);
        }

        private void butPageDownClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.NEXT);
        }

        private void butBackspaceClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.BACK);
        }

        private void butEnterClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
        }

        private void butDelClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.DELETE);
        }

        private void butInsertClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.INSERT);
        }

    }
}
