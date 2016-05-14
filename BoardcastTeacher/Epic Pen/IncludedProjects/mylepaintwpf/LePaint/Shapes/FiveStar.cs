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
    public class FiveStar : LePolyGon 
    {

        private float ratio = 2;
        public float Ratio
        {
            get { return ratio; }
            set
            {
                if (ratio <= 1)
                {
                    ratio = 1;
                }
                ratio = value;
            }
        }

        public int InnerRadius
        {
            get { return (int)(outerRadius / ratio); }
        }

        private int outerRadius = 60;
        public int OuterRadius
        {
            set
            {
                outerRadius = value;
            }
            get { return outerRadius; }
        }

        public FiveStar(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            FirstPoint  = Common.MovePoint(pt, new Point(outerRadius, outerRadius/2));
            CenterPoint = Common.MovePoint(pt, new Point(outerRadius/2, outerRadius/2)); 
            TotalPoints = (5); 
        }


        public FiveStar()
            : base()
        {
        }


        protected List<Point> CreatePath()
        {
            int dx, dy;
            int n = TotalPoints;
            int singleAngle = 360 / n;

            int angle = (int)Common.GetAngle(CenterPoint, FirstPoint);

            Point[] pt = new Point[n];

            pt[0] = FirstPoint;
            
            outerRadius = Common.GetLength(CenterPoint, FirstPoint);
            for (int i = 1; i < n; i++)
            {
                dx = (int)(outerRadius * Math.Cos((singleAngle * i +angle)* Math.PI / 180));
                dy = (int)(outerRadius * Math.Sin((singleAngle * i +angle)* Math.PI / 180));

                pt[i] = Common.MovePoint(CenterPoint , new Point(dx, dy));
            }

            Point[] pt1 = new Point[n];

            dx = (int)(InnerRadius * Math.Cos(((singleAngle / 2)+angle) * Math.PI / 180));
            dy = (int)(InnerRadius * Math.Sin(((singleAngle / 2)+angle) * Math.PI / 180));

            pt1[0] = Common.MovePoint(CenterPoint, new Point(dx, dy));
            for (int i = 1; i < n; i++)
            {
                dx = (int)(InnerRadius * Math.Cos((singleAngle * (i) + singleAngle / 2+angle) * Math.PI / 180));
                dy = (int)(InnerRadius * Math.Sin((singleAngle * (i) + singleAngle / 2+angle) * Math.PI / 180));

                pt1[i] = Common.MovePoint(CenterPoint, new Point(dx, dy));
            }


            List<Point> result=CreateNewShape(pt, pt1);
            return result;
        }

        protected List<Point> CreateNewShape(Point[] outerPt,Point[] innerPt)
        {
            Point[] pt = new Point[TotalPoints  * 2];

            int j = 0;
            for (j = 0; j < TotalPoints; j++)
            {
                pt[j * 2] = outerPt[j];
                pt[j * 2 + 1] = innerPt[j];
            }

            List<Point> ret = new List<Point>();
            ret.AddRange(pt);

            bounds = Common.GetBoundsFromPoints(pt);

            return ret;
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

        public override string ToString()
        {
            return "FiveStar";
        }

    }
}
