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
    public class RoundRectShape:BoundaryShape 
    {
        private int radius = 3;
        public int Radius
        {
            set
            {
                radius = value;
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

            drawingContext.DrawRoundedRectangle(fillBrush, borderPen, bounds, radius, radius);
        }


        public override string ToString()
        {
            return "RoundRectShape";
        }
    }
}
