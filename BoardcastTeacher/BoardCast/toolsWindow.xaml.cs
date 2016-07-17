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
        private InkCanvas m_inkCanvas;
        private InkCanvas m_bgCanvas;
        private Thread m_FileUploadThread;
        private Thickness m_toolsDockMargin;
        private ProcessManager m_ProcessManager;
        private ImageCaptureManager m_ImageCaptureManager;
        private PowerPointManager m_PowerPointManager;
        private int m_iCourseID;
        private bool m_bIsLastCanvasSaved = false;
        private double m_dPenSize=3;
        private double m_dToolsDockPanelDefaultHeight;
        private Style m_defaultButtonStyle;
        private List<CustomStroke> m_SavedStrokesList = new List<CustomStroke>();
        private StrokeCollection m_NewStrokeCollection;
        private StrokeCollection m_tempStrokeCollection;
        private IReadOnlyCollection<UIElement> m_lastInkCanvasChildrensElements;
        private IReadOnlyCollection<UIElement> m_tempInkCanvasChildrensElements;
        private string m_sFileName = "";
        private enum SelectedShape { None, Circle, Rectangle, Triangle , Line }
        private SelectedShape Shape1 = SelectedShape.None;
        public bool m_bIsTempCanvasOpen;
        public string m_sLastSavedCanvasName;

#region EventHandlers
        public event EventHandler CloseButtonClick;
        public event EventHandler HideInkCanvas;
        public event EventHandler CreateBlankCanvasClick;
        public event EventHandler HideBackgroundCanvas;
#endregion

