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
using System.IO;

using System.Collections;
using System.Xml.Serialization;
using System.Xml;


namespace LePaint.Controller
{
    public class DrawingController : LeShape, ICanvas
    {
        #region enums
        internal enum ShapeAction
        {
            Nothing,
            MovingShape,
            Drawing,
            MoveGroupShapes,
            ResizingShape
        }

        internal enum GroupAction
        {
            Nothing,
            Drawing,
            MoveGroupShapes,
        }

        internal enum EditMode
        {
            Nothing,
            ShapeMode,
            GroupMode
        }

        internal enum MouseAction
        {
            Down,
            Up,
            Move
        }
        internal enum Position
        {
            TopLeft,
            RightTop,
            RightBottom,
            LeftBottom,
            Center
        }
        #endregion

        #region properties
        public Image BackGround
        {
            set
            {
                Canvas.Background = new ImageBrush(value.Source);
            }
        }

        #endregion

        #region fields
        EditMode curMode = EditMode.Nothing;

        ShapeAction curShapeAction = ShapeAction.Nothing;
        GroupAction curGroupAction = GroupAction.Nothing;

        private ArrayList selectedShapes;
        #endregion

        #region constructor, paint

        public XMLShapes xmlShapes;

        public static DrawingController self;

        public Canvas Canvas;

        public DrawingController(Canvas canvas)
        {
            xmlShapes = new XMLShapes();
            Canvas = canvas;

            self = this;
        }

        #endregion

        public static LeShape SelectedShape = null;
        GroupShapes groupShapes = new GroupShapes();

        #region Mouse Actions
        public void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedShape != null)
            {
                if (SelectedShape.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                {
                    //myMenu.ShowSelectedProperties(SelectedShape);
                }
            }
            else
            {
                //myMenu.ShowSelectedProperties(this);
            }
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                CheckRightMouseDown(e);
                return;
            }

            if (MouseOnShape(e.GetPosition(Window1.Self.myCanvas)) == false)
            {
                if (Window1.Self.DrawShape == false)
                {
                    curMode = EditMode.GroupMode;
                }
            }
            else
            {
                curMode = EditMode.ShapeMode;
            }

