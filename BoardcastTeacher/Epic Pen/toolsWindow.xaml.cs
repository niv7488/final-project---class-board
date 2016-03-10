using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Core;
using Button = System.Windows.Controls.Button;
using Cursors = System.Windows.Input.Cursors;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Rectangle = System.Windows.Shapes.Rectangle;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Epic_Pen
{
    /// <summary>
    /// Interaction logic for toolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        public static int courseID;
        private string screenshotFolderPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
        public string fileName,date;
        InkCanvas inkCanvas;
        private InkCanvas bgCanvas;
        public ToolsWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        Microsoft.Office.Interop.PowerPoint.Application oPPT;
        Microsoft.Office.Interop.PowerPoint.Presentations objPresSet;
        Microsoft.Office.Interop.PowerPoint.Presentation objPres;
        Microsoft.Office.Interop.PowerPoint.SlideShowView oSlideShowView;

        public void setInkCanvas(InkCanvas _inkCanvas)
        { inkCanvas = _inkCanvas; }


        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Top = desktopWorkingArea.Bottom - Height+40;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Border) Color).Background = ((Border) sender).Background;
            colorPanel.Visibility = Visibility.Hidden;
            selectedColourBorder.Background = ((Border)sender).Background;
            inkCanvas.DefaultDrawingAttributes.Color = ((SolidColorBrush)((Border)sender).Background).Color;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //System.Media.SystemSounds.Asterisk.Play();
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        public event EventHandler CloseButtonClick;

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            onCloseButtonClick();
        }

        void onCloseButtonClick()
        {
            if (CloseButtonClick != null)
                CloseButtonClick.Invoke(new object(), new EventArgs());
        }

        private void resetAllToolBackgrounds()
        {
            foreach (Button i in toolStackPanel.Children)
               // if (i.Name != "brushSize")
                i.Style = defaultButtonStyle;
        }

        public void cursorButton_Click(object sender, RoutedEventArgs e)
        {
            resetAllToolBackgrounds();
            cursorButton.Style = (Style)FindResource("highlightedButtonStyle");
        }
        public void penButton_Click(object sender, RoutedEventArgs e)
        {
            
            inkCanvas.Cursor = Cursors.Pen;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
            setBrushSize();
            resetAllToolBackgrounds();
            penButton.Style = (Style)FindResource("highlightedButtonStyle");

        }

        public void highlighterButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Cursor = Cursors.Pen;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = true;
            setBrushSize();
            resetAllToolBackgrounds();
            highlighterButton.Style = (Style)FindResource("highlightedButtonStyle");

        }
        
        public void eraserButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Cursor = Cursors.Cross;
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            setBrushSize();
            resetAllToolBackgrounds();
            eraserButton.Style = (Style)FindResource("highlightedButtonStyle");   
        }
        
        public void eraseAllButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }
        double penSize=3;
        private void penSizeButton_MouseDown(object sender, RoutedEventArgs e)
        {
            brushSizeStackPanel.Visibility = Visibility.Hidden;
            penSize = ((Ellipse)((Button)sender).Content).Width;

            ((Ellipse) ((Button) brushSize).Content).Width = penSize;
            ((Ellipse) ((Button) brushSize).Content).Height = ((Ellipse) ((Button) sender).Content).Height;
            setBrushSize();

            foreach (Button i in brushSizeStackPanel.Children)
                i.Style = defaultButtonStyle;
            ((Button)sender).Style = (Style)FindResource("highlightedButtonStyle");   
        }

        private void setBrushSize()
        {
            if (inkCanvas.Cursor == Cursors.Cross)
            {
                inkCanvas.DefaultDrawingAttributes.Width = penSize * 5;
                inkCanvas.DefaultDrawingAttributes.Height = penSize * 5;
            }
            else
            {
                inkCanvas.DefaultDrawingAttributes.Width = penSize;
                inkCanvas.DefaultDrawingAttributes.Height = penSize;
            }
        }

        private void clickThroughCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if ((bool)hideInkCheckBox.IsChecked)
            {
                //toolsDockPanel.Height = 0;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = toolsDockPanelDefaultHeight;
                doubleAnimation.To = 0;
                doubleAnimation.Duration = new Duration(new TimeSpan(0,0,0,0,200));
                ExponentialEase expoEase = new ExponentialEase();
                expoEase.Exponent = 7;
                doubleAnimation.EasingFunction = expoEase;
                //Storyboard.SetTargetName(doubleAnimation, toolsDockPanel.Name);
                Storyboard.SetTarget(doubleAnimation, toolsDockPanel);
                Rectangle rect = new Rectangle();
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(DockPanel.HeightProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }
            else
            {
                //toolsDockPanel.Height = double.NaN;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 0;
                doubleAnimation.To = toolsDockPanelDefaultHeight;
                doubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0,0, 200));
                ExponentialEase expoEase = new ExponentialEase();
                expoEase.Exponent = 7;
                doubleAnimation.EasingFunction = expoEase;
                //Storyboard.SetTargetName(doubleAnimation, toolsDockPanel.Name);
                Storyboard.SetTarget(doubleAnimation, toolsDockPanel);
                Rectangle rect = new Rectangle();
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(DockPanel.HeightProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }

        }
        Style defaultButtonStyle;
        double toolsDockPanelDefaultHeight;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            toolsDockPanel.Height = toolsDockPanel.ActualHeight;
            toolsDockPanelDefaultHeight = toolsDockPanel.Height;
            Height = ActualHeight;
            SizeToContent = System.Windows.SizeToContent.Manual;
            defaultButtonStyle = eraseAllButton.Style;
        }

        private void BrushSize_OnClick(object sender, RoutedEventArgs e)
        {
            if(brushSizeStackPanel.IsVisible)
                brushSizeStackPanel.Visibility = Visibility.Hidden;
            else
                brushSizeStackPanel.Visibility=Visibility.Visible;
        }

        private void onColorClick(object sender, MouseButtonEventArgs e)
        {
            if (colorPanel.IsVisible)
                colorPanel.Visibility = Visibility.Hidden;
            else
                colorPanel.Visibility = Visibility.Visible;
        }

        private void onPPTClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("ppt clicked");
        }

        private void onCaptureClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
             date = DateTime.Now.ToString("ddMMyyyy");
            fileName = screenshotFolderPath + DateTime.Now.ToString("hhmmss");
            courseID = 1234;
            int ix, iy, iw, ih;
            ix = Convert.ToInt32(Screen.PrimaryScreen.Bounds.X);
            iy = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Y);
            iw = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width);
            ih = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height);
            Bitmap image = new Bitmap(iw, ih,
                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(ix, iy, ix, iy,
                     new System.Drawing.Size(iw, ih),
                     CopyPixelOperation.SourceCopy);
            if (Directory.Exists(screenshotFolderPath))
                image.Save(System.IO.Path.Combine(screenshotFolderPath, fileName + ".jpeg"), ImageFormat.Jpeg);
            else
            {
                Directory.CreateDirectory(screenshotFolderPath);
                image.Save(System.IO.Path.Combine(screenshotFolderPath, fileName + ".jpeg"), ImageFormat.Jpeg);
            }
            this.Show();
            inkCanvas.Strokes.Clear();
            MessageBox.Show(System.IO.Path.Combine(screenshotFolderPath, fileName + ".jpeg"));
            Thread Base64converter = new Thread(Base64Thread);
            Base64converter.Start();
        }
