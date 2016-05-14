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

using LePaint.MainPart;
using LePaint.Basic;

namespace LePaint.Shapes
{
    public class Rhombus : BoundaryShape  
    {
        List<Point> tempPointList;
        public Rhombus(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            CreatePath();
        }

        private Rhombus() {
            //LeMenu.ShapeReloaded += new //LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            RegisterEvents();
        }

        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > 30&& AreaRect.Height > 30) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                CreatePath();
                RegisterEvents();
            }
            else path = null;

            ;

            return check;
        }

        private void CreatePath()
        {
            ArrayList origin = Common.GetPointsFromRect(Boundary);
            Point[] pt = new Point[origin.Count];

            pt[0] = Common.GetMidPoint((Point)origin[0], (Point)origin[1]);
            pt[1] = Common.GetMidPoint((Point)origin[1], (Point)origin[2]);
            pt[2] = Common.GetMidPoint((Point)origin[2], (Point)origin[3]);
            pt[3] = Common.GetMidPoint((Point)origin[3], (Point)origin[0]);

            tempPointList = new List<Point>();
            tempPointList.AddRange(pt);
            // path.AddPolygon(pt);
        }

        private void RegisterEvents()
        {
            base.ShapeMoved += new BoundaryShape.ShapeMoveHandler(OnMoveBorder);
            base.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(ResizeBorder);
        }
        void OnMoveBorder(object sender, Point dPoint)
        {
            Boundary = base.Boundary;
            CreatePath(); 
        }

        void ResizeBorder(object sender, Rect newRect, Rect oldRect)
        {
            Boundary = newRect;
            CreatePath();
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
            return "Rhombus";
        }

    }
}
