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
using LePaint.Basic;

namespace LePaint.Shapes
{
    public class RoundRectShape:BoundaryShape 
    {
        private int radius = 3;
        public int Radius
        {
            set
            {
                radius = value;
                ;
            }
            get { return radius; }
        }

        public RoundRectShape(Point pt)
            : base(pt)
        {
            ShowBorder = true;
            Fill = true;
        }
        private RoundRectShape() { }

        public bool ShapeSizeOK(Point ptOrigin, Point ptCurrent)
        {
            Rect areaRect = Common.GetRect(ptOrigin, ptCurrent);

            if (areaRect.Width > 20 && areaRect.Height > 10)
            {
                return true;
            }
            else return false;
        }

        public override void Draw()
        {
            Point[] pt = new Point[8];
            pt[0] = Common.MovePoint(Boundary.Location, new Point(radius, 0));
            pt[1] = Common.MovePoint(pt[0], new Point(Boundary.Width - radius * 2, 0));

            pt[2] = Common.MovePoint(pt[1], new Point(radius, radius));
            pt[3] = Common.MovePoint(pt[2], new Point(0, Boundary.Height - radius * 2));

            pt[4] = Common.MovePoint(pt[1], new Point(0, Boundary.Height));
            pt[5] = Common.MovePoint(pt[0], new Point(0, Boundary.Height));

            pt[6] = Common.MovePoint(pt[3], new Point(-Boundary.Width, 0));
            pt[7] = Common.MovePoint(Boundary.Location, new Point(0,radius));

            path = new Path();

                //path.AddArc(new Rect(Boundary.Location, new Size(radius, radius)), 180, 90);
                //path.AddLine(pt[0], pt[1]);
                //path.AddArc(new Rect(pt[1], new Size(radius, radius)), 270, 90);
                //path.AddLine(pt[2], pt[3]);
                //path.AddArc(new Rect(new Point(pt[1].X, pt[3].Y), new Size(radius, radius)),
                //    0, 90);
                //path.AddLine(pt[4], pt[5]);
                //path.AddArc(new Rect(pt[6], new Size(radius, radius)),
                 //   90, 90);
                //path.AddLine(pt[6], pt[7]);

            if (path != null)
            {
                //g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(
                //    Boundary, FromColor, ToColor, LightAngle), path);
            }
        }

        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > 30&& AreaRect.Height > 30) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                base.ShapeResized += new ResizingShapeMoveHandler(RoundRectShape_ShapeResized);
            }
            else path = null;

            ;

            return check;
        }

        void RoundRectShape_ShapeResized(object sender, Rect newRect, Rect oldRect)
        {
            Boundary = newRect;
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

            drawingContext.DrawRoundedRectangle(fillBrush, borderPen, bounds, radius, radius);
        }


        public override string ToString()
        {
            return "RoundRectShape";
        }
    }
}
