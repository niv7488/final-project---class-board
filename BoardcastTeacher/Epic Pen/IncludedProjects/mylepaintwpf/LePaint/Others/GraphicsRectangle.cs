using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LePaint
{
    /// <summary>
    ///  Rect graphics object.
    /// </summary>
    public class GraphicsRect : GraphicsRectBase
    {
        #region Constructors

        public GraphicsRect(double left, double top, double right, double bottom,
            double lineWidth, Color objectColor, double actualScale)
        {
            this.RectLeft = left;
            this.RectTop = top;
            this.RectRight = right;
            this.RectBottom = bottom;
            this.graphicsLineWidth = lineWidth;
            this.graphicsObjectColor = objectColor;
            this.graphicsActualScale = actualScale;

        }

        public GraphicsRect()
            :
            this(0.0, 0.0, 100.0, 100.0, 1.0, Colors.Black, 1.0)
        {
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Draw object
        /// </summary>
        public override void Draw(DrawingContext drawingContext)
        {
            if (drawingContext == null)
            {
                throw new ArgumentNullException("drawingContext");
            }

            //drawingContext.DrawRectangle(
            //    null,
            //    new Pen(new SolidColorBrush(ObjectColor), ActualLineWidth),
            //    Rect);

            drawingContext.DrawRectangle(
                new SolidColorBrush(Color.FromArgb(10,222,222,222)),
                new Pen(Brushes.Orange, ActualLineWidth),
                Rect);

            base.Draw(drawingContext);
        }

        /// <summary>
        /// Test whether object contains point
        /// </summary>
        public override bool Contains(Point point)
        {
            return this.Rect.Contains(point);
        }

        #endregion Overrides

    }
}