            switch (curMode)
            {
                case EditMode.ShapeMode:
                    Canvas.Cursor = Cursors.Arrow;
                    ShapeMouseDownActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseDownAction(e);
                    break;
                case EditMode.Nothing:
                    curMode = EditMode.ShapeMode;
                    ShapeMouseDownActions(e);
                    break;
            }
        }

        private void CheckRightMouseDown(MouseButtonEventArgs e)
        {
            if (groupShapes.Count > 0)
            {
                if (groupShapes.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                {
                    //myMenu.ShowEditShapesMenus(e);
                }
                else
                {
                    groupShapes = new GroupShapes();
                }
            }
            else
            {
                if (MouseOnShape(e.GetPosition(Window1.Self.myCanvas)))
                {
                    //myMenu.ShowEditShapesMenus(e);
                    curMode = EditMode.ShapeMode;
                }
                else
                {
                    //myMenu.ShowDrawingShapeMenus(e);
                }
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            // if (myMenu.DrawShape == true) curMode = EditMode.ShapeMode;

            switch (curMode)
            {
                case EditMode.ShapeMode:
                    Canvas.Cursor = Cursors.Arrow;
                    ShapeMouseMoveActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseMoveAction(e);
                    break;
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (curMode)
            {
                case EditMode.ShapeMode:
                    Canvas.Cursor = Cursors.Arrow;
                    ShapeMouseUpActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseUpAction(e);
                    break;
            }
        }


        public void Draw()
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                //shape.Paint(sender, e.Graphics);
                if (shape.Selected)
                {
                    //  shape.DrawSelected(e);
                }
            }

            if (groupShapes.Count > 0)
            {
                //groupShapes.Paint(sender, e);
            }
        }

        #endregion

        #region Groups Mode
        private void GroupMouseDownAction(MouseButtonEventArgs e)
        {
            switch (curGroupAction)
            {
                case GroupAction.Drawing:
                    break;
                case GroupAction.MoveGroupShapes:
                    break;
                case GroupAction.Nothing:
                    if (groupShapes.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                    {
                        curGroupAction = GroupAction.MoveGroupShapes;
                        groupShapes.MouseDown(this, e);
                    }
                    else
                    {
                        ClearSelectedShape();
                        curGroupAction = GroupAction.Drawing;
                        ptOrigin = e.GetPosition(Window1.Self.myCanvas);
                    }
                    break;
            }

        }
        private void GroupMouseMoveAction(MouseEventArgs e)
        {
            switch (curGroupAction)
            {
                case GroupAction.Drawing:
                    base.DrawMouseMove(e);
                    break;
                case GroupAction.MoveGroupShapes:
                    groupShapes.MouseMove(this, e);
                    break;
                case GroupAction.Nothing:
                    if (groupShapes.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                    {
                        Canvas.Cursor = Cursors.Hand;
                        groupShapes.MouseMove(this, e);
                    }
                    else
                        Canvas.Cursor = Cursors.Arrow;
                    break;
            }

        }

        private void GroupMouseUpAction(MouseButtonEventArgs e)
        {
            switch (curGroupAction)
            {
                case GroupAction.Drawing:
                    groupShapes.SetSelectedShapes(groupShapes.Boundary);
                    if (groupShapes.Count == 0)
                        groupShapes = new GroupShapes();

                    curGroupAction = GroupAction.Nothing;
                    base.DrawMouseMove(e);
                    break;
                case GroupAction.MoveGroupShapes:
                    groupShapes.Move();
                    groupShapes.MouseUp(this, e);
                    curGroupAction = GroupAction.Nothing;
                    break;
                case GroupAction.Nothing:
                    if (groupShapes.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                    {
                        Canvas.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        Canvas.Cursor = Cursors.Arrow;
                    }
                    break;
            }
        }

        private void OnShapeMoved(MouseButtonEventArgs e)
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Selected)
                {
                    Rect rect = Common.MoveRect(ptOrigin, e.GetPosition(Window1.Self.myCanvas), Boundary);
                    shape.Boundary = rect;
                }
            }
        }

        private bool MouseOnShape(Point point)
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Boundary.Contains(point))
                {
                    return true;
                }
                if (shape is ZoneShape)
                {
                    if ((shape as ZoneShape).TextField.Boundary.Contains(point))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Shapes Mode

        private void ShapeMouseDownActions(MouseButtonEventArgs e)
        {
            ClearSelectedShape();
            switch (curShapeAction)
            {
                case ShapeAction.Nothing:
                    if (MouseOnShape(e.GetPosition(Window1.Self.myCanvas)) == false)
                    {
                        if (Window1.Self.DrawShape == true)
                        {
                            curShapeAction = ShapeAction.Drawing;
                            DrawMouseDown(e);
                        }
                    }
                    else
                    {
                        curShapeAction = ShapeAction.MovingShape;
                        selectedShapes = GetSelectedShape(e);
                        SendMouseAction(selectedShapes, e, MouseAction.Down);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            ClearSelectedShape();
            Window1.Self.CreateNewShape(e);
            Window1.Self.CurShape.Boundary = new Rect();
            Window1.Self.CurShape.DrawMouseDown(e);
        }

        private void ShapeMouseMoveActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    Window1.Self.CurShape.Selected = true;
                    Window1.Self.CurShape.DrawMouseMove(e);
                    break;
                case ShapeAction.MovingShape:
                    SendMouseMoveAction(selectedShapes, e);
                    break;
                case ShapeAction.MoveGroupShapes:
                    break;
                case ShapeAction.Nothing:
                    SendMouseMoveAction(GetSelectedShape(e), e);
                    break;
            }
        }

        private void ShapeMouseUpActions(MouseButtonEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    curShapeAction = ShapeAction.Nothing;
                    AddNewShape(e);
                    break;
                case ShapeAction.MovingShape:
                    SendMouseAction(selectedShapes, e, MouseAction.Up);
                    curShapeAction = ShapeAction.Nothing;
                    break;
            }
        }


        private void SendMouseAction(ArrayList arr, MouseButtonEventArgs e, MouseAction act)
        {
            foreach (LeShape shape in arr)
            {
                shape.Selected = true;
                switch (act)
                {
                    case MouseAction.Down:
                        shape.MouseDown(this, e);
                        break;
                    case MouseAction.Up:
                        shape.MouseUp(this, e);
                        break;
                }
                shape.RefreshDrawing();
            }
        }

        private void SendMouseMoveAction(ArrayList arr, MouseEventArgs e)
        {
            foreach (LeShape shape in arr)
            {
                shape.MouseMove(this, e);
            }
        }

        public virtual ArrayList GetMouseOnShapes(MouseButtonEventArgs ex)
        {
            ArrayList ret = new ArrayList();

            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Boundary.Contains(ex.GetPosition(Window1.Self.myCanvas)))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        public void AddNewShape(MouseButtonEventArgs e)
        {
            if (Window1.Self.DrawShape == true)
            {
                xmlShapes.Add(Window1.Self.CurShape);
            }
        }
        #endregion

        public void DeleteSelectedShapes()
        {
            List<LeShape> ret = new List<LeShape>();
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Selected) ret.Add(shape);
            }

            foreach (LeShape shape in ret)
            {
                xmlShapes.GetList().Remove(shape);
            }

        }

        private void ClearSelectedShape()
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Selected)
                {
                    shape.Selected = false;
                    shape.RefreshDrawing();
                }
            }
        }

        private ArrayList GetSelectedShapes()
        {
            ArrayList ret = new ArrayList();

            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Selected)
                    ret.Add(shape);
            }
            return ret;
        }

        private ArrayList GetSelectedShape(MouseButtonEventArgs e)
        {
            ArrayList ret = new ArrayList();
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        private ArrayList GetSelectedShape(MouseEventArgs e)
        {
            ArrayList ret = new ArrayList();
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        #region xml shapes handling
        protected void RemoveShape(LeShape ob)
        {
            xmlShapes.Remove(ob);
        }
        #endregion

        public void Save(String fileName)
        {
            if (fileName.ToUpper().EndsWith("XML"))
            {
                ExportToXML(fileName);
            }
            else
            {
                SaveUIAsGraphicFile(Window1.Self.myCanvas,fileName);
            }
        }

        private void ExportToXML(string fileName)
        {
            TextWriter w = new StreamWriter(fileName);
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(XMLShapes));
                s.Serialize(w, xmlShapes);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Serialize XML " + e.Message);
                System.Console.WriteLine("Serialize XML " + e.InnerException.Message);
            }
            finally
            {
                w.Close();
            }
        }

        public void Load(string fileName)
        {
            DeSerializeXML(fileName); 
        }

        private void DeSerializeXML(string fileName)
        {

            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                XmlTextReader xr = new XmlTextReader(sr);
                XmlSerializer xs = new XmlSerializer(typeof(XMLShapes));
                if (xs.CanDeserialize(xr))
                {
                    try
                    {
                        XMLShapes ret = (XMLShapes)xs.Deserialize(xr);
                        xmlShapes = ret;
                        Redraw();
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Open file " + e.InnerException.Message);
                    }
                }
                xr.Close();
                sr.Close();
            }
        }

        public void Redraw()
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                Window1.Self.shapeCollection.AddObject(shape.myVisual);
                shape.RefreshDrawing();
            }
        }

        public static void SaveUIAsGraphicFile(UIElement sp, string file)
        {

            double dpiX = 96;

            double dpiY = 96;

            FrameworkElement elem = sp as FrameworkElement;

            if (elem == null)
            {

                return;
            }

            Rect bounds = VisualTreeHelper.GetContentBounds(elem);

            if (bounds.Width == double.PositiveInfinity || bounds.Width == double.NegativeInfinity)
            {
                bounds = new Rect(0, 0, elem.Width, elem.Height);
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap((Int32)(bounds.Width * dpiX / 96.0),

            (Int32)(bounds.Height * dpiY / 96.0), dpiX, dpiY, PixelFormats.Default);

            bmp.Render(elem);

            string Extension = System.IO.Path.GetExtension(file).ToLower();

            BitmapEncoder encoder;

            if (Extension == ".gif")

                encoder = new GifBitmapEncoder();

            else if (Extension == ".png")

                encoder = new PngBitmapEncoder();

            else if (Extension == ".jpg")

                encoder = new JpegBitmapEncoder();

            else return;

            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (Stream stm = File.Create(file))
            {

                encoder.Save(stm);

            }

        }
    }
}
