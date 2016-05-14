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

namespace LePaint.Controller
{
    public class GroupShapes : BoundaryShape
    {
        public int Count
        {
            get { return selectedShapes.Count; }
        }

        List<LeShape> selectedShapes = new List<LeShape>();
        internal void SetSelectedShapes(Rect AreaRect)
        {
            selectedShapes = new List<LeShape>();
            foreach (LeShape shape in DrawingController.self.xmlShapes.GetList())
            {
                if (AreaRect.Contains(shape.Boundary.Location))
                {
                    shape.Selected = true;
                    selectedShapes.Add(shape);
                }
                else
                {
                    shape.Selected = false;
                }
            }
            Boundary = AreaRect;
        }

        internal void Move()
        {
            /*
            double dx = AreaRect.X - Boundary.X;
            double dy = AreaRect.Y - Boundary.Y;

            foreach (BoundaryShape shape in selectedShapes)
            {
                shape.OnShapeMoved(new Point(dx, dy));
            }
             */
        }
    }
}