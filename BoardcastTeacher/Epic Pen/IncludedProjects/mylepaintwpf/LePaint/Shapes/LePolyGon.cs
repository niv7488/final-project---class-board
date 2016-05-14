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

using LePaint.Controller;

namespace LePaint.Shapes
{
    public class LePolyGon : BoundaryShape
    {
        private Point firstPoint;
        public Point FirstPoint
        {
            get { return firstPoint; }
            set { firstPoint = value; }
        }

        private int totalPoints;
        public int TotalPoints
        {
            get { return totalPoints; }
            set { totalPoints = value; }
        }

        #region constructor
        public LePolyGon(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            int size = 20;
            CenterPoint = Common.MovePoint(pt, new Point(size, size));
            firstPoint = Common.MovePoint(pt, new Point(size * 2, size));
            tempPoint = firstPoint;
        }

        public LePolyGon()
            : base()
        {

        }

        #endregion

        #region create shape part

        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            base.DrawMouseDown(e);
            ptOrigin = e.GetPosition(Window1.Self.myCanvas);

            Selected = true;
            RefreshDrawing();
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
            //stop the base class been called!
            //base.DrawMouseMove(e);
        }
        #endregion

        #region IShape implementation

        Point tempPoint;
        public override void BackUpPoints()
        {
            tempPoint = firstPoint;
            //orginPoints = curPoints;
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizingShape)
            {
                ptCurrent = e.GetPosition(Window1.Self.myCanvas);

                firstPoint = Common.TurnPoint(tempPoint, CenterPoint, hotPoint, ptCurrent, 1);

                RefreshDrawing();
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
        private List<Point> CreatePath()
        {
            int n = totalPoints;

            int totalAngle = 180 * (n - 2);
            int singleAngle = 360 / n;

            //int angle0 = (int)Common.GetAngle(CenterPoint , tempPoint);
            int angle = (int)Common.GetAngle(CenterPoint, firstPoint);

            List<Point> ret = new List<Point>();

            Point[] pt = new Point[n];

            int size = Common.GetLength(CenterPoint, firstPoint);
            pt[0] = firstPoint;

            for (int i = 1; i < n; i++)
            {
                int dx = (int)(size * Math.Cos((singleAngle * i+angle) * Math.PI / 180));
                int dy = (int)(size * Math.Sin((singleAngle * i+angle) * Math.PI / 180));

                pt[i] = Common.MovePoint(CenterPoint, new Point(dx, dy));
            }
            ret.AddRange(pt);
            bounds = Common.GetBoundsFromPoints(pt);

            return ret;
        }

        public override void MoveShape(Point ptPrevious, Point ptCurrent)
        {
            Point pt = new Point(ptCurrent.X - ptPrevious.X, ptCurrent.Y - ptPrevious.Y);
            CenterPoint = Common.MovePoint(CenterPoint, pt);
            firstPoint = Common.MovePoint(firstPoint, pt);
        }

        #endregion

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

            PathGeometry pg = new PathGeometry();

            PathFigure pf = new PathFigure();

            List<Point> points = CreatePath();

            pf.StartPoint = points[0];

            foreach (Point p in points)
            {
                pf.Segments.Add(new LineSegment(p, true));
            }

            pg.Figures.Add(pf);
            drawingContext.DrawGeometry(fillBrush, borderPen, pg);
        }
    }
}
