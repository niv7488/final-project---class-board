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

namespace LePaint.Basic
{
    internal struct Common
    {
        public const float PI = 3.14159265358979F;
        public const int MaxUndoNum = 50;
        public const int No_Click = 0x80;
        public const int ERASE_OP = 0x8000;
        public const int INIT_OP = 0x8000;

        public static Rect MoveRect(Point cur, Point org, Rect rc)
        {
            int dirX, dirY;
            if (cur.X < org.X) //move to left?
                dirX = -1;
            else dirX = 1;
            if (cur.Y < org.Y) //move to top?
                dirY = -1;
            else dirY = 1;

            rc.X = rc.X + dirX * Math.Abs(cur.X - org.X);
            rc.Y = rc.Y + dirY * Math.Abs(cur.Y - org.Y);
            return rc;
        }

        internal static Point MovePoint(Point rc, Point dPoint)
        {
            int dirX, dirY;
            if (dPoint.X < 0) //move to left?
                dirX = -1;
            else dirX = 1;
            if (dPoint.Y < 0) //move to top?
                dirY = -1;
            else dirY = 1;

            rc.X = rc.X + dirX * Math.Abs(dPoint.X);
            rc.Y = rc.Y + dirY * Math.Abs(dPoint.Y);

            return rc;
        }

        public static Rect GetRect(Point cur, Point org)
        {
            Rect rc0 = new Rect();
            rc0.Width = Math.Abs(cur.X - org.X);
            rc0.Height = Math.Abs(cur.Y - org.Y);

            if (cur.X < org.X) rc0.X = cur.X;
            else rc0.X = org.X;

            if (cur.Y < org.Y) rc0.Y = cur.Y;
            else rc0.Y = org.Y;
            return rc0;
        }


        public static bool DotInRect(int x, int y, Rect rc)
        {
            if (x > rc.Left && x < rc.Left + rc.Width &&
                y > rc.Top && y < rc.Top + rc.Height)
                return true;
            return false;
        }
        public static bool DotInBox(int X, int Y, int X0, int Y0, int X1, int Y1)
        {
            int Temp;
            if (X0 > X1)
            {
                Temp = X0;
                X0 = X1;
                X1 = Temp;
            }
            if (Y0 > Y1)
            {
                Temp = Y0;
                Y0 = Y1;
                Y1 = Temp;
            }

            if (X >= X0 && X <= X1 && Y >= Y0 && Y <= Y1)
            {
                return true;
            }
            return false;
        }

        public static void SwapXY(ref int X, ref int Y)
        {
            int Temp;
            Temp = X;
            X = Y;
            Y = Temp;
        }

        public static bool DotInLineH(int X, int Y, int X0, int Y0, int X1)
        {
            if (X0 > X1) SwapXY(ref X0, ref X1);
            if (X < X0 || X > X1) return false;
            if (Y < Y0 - 4 || Y > Y0 + 4) return false;
            return true;

        }

        public static bool DotInLineV(int X, int Y, int X0, int Y0, int Y1)
        {
            if (Y0 > Y1) SwapXY(ref Y0, ref Y1);
            if (Y < Y0 || Y > Y1) return false;
            if (X < X0 - 4 || X > X0 + 4) return false;
            return true;
        }


        internal static Rect GetHotSpot(Point p)
        {
            Rect ret = new Rect();

            ret.X = p.X - 3;
            ret.Y = p.Y - 3;
            ret.Width = 6;
            ret.Height =6;
            return ret;
        }

        internal static Rect GetRealRect(Control canvas, Rect AreaRect)
        {
            //Rect newRect = canvas.RectToScreen(AreaRect);
            return  new Rect();
        }

        public static bool CheckForBoundary(Control canvas, ref Rect AreaRect, Rect old)
        {
            double x0, y0;
            double x1, y1;
            bool check = true;

            x0 = AreaRect.X;
            y0 = AreaRect.Y;

            x1 = AreaRect.X + AreaRect.Width;
            y1 = AreaRect.Y + AreaRect.Height;

            /*
            Rect toTest = GDIApi.GetViewableRect(canvas);

            if (x0 < toTest.X + 5)
            {
                AreaRect.X = toTest.X + 5;
                check = false;
            }
            if (y0 < toTest.Y + 5)
            {
                AreaRect.Y = toTest.Y + 5;
                check = false;
            }
            if (x1 > toTest.Width + toTest.X - 5)
            {
                AreaRect = old;
                check = false;
            }
            if (y1 > toTest.Height + toTest.Y - 5)
            {
                AreaRect = old;
                check = false;
            }
             */
            return check;
        }


