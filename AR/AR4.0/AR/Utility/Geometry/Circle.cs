using System;
using System.Collections.Generic;

namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// 圆的平面几何计算类
    /// </summary>
    public struct Circle
    {
        /// <summary>
        /// 圆心坐标
        /// </summary>
        private Vector2f _center;

        /// <summary>
        /// 圆半径
        /// </summary>
        private double _radius;

        /// <summary>
        /// 圆的构造函数
        /// </summary>
        /// <param name="pos">圆心坐标</param>
        /// <param name="dR">圆半径</param>
        public Circle(Vector2f pos, double dR)
        {
            _center = pos;
            _radius = dR;
        }

        /// <summary>
        /// 得到圆的半径
        /// </summary>
        public double Radius
        {
            get { return _radius; }
        }

        /// <summary>
        /// 得到圆心坐标
        /// </summary>
        public Vector2f Center
        {
            get { return _center; }
        }

        /// <summary>
        /// 判断某点是否在圆的内部
        /// </summary>
        /// <param name="pos">
        /// 需要判断的点的坐标
        /// </param>
        /// <returns>该点是否在圆的内部</returns>
        public bool IsInside(Vector2f pos)
        {
            return _center.GetDistanceTo(pos) < _radius;
        }

        /// <summary>
        /// 得到2个圆的交点
        /// </summary>
        /// <param name="c">需要比较的另一个圆</param>
        /// <returns>交点的集合，可能出现0（无交点），1（1个交点）和2（2个交点）三种情况</returns>
        /// <remarks>如果2个圆是重合的，返回的结果还是2个交点，使用的时候应该避免出现这种情况：）</remarks>
        public Vector2f[] GetIntersectionPoints(Circle c)
        {
            double x0, y0, r0;
            double x1, y1, r1;

            x0 = _center.X;
            y0 = _center.Y;
            r0 = _radius;
            x1 = c._center.X;
            y1 = c._center.Y;
            r1 = c._radius;

            double d, dx, dy, h, a, x, y, p2_x, p2_y;

            // first calculate distance between two centers circles P0 and P1.
            dx = x1 - x0;
            dy = y1 - y0;
            d = Math.Sqrt(dx * dx + dy * dy);

            // normalize differences
            dx /= d;
            dy /= d;

            // a is distance between p0 and point that is the intersection point P2
            // that intersects P0-P1 and the line that crosses the two intersection
            // points P3 and P4.
            // Define two triangles: P0,P2,P3 and P1,P2,P3.
            // with distances a, h, r0 and b, h, r1 with d = a + b
            // We know a^2 + h^2 = r0^2 and b^2 + h^2 = r1^2 which then gives
            // a^2 + r1^2 - b^2 = r0^2 with d = a + b ==> a^2 + r1^2 - (d-a)^2 = r0^2
            // ==> r0^2 + d^2 - r1^2 / 2*d
            a = (r0 * r0 + d * d - r1 * r1) / (2.0 * d);

            // h is then a^2 + h^2 = r0^2 ==> h = sqrt( r0^2 - a^2 )
            double arg = r0 * r0 - a * a;
            h = (arg > 0.0) ? Math.Sqrt(arg) : 0.0;

            // First calculate P2
            p2_x = x0 + a * dx;
            p2_y = y0 + a * dy;

            // and finally the two intersection points
            x = p2_x - h * dy;
            y = p2_y + h * dx;
            Vector2f p1 = new Vector2f(x, y);

            x = p2_x + h * dy;
            y = p2_y - h * dx;
            Vector2f p2 = new Vector2f(x, y);


            List<Vector2f> result = new List<Vector2f>();

            if (arg < 0.0)
            {
                ;
            }
            else if (NumeralHelper.IsEquals(arg, 0.0))
            {
                result.Add(p1);
            }
            else
            {
                result.Add(p1);
                result.Add(p2);
            }

            return result.ToArray();
        }
    }
}