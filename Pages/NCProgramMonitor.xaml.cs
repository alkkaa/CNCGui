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
using ICSharpCode;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Highlighting;
using keba.CNC.Afl;
using keba.CNC.Afm;
using keba.CNC.NCConverter;
using CNCGui.Programs;

namespace CNCGui.Pages
{
    /// <summary>
    /// Interaction logic for NCProgramMonitor.xaml
    /// </summary>
    public partial class NCProgramMonitor : Page
    {
        /* 
         * Variables 
         */
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
        static  int channel = 0;                    // channel 0
        static uint lcid = 0x7;                  // language german

        // CNC service
        LibAfl ctx = null;
        static CNCService cncsvc = null;

        public NCProgramMonitor()
        {
            InitializeComponent();
        }

        /* Program load */
        private void butLoadNCFileClick(object sender, RoutedEventArgs e)
        {
            LoadNCFile();
        }

        public void LoadNCFile()
        {
            ctx = LibAfl.GetInstance(channel, Programs.ControllerComms.strControllerIPAddress, lcid, IntPtr.Zero);
            cncsvc = ctx.GetCNCService();
            //new Task(GCodeObserverCreate).Start();
            //Task.Delay(500);
            new Task(LoadProgramToController).Start();
            Task.Delay(500);
            new Task(LoadProgramToEditor).Start();
            Task.Delay(500);
            cncsvc.GCodeDataChanged += cncsvc_GCodeDataChanged;
            cncsvc.SubscribeCNCData(CNCServiceSubscription.GCodeData);
            iHighlightLineNumber = 0;

            //CNCServiceProcess.LoadProgramToController();
            Console.WriteLine("NC file loaded");
        }