#region Base64 convert+Upload
        private void Base64Thread()
        {
            //the path is the folder that saves the Export image screen shot
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(screenshotFolderPath, fileName + ".png")));
            Console.WriteLine("Bytes Length " + bytes.Length);
            string base64String = System.Convert.ToBase64String(bytes);

            //serialize the json so that the server will know what values we sent
            string json = new JavaScriptSerializer().Serialize(new
            {
                base64 = base64String,                  //the picture after transfoming into base64 string
                filename = fileName,                 //the name of the pic-->need to be changed according to each pic
                cours_id = courseID,
                date = date
            });
            //opening a connection with the server
            var baseAddress = "https://boardcast-ws.herokuapp.com/decode64/";
            //deffine the request methood
            //var http = HttpWebRequest.Create(new Uri(baseAddress));
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";

            string parsedContent = json;
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes1 = encoding.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes1, 0, bytes1.Length);
            newStream.Close();

            var response2 = http.GetResponse();

            var stream = response2.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            Console.WriteLine(content);
            Console.WriteLine(DateTime.Now);
        }
#endregion
        #region PPT Handler

        /// <summary>
        /// Open Microsoft office Power point 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPPT(object sender, RoutedEventArgs e)
        {
            try
            {
                //Create an instance of PowerPoint.
                oPPT = new Microsoft.Office.Interop.PowerPoint.Application();
                // Show PowerPoint to the user.
                oPPT.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                objPresSet = oPPT.Presentations;


                OpenFileDialog Opendlg = new OpenFileDialog();

                Opendlg.Filter = "Powerpoint|*.ppt;*.pptx|All files|*.*";

                // Open file when user  click "Open" button  
                if (Opendlg.ShowDialog() == true)
                {
                    string pptFilePath = Opendlg.FileName;
                    //open the presentation
                    objPres = objPresSet.Open(pptFilePath, MsoTriState.msoFalse,
                    MsoTriState.msoTrue, MsoTriState.msoTrue);

                    objPres.SlideShowSettings.ShowPresenterView = MsoTriState.msoFalse;
                    System.Diagnostics.Debug.WriteLine(objPres.SlideShowSettings.ShowWithAnimation);
                    objPres.SlideShowSettings.Run();

                    oSlideShowView = objPres.SlideShowWindow.View;
                    PowerPointPanel.Visibility = Visibility.Visible;

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to open Power Point, please make sure you have the program installed correctly");
            }
            
        }

        /// <summary>
        /// Forward to next slide in presentation, save screenshot if notation canvas detect as not empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            string lastSlide = oSlideShowView.Slide.SlideNumber.ToString();
            string currentSlide;
            oSlideShowView.Application.SlideShowWindows[1].Activate();
            if (inkCanvas.Strokes.Count > 0)
                onCaptureClick(sender,e);
            oSlideShowView.Next();
        }

        /// <summary>
        /// Load previous slide in presentation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            //oSlideShowView.Application.SlideShowWindows[1].Activate();
            oSlideShowView.Previous();
        }

        /// <summary>
        /// Exit Power point application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitPowerPoint(object sender, RoutedEventArgs e)
        {
            oSlideShowView.Exit();
            objPres.Close();
            PowerPointPanel.Visibility=Visibility.Hidden;
            Process[] pros = Process.GetProcesses();
            for (int i = 0; i < pros.Count(); i++)
            {
                if (pros[i].ProcessName.ToLower().Contains("powerpnt"))
                {
                    pros[i].Kill();
                }
            }

        }
#endregion

        
    }
}
