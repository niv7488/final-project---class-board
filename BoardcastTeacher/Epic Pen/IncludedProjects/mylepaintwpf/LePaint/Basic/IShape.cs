using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace LePaint.Basic
{
    public interface IShape
    {
        void MouseDown(object sender, MouseButtonEventArgs e);
        void MouseMove(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseButtonEventArgs e);
        bool DrawMouseUp(MouseButtonEventArgs e);
        void DrawMouseDown(MouseButtonEventArgs e);
        void DrawMouseMove(MouseEventArgs e);
    }
}
