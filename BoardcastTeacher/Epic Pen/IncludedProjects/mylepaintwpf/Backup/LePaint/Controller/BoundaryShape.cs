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

namespace LePaint.Controller
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
                    ptPrevious = e.GetPosition(Window1.Self.myCanvas);
                    break;
                case Action.MoveShape:
                    ptCurrent = e.GetPosition(Window1.Self.myCanvas);
                    MoveShape(ptPrevious, ptCurrent);
                    bounds = Common.MoveRect(ptCurrent, ptPrevious, bounds);
                    RefreshDrawing();
                    ptPrevious = ptCurrent;
                    break;

                case Action.ResizeShape:
                    ptCurrent = e.GetPosition(Window1.Self.myCanvas);
                    bounds = Common.GetRect(ptPrevious, ptCurrent);

                    RefreshDrawing();
                    break;

                case Action.Nothing:
                    DrawingController.self.Canvas.Cursor = GetMouseIcon(e);
                    if (Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
                    {
                        curAction = Action.MouseAtShape;
                        base.DrawMouseHoverShape();
                    }
                    break;
                case Action.MouseAtShape:
                    DrawingController.self.Canvas.Cursor = GetMouseIcon(e);
                    if (Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)) == false)
                    {
                        curAction = Action.Nothing;
                    }
                    break;
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            curAction = Action.Nothing;
            isResizingShape = false;
            DrawingController.self.Canvas.Cursor = Cursors.Arrow;
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
                    ptPrevious = e.GetPosition(Window1.Self.myCanvas);
                    break;
                case Position.Corner:
                    curAction = Action.ResizeShape;
                    isResizingShape = true;
                    ptPrevious = BoundaryShape.GetOriginPoint(hotPoint, Boundary);
                    BackUpPoints();
                    break;
            }
        }

        private Position CheckHotSpot(MouseEventArgs e)
        {
            Point pt = e.GetPosition(Window1.Self.myCanvas);
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

        public void MoveBorder(object sender, Point dPoint)
        {
            Point pt = Common.MovePoint(Boundary.Location, dPoint);
            Rect rect = new Rect(pt, Boundary.Size);

            Boundary = rect;
        }

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
