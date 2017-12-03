using System;
using System.Collections.Generic;

namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// ����ֱ���Լ���Ӧ�ļ���
    /// </summary>
    public struct Line
    {
        private double _a;
        private double _b;
        private double _c;

        /// <summary>
        /// ���캯��
        /// ֱ�ߵı�ʾΪ���̣�ay + bx + c = 0.
        /// </summary>
        /// <param name="a">ֱ�߷��̵�ϵ��A</param>
        /// <param name="b">ֱ�߷��̵�ϵ��B</param>
        /// <param name="c">ֱ�߷��̵�ϵ��C</param>
        public Line(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        /// <summary>
        /// ����ֱ����ĳ���Xֵ����øõ��Yֵ
        /// </summary>
        /// <param name="x">ĳ���Xֵ</param>
        /// <returns>�õ��Yֵ</returns>
        /// <remarks>�����ֱ����Y��ƽ�У����޷�����</remarks>
        public double GetYGivenX(double x)
        {
            if (_a == 0)
            {
                return 0;
            }

            // ay + bx + c = 0 ==> ay = -(b*x + c)/a
            return -(this._b * x + this._c) / this._a;
        }

        /// <summary>
        /// ����ֱ����ĳ���Yֵ����øõ��Xֵ
        /// </summary>
        /// <param name="x">ĳ���Yֵ</param>
        /// <returns>�õ��Xֵ</returns>
        /// <remarks>�����ֱ����X��ƽ�У����޷�����</remarks>
        public double GetXGivenY(double y)
        {
            if (_b == 0)
            {
                return 0;
            }

            // ay + bx + c = 0 ==> bx = -(a*y + c)/a
            return -(this._a * y + this._c) / this._b;
        }

        /// <summary>
        /// ֱ�߷��̵�ϵ��A
        /// </summary>
        public double A
        {
            set { _a = value; }
            get { return _a; }
        }

        /// <summary>
        /// ֱ�߷��̵�ϵ��B
        /// </summary>
        public double B
        {
            set { _b = value; }
            get { return _b; }
        }

        /// <summary>
        /// ֱ�߷��̵�ϵ��C
        /// </summary>
        public double C
        {
            set { _c = value; }
            get { return _c; }
        }

        /// <summary>
        /// �õ���ֱ������һ��ֱ�ߵĽ���
        /// </summary>
        /// <param name="line">��һ��ֱ��</param>
        /// <returns>��ֱ������һ��ֱ�ߵĽ���</returns>
        /// <remarks>���2��ֱ��ƽ�У��򷵻�Vector2f.NotExist</remarks>
        public Vector2f GetIntersection(Line line)
        {
            double x, y;

            if (_b == 0 || line.B == 0)
            {
                // lines are parallel, no intersection
                if (_b == line.B)
                {
                    return Vector2f.NotExist;
                }
                else if (_b == 0)
                {
                    y = -_c / _a;
                    x = line.GetXGivenY(y);
                }
                else
                {
                    y = -line.C / line.A;
                    x = GetXGivenY(y);
                }
                return new Vector2f(x, y);
            }

            // lines are parallel, no intersection
            if ((_a / _b) == (line.A / line.B))
            {
                return Vector2f.NotExist;
            }

            if (_a == 0) // bx + c = 0 and a2*y + b2*x + c2 = 0 ==> x = -c/b
            {
                // calculate x using the current line
                x = -_c / _b; // and calculate the y using the second line
                y = line.GetYGivenX(x);
            }
            else if (line.A == 0)
            {
                // ay + bx + c = 0 and b2*x + c2 = 0 ==> x = -c2/b2
                x = -line.C / line.B; // calculate x using
                y = GetYGivenX(x); // 2nd line and calculate y using current line
            }
            // ay + bx + c = 0 and a2y + b2*x + c2 = 0
            // y = (-b2/a2)x - c2/a2
            // bx = -a*y - c =>  bx = -a*(-b2/a2)x -a*(-c2/a2) - c ==>
            // ==> a2*bx = a*b2*x + a*c2 - a2*c ==> x = (a*c2 - a2*c)/(a2*b - a*b2)
            // calculate x using the above formula and the y using the current line
            else
            {
                x = (_a * line.C - line.A * _c) / (line.A * _b - _a * line.B);
                y = GetYGivenX(x);
            }

            return new Vector2f(x, y);
        }

        /// <summary>
        /// �õ���ֱ������һ��Բ�Ľ���
        /// </summary>
        /// <param name="circle">��һ��Բ</param>
        /// <returns>����ļ��ϣ� 0�����޽��㣩��1����1�����㣩��2����2�����㣩</returns>
        public Vector2f[] GetCircleIntersectionPoints(Circle circle)
        {
            double[] iSol;
            double h = circle.Center.X;
            double k = circle.Center.Y;
            List<Vector2f> result = new List<Vector2f>();

            // line:   x = -c/b (if a = 0)
            // circle: (x-h)^2 + (y-k)^2 = r^2, with h = center.x and k = center.y
            // fill in:(-c/b-h)^2 + y^2 -2ky + k^2 - r^2 = 0
            //         y^2 -2ky + (-c/b-h)^2 + k^2 - r^2 = 0
            // and determine solutions for y using abc-formula
            if (Math.Abs(_a) < Double.Epsilon)
            {
                iSol = NumeralHelper.SolveABCFormula(1, -2 * k, ((-_c / _b) - h) * ((-_c / _b) - h)
                                                         + k * k - circle.Radius * circle.Radius);

                if (iSol.Length > 0)
                {
                    result.Add(new Vector2f((-_c / _b), iSol[0]));
                }
                if (iSol.Length > 1)
                {
                    result.Add(new Vector2f((-_c / _b), iSol[1]));
                }
                return result.ToArray();
            }

            // ay + bx + c = 0 => y = -b/a x - c/a, with da = -b/a and db = -c/a
            // circle: (x-h)^2 + (y-k)^2 = r^2, with h = center.x and k = center.y
            // fill in:x^2 -2hx + h^2 + (da*x-db)^2 -2k(da*x-db) + k^2 - r^2 = 0
            //         x^2 -2hx + h^2 + da^2*x^2 + 2da*db*x + db^2 -2k*da*x -2k*db
            //                                                         + k^2 - r^2 = 0
            //       (1+da^2)*x^2 + 2(da*db-h-k*da)*x + h2 + db^2  -2k*db + k^2 - r^2 = 0
            // and determine solutions for x using abc-formula
            // fill in x in original line equation to get y coordinate
            double da = -_b / _a;
            double db = -_c / _a;

            double dA = 1 + da * da;
            double dB = 2 * (da * db - h - k * da);
            double dC = h * h + db * db - 2 * k * db + k * k - circle.Radius * circle.Radius;

            iSol = NumeralHelper.SolveABCFormula(dA, dB, dC);

            if (iSol.Length > 0)
            {
                result.Add(new Vector2f(iSol[0], da * iSol[0] + db));
            }
            if (iSol.Length > 1)
            {
                result.Add(new Vector2f(iSol[1], da * iSol[1] + db));
            }
            return result.ToArray();
        }

        /// <summary>
        /// �õ�P�㵽��ֱ�ߵĴ���
        /// </summary>
        /// <param name="pos">P��</param>
        /// <returns>P�㵽��ֱ�ߵĴ���</returns>
        public Line GetTangentLine(Vector2f p)
        {
            // ay + bx + c = 0 -> y = (-b/a)x + (-c/a)
            // tangent: y = (a/b)*x + C1 -> by - ax + C2 = 0 => C2 = ax - by
            // with pos.y = y, pos.x = x
            return new Line(_b, -_a, _a * p.X - _b * p.Y);
        }

        /// <summary>
        /// �õ���ֱ���ϵ�P������ĵ�
        /// </summary>
        /// <param name="pos">P��</param>
        /// <returns>��ֱ���ϵ�P������ĵ�</returns>
        public Vector2f GetPointOnLineClosestTo(Vector2f p)
        {
            Line l2 = GetTangentLine(p); // get tangent line
            return GetIntersection(l2); // and intersection between the two lines
        }

        /// <summary>
        /// �õ�P�㵽��ֱ�ߵľ���
        /// </summary>
        /// <param name="pos">P��</param>
        /// <returns>P�㵽��ֱ�ߵľ���</returns>
        public double GetDistanceWithPoint(Vector2f p)
        {
            return p.GetDistanceTo(this.GetPointOnLineClosestTo(p));
        }

        /// <summary>
        /// ����2��õ�һ��ֱ��
        /// </summary>
        /// <param name="pos1">��һ����</param>
        /// <param name="pos2">�ڶ�����</param>
        /// <returns>����2��õ���ֱ��</returns>
        public static Line MakeLineFromTwoPoints(Vector2f pos1, Vector2f pos2)
        {
            // 1*y + bx + c = 0 => y = -bx - c
            // with -b the direction coefficient (or slope)
            // and c = - y - bx
            double dA, dB, dC;
            double dTemp = pos2.X - pos1.X; // determine the slope
            if (Math.Abs(dTemp) < Double.Epsilon)
            {
                // ay + bx + c = 0 with vertical slope=> a = 0, b = 1
                dA = 0.0;
                dB = 1.0;
            }
            else
            {
                // y = (-b)x -c with -b the slope of the line
                dA = 1.0;
                dB = -(pos2.Y - pos1.Y) / dTemp;
            }

            // ay + bx + c = 0 ==> c = -a*y - b*x
            dC = -dA * pos2.Y - dB * pos2.X;

            return new Line(dA, dB, dC);
        }

        /// <summary>
        /// ����һ�����һ���Ƕȵõ�һ��ֱ��
        /// </summary>
        /// <param name="vec">��</param>
        /// <param name="angle">�Ƕ�</param>
        /// <returns>����һ�����һ���Ƕȵõ���ֱ��</returns>
        public static Line MakeLineFromPositionAndAngle(Vector2f vec, double angle)
        {
            // calculate point somewhat further in direction 'angle' and make
            // line from these two points.
            return MakeLineFromTwoPoints(vec, vec + new Vector2f(1, angle, true));
        }
    }
}