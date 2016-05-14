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

using LePaint.Controller;

namespace LePaint.Shapes
{
    public class Parallelogram : BoundaryShape
    {
        public List<Point> curPoints = new List<Point>();
        public Parallelogram(Point pt)
            : base(pt)
        {
            ShowBorder = true;

            CreateShape();
        }

        private void CreateShape()
        {
            curPoints = new List<Point>();
            bounds.Width = 60;
            bounds.Height = 30;
            bounds.Location = ptOrigin;

            curPoints.Add(Common.MovePoint(ptOrigin, new Point(10, 0)));
            curPoints.Add(Common.MovePoint(ptOrigin, new Point(bounds.Width, 0)));
            curPoints.Add(Common.MovePoint(ptOrigin, new Point(bounds.Width - 10, bounds.Height)));
            curPoints.Add(Common.MovePoint(ptOrigin, new Point(0, bounds.Height)));
        }


        public Parallelogram()
            : base()
        {
            //LeMenu.ShapeReloaded += new //LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }


        #region IShape implementation

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizingShape)
            {
                ptCurrent = e.GetPosition(Window1.Self.myCanvas);
            }
            else
            {
                base.MouseMove(sender, e);
            }
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
            //stop the base class been called!
            //base.DrawMouseMove(e);
        }

        public override void MoveShape(Point ptPrevious, Point ptcurrent)
        {
            Point pt = new Point(ptCurrent.X - ptPrevious.X, ptCurrent.Y - ptPrevious.Y);
            List<Point> newPoints = new List<Point>();
            foreach (Point point in curPoints)
            {
                newPoints.Add(Common.MovePoint(point, pt));
            }
            curPoints = newPoints;
            bounds = Common.GetBoundsFromPoints(curPoints.ToArray()); 
        }
        #endregion


        private List<Point> RotatePoints(Point ptFixed, Point ptCurrent)
        {
            double dx = ptCurrent.X - hotPoint.X;
            double dy = ptCurrent.Y - hotPoint.Y;

            Point[] pt = curPoints.ToArray();
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


        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            ptOrigin = e.GetPosition(Window1.Self.myCanvas);
            CreateShape();

            Selected = true;
            RefreshDrawing();
        }

        internal override void Draw(DrawingContext drawingContext)
        {

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

                pf.StartPoint = curPoints[0];

                foreach (Point p in curPoints)
                {
                    pf.Segments.Add(new LineSegment(p, true));
                }

                //pf.Segments.Add(new LineSegment(curPoints[0],false));

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
