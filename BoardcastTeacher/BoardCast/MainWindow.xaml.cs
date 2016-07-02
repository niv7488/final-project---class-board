//Copyright (c) 2010 Brian Hoary

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Markup;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScreenCast;
using Path = System.IO.Path;

namespace BoardCast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private backgroundWindow bgwn,blankBackground;
        public static int courseID;
        private ScreenShareServer realTimeCasting;
        private Thread updateCastingDataThread;
        UIElement lastCanvas;
        private bool isInkVisible = true;
        public string AssemblyTitle
       {
           get
           {
               // Get all Title attributes on this assembly
                object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    System.Reflection.AssemblyTitleAttribute titleAttribute = (System.Reflection.AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);
        ToolsWindow toolsWindow = new ToolsWindow();
        bool is64Bit;
        System.Windows.Forms.MenuItem rememberContent;
        System.Windows.Forms.MenuItem enableHotkeys;
        string appDataDir;
        System.Windows.Forms.NotifyIcon sideIcon;


#region DLL Functions
        //if((IntPtr)
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        [DllImport("user32.dll")]
        public static extern int GetWindowLongPtr(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLongPtr(IntPtr hwnd, int index, int newStyle);

        IntPtr hwnd;

        int extendedStyle;
        protected override void OnSourceInitialized(EventArgs e)
        {
            is64Bit = (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr))) == 8;
            base.OnSourceInitialized(e);
            hwnd = new WindowInteropHelper(this).Handle;
            if (is64Bit)
                extendedStyle = GetWindowLongPtr(hwnd, GWL_EXSTYLE);
            else
                extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        }
        
#endregion

        /// <summary>
        /// MainWindow Ctr - init variables
        /// </summary>
        /// <param name="iCourseID">Course id of current lecture</param>
        public MainWindow(int iCourseID)
        {
            realTimeCasting = new ScreenShareServer();
            realTimeCasting.InitServer();
            toolsWindow.SetCourseID(iCourseID);
            courseID = iCourseID;
            InitializeComponent();
            blankBackground = new backgroundWindow();
            bgwn = new backgroundWindow();
            bgwn.Show();
            //global hotkeys:

            //SetupHotKey(_host.Handle);
            //ComponentDispatcher.ThreadPreprocessMessage += new ThreadMessageEventHandler(ComponentDispatcher_ThreadPreprocessMessage);
        }

        /// <summary>
        /// Trigger when Window loaded - init event handlers and variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + AssemblyTitle;
            if (!Directory.Exists(appDataDir))
                Directory.CreateDirectory(appDataDir);

            sideIcon = new System.Windows.Forms.NotifyIcon(new System.ComponentModel.Container());
            sideIcon.Icon = new System.Drawing.Icon(GetType(),"Logo.ico");

            //sideIcon.Icon = new System.Drawing.Icon(GetType(), "pencilIcon.ico");
            sideIcon.Visible = true;
            
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            int tempWidth = 0;
            int tempHeight = 0;
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                tempWidth += screen.Bounds.Width;
                tempHeight += screen.Bounds.Height;
            }
#if DEBUG
            tempWidth = 300;
            tempHeight = 300;
