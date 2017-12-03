using System;

namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// 提供角度方面的辅助计算
    /// </summary>
    public class AngleHelper
    {
        /// <summary>
        /// 把弧度转化成角度
        /// </summary>
        /// <param name="rad">弧度值</param>
        /// <returns>角度值</returns>
        public static double Rad2Deg(double rad)
        {
            return (rad*180/Math.PI);
        }
        
        /// <summary>
        /// 把角度转化为弧度
        /// </summary>
        /// <param name="deg">角度值</param>
        /// <returns>弧度值</returns>
        public static double Deg2Rad(double deg)
        {
            return (deg*Math.PI/180);
        }

        /// <summary>
        /// 计算某个角度的Cos值
        /// </summary>
        /// <param name="deg">角度值</param>
        /// <returns>该角度的Cos值</returns>
        public static double CosDeg(double deg)
        {
            return (Math.Cos(Deg2Rad(deg)));
        }

        /// <summary>
        /// 计算某个角度的Sin值
        /// </summary>
        /// <param name="deg">角度值</param>
        /// <returns>该角度的Sin值</returns>
        public static double SinDeg(double deg)
        {
            return (Math.Sin(Deg2Rad(deg)));
        }

        /// <summary>
        /// 计算某个角度的Tan值
        /// </summary>
        /// <param name="deg">角度值</param>
        /// <returns>该角度的Tan值</returns>
        public static double TanDeg(double deg)
        {
            return (Math.Tan(Deg2Rad(deg)));
        }

        /// <summary>
        /// 计算Atan的角度值
        /// </summary>
        /// <param name="deg">a double value</param>
        /// <returns>Atan的角度值</returns>
        public static double AtanDeg(double deg)
        {
            return (Rad2Deg(Math.Atan(deg)));
        }

        /// <summary>
        /// 计算Atan的角度值
        /// </summary>
        /// <param name="x">a double value</param>
        /// <param name="x">a double value</param>
        /// <returns>Atan的角度值</returns>
        public static double Atan2Deg(double x, double y)
        {
            if (Math.Abs(x) < Double.Epsilon && Math.Abs(y) < Double.Epsilon)
            {
                return (0.0);
            }

            return (Rad2Deg(Math.Atan2(x, y)));
        }

        /// <summary>
        /// 计算Acos的角度值
        /// </summary>
        /// <param name="deg">a double value</param>
        /// <returns>Acos的角度值</returns>
        public static double AcosDeg(double deg)
        {
            if (deg >= 1)
            {
                return (0.0);
            }
            else if (deg <= -1)
            {
                return (180.0);
            }

            return (Rad2Deg(Math.Acos(deg)));
        }

        /// <summary>
        /// 计算Asin的角度值
        /// </summary>
        /// <param name="deg">a double value</param>
        /// <returns>Asin的角度值</returns>
        public static double AsinDeg(double deg)
        {
            if (deg >= 1)
            {
                return (90.0);
            }
            else if (deg <= -1)
            {
                return (-90.0);
            }

            return (Rad2Deg(Math.Asin(deg)));
        }

        /// <summary>
        /// 判断ang是否在angMin和angMax之间
        /// </summary>
        /// <param name="ang">被检查的角度</param>
        /// <param name="angMin">最小角度值</param>
        /// <param name="angMax">最大角度值</param>
        /// <returns>ang是否在angMin和angMax之间</returns>
        public static bool IsAngInInterval(double ang, double angMin, double angMax)
        {
            // convert all angles to interval 0..360
            if ((ang + 360) < 360) ang += 360;
            if ((angMin + 360) < 360) angMin += 360;
            if ((angMax + 360) < 360) angMax += 360;

            if (angMin < angMax) // 0 ---false-- angMin ---true-----angMax---false--360
                return angMin < ang && ang < angMax;
            else // 0 ---true--- angMax ---false----angMin---true---360
                return !(angMax < ang && ang < angMin);
        }

        /// <summary>
        /// 得到2个角的平分角度
        /// </summary>
        /// <param name="angMin">最小角 范围[-180,180]</param>
        /// <param name="angMax">最大角 范围[-180,180]</param>
        /// <returns>angMin和angMax的平分角度</returns>
        public static double GetBisectorTwoAngles(double angMin, double angMax)
        {
            // separate sine and cosine part to circumvent boundary problem
            return NormalizeAngle(
                Atan2Deg((SinDeg(angMin) + SinDeg(angMax))/2.0,
                         (CosDeg(angMin) + CosDeg(angMax))/2.0));
        }

        /// <summary>
        ///  将角度正规化，使得其范围在[-180,180]
        /// </summary>
        /// <param name="angle">需要正规化的角度</param>
        /// <returns>正规化后的角度</returns>
        public static double NormalizeAngle(double angle)
        {
            while (angle > 180.0) angle -= 360.0;
            while (angle < -180.0) angle += 360.0;

            return (angle);
        }
    }
}