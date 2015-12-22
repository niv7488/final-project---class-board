using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xml;


namespace paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //XYCoord.Content = Mouse.GetPosition(icanvas);
        }
        Window window;
        private string screenshotFolderPath = @"..\..\Screenshots\";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnLocationchanged(object sender, EventArgs e)
        {
            if (window != null)
                window.Close();
        }

        void NewWindowAsDialog(object sender, RoutedEventArgs e)
        {
            Window myOwnedDialog = new Window();
            myOwnedDialog.Owner = this;
            myOwnedDialog.ShowDialog();
        }
        void NormalNewWindow(object sender, RoutedEventArgs e)
        {
            Window myOwnedWindow = new Window();
            myOwnedWindow.Owner = this;
            myOwnedWindow.Show();
        }

        private void icanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(this);

            XCoord.Content = position.X;
            YCoord.Content = position.Y;

        }

        private void Black_Click(object sender, RoutedEventArgs e)
        {
            icanvas.DefaultDrawingAttributes.Color = Colors.Black;

        }

        private void White_Click(object sender, RoutedEventArgs e)
        {

            icanvas.DefaultDrawingAttributes.Color = Colors.White;
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            icanvas.DefaultDrawingAttributes.Color = Colors.Red;
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            icanvas.DefaultDrawingAttributes.Color = Colors.Yellow;
        }

        private void Orange_Click(object sender, RoutedEventArgs e)
        {
            icanvas.DefaultDrawingAttributes.Color = Colors.Orange;
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            icanvas.DefaultDrawingAttributes.Color = Colors.Green;
        }


        private void Smaller_OnClick_Click(object sender, RoutedEventArgs e)
        {
            icanvas.DefaultDrawingAttributes.Width = 1;
            icanvas.DefaultDrawingAttributes.Height = 1;
        }

        private void Bigger_OnClick_Click(object sender, RoutedEventArgs e)
        {
            icanvas.DefaultDrawingAttributes.Width = 5;
            icanvas.DefaultDrawingAttributes.Height = 5;
        }
        //saves the screen shot image at a push of a button (Export Image button) - saves it in the choosen folder
        private void OnExport_Click(object sender, RoutedEventArgs e)
        {

            RenderTargetBitmap rtb = new RenderTargetBitmap((int) tcanvas.ActualWidth, (int) tcanvas.ActualHeight, 96d,
                96d, PixelFormats.Default);
            rtb.Render(tcanvas);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            string fileName = screenshotFolderPath+DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg";
            FileStream fs = File.Open(fileName, mode: FileMode.Create);
            encoder.Save(fs);
            fs.Close();
        }

        //saves the XAML file of the screen shot - same as the image
        private void OnExportCanvas_Click(object sender, RoutedEventArgs e)
        {
            string xaml = XamlWriter.Save(tcanvas);
            File.WriteAllText(
                @"C:\Users\niv\Documents\Visual Studio 2013\Projects\paint2.0\paint\paint\Images\testcanvas.xaml", xaml);
        }
        //C:\Users\niv\Documents\Visual Studio 2013\Projects\paint
        private void OnImportCanvas_Click(object sender, RoutedEventArgs e)
        {
            /*StreamResourceInfo sr = Application.GetResourceStream(new Uri("paint;component/Graphics/testcanvas.xaml", UriKind.Relative));
            InkCanvas result = (InkCanvas)XamlReader.Load(new XmlTextReader(sr.Stream));
            tcanvas.Children.Add(result);
            */
            UIElement rootElement;
            FileStream s = new FileStream(@"C:\Users\niv\Documents\Visual Studio 2013\Projects\paint2.0\paint\paint\Images\testcanvas.xaml", FileMode.Open);
            rootElement = (UIElement)XamlReader.Load(s);
            s.Close();
            tcanvas.Children.Add(rootElement);
            /*InkCanvas c = new InkCanvas();
            
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(@"d:\" + @"test.bmp", UriKind.Relative));
            tcanvas.Background = brush;*/

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            //window = new Window();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            //this.LocationChanged += OnLocationchanged;
            this.Show();
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            Process virtualMouse = new Process();

            virtualMouse.StartInfo.FileName = FileNameTextBox.Text; // Needs to be full path
            virtualMouse.StartInfo.Arguments = ""; // If you have any arguments

            bool result = virtualMouse.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            /*dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";*/

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                FileNameTextBox.Text = filename;
            }
        }
        /*Image to base64 string function*/
        private void Base64_click(object sender, RoutedEventArgs e) {

        }

        private void OnImageToBase64_Click(object sender, RoutedEventArgs e) {
            string test = @"C:\Users\niv\Documents\Visual Studio 2013\Projects\final-project---class-board\paint_GitHub\paint\Images\unnamed.png";
            //var fileStream = new FileStream(test, FileMode.Open, FileAccess.Read);
            //the path is the folder that saves the Export image screen shot
            //byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(test);
            byte[] bytes = File.ReadAllBytes(test);
            Console.WriteLine(bytes.Length);
            string base64String = System.Convert.ToBase64String(bytes);
            Console.WriteLine("Base 64 string: " + base64String);

            //opening http connection
            WebRequest req = WebRequest.Create("https://boardcast-ws.herokuapp.com/"); //  + "?key=" + Tags.apiKey);
            // Console.WriteLine(req.RequestUri);
            req.Method = "GET";
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            Stream dataStream = res.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string result = reader.ReadToEnd();
            dataStream.Close();
            reader.Close();
            res.Close();
            Console.WriteLine(result);

        }

        
    }

}
