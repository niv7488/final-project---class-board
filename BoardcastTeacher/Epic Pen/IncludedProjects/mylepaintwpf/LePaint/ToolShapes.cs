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

using System.Reflection;

using LePaint.Shapes;
using LePaint.Basic;

namespace LePaint
{
    public class ToolShapes
    {
        public static Dictionary<Type, string> shapeMenus = new Dictionary<Type, string>(){
            {typeof(ArrowShape),"Arrow"},
            {typeof(RoundRectShape),"Round Rect x"},
            {typeof(TextShape),"Text"},
            {typeof(LeRectangle),"Rect"},
            {typeof(Rhombus),"Rhombus"},
            {typeof(Triangle),"Triangle"},
            {typeof(Square),"Square"},
            {typeof(Pentagon),"Pentagon"},
            {typeof(Hexagon),"Hexagon"},
            {typeof(Heptagon),"Heptagon"},
            {typeof(Octagon),"Octagon"},
            {typeof(Nonagon),"Nonagon"},
            {typeof(Decagon),"Decagon"},
            {typeof(OvalShape),"Oval"},
            {typeof(FiveStar),"Five Star"},
            {typeof(PolyStar),"Multi Star"},
            {typeof(Parallelogram),"Parallelogram"},
        };

        public ToolShapes()
        {
            Rect rect = new Rect();
            int i = 1;
            rect.Width = 30;
            rect.Height = 30;

            rect.X += rect.Width + 5;
            foreach (Type type in shapeMenus.Keys)
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(Point) });
                LeShape shape = constructor.Invoke(new object[] { rect.Location }) as LeShape;
                shape.Boundary = rect;
                //curTools.Add(shape);

                rect.X += rect.Width + 5;
                i++;
                if (i > 1)
                {
                    i = 0;
                    rect.X = 10;
                    rect.Y += rect.Height + 5;
                }
            }
        }
    }
}