        /* NC Data degerlerini alma */
        private void cncsvc_GCodeDataChanged(object sender, GMDataEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                if (e.Process == 0)
                {
                    NCData data = e.MSTE;
                    CNCOperationMode cncOpMode = cncsvc.GetOperationMode();

                    /*
                     LineNumber 0, LineNumberStartProgram 1, FeedrateNominal 2, FeedrateActual 3, NestingLevel 4, M_Function 5, M_Address 6, S_Function 7, S_Address 8,
                     T_Function 9, T_Address 10, D_Function 11, D_Address 12, E_Function 13, E_Address 14, EstimatedProgamRunningTime 15, NoofElements 16
                    */

                    /* LineNumber */
                    if (data.values[0] > 0 && data.values[0] != iOldLineNumber & cncOpMode == CNCOperationMode.AutomaticRunning)
                    {
                        int iLineNum = data.values[0] - iOldLineNumber;
                        while (iLineNum > 0)
                        {
                            LineDown();
                            iLineNum--;
                        }
                        iOldLineNumber = data.values[0];
                    }
                }
            });
        }

        private void LoadProgramToEditor()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                string path = @"C:\Program Files\KEBA\KeStudio CNC Machine Setup\CustomData\Repository\User 1\Programs GM\kasa deneme1.nc";

                try
                {
                    var wMainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                    wMainWindow.lblNCFilePath.Content = path;
                    lblNCFilePath.Content = path;
                    teNCProgram.Load(path);
                }
                catch (Exception ex)
                {
                    var wMainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                    wMainWindow.lblNCFilePath.Content = "";
                    lblNCFilePath.Content = "";
                    MessageBox.Show($"Cannot open {path}! Error message:\n\n{ex.Message}", "Error");
                }
            });
        }

        public void LoadProgramToController()
        {
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
                gmconv.LineNumberChanged += gmconv_LineNumberChanged;

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

        }

        /* Program unload */
        private void butUnloadNCFileClick(object sender, RoutedEventArgs e)
        {
            UnloadNCFile();
        }

        public void UnloadNCFile()
        {
            cncsvc.ClearProgram();
            teNCProgram.Clear();
            iHighlightLineNumber = 0;
            iTotalGCodeLineNumber = 0L;

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                try
                {
                    var wMainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                    wMainWindow.lblNCFilePath.Content = "No NC file loaded";
                    lblNCFilePath.Content = "No NC file loaded";
                }
                catch (Exception ex)
                {
                    var wMainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
                    wMainWindow.lblNCFilePath.Content = "";
                    lblNCFilePath.Content = "";
                    MessageBox.Show($"NC file can not unloaded from controller! Error message:\n\n{ex.Message}", "Error");
                }
            });
            Console.WriteLine("NC file unloaded");
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

        
        private void gmconv_LineNumberChanged(object sender, LineNumberChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                iTotalGCodeLineNumber = e.LineNumber;
                lblTotalLine.Content = iTotalGCodeLineNumber.ToString();
            });
        }

        public void LineDown()
        {
            /*
             * Islenmekte olan satır eger donguden hizli giderse
             * geriye kalan satırlari boyamak icin farkı bulup
             * for dongusu ile boyamayi deneyebiliriz
             */
            
            
            iHighlightLineNumber += 1;
            if(iHighlightLineNumber > iTotalGCodeLineNumber)
            {
                iHighlightLineNumber = (int)iTotalGCodeLineNumber;
            }
            if(iHighlightLineNumber > 1)
            {
                teNCProgram.TextArea.TextView.BackgroundRenderers.Add(new HighlightCurrentLineBackgroundRenderer(teNCProgram, iHighlightLineNumber, iHighlightLineNumber - 1));
                teNCProgram.LineDown();
                if (iHighlightLineNumber < 15)
                {
                    teNCProgram.LineUp();
                }
                else
                {
                    if (!bScroolSetDone)
                    {
                        teNCProgram.ScrollTo(15, 0);
                        bScroolSetDone = true;
                    }
                }
            }
        }


        /*********************************************************************************************************************/

        public class OffsetColorizer : DocumentColorizingTransformer
        {
            public int StartOffset { get; set; }
            public int EndOffset { get; set; }

            protected override void ColorizeLine(DocumentLine line)
            {
                if (line.Length == 0)
                    return;

                if (line.Offset < StartOffset || line.Offset > EndOffset)
                    return;

                int start = line.Offset > StartOffset ? line.Offset : StartOffset;
                int end = EndOffset > line.EndOffset ? line.EndOffset : EndOffset;

                ChangeLinePart(start, end, element => element.TextRunProperties.SetForegroundBrush(Brushes.Red));
            }
        }

        /* Secilen karakter uzunlugunu renklendirir */
        public void SelectText(int offset, int length)
        {
            //Get the line number based off the offset.
            var line = teNCProgram.Document.GetLineByOffset(offset);
            var lineNumber = line.LineNumber;

            //Select the text.
            teNCProgram.SelectionStart = offset;
            teNCProgram.SelectionLength = length;

            //Scroll the textEditor to the selected line.
            var visualTop = teNCProgram.TextArea.TextView.GetVisualTopByDocumentLine(lineNumber);
            teNCProgram.ScrollToVerticalOffset(visualTop);
        }

        /******************************************************************************************************************/

        public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
        {
            private TextEditor _editor;
            private int _lineNumber;
            private int _lastlineNumber;

            public HighlightCurrentLineBackgroundRenderer(TextEditor editor, int LineNum, int LastLineNum)
            {
                _editor = editor;
                _lineNumber = LineNum;
                _lastlineNumber = LastLineNum;
            }


            public KnownLayer Layer
            {
                get { return KnownLayer.Caret; }
            }

            public void Draw(TextView textView, DrawingContext drawingContext)
            {
                if (_editor.Document == null)
                    return;

                textView.EnsureVisualLines();

                var currentLine = _editor.Document.GetLineByNumber(_lineNumber); /*_editor.CaretOffset*/
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
                {
                    drawingContext.DrawRectangle(
                        new SolidColorBrush(Color.FromArgb(0x20, 0, 0, 0xFF)), null,
                        new Rect(rect.Location, new Size(textView.ActualWidth - 32, rect.Height)));
                }

                var lastLine = _editor.Document.GetLineByNumber(_lastlineNumber);
                foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, lastLine))
                {
                    drawingContext.DrawRectangle(
                        new SolidColorBrush(Color.FromArgb(0x80, 0, 0x10, 0)), null,
                        new Rect(rect.Location, new Size(textView.ActualWidth - 32, rect.Height)));
                }
            }
        }

        private void butTextHighlightClick(object sender, RoutedEventArgs e)
        {
            /*
            OffsetColorizer myOffsetColorizer = new OffsetColorizer();

            myOffsetColorizer.StartOffset = 10;
            myOffsetColorizer.EndOffset = 50;

            teNCProgram.TextArea.TextView.LineTransformers.Add(myOffsetColorizer);
            */
            teNCProgram.TextArea.TextView.BackgroundRenderers.Add(new HighlightCurrentLineBackgroundRenderer(teNCProgram, iHighlightLineNumber, iHighlightLineNumber-1));
        }

        private void butLineDownClick(object sender, RoutedEventArgs e)
        {
            /*
             * Islenmekte olan satır eger donguden hizli giderse
             * geriye kalan satırlari boyamak icin farkı bulup
             * for dongusu ile boyamayi deneyebiliriz
             */

            iHighlightLineNumber += 1;
            teNCProgram.TextArea.TextView.BackgroundRenderers.Add(new HighlightCurrentLineBackgroundRenderer(teNCProgram, iHighlightLineNumber, iHighlightLineNumber-1));
            teNCProgram.LineDown();
            if (iHighlightLineNumber < 15)
            {
                teNCProgram.LineUp();
            }
            else
            {
                if (!bScroolSetDone)
                {
                    teNCProgram.ScrollTo(15, 0);
                    bScroolSetDone = true;
                }
                
            }
        }

        private void butScrollToClick(object sender, RoutedEventArgs e)
        {
            teNCProgram.ScrollToLine(iHighlightLineNumber); /* istenilen satırı ortaya hizalıyor ScrollToLine tipide var*/
        }

        private void butLineUpClick(object sender, RoutedEventArgs e)
        {
            teNCProgram.LineUp();
        }

    }
}
