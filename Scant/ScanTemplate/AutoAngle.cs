using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ScanTemplate;

namespace Tools
{
     public class AutoAngle
    {
        public AutoAngle(List<Point> list)
        {
            if(list.Count == 4 && global.featuretype ==0)
               this._T = list;
            else if (list.Count == 4 && global.featuretype == 1)
            {
                _T = new List<Point>() { list[2], list[3], list[0], list[1] };
            }
            else if (list.Count == 3)
            {
                _T = list;
            }

            _P = null;
            _Angle1 = Arcsin(_T[0], _T[1]);
        }
        public void SetPaper(List<Rectangle> list)
        {
            if ((list.Count == 4|| list.Count == 3) && global.featuretype == 0  )
               SetPaper(list[0].Location,list[1].Location,list[2].Location);
            else if (list.Count == 4 && global.featuretype == 1 )
            {
                SetPaper(list[2].Location, list[3].Location, list[0].Location);
            }
            else if (list.Count == 3 && global.featuretype == 1) //
            {
                SetPaper(list[0].Location, list[1].Location, list[2].Location);
            }
            
        }
        public void SetPaper(double angle)
        {
            _Angle2 = angle;
        }
        public Point GetCorrectPoint(int x, int y) //相对0，0而言
        {
            if (_T[0].X == 0)
            {
                double r = Math.Sqrt(x * x + y * y);
                if (r < 1)
                    return new Point(x, y);
                double angle = Math.Asin(x / r);
                angle -= _Angle1 - _Angle2;
                return new Point((int)(r * Math.Sin(angle)), (int)(r * Math.Cos(angle)));
            }
            else
            {
                double r = Math.Sqrt( (x-_T[0].X)* (x-_T[0].X) + (y-_T[0].Y)*(y-_T[0].Y));
                double angle =  Math.Asin( (x-_T[0].X) / r);
                angle -= _Angle1 - _Angle2;

                int nx = (int)(r * Math.Sin(angle))+ _P[0].X;
                int ny = (int)(r * Math.Cos(angle))+ _P[0].Y;

                return new Point(nx,ny);
            }
        }
        public double Angle1
        {
            get
            {
                return _Angle1;
            }
        }
        public double Angle2
        {
            get
            {
                return _Angle2;
            }
        }
        private double  Arcsin(Point P0, Point P1)
        {
            double r = Math.Sqrt((P1.X - P0.X) * (P1.X - P0.X) + (P1.Y - P0.Y) * (P1.Y - P0.Y));
            return Math.Asin( (P1.X-P0.X)/r );
        }
        private void SetPaper(Point P0, Point P1, Point P2)
        {
            _P = new List<Point>(){P0,P1,P2};
            _Angle2 = Arcsin(P0, P1);
        }

        private List<Point> _T;
        private List<Point> _P;
        private double _Angle1;
        private double _Angle2;
    }
}
