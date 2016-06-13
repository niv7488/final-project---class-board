using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScreenCast;


namespace BoardCast
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
       private BackgroundWorker bw = new BackgroundWorker();
       private int m_iSelectedCours;
       private string screenshotFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
       private string canvasFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "CanvasLayouts");
       
        public LoginWindow()
       {
           try
           {
               InitializeComponent();
               //fillCoursesList();
               bw.WorkerReportsProgress = true;
               bw.WorkerSupportsCancellation = true;
               bw.DoWork += new DoWorkEventHandler(bw_DoWork);
               bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
               bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
               if (!UploadManager.Instance.isUploading)
               {
                   if (!Directory.Exists(GlobalContants.screenshotFolderPath))
                   {
                       Directory.CreateDirectory(GlobalContants.screenshotFolderPath);
                   }
                   else if (Directory.EnumerateFileSystemEntries(GlobalContants.screenshotFolderPath).Any())
                   {
                       DirectoryInfo dir = new DirectoryInfo(GlobalContants.screenshotFolderPath);
                       foreach (FileInfo file in dir.GetFiles())
                       {
                           try
                           {
                               file.Delete();
                           }
                           catch (Exception e)
                           {}
                       }
                   }
                   if (!Directory.Exists(GlobalContants.canvasFolderPath))
                   {
                       Directory.CreateDirectory(GlobalContants.canvasFolderPath);
                   }
                   else if (Directory.EnumerateFileSystemEntries(GlobalContants.canvasFolderPath).Any())
                   {
                       System.IO.DirectoryInfo dir = new DirectoryInfo(GlobalContants.canvasFolderPath);
                       foreach (FileInfo file in dir.GetFiles())
                       {
                           try
                           {
                               file.Delete();
                           }
                           catch (Exception e)
                           { }
                       }
                   }
               }
           }
           catch (Exception e)
           {
               MessageBox.Show(e.Message);
           }
       }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtBxuserName.Text.ToLower();
            string pass = passBxPassword.Password;
            lblfrgtPass.Visibility = Visibility.Hidden;
            lblLoading.Visibility = Visibility.Visible;
            Mouse.OverrideCursor = Cursors.Wait;
            txtBxuserName.IsEnabled = false;
            passBxPassword.IsEnabled = false;
            btnLogin.IsEnabled = false;
            string jsonRequest = new JavaScriptSerializer().Serialize(new
            {
                teacher_id = userName,                  //the picture after transfoming into base64 string
                teacher_pass = pass
            });
            //opening a connection with the server
            //deffine the request methood
            //var http = HttpWebRequest.Create(new Uri(baseAddress));
            var http = (HttpWebRequest)WebRequest.Create(new Uri(GlobalContants.loginServerAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            string parsedContent = jsonRequest;
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
                JObject contentToObject = (JObject)JsonConvert.DeserializeObject(responeContent);
                
                var responeStatus = (string) contentToObject["status"];
                if (responeStatus != "success")
                {
                    lblLoading.Visibility = Visibility.Hidden;
                    lblfrgtPass.Visibility = Visibility.Visible;
                    wrongeDetails.Content = (string) contentToObject["msg"];
                    wrongeDetails.Visibility = Visibility.Visible;
                    lblfrgtPass.Content = "Forgot Password ?";
                    txtBxuserName.IsEnabled = true;
                    txtBxuserName.BorderBrush = Brushes.Red;
                    passBxPassword.IsEnabled = true;
                    passBxPassword.BorderBrush = Brushes.Red;
                    btnLogin.IsEnabled = true;
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    var JsonCoursesList = contentToObject["CoursesList"];
                    foreach (var course in JsonCoursesList)
                    {
                        CoursesList.Items.Add(course["course_id"]+":"+course["course_name"]);
                    }
                    if (bw.IsBusy == false)
                    {
                        bw.RunWorkerAsync();
                    } 
                }
                
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                if (bw.IsBusy == false)
                {
                    bw.RunWorkerAsync();
                }  
                
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "Canceled!";                
            }

            else if (!(e.Error == null))
            {
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "Error: " + e.Error.Message;                
            }
            else
            {
                lblLoading.Visibility = Visibility.Hidden;
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "LOGIN SUCCESSFUL";
                Mouse.OverrideCursor = null;
                txtBxuserName.Visibility = Visibility.Hidden;
                passBxPassword.Visibility = Visibility.Hidden;
                btnLogin.Visibility = Visibility.Hidden;
                CoursesList.Visibility = Visibility.Visible;
                btnLogout.Visibility = Visibility.Visible;
                //btnStart.Visibility = Visibility.Visible;
                /* MainWindow main = new MainWindow();
                App.Current.MainWindow = main;
                this.Close();
                main.Show();*/
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            App.Current.Shutdown();
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void txtBxuserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBxuserName.BorderBrush = Brushes.White;            
        }

        private void passBxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passBxPassword.BorderBrush = Brushes.White;
        }

        void fillCoursesList()
        {
            CoursesList.Items.Add("123456:לוגיקה");
            CoursesList.Items.Add("99999:חדווא 1");
            CoursesList.Items.Add("350876541:JAVA");
            CoursesList.Items.Add("350876543:לוגיקה");
            CoursesList.Items.Add("350876542:חדווא 1");
            CoursesList.Items.Add("350876541:JAVA");
            CoursesList.Items.Add("350876543:לוגיקה");
            CoursesList.Items.Add("350876542:חדווא 1");
            CoursesList.Items.Add("350876541:JAVA");
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            lblLoading.Visibility = Visibility.Hidden;
            lblfrgtPass.Visibility = Visibility.Hidden;
            //lblfrgtPass.Content = "LOGIN SUCCESSFUL";
            Mouse.OverrideCursor = null;
            txtBxuserName.Visibility = Visibility.Visible;
            passBxPassword.Visibility = Visibility.Visible;
            btnLogin.Visibility = Visibility.Visible;
            CoursesList.Visibility = Visibility.Hidden;
            btnLogout.Visibility = Visibility.Hidden;
            txtBxuserName.IsEnabled = true;
            passBxPassword.IsEnabled = true;
            btnLogin.IsEnabled = true;
            txtBxuserName.Text = "";
            passBxPassword.Password = "";
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(m_iSelectedCours);
            
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }

        private void PlaceholdersListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                string c = item.ToString();
                Console.WriteLine(c);
                string[] courseInfo = c.Split(':');
                m_iSelectedCours = Int32.Parse(courseInfo[1]); 
                btnStart.Visibility = Visibility.Visible;
                // ListBox item clicked - do some cool things here
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            btnLogin_Click(sender,e);
        }
    }
}
