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
using System.Globalization;

using System.Xml.Serialization;

using LePaint.Controller;

namespace LePaint.Shapes
{
    public class TextShape : BoundaryShape
    {
        #region properties
        string caption = string.Empty;
        public string Caption
        {
            set { caption = value;
            ;
            }
            get { return caption; }
        }

        private LeFont textFont;
        public LeFont LeTextFont
        {
            set { textFont = value;
            }
            get { return textFont; }
        }

        [XmlIgnore]
        public FontFamily TextFont
        {
            get { return textFont.ToFont(); }
            set
            {
                textFont = new LeFont(value);
                ;
            }
        }
        private LeColor textColor;
        public LeColor LeTextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        [XmlIgnore]
        public Color TextColor
        {
            get { return textColor.ToColor(); }
            set
            {
                textColor = new LeColor(value);
                ;
            }
        }

        public Point Location
        {
            get { return ptOrigin; }
            set { ptOrigin = value; }
        }

        public int TextSize
        {
            get { return (int)textFont.Size; }
            set { textFont.Size = value;
            ;
            }
        }
        #endregion

        private LeShape parent;
        public TextShape(string caption, Point pt, LeShape parent)
            : base(pt)
        {
            caption = "TEXT";
            this.parent = parent;
        }

        private TextShape() {
        }

        public TextShape(Point pt)
            :base(pt)
        {
            caption = "TEXT";
            TextSize = 30;
            ShowBorder = false;
            TextColor = (Brushes.CadetBlue).Color;
        }

        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            ptOrigin = e.GetPosition(Window1.Self.myCanvas);
            ptCurrent.X = -1;
            caption = "test";
            RefreshDrawing();
        }

        public override void MoveShape(Point ptPrevious, Point ptCurrent)
        {
            base.MoveShape(ptPrevious, ptCurrent);
            Point pt = new Point(ptCurrent.X - ptPrevious.X, ptCurrent.Y - ptPrevious.Y);
            ptOrigin = Common.MovePoint(ptOrigin, pt); 
        }


        internal override void Draw(DrawingContext drawingContext)
        {
            FormattedText text = new FormattedText(caption,
            CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
            new Typeface("Verdana"), TextSize, new SolidColorBrush(TextColor) ) ;

            bounds =new Rect(Location,new Size(text.Width, text.Height ));
            if (ShowBorder)
            {
                DrawBorder(drawingContext);
            }
            drawingContext.DrawText(text, bounds.Location);
        }

        internal void DrawBorder(DrawingContext drawingContext)
        {

            GradientStopCollection gradient = new GradientStopCollection(2);
            gradient.Add(new GradientStop(FromColor, 1.0));
            gradient.Add(new GradientStop(ToColor, 0.0));

            // Create the LinearGradientBrushes

            LinearGradientBrush fillBrush = new LinearGradientBrush(gradient, new Point(0.0, 0.0), new Point(1, 0.0));

            Pen borderPen = new Pen(new SolidColorBrush(BorderColor), BorderWidth);

            if (ShowBorder == false) borderPen = null;
            if (Fill == false) fillBrush = null;

            drawingContext.DrawRoundedRectangle(fillBrush, borderPen, bounds, 3, 3);
        }

        public override string ToString()
        {
            return "Text";
        }
    }
}
