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

using LePaint.MainPart;

namespace LePaint.Basic
{
    public class BoundaryShape : LeShape 
    {
        #region move or resize variables

        internal enum Position
        {
            Nothing,
            Corner,
            Side,
            Center,
        }

        protected enum PointAtPosition
        {
            TopLeft,
            TopMiddle,
            TopRight,
            RightMiddle,
            RightBottom,
            BottomMiddle,
            BottomLeft,
            LeftMidlle,
        }

        public enum Action
        {
            Nothing,
            MoveShape,
            MouseAtShape,
            AboutToMoveShape,
            ResizeShape
        }

        private Action curAction = Action.Nothing;
        protected bool shapeResizing=false ;

        private bool fill;
        public bool Fill
        {
            set { fill = value; }
            get { return fill; }
        }

        #endregion

        protected PointAtPosition PointAt;
        protected Point hotPoint;
        public BoundaryShape(Point pt)
            : base(pt)
        {
            //Boundary = new Rect(pt,new Size(30,30));
            Fill = true;
        }

        public BoundaryShape()
        {

        }

        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                return;
            }

            switch (curAction)
            {
                case Action.MouseAtShape:
                    DecideUserAction(e);
                    break;
                default:
                    break;
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            switch (curAction)
            {
                case Action.AboutToMoveShape:
                    curAction = Action.MoveShape;
                    ptPrevious = e.GetPosition(Window1.myCanvas);
                    break;
                case Action.MoveShape:
                    //MoveReversiableBoundary(e);
                    break;

                case Action.ResizeShape:
                    ptCurrent = e.GetPosition(Window1.myCanvas);
                    Rect old0 = AreaRect;

                    AreaRect = Common.GetRect(ptOrigin, ptCurrent);

                    Common.CheckForBoundary(LeCanvas.self.Canvas, ref AreaRect, old0);
                    break;
                case Action.Nothing:
                    LeCanvas.self.Canvas.Cursor = GetMouseIcon(e);
                    if (Boundary.Contains(e.GetPosition(Window1.myCanvas)))
                    {
                        curAction = Action.MouseAtShape;
                        base.DrawMouseHoverShape();
                    }
                    break;
                case Action.MouseAtShape:
                    LeCanvas.self.Canvas.Cursor = GetMouseIcon(e);
                    if (Boundary.Contains(e.GetPosition(Window1.myCanvas)) == false)
                    {
                        curAction = Action.Nothing;
                    }
                    break;
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (curAction)
            {
                case Action.MoveShape:
                    Point dPoint = new Point(AreaRect.X - Boundary.X,
                        AreaRect.Y - Boundary.Y);
                    OnShapeMoved(dPoint);
                    break;
                case Action.ResizeShape:
                    OnShapeResized(AreaRect);
                    curAction = Action.Nothing;
                    break;
                default:
                    break;
            }

            curAction = Action.Nothing;
            LeCanvas.self.Canvas.Cursor = Cursors.Arrow;
            OnMouseReleased(e);
        }

        private Cursor GetMouseIcon(MouseEventArgs e)
        {
            Cursor ret = Cursors.Arrow;
            Position pos = CheckHotSpot(e);

            switch (pos)
            {
                case Position.Center:
                    ret = Cursors.Hand;
                    break;
                case Position.Corner:
                    ret = Cursors.Cross;
                    break;
            }
            return ret;
        }

        private void DecideUserAction(MouseButtonEventArgs e)
        {
            Position pos = CheckHotSpot(e);

            switch (pos)
            {
                case Position.Center:
                    curAction = Action.AboutToMoveShape;
                    AreaRect = Boundary;
                    ptOrigin = e.GetPosition(Window1.myCanvas);
                    OnMouseCaptured(e);
                    break;
                case Position.Corner:
                    curAction = Action.ResizeShape;
                    ptOrigin = BoundaryShape.GetOriginPoint(hotPoint, Boundary);
                    OnPrepareResizeShape(e);
                    break;
            }
        }

        private Position CheckHotSpot(MouseEventArgs e)
        {
            Point pt = e.GetPosition(Window1.myCanvas);
            Point[] hots = RectTracker.GetPointsFromRect(Boundary);

            bool[] check = new bool[8];
            check[0] = CheckPointAt(hots[0], pt,PointAtPosition.TopLeft, ref PointAt, ref hotPoint);
            //check[1] = 
            CheckPointAt(hots[1], pt, PointAtPosition.TopMiddle, ref PointAt, ref hotPoint);
            check[2] = CheckPointAt(hots[2], pt, PointAtPosition.TopRight, ref PointAt, ref hotPoint);
            //check[3] = 
            CheckPointAt(hots[3], pt, PointAtPosition.RightMiddle, ref PointAt, ref hotPoint);
            check[4] = CheckPointAt(hots[4], pt, PointAtPosition.RightBottom, ref PointAt, ref hotPoint);
            //check[5] = 
            CheckPointAt(hots[5], pt, PointAtPosition.BottomMiddle, ref PointAt, ref hotPoint);
            check[6] = CheckPointAt(hots[6], pt, PointAtPosition.BottomLeft, ref PointAt, ref hotPoint);
            //check[7] = 
            CheckPointAt(hots[7], pt, PointAtPosition.LeftMidlle, ref PointAt, ref hotPoint);

            foreach (bool p in check)
            {
                if (p == true)
                {
                    return Position.Corner;
                }
            }

            if (Boundary.Contains(pt))
            {
                return Position.Center;
            }

            return Position.Nothing;
        }

        private bool CheckPointAt(Point point,Point mousePoint, PointAtPosition pointAtPosition, ref PointAtPosition PointAt, ref Point hotPoint)
        {
            bool ret = false;
            Rect rect = Common.GetHotSpot(point);
            rect = new Rect(Common.MovePoint(rect.Location, new Point(-5, -5)),
                new Size(rect.Width + 10, rect.Height + 10));

            if (rect.Contains(mousePoint))
            {
                PointAt = pointAtPosition;
                hotPoint = point;
                ret = true;
            }
            return ret;
        }

        public static Point GetOriginPoint(Point point, Rect rect)
        {
            Point ret = new Point();
            int dx, dy;
            dx = (int)(point.X - rect.X);
            dy = (int)(point.Y - rect.Y);
            if (dx == 0 && dy == 0)
            {
                ret = new Point(rect.X + rect.Width, rect.Y + rect.Height);
            }
            else if (dx == 0 && dy != 0)
            {
                ret = new Point(rect.X + rect.Width, rect.Y);
            }
            else if (dx != 0 && dy != 0)
            {
                ret = new Point(rect.X, rect.Y);
            }
            else if (dx != 0 && dy == 0)
            {
                ret = new Point(rect.X, rect.Y + rect.Height);
            }
            return ret;
        }

        #region raise events
        public delegate void ResizingShapeMoveHandler(object sender, Rect newRect,Rect oldRect);
        public event ResizingShapeMoveHandler ShapeResized;

        public delegate void ShapeMoveHandler(object sender, Point e);
        public event ShapeMoveHandler ShapeMoved;

        public delegate void ShapePrepareResizeHandler(MouseButtonEventArgs e);
        public event ShapePrepareResizeHandler ShapePrepareResize;

        public void OnShapeMoved(Point dPoint)
        {
            MoveBorder(this, dPoint);
            if (ShapeMoved != null)
            {
                ShapeMoved(this, dPoint);
            }
            ;
        }

        private void OnShapeResized(Rect newRect)
        {
            if (ShapeResized != null)
            {
                ShapeResized(this, newRect, Boundary);
            }
        }

        public void MoveBorder(object sender, Point dPoint)
        {
            Point pt = Common.MovePoint(Boundary.Location, dPoint);
            Rect rect = new Rect(pt, Boundary.Size);

            Boundary = rect;
        }

        private void OnPrepareResizeShape(MouseButtonEventArgs e)
        {
            if (ShapePrepareResize != null)
            {
                ShapePrepareResize(e); 
            }
        }

        #endregion

        #region mouse hover or leave
        public delegate void MouseCapturedHandler(object sender, MouseButtonEventArgs e);
        public event MouseCapturedHandler MouseCaptured;

        public delegate void MouseReleasedHandler(object sender, MouseButtonEventArgs e);
        public event MouseReleasedHandler MouseReleased;

        private void OnMouseCaptured(MouseButtonEventArgs e)
        {
            if (MouseCaptured != null)
            {
                MouseCaptured(LeCanvas.self.Canvas, e);
            }
        }

        private void OnMouseReleased(MouseButtonEventArgs e)
        {
            if (MouseReleased != null)
            {
                MouseReleased(LeCanvas.self.Canvas, e);
            }
        }
        #endregion

        internal override void Draw(DrawingContext drawingContext)
        {
            Brush fillBrush = new LinearGradientBrush(FromColor, ToColor, LightAngle);
            Pen borderPen =new Pen(new SolidColorBrush(FromColor),BorderWidth);

            if (ShowBorder == false) borderPen = null;
            if (Fill == false) fillBrush = null;
            drawingContext.DrawRectangle(fillBrush,borderPen,Boundary);
        }

    }
}
