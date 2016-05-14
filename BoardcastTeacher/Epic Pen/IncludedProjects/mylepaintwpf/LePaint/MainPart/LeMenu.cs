using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

using LePaint.Basic;
using LePaint.Shapes;
using LePaint.Properties;

namespace LePaint.MainPart
{
    class LeMenu
    {
        public const int Size = 20;

        public static Dictionary<Type, string> shapeMenus = new Dictionary<Type, string>(){
            {typeof(ArrowShape),"Arrow"},
            {typeof(RoundRectShape),"Round Rectangle x"},
            {typeof(TextShape),"Text"},
            {typeof(ZoneShape),"Zone"},
            {typeof(LeRectangle),"Rectangle"},
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

        private LeShape curShape;
        public LeShape CurShape{
            set { curShape = value; }
            get { return curShape; }
        }

        private Type curType;
        public Type CurType
        {
            set { curType = value; }
            get { return curType; }
        }

        private bool drawingShape = false;
        public bool DrawShape{
            set { drawingShape = value; }
            get { return drawingShape; }
        }

        public static LeMenu self=null;
        
        public LeMenu()
        {
            self = this;
        }

        public void ShowEditShapesMenus(MouseEventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;

            item = new ToolStripMenuItem();
            item.Text = "Delete";
            item.Font = new Font("Tahoma", 9, FontStyle.Regular);
            item.Click += delegate(object sender, EventArgs ex)
            {
                LeCanvas.self.DeleteSelectedShapes();
            };
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Save as a component file";
            item.Font = new Font("Tahoma", 9, FontStyle.Regular);
            item.Click +=new EventHandler(SaveSelected);
            menu.Items.Add(item);

            menu.Show(LeCanvas.self.Canvas,e.Location); 
        }

        void SaveSelected(object sender, EventArgs e)
        {

        }

        public void ShowDrawingShapeMenus(MouseEventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;

            item = new ToolStripMenuItem();
            item.Text = "Load Component";
            item.Image = Resources.ARROW;
            item.Font = new Font("Tahoma", 9, FontStyle.Regular);
            item.Click += delegate(object sender, EventArgs ex)
            {
            };
            menu.Items.Add(item);

            ToolStripMenuItem item0 = new ToolStripMenuItem("Draw Shapes");
            item0.Font = new Font("Tahoma", 9, FontStyle.Regular);
            item0.Image = Resources.GROUP;
            menu.Items.Add(item);

            foreach (Type type in shapeMenus.Keys)
            {
                if (type.Equals(typeof(Rhombus)))
                {
                    ToolStripSeparator sep = new ToolStripSeparator();
                    item0.DropDownItems.Add(sep);
                }
                item = new ToolStripMenuItem();
                item.Text = shapeMenus[type];
                item.Font = new Font("Tahoma", 9, FontStyle.Regular); 
                item.Click += Menu_Click;
                item0.DropDownItems.Add(item);
            }
            menu.Items.Add(item0);

            ToolStripMenuItem item1 = new ToolStripMenuItem("Print to a PDF");
            item1.Font = new Font("Tahoma", 9, FontStyle.Regular);
            item1.Image = Resources.NOTEBOOK;
            menu.Items.Add(item1);
            item.Click += PrintPDF_Click;

            menu.Show(LeCanvas.self.Canvas, e.Location);
        }

        private void PrintPDF_Click(object sender, EventArgs ex)
        {

        }

        private void Menu_Click(object sender, EventArgs ex)
        {
            DrawShape = true;
            foreach (Type type in shapeMenus.Keys)
            {
                if (shapeMenus[type].Equals(sender.ToString()))
                {
                    curType = type;
                    break;
                }
            }
        }

        public static void ExportToXML(string fileName)
        {
            TextWriter w = new StreamWriter(fileName);
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(XMLShapes));
                s.Serialize(w, LeCanvas.self.xmlShapes);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Serialize XML " + e.Message);
                System.Console.WriteLine("Serialize XML " + e.InnerException.Message);
            }
            finally
            {
                w.Close();
            }
        }

        public static void DeSerializeXML(string fileName)
        {
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                XmlTextReader xr = new XmlTextReader(sr);
                XmlSerializer xs = new XmlSerializer(typeof(XMLShapes));
                if (xs.CanDeserialize(xr))
                {
                    try
                    {
                        XMLShapes ret = (XMLShapes)xs.Deserialize(xr);
                        LeCanvas.self.xmlShapes = ret;
                        OnShapeReloaded();
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Open file " + e.InnerException.Message);
                    }
                }
                xr.Close();
                sr.Close();
            }
        }


        internal void CreateNewShape(MouseEventArgs e)
        {
            if (curType != null)
            {
                ConstructorInfo constructor = curType.GetConstructor(new Type[] { typeof(Point) });
                CurShape = constructor.Invoke(new object[] { e.Location }) as LeShape;
            }
        }

        public delegate void ShapeReloadedHandler(object sender);
        public static event ShapeReloadedHandler ShapeReloaded;

        internal static void OnShapeReloaded()
        {
            if (ShapeReloaded != null)
            {
                ShapeReloaded(typeof(XMLShapes));
            }
        }

        internal void ShowSelectedProperties(Object ob)
        {
            FrmProperty frm = new FrmProperty(ob);
            frm.Show();
        }
    }
}
