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
using System.Xml.Serialization;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class ArrowShape : BoundaryShape 
    {
        #region Properties
        private Point ptStart;
        public Point StartPoint
        {
            get { return ptStart; }
            set { ptStart = value; }
        }

        private Point ptEnd;
        public Point EndPoint
        {
            get { return ptEnd; }
            set { ptEnd = value; }
        }

        private int arrowTipWidth = 40;
        [XmlElement("ArrowTipWidth")]
        public int ArrowTipWidth
        {
            get { return arrowTipWidth; }
            set
            {
                arrowTipWidth = value;
                LeMenu_ShapeReloaded(this);
                ; 
            }
        }

        private int arrowButtWidth = 20;
        [XmlElement("ArrowButtWidth")]
        public int ArrowButtWidth
        {
            get { return arrowButtWidth; }
            set
            {
                arrowButtWidth = value;
                LeMenu_ShapeReloaded(this);
                ;
            }
        }

        #endregion

        #region private fields
        List<Point> tempPointList;

        Geometry curGeometry;
        #endregion

        #region constructor
        public ArrowShape(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            arrowButtWidth = 15;
            arrowTipWidth = 30;

            ptOrigin =bounds.Location;
            ptOrigin = Common.MovePoint(ptOrigin, new Point(0, bounds.Height/2));
            ptCurrent = Common.MovePoint(ptOrigin, new Point(bounds.Width, 0));
            tempPointList = CreateLines(this.ptOrigin, this.ptCurrent);

        }

        public ArrowShape()
        {
            ////LeMenu.ShapeReloaded += new //LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        public void Change()
        {
            arrowTipWidth = 20;
            arrowButtWidth = 10;
            Rect rect = Boundary;
            this.ptCurrent=Common.MovePoint(this.ptCurrent,new Point (-8,4)); 
            tempPointList = CreateLines(this.ptCurrent,this.ptOrigin);
            FromColor = Color.FromArgb(255,255,255,255);
            ToColor = Color.FromArgb(255, 255, 255, 255);

            CreatePath();

            Boundary = rect;
        }
        void LeMenu_ShapeReloaded(object sender)
        {
            tempPointList = CreateLines(StartPoint, EndPoint);
            CreatePath();
        }
        #endregion

        public void CreatePath()
        {
        }

        #region temp drawing rubber arrow
        internal void DrawReversibleArrow( Point ptOrigin, ref Point ptCurrent)
        {
            /*
            CheckBoundary(ref ptCurrent);
            Rect rect = new Rect();
            tempPointList = new ArrayList();

            rect = Common.GetRect(ptOrigin, ptCurrent);

            //if (rect.Width < 30 && rect.Height < 30 )
            {
                isDrawingOK = false;
            }
            //else
            {
                tempPointList = CreateLines(ptOrigin,ptCurrent);
                if (tempPointList.Count > 0)
                {
                    DrawReversibleLines(tempPointList);
                }

                isDrawingOK = true;
            }
             */ 
        }

        private List<Point> CreateLines(Point startPoint,Point endPoint)
        {
            int angle = GetAngle(startPoint,endPoint);

            Point[] pt = new Point[7];
            pt[0] = endPoint;

            double ra = (angle - 30) * Math.PI / 180;
            int dx = (int)(arrowTipWidth  * Math.Cos(ra));
            int dy = (int)(arrowTipWidth * Math.Sin(ra));
            pt[1] = new Point(endPoint.X - dx, endPoint.Y - dy);

            dx = (int)(arrowTipWidth * Math.Sin(angle * Math.PI / 180) / 4);
            dy = (int)(arrowTipWidth * Math.Cos(angle * Math.PI / 180) / 4);
            pt[2] = new Point(pt[1].X + dx, pt[1].Y - dy);

            pt[5] = new Point(pt[1].X + 3 * dx, pt[1].Y - 3 * dy);
            pt[6] = new Point(pt[1].X + 4 * dx, pt[1].Y - 4 * dy);

            dx = (int)(arrowButtWidth * Math.Sin(angle * Math.PI / 180) / 2);
            dy = (int)(arrowButtWidth * Math.Cos(angle * Math.PI / 180) / 2);

            pt[3] = new Point(startPoint.X - dx, startPoint.Y + dy);
            pt[4] = new Point(startPoint.X + dx, startPoint.Y - dy);

            List<Point> ret = new List<Point>(); 
            ret.AddRange(pt);
            return ret;
        }

        private int GetAngle(Point startPoint, Point endPoint)
        {
            double dx = (endPoint.X - startPoint.X);
            double dy = (endPoint.Y - startPoint.Y);
            double val = Math.Atan2(dy, dx);

            int ret = (int)(180*val / Math.PI);
            return ret;
        }

        private void DrawReversibleLines(ArrayList tempPointList)
        {
            Point[] points = (Point[])tempPointList.ToArray(typeof(Point));
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

        private void CheckBoundary( ref Point ptCurrent)
        {
            Rect toTest = new Rect();// GDIApi.GetViewableRect(LeCanvas.self.Canvas);
            if (ptCurrent.X < toTest.Left + 5) ptCurrent.X = toTest.Left + 5;
            if (ptCurrent.Y < toTest.Top + 5) ptCurrent.Y = toTest.Top + 5;

            if (ptCurrent.X > toTest.Width + toTest.X - 5)
            {
                ptCurrent.X = toTest.Width + toTest.X - 5;
            }
            if (ptCurrent.Y > toTest.Height + toTest.Y - 5)
            {
                ptCurrent.Y = toTest.Height + toTest.Y - 5;
            }
        }

        #endregion

        #region IShape implementation

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (shapeResizing)
            {
                if (ptCurrent.X > 0)
                {
                    DrawReversibleArrow(ptOrigin, ref ptCurrent);
                }
                ptCurrent = e.GetPosition(Window1.myCanvas);
                DrawReversibleArrow(ptOrigin, ref ptCurrent);
            }
            else
               base.MouseMove(sender, e);
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
            if (ptCurrent.X > 0)
            {
            }

            ptCurrent = e.GetPosition(Window1.myCanvas);
        }

        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {
            if (isDrawingOK  == true)
            {
                ptStart = ptOrigin;
                ptEnd = ptCurrent; 
                this.CreatePath();
                base.ShapeMoved+=new ShapeMoveHandler(ArrowShape_ShapeMoved);
                base.ShapePrepareResize += delegate(MouseButtonEventArgs e0)
                {
                    shapeResizing = true;
                };
                base.ShapeResized += new ResizingShapeMoveHandler(ArrowShape_ShapeResized);
            }

            ;
            return isDrawingOK; 
        }

        void ArrowShape_ShapeMoved(object sender, Point e)
        {
            /*
            tempPointList = new ArrayList();
            foreach (Point p in arrowPoints)
            {
                tempPointList.Add(Common.MovePoint(p, e));
            }
            ptOrigin = Common.MovePoint(ptOrigin, e);
            ptCurrent = Common.MovePoint(ptCurrent, e);
            */
            CreatePath();
        }

        void ArrowShape_ShapeResized(object sender, Rect newRect, Rect oldRect)
        {
            CreateLines(ptOrigin, ptCurrent);
            CreatePath();
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

        public override string ToString()
        {
            return "Arrow";
        }
    }
}
