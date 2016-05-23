using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CustomControls;
using CustomControls.Controls;
using CustomControls.OS;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Application = System.Windows.Forms.Application;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using Cursors = System.Windows.Input.Cursors;
using GradientStop = System.Windows.Media.GradientStop;
using Image = System.Windows.Controls.Image;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Orientation = System.Windows.Controls.Orientation;
using Path = System.IO.Path;
using Rectangle = System.Windows.Shapes.Rectangle;
using Shape = System.Windows.Shapes.Shape;


namespace BoardCast
{
    /// <summary>
    /// Interaction logic for toolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        private InkCanvas inkCanvas;
        private InkCanvas bgCanvas;
        private Thread FileUploadThread;
        private Thickness toolsDockMargin;
        private ProcessManager _mProcessManager;
        private ImageCaptureManager _mImageCaptureManager;
        private PowerPointManager _mPowerPointManager;
        private int courseID;
        private bool isLastCanvasSaved = false;
        double penSize=3;
        double toolsDockPanelDefaultHeight;
        Style defaultButtonStyle;
        private StrokeCollection _mTempStrokeCollection;
        private IReadOnlyCollection<UIElement> _mInkCanvasChildrensElements; 
        public static string date;
        public string fileName { get; set; }
        public bool isTempCanvasOpen { get; set; }
        public string lastSavedCanvasName { get; set; }
        public enum SelectedShape
        { None, Circle, Rectangle, Triangle , Line ,  }
        public SelectedShape Shape1 = SelectedShape.None;

#region EventHandlers
        public event EventHandler CloseButtonClick;
        public event EventHandler HideInkCanvas;
        public event EventHandler CreateBlankCanvasClick;
#endregion

#region Window Initialization
        public ToolsWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        public void setInkCanvas(InkCanvas _inkCanvas)
        { inkCanvas = _inkCanvas; }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Top = desktopWorkingArea.Bottom - Height+40;
            WindowStyle = WindowStyle.None;
            this.Width =SystemParameters.PrimaryScreenWidth;
            ResizeMode = ResizeMode.NoResize;
            FileUploadThread = new Thread(UploadManager.Instance.Main);
            FileUploadThread.Start();
            _mProcessManager = new ProcessManager();
            _mImageCaptureManager = new ImageCaptureManager();
            _mPowerPointManager = new PowerPointManager();
            isTempCanvasOpen = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            toolsDockPanel.Height = toolsDockPanel.ActualHeight;
            toolsDockPanelDefaultHeight = toolsDockPanel.Height;
            Height = ActualHeight;
            SizeToContent = System.Windows.SizeToContent.Manual;
            defaultButtonStyle = eraseAllButton.Style;
        }

        public void setCourseID(int cID)
        {
            courseID = cID;
        }
#endregion

