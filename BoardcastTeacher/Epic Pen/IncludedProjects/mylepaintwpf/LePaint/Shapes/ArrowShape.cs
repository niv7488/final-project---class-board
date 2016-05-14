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

using LePaint.Controller;

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
            }
        }

        #endregion

        #region constructor
        /// <summary>
        /// For tool box to use
        /// </summary>
        /// <param name="pt"></param>
        public ArrowShape(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            arrowButtWidth = 15;
            arrowTipWidth = 30;

            ptOrigin =bounds.Location;
            ptOrigin = Common.MovePoint(ptOrigin, new Point(0, bounds.Height/2));
            ptCurrent = Common.MovePoint(ptOrigin, new Point(bounds.Width, 0));
            StartPoint = ptOrigin;
            EndPoint = ptCurrent;
        }

        /// <summary>
        /// For XML Serializer to use
        /// </summary>
        public ArrowShape()
        {
        }

        #endregion

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

        #region IShape implementation

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizingShape)
            {
                ptCurrent = e.GetPosition(Window1.Self.myCanvas);
                StartPoint = ptOrigin;
                EndPoint = ptCurrent;
                bounds = Common.GetRect(ptOrigin, ptCurrent);

                RefreshDrawing();
            }
            else
               base.MouseMove(sender, e);
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
            ptCurrent = e.GetPosition(Window1.Self.myCanvas);
            bounds = Common.GetRect(ptOrigin, ptCurrent);

            StartPoint = ptOrigin;
            EndPoint = ptCurrent;

            RefreshDrawing();
        }

        public override void MoveShape(Point ptPrevious, Point ptCurrent)
        {
            Point pt = new Point(ptCurrent.X - ptPrevious.X, ptCurrent.Y - ptPrevious.Y);
            StartPoint = Common.MovePoint(StartPoint, pt);
            EndPoint = Common.MovePoint(EndPoint, pt);
        }

        #endregion

        internal override void Draw(DrawingContext drawingContext)
        {
            List<Point> points = CreateLines(StartPoint, EndPoint); 

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
                pf.StartPoint = points[0]; 
                foreach (Point p in points)
                {
                    pf.Segments.Add(new LineSegment(p, true));
                }


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
