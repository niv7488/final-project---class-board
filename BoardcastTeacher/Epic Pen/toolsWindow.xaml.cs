using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Core;
using Microsoft.Win32;

namespace Epic_Pen
{
    /// <summary>
    /// Interaction logic for toolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        InkCanvas inkCanvas;
        private InkCanvas bgCanvas;
        public ToolsWindow()
        {
            InitializeComponent();
        }

        Microsoft.Office.Interop.PowerPoint.Application oPPT;
        Microsoft.Office.Interop.PowerPoint.Presentations objPresSet;
        Microsoft.Office.Interop.PowerPoint.Presentation objPres;
        Microsoft.Office.Interop.PowerPoint.SlideShowView oSlideShowView;

        public void setInkCanvas(InkCanvas _inkCanvas)
        { inkCanvas = _inkCanvas; }




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
            MessageBox.Show("ScreenSHot!");
        }

        private void OpenPPT(object sender, RoutedEventArgs e)
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
                PowerPointPanel.Visibility=Visibility.Visible;

            }
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            string lastSlide = oSlideShowView.Slide.SlideNumber.ToString();
            string currentSlide;
            oSlideShowView.Application.SlideShowWindows[1].Activate();
            oSlideShowView.Next();
            currentSlide = oSlideShowView.Slide.SlideNumber.ToString();
            if (currentSlide != lastSlide)
                MessageBox.Show("New page!");
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            //oSlideShowView.Application.SlideShowWindows[1].Activate();
            oSlideShowView.Previous();
        }


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
    }
}
