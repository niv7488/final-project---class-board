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
using LePaint.Shapes;

namespace LePaint.Controller
{
    public class ZoneShape : BoundaryShape
    {
        public TextShape TextField;

        public string Caption
        {
            set { TextField.Caption = value; }
            get { return TextField.Caption; }
        }
        public ZoneShape(Point pt) :base(pt)
        {
            TextField = new TextShape("Z", pt, this);
            TextField.ShowBorder = false;
        }

        public ZoneShape() : base() {
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
            TextField.MoveBorder(sender, dPoint);

            ;
        }

        void ResizeBorder(object sender, Rect newRect, Rect oldRect)
        {
            Boundary = newRect;
            Point dPoint = new Point(newRect.X - oldRect.X, newRect.Y - oldRect.Y);

            Point pt = Common.MovePoint(TextField.Boundary.Location, dPoint);
            Rect rect = new Rect(pt, TextField.Boundary.Size);
            TextField.Boundary = rect;
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TextField.Boundary.Contains(e.GetPosition(Window1.Self.myCanvas)))
            {
                TextField.MouseDown(sender, e);
            }
            else
            {
                base.MouseDown(sender, e);
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            TextField.MouseMove(sender, e);
            base.MouseMove(sender, e);
        }
        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            base.MouseUp(sender, e);
            TextField.MouseUp(sender, e);
        }

        public override string ToString()
        {
            return "Zone";
        }

        internal virtual void Draw(DrawingContext drawingContext)
        {
            base.Draw(drawingContext);

        }

    }
}