#region Window Initialization
        public ToolsWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        public void SetInkCanvas(InkCanvas _inkCanvas)
        { m_inkCanvas = _inkCanvas; }

        /// <summary>
        /// After Mainwindow finished loaded, event called and Init variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Top = desktopWorkingArea.Bottom - Height+40;
            WindowStyle = WindowStyle.None;
            this.Width =SystemParameters.PrimaryScreenWidth;
            ResizeMode = ResizeMode.NoResize;
            m_FileUploadThread = new Thread(UploadManager.Instance.Main);
            m_FileUploadThread.Start();
            m_ProcessManager = ProcessManager.Instance;
            m_ImageCaptureManager = ImageCaptureManager.Instance;
            m_PowerPointManager = new PowerPointManager();
            m_bIsTempCanvasOpen = false;
            m_ImageCaptureManager.LoadPreviousStorke += new EventHandler(LoadPreviousStorkes);
        }

        /// <summary>
        /// Event called when window loaded . Init variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            toolsDockPanel.Height = toolsDockPanel.ActualHeight;
            m_dToolsDockPanelDefaultHeight = toolsDockPanel.Height;
            Height = ActualHeight;
            SizeToContent = System.Windows.SizeToContent.Manual;
            m_defaultButtonStyle = eraseAllButton.Style;
            
        }

        public void SetCourseID(int cID)
        {
            m_iCourseID = cID;
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
            m_inkCanvas.DefaultDrawingAttributes.Color = ((SolidColorBrush)((Border)sender).Background).Color;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            //System.Media.SystemSounds.Asterisk.Play();
            /*if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();*/
        }
        
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_ImageCaptureManager.DeleteAllThumbnails();
            onCloseButtonClick();
        }

        /// <summary>
        /// Close toolbar button clicked - trigger event in MainWindow to close toolsWindow
        /// </summary>
        private void onCloseButtonClick()
        {
            if (CloseButtonClick != null)
                CloseButtonClick.Invoke(new object(), new EventArgs());
        }

        /// <summary>
        /// Reset all buttons background to default 
        /// </summary>
        private void ResetAllToolBackgrounds()
        {
            foreach (Button i in toolStackPanel.Children)
               // if (i.Name != "brushSize")
                if (!(m_bIsTempCanvasOpen && i.Name.Equals("createBlankBackground")))
                    i.Style = m_defaultButtonStyle;
        }

        /// <summary>
        /// Event triggered when click on left side menu - hide unrelevant objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFilesLeft_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Event triggered when click on right side menu - hide unrelevant objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFilesRight_Click(object sender, RoutedEventArgs e)
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
            LastStrokesScrollViewer.Visibility = Visibility.Hidden;
            if (LastShotsScrollViewer.Visibility == Visibility.Visible)
            {
                LastShotsScrollViewer.Visibility = Visibility.Hidden;
            }
            else
            {
                LastShotsScrollViewer.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hide all submenus . submenus = all the non static buttons.
        /// </summary>
        private void HideAllSubMenus()
        {
            deleteStrokePanel.Visibility = Visibility.Hidden;
            colorPanel.Visibility = Visibility.Hidden;
            brushSizeStackPanel.Visibility = Visibility.Hidden;
            LastShotsScrollViewer.Visibility = Visibility.Hidden;
            WinApplicationsLeft.Visibility = Visibility.Hidden;
            WinApplicationsRight.Visibility = Visibility.Hidden;
            shapesStackPanel.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Get all canvas childrens *Elements*
        /// </summary>
        /// <returns>Collection with all canvas childrens elements</returns>
        private Collection<UIElement> GetAllCanvasChildrens()
        {
            Collection<UIElement> elementCollection = new Collection<UIElement>();
            for (int i = 0; i < m_inkCanvas.Children.Count; i++)
            {
                elementCollection.Add(m_inkCanvas.Children[i]);
            }
            return elementCollection;
        }

        /// <summary>
        /// Add to canvas Strokes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadPreviousStorkes(object sender, EventArgs e)
        {
            int strokeIndex = m_ImageCaptureManager.m_iLastSelectedStroke;
            m_inkCanvas.Strokes = m_SavedStrokesList[strokeIndex].m_strokeCollection;
            for (int i = 0; i < m_SavedStrokesList[strokeIndex].m_childElement.Count; i++)
            {
                m_inkCanvas.Children.Add(m_SavedStrokesList[strokeIndex].m_childElement[i]);
            }

        }

#endregion
        
#region onClickHandlers
        
        /// <summary>
        /// Set normal cursor mode 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CursorButton_Click(object sender, RoutedEventArgs e)
        {
            ResetAllToolBackgrounds();
            cursorButton.Style = (Style)FindResource("highlightedButtonStyle");
            HideAllSubMenus();
        }

        /// <summary>
        /// Set Pen cursor mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PenButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            m_inkCanvas.Cursor = Cursors.Pen;
            m_inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            m_inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
            SetBrushSize();
            ResetAllToolBackgrounds();
            penButton.Style = (Style)FindResource("highlightedButtonStyle");

        }

        /// <summary>
        /// Set highlighter cursor mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HighlighterButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            m_inkCanvas.Cursor = Cursors.Pen;
            m_inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            m_inkCanvas.DefaultDrawingAttributes.IsHighlighter = true;
            SetBrushSize();
            ResetAllToolBackgrounds();
            highlighterButton.Style = (Style)FindResource("highlightedButtonStyle");
        }

        /// <summary>
        /// Enter edit storkes / shapes mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            deleteStrokePanel.Visibility = Visibility.Visible;
            m_inkCanvas.Cursor = Cursors.Cross;
            m_inkCanvas.EditingMode = InkCanvasEditingMode.Select;
            /*m_inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            m_inkCanvas.DefaultDrawingAttributes.IsHighlighter = true;*/
            SetBrushSize();
            ResetAllToolBackgrounds();
            editbutton.Style = (Style)FindResource("highlightedButtonStyle");
            
        }
        
        /// <summary>
        /// Erase selected Storkes / shapes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            m_inkCanvas.Cursor = Cursors.Cross;
            m_inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            SetBrushSize();
            ResetAllToolBackgrounds();
            eraserButton.Style = (Style)FindResource("highlightedButtonStyle");   
        }
        
        /// <summary>
        /// Erase all strokes & shapes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EraseAllButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            m_inkCanvas.Strokes.Clear();
            m_inkCanvas.Children.Clear();
        }
        
        /// <summary>
        /// Open pen size selection menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PenSizeButton_MouseDown(object sender, RoutedEventArgs e)
        {
            HideAllSubMenus();
            m_dPenSize = ((Ellipse)((Button)sender).Content).Width;
            ((Ellipse) ((Button) brushSize).Content).Width = m_dPenSize;
            ((Ellipse) ((Button) brushSize).Content).Height = ((Ellipse) ((Button) sender).Content).Height;
            SetBrushSize();
            foreach (Button i in brushSizeStackPanel.Children)
                i.Style = m_defaultButtonStyle;
            ((Button)sender).Style = (Style)FindResource("highlightedButtonStyle");   
        }

        /// <summary>
        /// Set selected size as current brush size
        /// </summary>
        private void SetBrushSize()
        {
            if (m_inkCanvas.Cursor == Cursors.Cross)
            {
                m_inkCanvas.DefaultDrawingAttributes.Width = m_dPenSize * 5;
                m_inkCanvas.DefaultDrawingAttributes.Height = m_dPenSize * 5;
            }
            else
            {
                m_inkCanvas.DefaultDrawingAttributes.Width = m_dPenSize;
                m_inkCanvas.DefaultDrawingAttributes.Height = m_dPenSize;
            }
        }

        /// <summary>
        /// Hide / unhide toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickThroughCheckBox_Checked(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Delete selected stroke / shape
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private void OnDeleteStroke(object sender, RoutedEventArgs routedEventArgs)
        {
            m_inkCanvas.Strokes.Remove(m_inkCanvas.GetSelectedStrokes());
            for (int i = 0; i < m_inkCanvas.GetSelectedElements().Count; i++)
            {
                m_inkCanvas.Children.Remove(m_inkCanvas.GetSelectedElements()[i]);
            }
        }

        /// <summary>
        /// Display / hide last strokes menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadLastStrokesClicked(object sender, RoutedEventArgs e)
        {
            LastShotsScrollViewer.Visibility = Visibility.Hidden;
            if (LastStrokesScrollViewer.Visibility == Visibility.Visible)
            {
                LastStrokesScrollViewer.Visibility = Visibility.Hidden;
            }
            else
            {
                LastStrokesScrollViewer.Visibility = Visibility.Visible;
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
        private void OnColorClick(object sender, RoutedEventArgs routedEventArgs)
        {
            HideAllSubMenus();
            colorPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Open file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
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
                m_ProcessManager.GenerateProcess(filename);
            }
        }

        /// <summary>
        /// Open image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpenImageClicked(object sender, MouseButtonEventArgs e)
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
                m_ProcessManager.GenerateProcess(filename, true);
                Thread.Sleep(1000);
                SendKeys.SendWait("{F11}");
            }
        }

        /// <summary>
        /// Create blank background on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateBlankCanvasClicked(object sender, RoutedEventArgs e)
        {
            //Check if the transparent canvas is not blank
            if (m_inkCanvas.Strokes.Count > 0 || m_inkCanvas.Children.Count > 0)
            {
                if (!m_bIsTempCanvasOpen)
                {
                    m_bIsLastCanvasSaved = true;
                }
                OnCaptureClick(sender, e);
            }

            if (!m_bIsTempCanvasOpen)
            {
                m_bIsTempCanvasOpen = true;
                createBlankBackground.Style = (Style) FindResource("highlightedButtonStyle");
            }
            else
            {
                m_bIsTempCanvasOpen = false;
                if (m_tempStrokeCollection != null && m_tempStrokeCollection.Count > 0 && m_bIsLastCanvasSaved)
                {
                    m_inkCanvas.Strokes = m_tempStrokeCollection;
                }
                if (m_tempInkCanvasChildrensElements != null && m_tempInkCanvasChildrensElements.Count > 0 && m_bIsLastCanvasSaved)
                {
                    foreach (var element in m_tempInkCanvasChildrensElements)
                    {
                        m_inkCanvas.Children.Add(element);
                    }
                }
                m_tempStrokeCollection = null;
                m_tempInkCanvasChildrensElements = null;
                m_bIsLastCanvasSaved = false;
                createBlankBackground.Style = m_defaultButtonStyle;
            }

            //Fire event to create blank background window
            if (CreateBlankCanvasClick != null)
                CreateBlankCanvasClick.Invoke(new object(), new EventArgs());
        }

        /// <summary>
        /// Open browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenBrowser(object sender, MouseButtonEventArgs e)
        {
            HideAllSubMenus();
            m_ProcessManager.GenerateProcess("http://google.com");
        }

        /// <summary>
        /// Open windows keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyboardClick(object sender, RoutedEventArgs e)
        {
            string sKeyboardPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
                @"Microsoft Shared\ink\TabTip.exe");

            m_ProcessManager.GenerateProcess(sKeyboardPath);
        }

