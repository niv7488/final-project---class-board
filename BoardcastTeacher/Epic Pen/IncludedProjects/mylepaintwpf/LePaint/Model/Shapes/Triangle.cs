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
    public class Triangle : LePolyGon
    {
        public Triangle(Point pt)
            : base(pt)
        {
            InitShape( 3);
        }
        private Triangle():base() { }

        public override string ToString()
        {
            return "Triangle";
        }
    }
}
