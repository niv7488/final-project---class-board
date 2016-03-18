using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace BoardCast
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
       public BackgroundWorker bw = new BackgroundWorker();
       public int selectedCours;
       public bool isUploading = false;
       private string screenshotFolderPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
       private string canvasFolderPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "CanvasLayouts");
       public LoginWindow()
        {
           InitializeComponent();
           fillCoursesList();
           bw.WorkerReportsProgress = true;
           bw.WorkerSupportsCancellation = true;
           bw.DoWork += new DoWorkEventHandler(bw_DoWork);
           bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
           bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
           if (!isUploading)
           {
               if (Directory.EnumerateFileSystemEntries(screenshotFolderPath).Any())
               {
                   System.IO.DirectoryInfo dir = new DirectoryInfo(screenshotFolderPath);

                   foreach (FileInfo file in dir.GetFiles())
                   {
                       file.Delete();
                   }
               }
           }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            String userName = txtBxuserName.Text.ToLower();
            string pass = passBxPassword.Password;
            lblfrgtPass.Visibility = Visibility.Hidden;
            lblLoading.Visibility = Visibility.Visible;
            Mouse.OverrideCursor = Cursors.Wait;
            txtBxuserName.IsEnabled = false;
            passBxPassword.IsEnabled = false;
            btnLogin.IsEnabled = false;
            if (userName == "admin" && pass == "123")
            {
                if (bw.IsBusy == false)
                {
                    bw.RunWorkerAsync();                
                }                
            }
            else
            {                             
                lblLoading.Visibility = Visibility.Hidden;
                lblfrgtPass.Visibility = Visibility.Visible;
                lblfrgtPass.Content = "Forgot Password ?";
                txtBxuserName.IsEnabled = true;
                txtBxuserName.BorderBrush = Brushes.Red;
                passBxPassword.IsEnabled = true;
                passBxPassword.BorderBrush = Brushes.Red;
                btnLogin.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(5000);
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
            CoursesList.Items.Add("350876542:חדווא 1");
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
            MainWindow main = new MainWindow(selectedCours);
            
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
                selectedCours = Int32.Parse(courseInfo[1]); 
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
