using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Drawing.Image;
using Orientation = System.Windows.Forms.Orientation;

namespace BoardCast
{
    class ImageCaptureManager
    {
        public List<StackPanel> _mThumbnailsList;
        public static string date;

        public ImageCaptureManager()
        {
            _mThumbnailsList = new List<StackPanel>();
        }

        public string CaptureScreen()
        {
            date = DateTime.Now.ToString("ddMMyyyy");
            string fileName = DateTime.Now.ToString("hhmmss");
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
            if (Directory.Exists(GlobalContants.screenshotFolderPath))
                image.Save(Path.Combine(GlobalContants.screenshotFolderPath, fileName + ".jpeg"), ImageFormat.Jpeg);
            else
            {
                Directory.CreateDirectory(GlobalContants.screenshotFolderPath);
                image.Save(Path.Combine(GlobalContants.screenshotFolderPath, fileName + ".jpeg"), ImageFormat.Jpeg);
            }
            return fileName;
        }

        public StackPanel CreatePreviewThumbnail(string path)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
            System.Windows.Controls.Image Img = new System.Windows.Controls.Image();
            Img.Width = 150;
            Img.Height = 150;
            Img.Source = new BitmapImage(new Uri(path));
            sp.Children.Add(Img);
            sp.Margin = new Thickness(2, 0, 2, 50);
            Img.MouseDown += new MouseButtonEventHandler(OnThumbnailsClick);
            _mThumbnailsList.Add(sp);
            return sp;
        }

        void OnThumbnailsClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var image = sender as System.Windows.Controls.Image;
                // MessageBox.Show(image.Source.ToString());
                //_mProcessManager.GenerateProcess(image.Source.ToString(), true);
                ProcessStartInfo startInfo = new ProcessStartInfo(image.Source.ToString());
                startInfo.Verb = "edit";
                startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                Process.Start(startInfo);
                Thread.Sleep(1000);
                SendKeys.SendWait("{F11}");
            }
        }

        public void DeleteAllThumbnails()
        {
            for (int i = 0; i < _mThumbnailsList.Count; i++)
            {
                _mThumbnailsList[i].Children.RemoveAt(0);
            }
        }
    }
}
