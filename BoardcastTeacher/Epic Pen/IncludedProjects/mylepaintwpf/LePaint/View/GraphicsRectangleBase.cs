using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace LePaint
{
    /// <summary>
    /// Base class for Rect-based graphics:
    /// Rect and ellipse.
    /// </summary>
    public abstract class GraphicsRectBase : GraphicsBase
    {
        #region Class Members

        protected double RectLeft;
        protected double RectTop;
        protected double RectRight;
        protected double RectBottom;

        #endregion Class Members

        #region Properties

        /// <summary>
        /// Read-only property, returns Rect calculated on the fly from four points.
        /// Points can make inverted Rect, fix this.
        /// </summary>
        public Rect Rect
        {
            get
            {
                double l, t, w, h;

                if (RectLeft <= RectRight)
                {
                    l = RectLeft;
                    w = RectRight - RectLeft;
                }
                else
                {
                    l = RectRight;
                    w = RectLeft - RectRight;
                }

                if (RectTop <= RectBottom)
                {
                    t = RectTop;
                    h = RectBottom - RectTop;
                }
                else
                {
                    t = RectBottom;
                    h = RectTop - RectBottom;
                }

                return new Rect(l, t, w, h);
            }
        }

        public double Left
        {
            get { return RectLeft; }
            set { RectLeft = value; }
        }

        public double Top
        {
            get { return RectTop; }
            set { RectTop = value; }
        }

        public double Right
        {
            get { return RectRight; }
            set { RectRight = value; }
        }

        public double Bottom
        {
            get { return RectBottom; }
            set { RectBottom = value; }
        }

        #endregion Properties

        #region Overrides

        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        public override Point GetHandle(int handleNumber)
        {
            double x, y, xCenter, yCenter;

            xCenter = (RectRight + RectLeft) / 2;
            yCenter = (RectBottom + RectTop) / 2;
            x = RectLeft;
            y = RectTop;

            switch (handleNumber)
            {
                case 1:
                    x = RectLeft;
                    y = RectTop;
                    break;
                case 2:
                    x = xCenter;
                    y = RectTop;
                    break;
                case 3:
                    x = RectRight;
                    y = RectTop;
                    break;
                case 4:
                    x = RectRight;
                    y = yCenter;
                    break;
                case 5:
                    x = RectRight;
                    y = RectBottom;
                    break;
                case 6:
                    x = xCenter;
                    y = RectBottom;
                    break;
                case 7:
                    x = RectLeft;
                    y = RectBottom;
                    break;
                case 8:
                    x = RectLeft;
                    y = yCenter;
                    break;
            }

            return new Point(x, y);
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        public override int MakeHitTest(Point point)
        {
            if (IsSelected)
            {
                for (int i = 1; i <= HandleCount; i++)
                {
                    if (GetHandleRect(i).Contains(point))
                        return i;
                }
            }

            if (Contains(point))
                return 0;

            return -1;
        }



        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Cursors.Arrow;
            }
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            switch (handleNumber)
            {
                case 1:
                    RectLeft = point.X;
                    RectTop = point.Y;
                    break;
                case 2:
                    RectTop = point.Y;
                    break;
                case 3:
                    RectRight = point.X;
                    RectTop = point.Y;
                    break;
                case 4:
                    RectRight = point.X;
                    break;
                case 5:
                    RectRight = point.X;
                    RectBottom = point.Y;
                    break;
                case 6:
                    RectBottom = point.Y;
                    break;
                case 7:
                    RectLeft = point.X;
                    RectBottom = point.Y;
                    break;
                case 8:
                    RectLeft = point.X;
                    break;
            }

            RefreshDrawing();
        }

        /// <summary>
        /// Test whether object intersects with Rect
        /// </summary>
        public override bool IntersectsWith(Rect Rect)
        {
            return Rect.IntersectsWith(Rect);
        }

        /// <summary>
        /// Move object
        /// </summary>
        public override void Move(double deltaX, double deltaY)
        {
            RectLeft += deltaX;
            RectRight += deltaX;

            RectTop += deltaY;
            RectBottom += deltaY;

            RefreshDrawing();
        }

        /// <summary>
        /// Normalize Rect
        /// </summary>
        public override void Normalize()
        {
            if (RectLeft > RectRight)
            {
                double tmp = RectLeft;
                RectLeft = RectRight;
                RectRight = tmp;
            }

            if (RectTop > RectBottom)
            {
                double tmp = RectTop;
                RectTop = RectBottom;
                RectBottom = tmp;
            }
        }


        #endregion Overrides
    }
}
