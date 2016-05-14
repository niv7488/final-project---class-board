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
using System.Collections ;

namespace LePaint.Main
{
    public class CustomRender : FrameworkElement
    {
        ArrayList drawingList = new ArrayList();
        VisualCollection childrens;

        public CustomRender()
        {
            childrens = new VisualCollection(this); 
        }
        // Provide a required override for the VisualChildrenCount property.
        protected override int VisualChildrenCount
        {
            get { return childrens.Count; }
        }

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= childrens.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return childrens[index];
        }


        internal void AddPath(PathGeometry pathGeometry)
        {
            drawingList.Add(pathGeometry);
            this.InvalidateVisual();
        }

        internal void AddObject(Visual ob)
        {
            childrens.Add(ob); 
        }

        internal void AddPoint(Point pt)
        {
            throw new NotImplementedException();
        }

        internal VisualCollection GraphicsList
        {
            get
            {
                return childrens;
            }
        }

    }

}