#endregion
        
#region imageCapture
        /// <summary>
        /// Capture screenshot, saves it locally and add to upload manager stack
        /// saves the picture on date time format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCaptureClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            m_sFileName = m_ImageCaptureManager.CaptureScreen();
            ExportCanvasToFile();
            string fullFilePath = Path.Combine(GlobalContants.screenshotFolderPath, m_sFileName + ".jpeg");
            UploadManager.Instance.setCourseID(m_iCourseID);
            UploadManager.Instance.uploadFilesStack.Push(fullFilePath);
            //CursorButton_Click(sender,e);
            if (UploadManager.Instance.isThreadSleep)
            {
                m_FileUploadThread.Interrupt();
            }
            itemsControl.Items.Add(m_ImageCaptureManager.CreatePreviewThumbnail(fullFilePath));
            strokesItemsControl1.Items.Add(m_ImageCaptureManager.CreatePreviewStrokeThumbnail(fullFilePath));
        }

        /// <summary>
        /// Export transparent canvas layer to local xaml file and save last canvas strokes in temp variable
        /// </summary>
        public void ExportCanvasToFile()
        {
            string xaml = XamlWriter.Save(m_inkCanvas);
            if (Directory.Exists(GlobalContants.canvasFolderPath))
                File.WriteAllText(Path.Combine(GlobalContants.canvasFolderPath, m_sFileName + ".xaml"), xaml);
            else
            {
                Directory.CreateDirectory(GlobalContants.canvasFolderPath);
                File.WriteAllText(Path.Combine(GlobalContants.canvasFolderPath, m_sFileName + ".xaml"), xaml);
            }

            if (m_inkCanvas.Strokes.Count > 0)
            {
                m_NewStrokeCollection = new StrokeCollection();
                m_NewStrokeCollection = m_inkCanvas.Strokes.Clone();
                if (m_bIsLastCanvasSaved && !m_bIsTempCanvasOpen)
                {
                    m_tempStrokeCollection = new StrokeCollection();
                    m_tempStrokeCollection = m_inkCanvas.Strokes.Clone();
                }
            }
            if (m_inkCanvas.Children.Count > 0)
            {
                m_lastInkCanvasChildrensElements = GetAllCanvasChildrens();
                if (m_bIsLastCanvasSaved && !m_bIsTempCanvasOpen)
                {
                    m_tempInkCanvasChildrensElements = GetAllCanvasChildrens();    
                }
            }
            m_SavedStrokesList.Add(new CustomStroke(m_NewStrokeCollection,GetAllCanvasChildrens()));
            m_inkCanvas.Strokes.Clear();
            m_inkCanvas.Children.Clear();

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
            if (m_PowerPointManager.OpenPowerPoint())
            {
                PowerPointPanelLeft.Visibility = Visibility.Visible;
                PowerPointPanelRight.Visibility = Visibility.Visible;
                if (HideBackgroundCanvas != null)
                    HideBackgroundCanvas.Invoke(new object(), new EventArgs());
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
            if (m_inkCanvas.Strokes.Count > 0)
            {
                OnCaptureClick(sender, e);
            }

            try
            {
                m_PowerPointManager.ShowNextSlide();
            }
            catch (Exception)
            {
                OnExitPowerPoint(sender, e);
            }
        }

        /// <summary>
        /// Load previous slide in presentation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                m_PowerPointManager.ShowPreviousSlide();  
            }
            catch (Exception)
            {
                OnExitPowerPoint(sender, e);
            }
        }

        /// <summary>
        /// Exit Power point application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitPowerPoint(object sender, RoutedEventArgs e)
        {
            m_PowerPointManager.ClosePowerPoint();
            PowerPointPanelLeft.Visibility = Visibility.Hidden;
            PowerPointPanelRight.Visibility = Visibility.Hidden;
            if (HideBackgroundCanvas != null)
            {
                HideBackgroundCanvas.Invoke(sender,e);
            }
        }
