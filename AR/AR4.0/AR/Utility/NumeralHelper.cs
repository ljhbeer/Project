using System;
using System.Collections.Generic;

namespace CnblogsDotNetSDK.Utility
{
    /// <summary>
    /// 提供数字计算方面的帮助类
    /// </summary>
    public class NumeralHelper
    {
        /// <summary>
        /// 判断2个Double类型的数是否相等
        /// </summary>
        /// <param name="a">第一个数</param>
        /// <param name="b">第二个数</param>
        /// <returns>2个Double类型的数是否相等</returns>
        public static bool IsEquals(double a, double b)
        {
            return Math.Abs(a - b) < Double.Epsilon ? true : false;
        }

        /// <summary>
        /// 将Double转化为int
        /// </summary>
        /// <param name="d">Double</param>
        /// <param name="round">是否向下取整</param>
        /// <returns>int</returns>
        public static int CastDouble2Int(double d, bool round)
        {
            bool positive = d >= 0;
            d = Math.Abs(d);

            int i = (int)d;

            if (!round && d > i)
            {
                return positive ? i + 1 : -(i + 1);
            }
            return positive ? i : -i;
        }

        /// <summary>
        /// 得到2元1次方程的解
        /// ax^2 + bx + c = 0
        /// </summary>
        /// <param name="a">方程的参数A</param>
        /// <param name="b">方程的参数B</param>
        /// <param name="c">方程的参数C</param>
        /// <returns>方程的解集合：0个（无解），1个（1个解），2个（2个解）</returns>
        public static double[] SolveABCFormula(double a, double b, double c)
        {
            double dDiscr = b * b - 4 * a * c; // discriminant is b^2 - 4*a*c
            List<double> result = new List<double>();

            if (Math.Abs(dDiscr) < Double.Epsilon) // if discriminant = 0
            {
                //  only one solution
                result.Add(-b / (2 * a));
                return result.ToArray();
            }
            else if (dDiscr < 0) // if discriminant < 0
            {
                //  no solutions
                return result.ToArray();
            }
            else // if discriminant > 0
            {
                dDiscr = Math.Sqrt(dDiscr); //  two solutions

                result.Add((-b + dDiscr) / (2 * a));
                result.Add((-b - dDiscr) / (2 * a));
                return result.ToArray();
            }
        }

        /// <summary>
        /// A geometric series is one in which there is a constant ratio between each
        /// element and the one preceding it. This method determines the
        /// length of a geometric series given its first element, the sum of the
        /// elements in the series and the constant ratio between the elements.
        /// Normally: s = a + ar + ar^2 + ...  + ar^n
        /// Now: dSum = dFirst + dFirst*dRatio + dFirst*dRatio^2 + .. + dFist*dRatio^n
        /// </summary>
        /// <param name="dFirst">first term of the series</param>
        /// <param name="dRatio">ratio with which the the first term is multiplied</param>
        /// <param name="dSum">the total sum of all the serie</param>
        /// <returns>he length(n in above example) of the series</returns>
        public static double GetLengthGeomSeries(double dFirst, double dRatio, double dSum)
        {
            if (dRatio < 0)
                throw new Exception("(GetLengthGeomSeries): negative ratio");

            // s = a + ar + ar^2 + .. + ar^n-1 and thus sr = ar + ar^2 + .. + ar^n
            // subtract: sr - s = - a + ar^n) =>  s(1-r)/a + 1 = r^n = temp
            // log r^n / n = n log r / log r = n = length
            double temp = (dSum * (dRatio - 1) / dFirst) + 1;
            if (temp <= 0)
                return -1.0;
            return Math.Log(temp) / Math.Log(dRatio);
        }

