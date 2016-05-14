using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Reflection;
using LePaint.Controller;

namespace LePaint
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
  
    public partial class Window1 : Window
    {
        enum Action
        {
            Nothing,
            Move,
            DrawRect,
        }

        private Action curAction = Action.Nothing;

        public Canvas myCanvas;
        public Type myTool;
        public bool DrawShape = false;
        DrawingController drawingController;
        public LeShape CurShape;

        // Collection contains instances of GraphicsBase-derived classes.
        public static Window1 Self;
        public Window1()
        {
            InitializeComponent();
            Self = this;

            myCanvas = DrawingCanvas;
            DrawingCanvas.MouseDown += new MouseButtonEventHandler(DrawingCanvas_MouseDown);
            DrawingCanvas.MouseMove += new MouseEventHandler(DrawingCanvas_MouseMove);
            DrawingCanvas.MouseUp += new MouseButtonEventHandler(DrawingCanvas_MouseUp);

            this.Loaded += new RoutedEventHandler(Window1_Loaded);

            drawingController=new DrawingController (DrawingCanvas);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            ToolWindow wnd = new ToolWindow();
            wnd.Show(); 
        }

        void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            drawingController.MouseDown(sender, e);
        }

        void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            drawingController.MouseMove(sender, e);
        }

        void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            drawingController.MouseUp(sender, e);
            DrawingCanvas.ReleaseMouseCapture();

        }

        public void CreateNewShape(MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(myCanvas);

            ConstructorInfo constructor = myTool.GetConstructor(new Type[] { typeof(Point) });
            CurShape = constructor.Invoke(new object[] { pt }) as LeShape;


            shapeCollection.AddObject(CurShape.myVisual);

            DrawingCanvas.CaptureMouse();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            string filter = "XML File (*.xml) |*.xml|JPEG File (*.jpg) |*.jpg|PNG File (*.png) |*.png |";
            filter += "GIF File (*.gif) |*.gif";

            dlg.Filter = filter;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                drawingController.Save(filename); 

            } 
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window3 wnd = new Window3();
            wnd.Show();

            DrawShape = false;
        }

        public void SetDrawingTool(Type t){
            myTool = t;
            DrawShape = true;
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XML File (*.xml) |*.xml";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                drawingController.Load(filename);
            } 

        }

        private void LoadFace(object sender, RoutedEventArgs e)
        {
            ImageBrush brush=new ImageBrush ();


            brush.ImageSource =  new BitmapImage(
                    new Uri(@"Resources/lady_sketch.jpg", UriKind.Relative)
                );

            myCanvas.Background=brush;
            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            List<LeShape> ret= drawingController.xmlShapes.GetSelectedShapes();
            foreach (LeShape shape in ret)
            {
                drawingController.xmlShapes.Remove(shape);

                shapeCollection.RemoveShape(shape.myVisual);
            }

        }
    }

}
