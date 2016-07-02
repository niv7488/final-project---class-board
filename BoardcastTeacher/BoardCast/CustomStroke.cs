using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;

namespace BoardCast
{
    /// <summary>
    /// Class that saves object with 2 variables : StrokeCollection and Collection<UIElement> - Shapes
    /// </summary>
    class CustomStroke
    {
        public StrokeCollection m_strokeCollection { get; set; }
        public Collection<UIElement> m_childElement { get; set; }

        public CustomStroke(StrokeCollection stroke, Collection<UIElement> childs)
        {
            m_strokeCollection = stroke;
            m_childElement = childs;
        }
    }
    
}