#region ToolWindowDesign

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //((Border)Color).Background = ((Border) sender).Background;
            SelectedColorRect.Fill = ((Border) sender).Background;
            SelectedColorRect.Stroke = ((Border) sender).Background;
            colorPanel.Visibility = Visibility.Hidden;
            selectedColourBorder.Background = ((Border)sender).Background;
            inkCanvas.DefaultDrawingAttributes.Color = ((SolidColorBrush)((Border)sender).Background).Color;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            //System.Media.SystemSounds.Asterisk.Play();
            /*if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();*/
        }
        
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mProcessManager.CloseAllProcess();
            _mImageCaptureManager.DeleteAllThumbnails();
            onCloseButtonClick();
        }

        private void onCloseButtonClick()
        {
            if (CloseButtonClick != null)
                CloseButtonClick.Invoke(new object(), new EventArgs());
        }

        private void resetAllToolBackgrounds()
        {
            foreach (Button i in toolStackPanel.Children)
               // if (i.Name != "brushSize")
                if (!(isTempCanvasOpen && i.Name.Equals("createBlankBackground")))
                    i.Style = defaultButtonStyle;
        }

        private void openFilesLeft_Click(object sender, RoutedEventArgs e)
        {
            WinApplicationsRight.Visibility = Visibility.Hidden;
            if (WinApplicationsLeft.Visibility == Visibility.Visible)
            {
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5f));
                WinApplicationsLeft.BeginAnimation(Grid.OpacityProperty, animation);
                WinApplicationsLeft.Visibility = Visibility.Hidden;
            }
            else
            {
                DoubleAnimation animation = new DoubleAnimation(1,TimeSpan.FromSeconds(0.5f));
                WinApplicationsLeft.Visibility = Visibility.Visible;
                WinApplicationsLeft.BeginAnimation(Grid.OpacityProperty,animation);
            }
        }

        private void openFilesRight_Click(object sender, RoutedEventArgs e)
        {
            WinApplicationsLeft.Visibility = Visibility.Hidden;
            if (WinApplicationsRight.Visibility == Visibility.Visible)
            {
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5f));
                WinApplicationsRight.BeginAnimation(Grid.OpacityProperty, animation);
                WinApplicationsRight.Visibility = Visibility.Hidden;
            }
            else
            {
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5f));
                WinApplicationsRight.Visibility = Visibility.Visible;
                WinApplicationsRight.BeginAnimation(Grid.OpacityProperty, animation);
            }
        }

        /// <summary>
        /// open last screenshots thumbnail button listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadLastShotsClicked(object sender, RoutedEventArgs e)
        {
            if (LastShotsScrollViewer.Visibility == Visibility.Visible)
            {
                LastShotsScrollViewer.Visibility = Visibility.Hidden;
            }
            else
            {
                LastShotsScrollViewer.Visibility = Visibility.Visible;
            }
        }

        private void HideAllSubMenus()
        {
            PowerPointPanel.Visibility = Visibility.Hidden;
            deleteStrokePanel.Visibility = Visibility.Hidden;
            colorPanel.Visibility = Visibility.Hidden;
            brushSizeStackPanel.Visibility = Visibility.Hidden;
            LastShotsScrollViewer.Visibility = Visibility.Hidden;
            WinApplicationsLeft.Visibility = Visibility.Hidden;
            WinApplicationsRight.Visibility = Visibility.Hidden;
            shapesStackPanel.Visibility = Visibility.Hidden;
        }

        private Collection<UIElement> GetAllCanvasChildrens()
        {
            Collection<UIElement> elementCollection = new Collection<UIElement>();
            for (int i = 0; i < inkCanvas.Children.Count; i++)
            {
                elementCollection.Add(inkCanvas.Children[i]);
            }
            return elementCollection;
        }

#endregion
        
#region onClickHandlers
        
        public void cursorButton_Click(object sender, RoutedEventArgs e)
        {
            resetAllToolBackgrounds();
            cursorButton.Style = (Style)FindResource("highlightedButtonStyle");
            HideAllSubMenus();
        }

        public void penButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            inkCanvas.Cursor = Cursors.Pen;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
            setBrushSize();
            resetAllToolBackgrounds();
            penButton.Style = (Style)FindResource("highlightedButtonStyle");

        }

        public void highlighterButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            inkCanvas.Cursor = Cursors.Pen;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = true;
            setBrushSize();
            resetAllToolBackgrounds();
            highlighterButton.Style = (Style)FindResource("highlightedButtonStyle");
        }

        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Cursor = Cursors.Cross;
            inkCanvas.EditingMode = InkCanvasEditingMode.Select;
            HideAllSubMenus();
            /*inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = true;*/
            setBrushSize();
            resetAllToolBackgrounds();
            editbutton.Style = (Style)FindResource("highlightedButtonStyle");
            deleteStrokePanel.Visibility = Visibility.Visible;
        }
        
        public void eraserButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            inkCanvas.Cursor = Cursors.Cross;
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            setBrushSize();
            resetAllToolBackgrounds();
            eraserButton.Style = (Style)FindResource("highlightedButtonStyle");   
        }
        
        public void eraseAllButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            inkCanvas.Strokes.Clear();
            inkCanvas.Children.Clear();
        }
        
        private void penSizeButton_MouseDown(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
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
            HideAllSubMenus();
            if ((bool)hideInkCheckBox.IsChecked)
            {
                toolsDockPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                toolsDockPanel.Visibility = Visibility.Visible;
            }
        }

        private void OnDeleteStroke(object sender, RoutedEventArgs routedEventArgs)
        {
            inkCanvas.Strokes.Remove(inkCanvas.GetSelectedStrokes());
            for (int i = 0; i < inkCanvas.GetSelectedElements().Count; i++)
            {
                inkCanvas.Children.Remove(inkCanvas.GetSelectedElements()[i]);
            }
        }

        /// <summary>
        /// Brush size button listener to open size select panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrushSize_OnClick(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            brushSizeStackPanel.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// Color button listener to open panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onColorClick(object sender, RoutedEventArgs routedEventArgs)
        {
            HideAllSubMenus();
            colorPanel.Visibility = Visibility.Visible;
        }

        private void OnOpenFileClicked(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            HideAllSubMenus();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All files (*.*)|*.*";
            // Get the selected file name and display in a TextBox 
            Nullable<bool> result = openFileDialog1.ShowDialog();
            // Get the selected file name
            if (result == true)
            {
                string filename = openFileDialog1.FileName;
                _mProcessManager.GenerateProcess(filename,false);
            }
        }

        void OnOpenImageClicked(object sender, MouseButtonEventArgs e)
        {
            HideAllSubMenus();
            FormOpenFileDialog controlex = new FormOpenFileDialog();
            controlex.StartLocation = AddonWindowLocation.Right;
            controlex.DefaultViewMode = FolderViewMode.Thumbnails;
            controlex.OpenDialog.InitialDirectory = GlobalContants.screenshotFolderPath;
            controlex.OpenDialog.AddExtension = true;
            controlex.OpenDialog.Filter = "Image Files(*.bmp;*.jpg;*.gif;*.png; *.jpeg)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            var result = controlex.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string filename = controlex.OpenDialog.FileName;
                _mProcessManager.GenerateProcess(filename, true);
                Thread.Sleep(1000);
                SendKeys.SendWait("{F11}");
            }
        }

        /// <summary>
        /// Create blank background on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void onCreateBlankCanvasClicked(object sender, RoutedEventArgs e)
        {
            //Check if the transparent canvas is not blank
            if (inkCanvas.Strokes.Count > 0 || inkCanvas.Children.Count > 0)
            {
                if (!isTempCanvasOpen)
                    isLastCanvasSaved = true;

                onCaptureClick(sender, e);
            }
            if (!isTempCanvasOpen)
            {
                isTempCanvasOpen = true;
                createBlankBackground.Style = (Style) FindResource("highlightedButtonStyle");
            }
            else
            {
                isTempCanvasOpen = false;
                if (_mTempStrokeCollection != null && _mTempStrokeCollection.Count > 0)
                {
                    inkCanvas.Strokes = _mTempStrokeCollection;
                }
                if (_mInkCanvasChildrensElements.Count > 0)
                {
                    foreach (var element in _mInkCanvasChildrensElements)
                    {
                        inkCanvas.Children.Add(element);
                    }
                }
                _mTempStrokeCollection = null;
                _mInkCanvasChildrensElements = null;
                createBlankBackground.Style = defaultButtonStyle;
            }

            //Fire event to create blank background window
            if (CreateBlankCanvasClick != null)
                CreateBlankCanvasClick.Invoke(new object(), new EventArgs());
        }

        private void OpenBrowser(object sender, MouseButtonEventArgs e)
        {
            HideAllSubMenus();
            _mProcessManager.GenerateProcess("http://google.com", false);
        }

