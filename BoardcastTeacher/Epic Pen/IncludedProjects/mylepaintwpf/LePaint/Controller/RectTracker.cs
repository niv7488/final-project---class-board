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


namespace LePaint.Controller
{
    internal class RectTracker
    {
        #region Fields
        private int offset = 2;
        #endregion

        #region Constructor
        public RectTracker()
        {
        }
        #endregion

        #region Public Methods

        public void DrawSelectionTrackers(Rect rc)
        {
            //InitTrackerRects(rc);
            //this.BaseCanvas.Canvas = BaseCanvas.Canvas;

            //foreach (Rect rect in hotSpots)
            {
              //  ControlPaint.DrawReversibleFrame(rect, Color.Black, FrameStyle.Thick);
            }
        }

        #endregion

        public static Point[] GetPointsFromRect(Rect rect)
        {
            Point[] hots = new Point[8];
            hots[0] = rect.Location;
            hots[1] = Common.MovePoint(hots[0], new Point(rect.Width / 2, 0));
            hots[2] = Common.MovePoint(hots[0], new Point(rect.Width, 0));
            hots[3] = Common.MovePoint(hots[0], new Point(rect.Width, rect.Height / 2));
            hots[4] = Common.MovePoint(hots[0], new Point(rect.Width, rect.Height));
            hots[5] = Common.MovePoint(hots[0], new Point(rect.Width / 2, rect.Height));
            hots[6] = Common.MovePoint(hots[0], new Point(0, rect.Height));
            hots[7] = Common.MovePoint(hots[0], new Point(0, rect.Height / 2));

            return hots;
        }
    }
}