#endregion

#region ShapesHandler

        /// <summary>
        /// Open shapes menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShapeSelect(object sender, RoutedEventArgs e)
        {
            if (shapesStackPanel.IsVisible)
            {
                shapesStackPanel.Visibility= Visibility.Hidden;
            }
            else
                shapesStackPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Draw circle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCircleDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Circle;
            DrawShape();
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRectangleDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Rectangle;
            DrawShape();
        }

        /// <summary>
        /// Draw triangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTriangleDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Triangle;
            DrawShape();
        }
        
        /// <summary>
        /// Draw line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLineDrawClick(object sender, RoutedEventArgs e)
        {
            Shape1 = SelectedShape.Line;
            DrawShape();
        }

        /// <summary>
        /// Draw coordinator system 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCoordinateDrawClick(object sender, RoutedEventArgs e)
        {
            string src = Path.Combine(Directory.GetCurrentDirectory(), "Images/coordinate.png");
            Image img = new Image();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(src);
            image.EndInit();
            img.Source = image;
            img.Width = 200;
            img.Height = 200;
            m_inkCanvas.Children.Add(img);
        }

        /// <summary>
        /// Draw selected shape on canvas
        /// </summary>
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
                    Rendershape = new Rectangle() { Height = 100, Width = 100, RadiusX = 2, RadiusY = 2 };
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
            m_inkCanvas.Children.Add(Rendershape);
        }

#endregion


    }
}