        /// <summary>
        /// A geometric series is one in which there is a constant ratio between each
        /// element and the one preceding it. This method determines the sum of a
        /// geometric series given its first element, the ratio and the number of steps
        /// in the series
        /// Normally: s = a + ar + ar^2 + ...  + ar^n
        /// Now: dSum = dFirst + dFirst*dRatio + ... + dFirst*dRatio^dSteps
        /// </summary>
        /// <param name="dFirst">first term of the series</param>
        /// <param name="dRatio">ratio with which the the first term is multiplied</param>
        /// <param name="dLength">the number of steps to be taken into account</param>
        /// <returns>the sum of the series</returns>
        public static double GetSumGeomSeries(double dFirst, double dRatio, double dLength)
        {
            // s = a + ar + ar^2 + .. + ar^n-1 and thus sr = ar + ar^2 + .. + ar^n
            // subtract: s - sr = a - ar^n) =>  s = a(1-r^n)/(1-r)
            return dFirst * (1 - Math.Pow(dRatio, dLength)) / (1 - dRatio);
        }

        /// <summary>
        /// A geometric series is one in which there is a constant ratio between each
        /// element and the one preceding it. This method determines the sum of an
        /// infinite geometric series given its first element and the constant ratio
        /// between the elements. Note that such an infinite series will only converge
        /// when r == （0，1）
        /// Normally: s = a + ar + ar^2 + ar^3 + ....
        /// Now: dSum = dFirst + dFirst*dRatio + dFirst*dRatio^2...
        /// </summary>
        /// <param name="dFirst">first term of the series</param>
        /// <param name="dRatio">ratio with which the the first term is multiplied</param>
        /// <returns>the sum of the series</returns>
        public static double GetSumInfGeomSeries(double dFirst, double dRatio)
        {
            if (dRatio > 1)
                throw new Exception("(GetSumInfGeomSeries): series does not converge");

            // s = a(1-r^n)/(1-r) with n->inf and 0<r<1 => r^n = 0
            return dFirst / (1 - dRatio);
        }

        /// <summary>
        /// A geometric series is one in which there is a constant ratio between each
        /// element and the one preceding it. This method determines the first element
        /// of a geometric series given its element, the ratio and the number of steps
        /// in the series
        /// Normally: s = a + ar + ar^2 + ...  + ar^n
        /// Now: dSum = dFirst + dFirst*dRatio + ... + dFirst*dRatio^dSteps
        /// </summary>
        /// <param name="dSum">sum of the series</param>
        /// <param name="dRatio">ratio with which the the first term is multiplied</param>
        /// <param name="dLength">the number of steps to be taken into account</param>
        /// <returns>the first element (a) of a serie</returns>
        public static double GetFirstGeomSeries(double dSum, double dRatio, double dLength)
        {
            // s = a + ar + ar^2 + .. + ar^n-1 and thus sr = ar + ar^2 + .. + ar^n
            // subtract: s - sr = a - ar^n) =>  s = a(1-r^n)/(1-r) => a = s*(1-r)/(1-r^n)
            return dSum * (1 - dRatio) / (1 - Math.Pow(dRatio, dLength));
        }

        /// <summary>
        /// A geometric series is one in which there is a constant ratio between each
        /// element and the one preceding it. This method determines the first element
        /// of an infinite geometric series given its first element and the constant
        /// ratio between the elements. Note that such an infinite series will only
        /// converge when r == (0,1)
        /// Normally: s = a + ar + ar^2 + ar^3 + ....
        /// Now: dSum = dFirst + dFirst*dRatio + dFirst*dRatio^2...
        /// </summary>
        /// <param name="dSum">sum of the series</param>
        /// <param name="dRatio">ratio with which the the first term is multiplied</param>
        /// <returns>the first term of the series</returns>
        public static double GetFirstInfGeomSeries(double dSum, double dRatio)
        {
            if (dRatio > 1)
                throw new Exception("(GetFirstInfGeomSeries): series does not converge");

            // s = a(1-r^n)/(1-r) with r->inf and 0<r<1 => r^n = 0 => a = s ( 1 - r)
            return dSum * (1 - dRatio);
        }
    }
}
