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

using LePaint.Basic;
using LePaint.MainPart;

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
            this.parent = parent;
            Initialize(caption, pt);
            InitBoundary();
        }

        private TextShape() {
        }

        public TextShape(Point pt)
            :base(pt)
        {
            TextSize = 30;
            ShowBorder = false;
            TextColor = (Brushes.CadetBlue).Color;
        }

        private void Initialize(string caption, Point pt)
        {
            Caption = caption;
            TextSize = 15;
            LeTextColor = new LeColor(Color.FromArgb(255, 255, 255, 255));
            LeTextFont = new LeFont(new FontFamily("Tahoma"));
            Boundary = new Rect(pt, new Size(0, 0));
        }

        private void InitBoundary()
        {
            FontFamily font = TextFont;
            //SizeF size = LeCanvas.self.Canvas.CreateGraphics().MeasureString(Caption, font);
            //Rect rect = new Rect(Boundary.X - 10, Boundary.Y + 10, (int)size.Width + 5, (int)size.Height + 5);

            //Boundary = rect;
        }

        private void CalculateNewBoundary()
        {
            //SizeF size = LeCanvas.self.Canvas.CreateGraphics().MeasureString(Caption, TextFont);
            //Rect rect = new Rect(Boundary.X, Boundary.Y, (int)size.Width+5 , (int)size.Height+5 );
            //Boundary = rect;
        }

        public override void DrawMouseDown(MouseButtonEventArgs e)
        {
            ptOrigin = e.GetPosition(Window1.myCanvas);
            ptCurrent.X = -1;
            Initialize("test",e.GetPosition(Window1.myCanvas));
            InitBoundary();
            ShowBorder = true;
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
        }

        public override bool DrawMouseUp(MouseButtonEventArgs e)
        {
            ;
            return true;
        }


        public override void Draw()
        {
            CalculateNewBoundary();
            if (ShowBorder == true)
            {
                base.Draw();
            }
            DrawMySelf();
        }

        internal override void Draw(DrawingContext drawingContext)
        {
            DrawText(drawingContext);

            FormattedText text = new FormattedText(Name,
            CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
            new Typeface("Verdana"), TextSize, new SolidColorBrush(TextColor) ) ;

            drawingContext.DrawText(text, bounds.Location);

            if (ShowBorder)
            {
                DrawBorder(drawingContext);
            }
            //Point center = Common.MovePoint(ptOrigin, new Point(bounds.Width / 2, bounds.Height / 2));
            //drawingContext.DrawText(.DrawEllipse(fillBrush, borderPen, center, bounds.Width / 2, bounds.Height / 2);

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

        private void DrawMySelf()
        {
            if (Caption.Length > 0)
            {
                //g.DrawString(Caption, LeTextFont.ToFont()
                //    , new SolidBrush(LeTextColor.ToColor()), Boundary.Location.X + 3, Boundary.Location.Y + 3);
            }
        }

        public override string ToString()
        {
            return "Text";
        }
    }
}
