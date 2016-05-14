using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.IO;


using LePaint.MainPart;
using LePaint.Basic;
using LePaint.Shapes;

namespace LePaint.Basic
{
    public enum LeShapeType
    {
        NotKnown,
        Arrow,
        PolyGon,
        Rect,
        Text
    }

    [XmlInclude(typeof(ZoneShape)),
    XmlInclude(typeof(ArrowShape)),
    XmlInclude(typeof(TextShape)),
    XmlInclude(typeof(BoundaryShape)),
    XmlInclude(typeof(OvalShape)),
    XmlInclude(typeof(LeShape)),
    XmlInclude(typeof(Cube)),
    XmlInclude(typeof(Cylinder)),
    XmlInclude(typeof(Decagon)),
    XmlInclude(typeof(Heptagon)),
    XmlInclude(typeof(Hexagon)),
    XmlInclude(typeof(Nonagon)),
    XmlInclude(typeof(Octagon)),
    XmlInclude(typeof(Pentagon)),
    XmlInclude(typeof(Rhombus)),
    XmlInclude(typeof(Square)),
    XmlInclude(typeof(LeRect)),
    XmlInclude(typeof(RoundRectShape)),
    XmlInclude(typeof(FiveStar)),
    XmlInclude(typeof(PolyStar)),
    XmlInclude(typeof(Triangle)),
    XmlInclude(typeof(Parallelogram))
    ]
    [XmlRoot("ShapeList")]
    public class XMLShapes
    {
        public static int Total = 0;

        public List<LeShape> ShapeList;

        public XMLShapes()
        {
            ShapeList = new List<LeShape>();
        }

        public List<LeShape> GetList(Type t)
        {
            List<LeShape> ret = new List<LeShape>();
            if (t != null)
            {
                foreach (LeShape shape in ShapeList)
                {
                    if (shape.GetType().Equals(t))
                    {
                        ret.Add(shape);
                    }
                }
            }
            return ret;
        }

        public List<LeShape> GetList()
        {
            return ShapeList;
        }

        internal void Remove(LeShape leShape)
        {
            ShapeList.Remove(leShape);
        }

        internal void Add(LeShape leShape)
        {
            ShapeList.Add(leShape);
            XMLShapes.Total++;
        }
        internal void AddFriend(LeShape leShape)
        {

        }
        private void Clear()
        {
            ShapeList = new List<LeShape>();
        }


        internal List<LeShape> GetSelectedShapes()
        {
            throw new NotImplementedException();
        }
    }
}