#endregion
        
#region imageCapture
        /// <summary>
        /// Capture screenshot, saves it locally and add to upload manager stack
        /// saves the picture on date time format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onCaptureClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            fileName = _mImageCaptureManager.CaptureScreen();
            ExportCanvasToFile();
            string fullFilePath = Path.Combine(GlobalContants.screenshotFolderPath, fileName + ".jpeg");
            UploadManager.Instance.setCourseID(courseID);
            UploadManager.Instance.uploadFilesStack.Push(fullFilePath);
            //cursorButton_Click(sender,e);
            if (UploadManager.Instance.isThreadSleep)
            {
                FileUploadThread.Interrupt();
            }
            itemsControl.Items.Add(_mImageCaptureManager.CreatePreviewThumbnail(fullFilePath));
        }

        /// <summary>
        /// Export transparent canvas layer to local xaml file
        /// </summary>
        public void ExportCanvasToFile()
        {
            string xaml = XamlWriter.Save(inkCanvas);
            if (Directory.Exists(GlobalContants.canvasFolderPath))
                File.WriteAllText(Path.Combine(GlobalContants.canvasFolderPath, fileName + ".xaml"), xaml);
            else
            {
                Directory.CreateDirectory(GlobalContants.canvasFolderPath);
                File.WriteAllText(Path.Combine(GlobalContants.canvasFolderPath, fileName + ".xaml"), xaml);
            }

            if (isLastCanvasSaved)
            {
                lastSavedCanvasName = Path.Combine(GlobalContants.canvasFolderPath, fileName + ".xaml");
                isLastCanvasSaved = false;
            }
            if (inkCanvas.Strokes.Count > 0)
            {
                _mTempStrokeCollection = new StrokeCollection();
                _mTempStrokeCollection = inkCanvas.Strokes.Clone();
            }
            if (inkCanvas.Children.Count > 0)
            {
                _mInkCanvasChildrensElements = GetAllCanvasChildrens();
            }
            
            inkCanvas.Strokes.Clear();
            inkCanvas.Children.Clear();

            this.Show();
        }

#endregion

