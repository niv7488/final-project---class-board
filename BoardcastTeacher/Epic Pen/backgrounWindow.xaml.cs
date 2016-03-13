using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BoardCast
{
    /// <summary>
    /// Interaction logic for backgrounWindow.xaml
    /// </summary>
    public partial class backgroundWindow : Window
    {
        private InkCanvas inkCanvas;
        public backgroundWindow()
        {
            InitializeComponent();
            OnWindowLoad();
        }



        void OnWindowLoad()
        {
            
           /* ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"/whiteBg.jpg", UriKind.Relative));
            MessageBox.Show(Directory.GetCurrentDirectory());
            InkCanvas cnvs = new InkCanvas();
            cnvs.Background = imageBrush;*/

        }
    }
}
