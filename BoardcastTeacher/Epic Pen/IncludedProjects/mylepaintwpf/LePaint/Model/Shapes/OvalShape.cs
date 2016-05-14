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
    public class OvalShape:BoundaryShape  
    {
        public OvalShape(Point pt)
            : base(pt)
        {
            ShowBorder = false;
        }

        private OvalShape()
        {
            //LeMenu.ShapeReloaded+=new //LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded); 
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            CreateNewShape();
        }


        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            ptOrigin = e.GetPosition(Window1.myCanvas);
            ptCurrent.X = -1;
            ShowBorder = false;
            LeFromColor = new LeColor(Color.FromArgb(255, 255, 255, 255));

            int size = 30* 3;
            Boundary = new Rect(Common.MovePoint(e.GetPosition(Window1.myCanvas), new Point(-size / 2, -size / 2)), new Size(size, size));
            base.ShapeResized += new ResizingShapeMoveHandler(OvalShape_ShapeResized);
            CreateNewShape();
        }

        void OvalShape_ShapeResized(object sender, Rect newRect, Rect oldRect)
        {
            Boundary = newRect;
        }

        private void CreateNewShape()
        {
            Rect rect = new Rect(MyLeRect.X, MyLeRect.Y, MyLeRect.Width, MyLeRect.Height);
            Boundary = rect;
        }

        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {
            ;
            return true;
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

            Point center = Common.MovePoint(ptOrigin, new Point(bounds.Width / 2, bounds.Height / 2));
            drawingContext.DrawEllipse(fillBrush, borderPen, center, bounds.Width / 2, bounds.Height / 2);

        }

        public override string ToString()
        {
            return "Oval";
        }

    }
}
