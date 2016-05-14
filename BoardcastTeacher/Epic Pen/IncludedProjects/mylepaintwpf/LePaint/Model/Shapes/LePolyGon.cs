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

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class LePolyGon : BoundaryShape
    {
        public List<Point> tempPointList;
        public int TotalPoints;

        int size = 30;

        #region constructor
        public LePolyGon(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            ptOrigin = Common.MovePoint(ptOrigin, new Point(size,size/2));
        }

        public LePolyGon()
            : base()
        {
            ////LeMenu.ShapeReloaded += new //LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            Point[] pt = new Point[tempPointList.Count];

            int i = 0;
            foreach (Point p in tempPointList)
            {
                pt[i++] = p;
            }

            tempPointList.AddRange(pt);
            CreateNewShape(pt);

            RegisterEvents();
        }
        #endregion

        #region create shape part

        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            base.DrawMouseDown(e);
            size = 30* 2;
            ptOrigin = e.GetPosition(Window1.myCanvas);

            InitShape(TotalPoints);
            RegisterEvents();
        }

        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {

            return true;
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

        #region Shape creation
        /// <summary>
        /// Mouse down create a equal side length triangle
        /// </summary>
        /// <param name="ptTemp"></param>
        public virtual void InitShape(int n)
        {
            TotalPoints = n;
            int totalAngle = 180 * (n - 2);
            int singleAngle = 360 / n;

            tempPointList = new List<Point>();

            Point[] pt = new Point[n];

            centerPoint = ptOrigin;

            pt[0] = Common.MovePoint(ptOrigin, new Point(size, 0));
            for (int i = 1; i < n; i++)
            {
                int dx = (int)(size * Math.Cos(singleAngle * i * Math.PI / 180));
                int dy = (int)(size * Math.Sin(singleAngle * i * Math.PI / 180));

                pt[i] = Common.MovePoint(ptOrigin, new Point(dx, dy));
            }
            tempPointList.AddRange(pt);
            CreateNewShape(pt);
        }

        protected void CreateNewShape(Point[] pt)
        {
            path = new Path();
            //// path.AddPolygon(pt);
            //Rect rect = Common.Convert(new Rect());//path.GetBounds());
            //Boundary = rect;

        }

        private void MovePoints(Point[] pt)
        {
            tempPointList = new List<Point>();
            tempPointList.AddRange(pt);

            path = new Path();
            //// path.AddPolygon(pt);
           // Rect rect = Common.Convert(new Rect());//path.GetBounds());
           // Boundary = rect;
        }

        protected bool CheckShape(Point[] pt)
        {
            Rect oldRect = new Rect();// Common.Convert(new Rect());//path.GetBounds());
            //Path newPath = new Path();
            //new// path.AddPolygon(pt);
            //Rect rect = Common.Convert(newnew Rect());//path.GetBounds());
            //bool check = Common.CheckForBoundary(LeCanvas.self.Canvas, ref rect, oldRect);
            //newPath = null;
            return false;// check;
        }

        #endregion

        #region rotate triangle
        internal virtual void DrawReversiblePoints(Point ptDown, ref Point ptCurrent)
        {
            isDrawingOK = false;
            tempPointList = RotatePoints(ptOrigin, ptCurrent);
            if (CheckShape(tempPointList.ToArray()))
            {
                DrawReversibleLines(tempPointList);
                isDrawingOK = true;
            }
        }

        private List<Point> RotatePoints(Point originPoint, Point endPoint)
        {
            List<Point> ret = Common.TurnPoints(tempPointList, centerPoint, originPoint, endPoint,1);
            return ret;
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

            MovePoints(pt);
        }

        void Pentagon_ShapeResized(object sender, Rect newRect, Rect oldRect)
        {
            if (isDrawingOK == true)
            {
                MovePoints(tempPointList.ToArray());
            }
            shapeResizing = false;
        }
        #endregion

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

            /*

            StreamGeometry geomentry = new StreamGeometry();

            using (StreamGeometryContext ctx = geomentry.Open())
            {

                //polyPoints array contains the polyline points.

                ctx.BeginFigure(tempPointList[0], false, false);

                ctx.PolyLineTo(tempPointList, true, true);

            }

            //geomentry.Freeze();


            drawingContext.DrawGeometry(fillBrush, borderPen, geomentry);

            /*
            GradientStopCollection gradient = new GradientStopCollection(2);
            gradient.Add(new GradientStop(FromColor, 1.0));
            gradient.Add(new GradientStop(ToColor, 0.0));

            // Create the LinearGradientBrushes

            LinearGradientBrush fillBrush = new LinearGradientBrush(gradient, new Point(0.0, 0.0), new Point(1, 0.0));

            Pen borderPen = new Pen(new SolidColorBrush(BorderColor), BorderWidth);

            if (ShowBorder == false) borderPen = null;
            if (Fill == false) fillBrush = null;

            drawingContext.DrawGeometry(fillBrush, borderPen, path);
            */
            //drawingContext.DrawRoundedRectangle(fillBrush, borderPen, bounds, radius, radius);
        }
    }
}