        internal static System.Collections.ArrayList GetPointsFromRect(Rect rect)
        {
            System.Collections.ArrayList points = new System.Collections.ArrayList();

            points.Add(new Point(rect.X, rect.Y));
            points.Add(new Point(rect.X + rect.Width, rect.Y));
            points.Add(new Point(rect.X + rect.Width, rect.Y + rect.Height));
            points.Add(new Point(rect.X, rect.Y + rect.Height));

            return points;
        }

        internal static void MakeSquare(ref Rect rect)
        {
            if (rect.Width > rect.Height)
            {
                rect.Y = rect.Y - (rect.Width - rect.Height);
                rect.Height = rect.Width;
            }
            else
            {
                rect.X = rect.X - (rect.Height - rect.Width);
                rect.Width = rect.Height;
            }
            Common.CheckForBoundary(LeCanvas.self.Canvas, ref rect, new Rect());
        }

        internal static int GetLength(Point ptEnd, Point ptStart)
        {
            double dx, dy;
            dx = ptEnd.X - ptStart.X;
            dy = ptStart.Y - ptEnd.Y;

            dx = (int)Math.Pow(dx, 2);
            dy = (int)Math.Pow(dy, 2);

            int length = (int)Math.Sqrt(dx + dy);
            return length;
        }

        internal static Rect Convert(Rect rect)
        {
            Rect ret = new Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

            return ret;
        }

        public static double GetAngle(Point startPoint, Point endPoint)
        {
            double dx = (endPoint.X - startPoint.X);
            double dy = (endPoint.Y - startPoint.Y);
            double val = Math.Atan2(dy, dx);

            double ret = (180 * val / Math.PI);
            return ret;
        }


        internal static bool PointCloseToPoints(List<Point> Points, Point point, ref Point ptOrigin)
        {
            bool check = false;
            if (Points.Count > 1)
            {
                Point ptPrevious = Points.Last<Point> ();
                foreach (Point p in Points)
                {
                    Rect bound = new Rect(p.X - 10, p.Y - 10, 20, 20);
                    if (bound.Contains(point))
                    {
                        check = true;
                        ptOrigin = ptPrevious;
                        break;
                    }
                    ptPrevious = p;
                }
            }
            return check;
        }

        internal static Point GetCentre(List<Point> Points)
        {
            Point ret = new Point();
            foreach (Point p in Points)
            {
                ret.X += p.X;
                ret.Y += p.Y;
            }
            ret.X = ret.X / Points.Count;
            ret.Y = ret.Y / Points.Count;

            return ret;
        }

        internal static List<Point> TurnPoints(List<Point> Points, Point centerPoint, Point originPoint, Point endPoint,float ratio)
        {
            double angle = GetAngle(centerPoint, endPoint);
            double oldAngle = GetAngle(centerPoint, originPoint);
            double dLength = GetLength(centerPoint, endPoint) - GetLength(centerPoint, originPoint);
            dLength =(int)( dLength / ratio);

            Point[] pt = new Point[Points.Count];

            int i = 0;
            foreach (Point p in Points)
            {
                pt[i++] = Common.TurnPoint(centerPoint, p, (angle-oldAngle),dLength);
            }

            List<Point> ret = new List<Point>();
            ret.AddRange(pt);
            return ret;
        }

        internal static Point TurnPoint(Point orgin, Point p, double angle,double dLength)
        {
            double radius = Common.GetLength(orgin, p) + dLength;

            double newAngle = Common.GetAngle(orgin, p); 
            Point ret = new Point();
            ret.X = (int)(orgin.X + radius * Math.Sin((newAngle+ angle) * Math.PI / 180));
            ret.Y = (int)(orgin.Y - radius * Math.Cos ((newAngle + angle) * Math.PI / 180));
            return ret;
        }

        internal static Point GetMidPoint(Point pt1, Point pt2)
        {
            double dx = (pt2.X - pt1.X)/2;
            double dy = (pt2.Y - pt1.Y)/2;
            Point ret = new Point();
            ret.X  = pt1.X + dx;
            ret.Y = pt1.Y + dy;
            return ret;
        }
    }
}
