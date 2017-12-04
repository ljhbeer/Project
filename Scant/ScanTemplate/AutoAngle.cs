using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ScanTemplate
{
     public class AutoAngle
    {
        private List<Point> _T;
        private List<Point> _P;
        private double _Angle1;
        private double _Angle2;
        public AutoAngle(List<Point> list)
        {
            this._T = list;
            _P = null;
            _Angle1 = Arcsin(_T[0], _T[2]);
        }

        private double  Arcsin(Point P0, Point P1)
        {
            double r = Math.Sqrt((P1.X - P0.X) * (P1.X - P0.X) + (P1.Y - P0.Y) * (P1.Y - P0.Y));
            return Math.Asin( (P1.X-P0.X)/r );
        }

        public double SetPaper(Point P0, Point P1, Point P2)
        {
            _P = new List<Point>(){P0,P1,P2};
            _Angle2 = Arcsin(P0, P2);
            return _Angle2;

        }
        public void SetPaper(double angle)
        {
            _Angle2 = angle;
        }
        public Point GetCorrectPoint(int x, int y) //相对0，0而言
        {
            double r = Math.Sqrt(x * x + y * y);
            double angle = Math.Asin( x/r);
            angle -= _Angle1-_Angle2;
            return new Point( (int)(r * Math.Sin(angle)),(int)(r*Math.Cos(angle)));
        }
    }
}
