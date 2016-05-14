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

using System.Xml;
using System.Xml.Serialization;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class Parallelogram : BoundaryShape
    {
        public List<Point> tempPointList = new List<Point>();
        public Parallelogram(Point pt)
            : base(pt)
        {
            ShowBorder = true;

            //ptOrigin = Common.MovePoint(ptOrigin, new Point(bounds.Width/2 , bounds.Height/2 ));

            tempPointList = new List<Point>();

            Point[] ptArray = new Point[4];

            ptArray[0] = Common.MovePoint(ptOrigin, new Point(10, 0));
            ptArray[1] = Common.MovePoint(ptOrigin, new Point(bounds.Width, 0));
            ptArray[2] = Common.MovePoint(ptOrigin, new Point(bounds.Width-10,bounds.Height));
            ptArray[3] = Common.MovePoint(ptOrigin, new Point(0, bounds.Height));

            CreateNewShape(ptArray);
        }


        public Parallelogram()
            : base()
        {
            //LeMenu.ShapeReloaded += new //LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            Point[] pt = new Point[tempPointList.Count];

            int i = 0;
            foreach (Point p in tempPointList)
            {
                pt[i++] = p;
            }

            CreateNewShape(pt);

            RegisterEvents();
        }

        #region create shape part


        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > 30&& AreaRect.Height > 30) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                InitShape(AreaRect);
                RegisterEvents();
            }
            else path = null;

            ;

            return check;
        }

        private void InitShape(Rect AreaRect)
        {
            Point[] pt = new Point[4];
            pt[0] = Common.MovePoint(AreaRect.Location, new Point(10, 0));
            pt[1] = Common.MovePoint(AreaRect.Location, new Point(AreaRect.Width, 0));
            pt[2] = Common.MovePoint(pt[1], new Point(-10, AreaRect.Height));
            pt[3] = Common.MovePoint(AreaRect.Location, new Point(0, AreaRect.Height));

            CreateNewShape(pt);
        }
        #endregion

        #region IShape implementation

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (shapeResizing)
            {
                if (ptCurrent.X > 0)
                {
                    DrawReversiblePoints(ptOrigin, ref ptCurrent);
                }
                ptCurrent = e.GetPosition(Window1.myCanvas);
                DrawReversiblePoints(ptOrigin, ref ptCurrent);
            }
            else
            {
                base.MouseMove(sender, e);
            }
        }

        #endregion

        protected void CreateNewShape(Point[] pt)
        {
            tempPointList = new List<Point>();

            tempPointList.AddRange(pt);

            Rect rect = Common.Convert(new Rect());//path.GetBounds());
            Boundary = rect;
        }

        internal void DrawReversiblePoints(Point ptFixed, ref Point ptCurrent)
        {
            /*
            isDrawingOK = false;
            tempPoints = RotatePoints(ptFixed, ptCurrent);
            if (CheckShape(tempPoints.ToArray()))
            {
                DrawReversibleLines(tempPoints);
                isDrawingOK = true;
            }
             */ 
        }

        private List<Point> RotatePoints(Point ptFixed, Point ptCurrent)
        {
            double dx = ptCurrent.X - hotPoint.X;
            double dy = ptCurrent.Y - hotPoint.Y;

            Point[] pt = tempPointList.ToArray(); 
            if (PointAt == PointAtPosition.BottomLeft ||
                PointAt == PointAtPosition.RightBottom)
            {
                pt[2] = Common.MovePoint(pt[2], new Point(dx, dy));
                pt[3] = Common.MovePoint(pt[3], new Point(dx, dy));
            }
            else if (PointAt == PointAtPosition.TopLeft ||
                PointAt == PointAtPosition.TopRight)
            {
                pt[0] = Common.MovePoint(pt[0], new Point(dx, dy));
                pt[1] = Common.MovePoint(pt[1], new Point(dx, dy));
            }
            else if (PointAt == PointAtPosition.RightMiddle)
            {
                pt[1] = Common.MovePoint(pt[1], new Point(dx, dy));
                pt[2] = Common.MovePoint(pt[2], new Point(dx, dy));
            }

            List<Point> ret = new List<Point>();
            ret.AddRange(pt); 
            return ret;
        }

        protected bool CheckShape(Point[] pt)
        {
            Rect oldRect = Common.Convert(new Rect());//path.GetBounds());
            Path newPath = new Path();
            //newPath.po.AddPolygon(pt);
            //Rect rect = Common.Convert(newnew Rect());//path.GetBounds());
            //bool check = Common.CheckForBoundary(LeCanvas.self.Canvas, ref rect, oldRect);
            //newPath = null;
            return false;// check;
        }

        protected void DrawReversibleLines(List<Point> tempPointList)
        {
            Point[] points = (Point[])tempPointList.ToArray();
            Point p0 = points[0];
            for (int i = 1; i < points.GetLength(0); i++)
            {
                Point p1 = points[i];
                Point p00 = LeCanvas.self.Canvas.PointToScreen(p0);
                Point p11 = LeCanvas.self.Canvas.PointToScreen(p1);
                //ControlPaint.DrawReversibleLine(p00, p11, Color.Black);
                p0 = points[i];
            }

            Point p000 = LeCanvas.self.Canvas.PointToScreen(p0);
            Point p111 = LeCanvas.self.Canvas.PointToScreen(points[0]);
            //ControlPaint.DrawReversibleLine(p000, p111, Color.Black);
        }

        private void RegisterEvents()
        {
            base.ShapePrepareResize += delegate(MouseButtonEventArgs e)
            {
                centerPoint = Common.GetCentre(tempPointList);
                ptCurrent.X = -1;
                shapeResizing = true;
            };
            base.ShapeResized += new ResizingShapeMoveHandler(Pentagon_ShapeResized);
            base.ShapeMoved += new ShapeMoveHandler(Pentagon_ShapeMoved);
        }

        void Pentagon_ShapeMoved(object sender, Point e)
        {
            Point[] pt = new Point[tempPointList.Count];

            int i = 0;
            foreach (Point p in tempPointList)
            {
                pt[i++] = Common.MovePoint(p, e);
            }

            CreateNewShape(pt);
        }


        void Pentagon_ShapeResized(object sender, Rect newRect, Rect oldRect)
        {
            if (isDrawingOK == true)
            {
                //Point[] pt1 = tempPoints.ToArray();
               // CreateNewShape(pt1);
            }
            shapeResizing = false;
        }

        internal override void Draw(DrawingContext drawingContext)
        {
            DrawText(drawingContext);

            GradientStopCollection gradient = new GradientStopCollection(2);
            gradient.Add(new GradientStop(FromColor, 1.0));
            gradient.Add(new GradientStop(ToColor, 0.0));

            // Create the LinearGradientBrushes

            LinearGradientBrush fillBrush = new LinearGradientBrush(gradient, new Point(0.0, 0.0), new Point(1, 0.0));

            Pen borderPen = new Pen(new SolidColorBrush(BorderColor), BorderWidth);

            if (ShowBorder == false) borderPen = null;
            if (Fill == false) fillBrush = null;
            ///method 1 to draw polygon
            {

                PathGeometry pg = new PathGeometry();

                PathFigure pf = new PathFigure();

                foreach (Point p in tempPointList)
                {
                    pf.Segments.Add(new LineSegment(p, true));
                }

                pf.Segments.Add(new LineSegment(tempPointList[0], true));

                pg.Figures.Add(pf);
                drawingContext.DrawGeometry(fillBrush, borderPen, pg);
            }
        }

        public override string ToString()
        {
            return "Parallelogram";
        }

    }
}
