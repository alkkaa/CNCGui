using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Threading;
using WindowsInput;
using CNCGui;

namespace CNCGui.Pages
{
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class EmbeddedKeyboard : Page
    {
        public EmbeddedKeyboard()
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

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
            //sim.Mouse.LeftButtonClick();
        }

        private void butQCharClick(object sender, RoutedEventArgs e)
        {
            //MainWindow myMainWindow = new MainWindow();
            //SendKey(myMainWindow.txtbx, Key.A);
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Q);

            // öncesinde deger göndereceğimiz textboxa focus yapmak gerekiyor ve window seçimi nasıl yapılmalı bakmak gerek
            //SendMessage(new WindowInteropHelper(this).Handle, 0x0102, 72, 0)
        }

        private void butWCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_W);
        }

        private void butECharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_E);
        }

        private void butRCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_R);
        }

        private void butTCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_T);
        }

        private void butYCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Y);
        }

        private void butUCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_U);
        }

        private void butICharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_I);
        }

        private void butOCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_O);
        }

        private void butPCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_P);
        }

        private void butACharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
        }

        private void butSCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);
        }

        private void butDCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_D);
        }

        private void butFCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_F);
        }

        private void butGCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_G);
        }

        private void butHCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_H);
        }

        private void butJCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_J);
        }

        private void butKCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_K);
        }

        private void butLCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_L);
        }

        private void butZCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
        }

        private void butXCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_X);
        }

        private void butCCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);
        }

        private void butVCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_V);
        }

        private void butBCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_B);
        }

        private void butNCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_N);
        }

        private void butMCharClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_M);
        }

        private void butEscClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);
        }

        private void butTabClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
        }

        private void butShiftChecked(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SHIFT);
        }

        private void butCtrlClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.CONTROL);
        }

        private void butAltClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MENU);
        }

        private void butSpaceClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
        }

        private void butBackslashClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.BACK);
        }

        private void butUnderscoreClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT, WindowsInput.Native.VirtualKeyCode.OEM_MINUS);
        }

        private void butColonClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT, WindowsInput.Native.VirtualKeyCode.OEM_PERIOD);
        }

        private void butPlusMinusClick(object sender, RoutedEventArgs e)
        {

        }

        private void butApostropheClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT, WindowsInput.Native.VirtualKeyCode.VK_2);
        }

        private void butLeftBracesClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.MENU, WindowsInput.Native.VirtualKeyCode.VK_8);
        }

        private void butRighttBracesClick(object sender, RoutedEventArgs e)
        {
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.MENU, WindowsInput.Native.VirtualKeyCode.VK_9);
        }

        private void butTempClick(object sender, RoutedEventArgs e)
        {

        }

    }
   
}
