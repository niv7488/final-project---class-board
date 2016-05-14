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

using System.Xml.Serialization;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class LeRectangle : BoundaryShape
    {
        public LeRectangle()
            : base()
        {
            //LeMenu.ShapeReloaded += new //LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            RegisterEvents();
        }
        public LeRectangle(Point pt)
            : base(pt)
        {
            ShowBorder = true;
        }

        public bool ShapeSizeOK(Point ptOrigin, Point ptCurrent)
        {
            Rect areaRect = Common.GetRect(ptOrigin, ptCurrent);

            if (areaRect.Width > 20 && areaRect.Height > 10)
            {
                return true;
            }
            else return false;
        }

        void OnMoveBorder(object sender, Point dPoint)
        {
            ;
        }

        void ResizeBorder(object sender, Rect newRect, Rect oldRect)
        {
            Boundary = newRect;
        }

        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > 30&& AreaRect.Height > 30) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                RegisterEvents();
            }
            else path = null;

            ;

            return check;
        }

        private void RegisterEvents()
        {
            base.ShapeMoved += new BoundaryShape.ShapeMoveHandler(OnMoveBorder);
            base.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(ResizeBorder);
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

            drawingContext.DrawRectangle(fillBrush, borderPen, bounds);
        }

        public override string ToString()
        {
            return "Rectangle";
        }
    }
}
