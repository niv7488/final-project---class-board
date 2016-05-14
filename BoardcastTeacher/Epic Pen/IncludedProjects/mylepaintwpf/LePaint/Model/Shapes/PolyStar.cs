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

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class PolyStar:FiveStar 
    {
        public PolyStar(Point pt)
            : base(pt)
        {
            InitShape(10);
        }

        private PolyStar()
            : base()
        {

        }
        public override string ToString()
        {
            return "PolyStar";
        }

    }
}
