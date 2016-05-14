using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Collections;

namespace LePaint
{
    /// <summary>
    /// Canvas used as host for DrawingVisual objects.
    /// Allows to draw graphics objects using mouse.
    /// </summary>
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
        }

        internal VisualCollection GraphicsList
        {
            get
            {
                return childrens;
            }
        }

        internal void RemoveShape(Visual ob)
        {
            childrens.Remove(ob); 
        }
    }
}