#endif
            //debug
            this.Width = tempWidth;
            this.Height = tempHeight;
            this.Left = 0;
            this.Top = 0;

            inkCanvas.Cursor = Cursors.Pen;
            inkCanvas.UseCustomCursor = true;
            inkCanvas.DefaultDrawingAttributes.IgnorePressure = false;
            toolsWindow.SetInkCanvas(inkCanvas);
            toolsWindow.Owner = this;
            //Event handlers init
            realTimeCasting.SetCastingAddress += new EventHandler(setCastingAddress);
            toolsWindow.CloseButtonClick += new EventHandler(toolsWindow_CloseButtonClick);
            toolsWindow.CreateBlankCanvasClick += new EventHandler(toolsWindow_CreateBlankCanvasClick);
            toolsWindow.HideBackgroundCanvas += new EventHandler(HideShowBackgroundCanvas);
            toolsWindow.hideInkCheckBox.Checked += new RoutedEventHandler(hideInkCheckBox_Checked);
            toolsWindow.hideInkCheckBox.Unchecked += new RoutedEventHandler(hideInkCheckBox_Checked);
            toolsWindow.cursorButton.Click += new RoutedEventHandler(cursorButton_Click);
            toolsWindow.penButton.Click += new RoutedEventHandler(drawButton_Click);
            toolsWindow.highlighterButton.Click += new RoutedEventHandler(drawButton_Click);
            toolsWindow.eraserButton.Click += new RoutedEventHandler(drawButton_Click);
            toolsWindow.editbutton.Click += new RoutedEventHandler(drawButton_Click);
            cursorButton_Click(new object(), new RoutedEventArgs());
            toolsWindow.Show();
        }

        /// <summary>
        /// Hide / show temp with canvas layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideShowBackgroundCanvas(object sender, EventArgs e)
        {
            if (bgwn.IsVisible)
            {
                bgwn.Hide();
            }
            else
            {
                bgwn.Show();
            }
        }

        /// <summary>
        /// Thread that manage the screencast url process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setCastingAddress(object sender, EventArgs e)
        {
            updateCastingDataThread = new Thread(UpdateCastingUrlInDB);
            updateCastingDataThread.Start();
        }

        /// <summary>
        /// Update screencast address on DB
        /// </summary>
        private void UpdateCastingUrlInDB()
        {
            //serialize the json so that the server will know what values we sent
            string json = new JavaScriptSerializer().Serialize(new
            {
                course_id = courseID,
                url = Variables.castingAddress
            });
            //opening a connection with the server
            //deffine the request methood
            var http = (HttpWebRequest)WebRequest.Create(new Uri(Variables.updateCastingAddressUrl));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            string parsedContent = json;
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes1 = encoding.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes1, 0, bytes1.Length);
            newStream.Close();
            try
            {
                var response2 = http.GetResponse();
                var stream = response2.GetResponseStream();
                var sr = new StreamReader(stream);
                var responeContent = sr.ReadToEnd();
                //Console.WriteLine(content);
                var contentToObject = (JObject)JsonConvert.DeserializeObject(responeContent);

                var responeStatus = (string)contentToObject["status"];
                if (responeStatus.Equals("Success"))
                {
                    Console.WriteLine("Screen share url updated on server succesfully!");
                }
                else
                {
                    MessageBox.Show("Unable to set screen cast address on DB");
                }
            }
            catch (WebException e)
            {
                MessageBox.Show("Error from server: " + e.Message);
            }
        
        }

        /// <summary>
        /// Event trigger when any cursor except normal is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void drawButton_Click(object sender, RoutedEventArgs e)
        {
            ClickThrough = false;  //Write on transparent canvas
            
        }

        void cursorButton_Click(object sender, RoutedEventArgs e)
        {
            ClickThrough = true; //Manage click behinde transparent canvas
            
        }

        bool clickThrough = false;

        public bool ClickThrough
        {
            get { return clickThrough; }
            set
            {
                clickThrough = value;
                if (clickThrough)
                {
                    if (is64Bit)
                        SetWindowLongPtr(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
                    else
                        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
                    Background = null;
                }
                else
                {
                    if (is64Bit)
                        SetWindowLongPtr(hwnd, GWL_EXSTYLE, extendedStyle | 0);
                    else
                        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | 0);
                    Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                }
            }
        }

        /// <summary>
        /// Trigger runs when Mainwindow close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            sideIcon.Visible = false;
        }

        /// <summary>
        /// Eventhandler when toolswindow close button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolsWindow_CloseButtonClick(object sender, EventArgs e)
        {
            realTimeCasting.CloseCastingService();
            toolsWindow.itemsControl.Items.Clear();
            Console.WriteLine("Total items in itemscontroller "+ toolsWindow.itemsControl.Items.Count);
            toolsWindow.Close();
            LoginWindow main = new LoginWindow();
            App.Current.MainWindow = main;
            Close();
            bgwn.Close();
            blankBackground.Close();
            //Application.Current.Shutdown();
            main.Show();
        }

        /// <summary>
        /// Event handler when open blank canvas was clicked in toolWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolsWindow_CreateBlankCanvasClick(object sender, EventArgs e)
        {
            if (toolsWindow.m_bIsTempCanvasOpen)
            {
                blankBackground.Show();
            }
            else
            {
                
                blankBackground.Hide();
            }
        }

        /// <summary>
        /// Event handler when hide toolWindow is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void hideInkCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)((CheckBox)sender).IsChecked)
                this.Visibility = System.Windows.Visibility.Hidden;
            else
                this.Visibility = System.Windows.Visibility.Visible;
        }

    }
}