#region PPT Handler

        /// <summary>
        /// Open Microsoft office Power point 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPPT(object sender, MouseButtonEventArgs e)
        {
            WinApplicationsLeft.Visibility = Visibility.Hidden;
            WinApplicationsRight.Visibility = Visibility.Hidden;
            if (_mPowerPointManager.OpenPowerPoint())
            {
                PowerPointPanel.Visibility = Visibility.Visible;
            }

            /*try
            {
                //Create an instance of PowerPoint.
                oPPT = new Microsoft.Office.Interop.PowerPoint.Application();
                // Show PowerPoint to the user.
                oPPT.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                objPresSet = oPPT.Presentations;
                //oPPT.SlideShowNextClick += this.SlideShowNextClick;

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
            }*/
        }

        /// <summary>
        /// Forward to next slide in presentation, save screenshot if notation canvas detect as not empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnNextClicked(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.Strokes.Count > 0)
            {
                _mImageCaptureManager.CaptureScreen();
            }

            _mPowerPointManager.ShowNextSlide();
                //onCaptureClick(sender, e);
            /*string lastSlide = oSlideShowView.Slide.SlideNumber.ToString();
            string currentSlide;
            oSlideShowView.Application.SlideShowWindows[1].Activate();
            
            oSlideShowView.Next();
            */
        }

        /// <summary>
        /// Load previous slide in presentation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            _mPowerPointManager.ShowPreviousSlide();
            //oSlideShowView.Application.SlideShowWindows[1].Activate();

            //oSlideShowView.Previous();
        }

        /// <summary>
        /// Exit Power point application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitPowerPoint(object sender, RoutedEventArgs e)
        {
            /*oSlideShowView.Exit();
            objPres.Close();*/
            _mPowerPointManager.ClosePowerPoint();
            PowerPointPanel.Visibility=Visibility.Hidden;
        }
#endregion

#region ShapesHandler
        private void onShapeSelect(object sender, RoutedEventArgs e)
        {
            if (shapesStackPanel.IsVisible)
            {
                shapesStackPanel.Visibility= Visibility.Hidden;
            }
            else
                shapesStackPanel.Visibility = Visibility.Visible;
        }

        private void OnCircleDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Line;
            DrawShape();
        }

        private void OnRectangleDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Rectangle;
            DrawShape();
        }

        private void OnTriangleDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Triangle;
            DrawShape();
        }

        private void OnLineDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Line;
            DrawShape();
        }

        private void DrawShape()
        {
            Shape Rendershape = null;

            switch (Shape1)
            {

                case SelectedShape.Circle:
                    Rendershape = new Ellipse() { Height = 100, Width = 100 };
                    RadialGradientBrush Ellipsebrush = new RadialGradientBrush();
                    Ellipsebrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 255), 0));
                    Rendershape.Stroke = SelectedColorRect.Stroke;
                    Rendershape.StrokeThickness = 2;
                    Rendershape.Fill = Ellipsebrush;
                    break;
                case SelectedShape.Rectangle:
                    Rendershape = new Rectangle() { Height = 100, Width = 100, RadiusX = 12, RadiusY = 12 };
                    RadialGradientBrush Rectbrush = new RadialGradientBrush();
                    Rectbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 255), 0));
                    Rendershape.Stroke = SelectedColorRect.Stroke;
                    Rendershape.StrokeThickness = 2;
                    Rendershape.Fill = Rectbrush;
                    break;

                case SelectedShape.Line:
                    Rendershape = new Rectangle() { Height = 1, Width = 100, RadiusX = 12, RadiusY = 12 };
                    RadialGradientBrush Linebrush = new RadialGradientBrush();
                    Linebrush.GradientStops.Add(new GradientStop(Color.FromRgb(0,0,0), 0));
                    Rendershape.Fill = Linebrush;
                    Rendershape.Stroke = SelectedColorRect.Stroke;
                    Rendershape.StrokeThickness = 2;
                    break;

                case SelectedShape.Triangle:
                    PointCollection myPointCollection = new PointCollection();
                    myPointCollection.Add(new System.Windows.Point(0,0));
                    myPointCollection.Add(new System.Windows.Point(50,100));
                    myPointCollection.Add(new System.Windows.Point(100,0));
                    Rendershape = new Polygon() { Height = 100, Width = 100, Points =myPointCollection };
                    Rendershape.Stroke = SelectedColorRect.Stroke;
                    Rendershape.StrokeThickness = 2;
                    break;
                default:
                    return;
            }
            InkCanvas.SetTop(Rendershape,300);
            InkCanvas.SetLeft(Rendershape, 300);
            inkCanvas.Children.Add(Rendershape);
        }

#endregion

        
    }
}
