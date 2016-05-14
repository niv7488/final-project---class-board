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

using LePaint.Controller;

namespace LePaint.Shapes
{
    public class Rhombus : BoundaryShape  
    {
        public Rhombus(Point pt)
            : base(pt)
        {
            ShowBorder = false;
        }

        private Rhombus() {
        }

        private List<Point> CreatePath()
        {
            ArrayList origin = Common.GetPointsFromRect(Boundary);
            Point[] pt = new Point[origin.Count];

            pt[0] = Common.GetMidPoint((Point)origin[0], (Point)origin[1]);
            pt[1] = Common.GetMidPoint((Point)origin[1], (Point)origin[2]);
            pt[2] = Common.GetMidPoint((Point)origin[2], (Point)origin[3]);
            pt[3] = Common.GetMidPoint((Point)origin[3], (Point)origin[0]);

            List<Point> ret = new List<Point>();
            ret.AddRange(pt);

            return ret;
        }

        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            ptOrigin = e.GetPosition(Window1.Self.myCanvas);
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
            ptCurrent = e.GetPosition(Window1.Self.myCanvas);
            bounds = Common.GetRect(ptOrigin, ptCurrent);

            RefreshDrawing();
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizingShape)
            {
                ptCurrent = e.GetPosition(Window1.Self.myCanvas);
                bounds = Common.GetRect(ptPrevious, ptCurrent);

                RefreshDrawing();
            }
            else
            {
                base.MouseMove(sender, e);
            }
        }

        internal override void Draw(DrawingContext drawingContext)
        {
            List<Point> points = CreatePath();

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

            pf.StartPoint = points[0];
            foreach (Point p in points)
            {
                pf.Segments.Add(new LineSegment(p, true));
            }
            pg.Figures.Add(pf);
            drawingContext.DrawGeometry(fillBrush, borderPen, pg);
        }

        public override string ToString()
        {
            return "Rhombus";
        }

    }
}
