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

using System.Collections;

using LePaint.Basic;

namespace LePaint.MainPart
{
    public class LeCanvas :LeShape,ICanvas
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
                Canvas.Background  = new ImageBrush(value.Source) ;
            }
        }

        #endregion

        #region fields
        EditMode curMode = EditMode.Nothing;

        ShapeAction curShapeAction = ShapeAction.Nothing;
        GroupAction curGroupAction = GroupAction.Nothing;

        private ArrayList selectedShapes;
        private EditMode prevMode;
        #endregion

        #region constructor, paint
        
        public XMLShapes xmlShapes;

        public static LeCanvas self;

        public Control Canvas;

        public LeCanvas(Control canvas)
        {
            xmlShapes = new XMLShapes();
            Canvas = canvas;

            //myMenu = new LeMenu();
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
                if (SelectedShape.Boundary.Contains(e.GetPosition(Window1.myCanvas)))
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

            if (MouseOnShape(e.GetPosition(Window1.myCanvas)) == false)
            {
                //if (myMenu.DrawShape == false)
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
                if (groupShapes.Boundary.Contains(e.GetPosition(Window1.myCanvas)))
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
                if (MouseOnShape(e.GetPosition(Window1.myCanvas)))
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
                    if (groupShapes.Boundary.Contains(e.GetPosition(Window1.myCanvas)))
                    {
                        curGroupAction = GroupAction.MoveGroupShapes;
                        groupShapes.MouseDown(this, e);
                    }
                    else
                    {
                        ClearSelectedShape();
                        curGroupAction = GroupAction.Drawing;
                        ptOrigin = e.GetPosition(Window1.myCanvas);
                        ptCurrent.X = -1;
                        AreaRect = new Rect();
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
                    if (groupShapes.Boundary.Contains(e.GetPosition(Window1.myCanvas)))
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
                    groupShapes.SetSelectedShapes(AreaRect);
                    if (groupShapes.Count == 0)
                        groupShapes = new GroupShapes();

                    curGroupAction = GroupAction.Nothing;
                    base.DrawMouseMove(e);
                    break;
                case GroupAction.MoveGroupShapes:
                    groupShapes.Move();
                    groupShapes.MouseUp(this,e);
                    curGroupAction = GroupAction.Nothing;
                    break;
                case GroupAction.Nothing:
                    if (groupShapes.Boundary.Contains(e.GetPosition(Window1.myCanvas)))
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
                    Rect rect=Common.MoveRect(ptOrigin,e.GetPosition(Window1.myCanvas),Boundary);
                    shape.Boundary=rect ;
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
            switch (curShapeAction)
            {
                case ShapeAction.Nothing:
                    if (MouseOnShape(e.GetPosition(Window1.myCanvas)) == false)
                    {
                       // if (myMenu.DrawShape == true)
                        {
                            curShapeAction = ShapeAction.Drawing;
                            DrawMouseDown(e);
                            ClearSelectedShape();
                        }
                        OnSelectedShapeChanged(Canvas);
                    }
                    else
                    {
                        curShapeAction = ShapeAction.MovingShape;
                        selectedShapes = GetSelectedShape(e);
                        SendMouseAction(selectedShapes, e, MouseAction.Down);

                        SelectedShape = selectedShapes[0] as LeShape;

                        OnSelectedShapeChanged(selectedShapes[0]);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            //myMenu.CreateNewShape(e);
            //myMenu.CurShape.Boundary = new Rect(); 
            //myMenu.CurShape.DrawMouseDown(e);
        }

        private void ShapeMouseMoveActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    //myMenu.CurShape.DrawMouseMove(e); 
                    break;
                case ShapeAction.MovingShape:
                    //SendMouseAction(selectedShapes, e, MouseAction.Move);
                    break;
                case ShapeAction.MoveGroupShapes:
                    break;
                case ShapeAction.Nothing:
                    //SendMouseAction(GetSelectedShape(e), e, MouseAction.Move);
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
                switch (act)
                {
                    case MouseAction.Down:
                        shape.MouseDown(this, e);
                        break;
                    case MouseAction.Up:
                        shape.MouseUp(this, e);
                        break;
                    case MouseAction.Move:
                        shape.MouseMove(this, e);
                        break;
                }
            }
        }

        public virtual ArrayList GetMouseOnShapes(MouseButtonEventArgs ex)
        {
            ArrayList ret = new ArrayList();

            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Boundary.Contains(ex.GetPosition(Window1.myCanvas)))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        public void AddNewShape(MouseButtonEventArgs e) {
            //if (myMenu.selectBack == false)
            {
                bool check = false; // myMenu.CurShape.DrawMouseUp(e);
                if (check == true)
                {
                    //xmlShapes.Add(myMenu.CurShape);
                }
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
                shape.Selected = false;
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
            LeShape shape0=null;
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.UpdateSelected(e.GetPosition(Window1.myCanvas), ref shape0))
                {
                    ret.Add(shape0);
                    break;
                }
            }
            return ret;
        }

        public delegate void SelectedShapeChangedHandler(object p);
        public static event SelectedShapeChangedHandler SelectedShapeChanged;
        private void OnSelectedShapeChanged(object shape)
        {
            if (SelectedShapeChanged != null)
            {
                SelectedShapeChanged(shape);
            }
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
                //LeMenu.ExportToXML(fileName);
            }
            else
            {
                //if (Canvas is PictureBox)
                {
                  //  PictureBox me = (Canvas as PictureBox);
                    
                   // Image myImage = new System.Drawing.Bitmap(me.Width, me.Height);
                    //Graphics g= System.Drawing.Graphics.FromImage(myImage);
                    //g.FillRect(new SolidBrush(Color.White),0,0, me.Width, me.Height);
                    foreach (LeShape shape in xmlShapes.GetList())
                    {
                        shape.Draw();
                    }
                    //me.Image = myImage ;
                    //myImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
        
        public void Load(string fileName)
        {
            //LeMenu.DeSerializeXML(fileName); 
        }
    }
}
