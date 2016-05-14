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
using System.Collections;
using System.Globalization;

using System.Xml.Serialization;

using LePaint.Main;
using LePaint;

namespace LePaint.Basic
{
    public enum ShapeStyle
    {
        Sqaure,
        Round,
        Circle,
        DashedRound
    }

    public struct LeColor
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;
        public LeColor(Color color)
        {
            this.A = color.A;
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public static LeColor FromColor(Color color)
        {
            return new LeColor(color);
        }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }

    public struct LeRect{
        public double X,Y,Width,Height;
        public LeRect(Rect rect){
            X =rect.X;
            Y=rect.Y;
            Width =rect.Width ;
            Height=rect.Height ;
        }
    }

    public struct LeFont
    {
        public float Size;
        public string Name;
        public FontStyle Style;

        public LeFont(FontFamily font)
        {
            Name = font.ToString();
            Style = new FontStyle();
            Size = 9;
        }
        public static LeFont FromFont(FontFamily font)
        {
            return new LeFont(font);
        }
        public FontFamily ToFont()
        {
            if (Name != null && Size > 1)
            {
                return new FontFamily(Name);
            }
            else return new FontFamily("Times New Roman");
        }
    }

    public abstract class LeShape :  IShape
    {
        #region xml serializable variables

        private bool showBorder = true;
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
            }
        }
        private LeColor borderColor = new LeColor(Color.FromArgb(255,255,255,255));
        public LeColor LeBorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
            }
        }

        [XmlIgnore]
        public Color BorderColor
        {
            get { return LeBorderColor.ToColor(); }
            set { LeBorderColor = new LeColor(value); }

        }
        private int borderWidth = 1;
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
            }
        }

        protected Rect bounds;
        [XmlIgnore]
        public Rect Boundary
        {
            set { bounds = value; 
            MyLeRect =new LeRect(value); 
            }
            get { return bounds; }
        }

        public LeRect MyLeRect
        {
            set
            {
                bounds = new Rect(new Point(value.X,value.Y),new Size(value.Width,value.Height)  );
            }
            get { return new LeRect(bounds); }
        }

        private LeColor fromColor = new LeColor(Color.FromArgb(255, 255, 255, 255));
        public LeColor LeFromColor
        {
            get { return fromColor; }
            set { fromColor = value; }
        }
        [XmlIgnore]
        public Color FromColor
        {
            get { return LeFromColor.ToColor(); }
            set
            {
                LeFromColor = new LeColor(value);
            }
        }
        private LeColor toColor = new LeColor(Color.FromArgb(255, 255, 255, 255));
        public LeColor LeToColor
        {
            get { return toColor; }
            set { toColor = value; }
        }
        [XmlIgnore]
        public Color ToColor
        {
            get { return LeToColor.ToColor(); }
            set
            {
                LeToColor = new LeColor(value);
                ;
            }
        }

        private int angle=225;
        public int LightAngle
        {
            set { angle = value; }
            get { return angle; }
        }
        #endregion

        #region System Running variables
        private bool selected = false;
        [XmlIgnore]
        public bool Selected
        {
            set { selected = value; }
            get { return selected; }
        }

        public string Name
        {
            get { return this.ToString(); }
        }

        public Shape Element
        {
            get {
                Rectangle rect = new Rectangle();

                return rect;
            }
        }

        protected ArrayList objectsInPath;
        protected Point ptOrigin;
        protected Point ptCurrent;
        protected Point ptPrevious;

        protected Rect AreaRect;

        //protected bool shapeResizing;
        //protected BoundaryShape boundaryShape;

        protected Point centerPoint;
        protected bool isDrawingOK = false;

        protected Path path;
        #endregion

        static LeShape()
        {
        }

        public LeShape()
        {
            path = new Path();
            objectsInPath = new ArrayList();
        }
        public LeShape(Point pt)
        {
            path = new Path();
            objectsInPath = new ArrayList();
            AreaRect = new Rect();

            Color c = new Color();
            c = (Brushes.AliceBlue).Color;
            FromColor = c;

            c = (Brushes.Brown).Color;
            ToColor = c;

            bounds.X = 90;
            bounds.Y = 15;
            bounds.Width = 60;
            bounds.Height = 30;

            ptOrigin = bounds.Location ;
            ptCurrent = bounds.Location ;
            ptPrevious = bounds.Location ;

        }

        public virtual bool DrawMouseUp(MouseButtonEventArgs e) { return false; }
        public virtual void DrawMouseDown(MouseButtonEventArgs e)
        {
        }
        public virtual void DrawMouseMove(MouseEventArgs e)
        {
        }

        protected virtual void DrawMouseHoverShape()
        {
            if (path != null)
            {
                //Graphics g = LeCanvas.self.Canvas.CreateGraphics();
                //g.DrawPath(new Pen(Color.FromArgb(255, 255, 255, 255)), path);
            }
        }

        public virtual void MouseDown(object sender, MouseButtonEventArgs e) { }
        public virtual void MouseMove(object sender, MouseEventArgs e) { }
        public virtual void MouseUp(object sender, MouseButtonEventArgs e) { }
        public virtual void Draw() { }

        internal void DrawSelected()
        {
            //RectTracker.DrawPoints(bounds);
        }

        public virtual bool UpdateSelected(Point point, ref LeShape shape0)
        {
            return selected;
        }

        internal virtual void Draw(DrawingContext drawingContext)
        {
            DrawText(drawingContext);
        }

        internal void DrawText(DrawingContext drawingContext)
        {
            FormattedText text = new FormattedText(Name,
            CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
            new Typeface("Verdana"), 12, Brushes.Black);

            drawingContext.DrawText(text, new Point(0, 0));
        }
    }
}
