using System;

namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// �ṩ�Ƕȷ���ĸ�������
    /// </summary>
    public class AngleHelper
    {
        /// <summary>
        /// �ѻ���ת���ɽǶ�
        /// </summary>
        /// <param name="rad">����ֵ</param>
        /// <returns>�Ƕ�ֵ</returns>
        public static double Rad2Deg(double rad)
        {
            return (rad*180/Math.PI);
        }
        
        /// <summary>
        /// �ѽǶ�ת��Ϊ����
        /// </summary>
        /// <param name="deg">�Ƕ�ֵ</param>
        /// <returns>����ֵ</returns>
        public static double Deg2Rad(double deg)
        {
            return (deg*Math.PI/180);
        }

        /// <summary>
        /// ����ĳ���Ƕȵ�Cosֵ
        /// </summary>
        /// <param name="deg">�Ƕ�ֵ</param>
        /// <returns>�ýǶȵ�Cosֵ</returns>
        public static double CosDeg(double deg)
        {
            return (Math.Cos(Deg2Rad(deg)));
        }

        /// <summary>
        /// ����ĳ���Ƕȵ�Sinֵ
        /// </summary>
        /// <param name="deg">�Ƕ�ֵ</param>
        /// <returns>�ýǶȵ�Sinֵ</returns>
        public static double SinDeg(double deg)
        {
            return (Math.Sin(Deg2Rad(deg)));
        }

        /// <summary>
        /// ����ĳ���Ƕȵ�Tanֵ
        /// </summary>
        /// <param name="deg">�Ƕ�ֵ</param>
        /// <returns>�ýǶȵ�Tanֵ</returns>
        public static double TanDeg(double deg)
        {
            return (Math.Tan(Deg2Rad(deg)));
        }

        /// <summary>
        /// ����Atan�ĽǶ�ֵ
        /// </summary>
        /// <param name="deg">a double value</param>
        /// <returns>Atan�ĽǶ�ֵ</returns>
        public static double AtanDeg(double deg)
        {
            return (Rad2Deg(Math.Atan(deg)));
        }

        /// <summary>
        /// ����Atan�ĽǶ�ֵ
        /// </summary>
        /// <param name="x">a double value</param>
        /// <param name="x">a double value</param>
        /// <returns>Atan�ĽǶ�ֵ</returns>
        public static double Atan2Deg(double x, double y)
        {
            if (Math.Abs(x) < Double.Epsilon && Math.Abs(y) < Double.Epsilon)
            {
                return (0.0);
            }

            return (Rad2Deg(Math.Atan2(x, y)));
        }

        /// <summary>
        /// ����Acos�ĽǶ�ֵ
        /// </summary>
        /// <param name="deg">a double value</param>
        /// <returns>Acos�ĽǶ�ֵ</returns>
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
        /// ����Asin�ĽǶ�ֵ
        /// </summary>
        /// <param name="deg">a double value</param>
        /// <returns>Asin�ĽǶ�ֵ</returns>
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
        /// �ж�ang�Ƿ���angMin��angMax֮��
        /// </summary>
        /// <param name="ang">�����ĽǶ�</param>
        /// <param name="angMin">��С�Ƕ�ֵ</param>
        /// <param name="angMax">���Ƕ�ֵ</param>
        /// <returns>ang�Ƿ���angMin��angMax֮��</returns>
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
        /// �õ�2���ǵ�ƽ�ֽǶ�
        /// </summary>
        /// <param name="angMin">��С�� ��Χ[-180,180]</param>
        /// <param name="angMax">���� ��Χ[-180,180]</param>
        /// <returns>angMin��angMax��ƽ�ֽǶ�</returns>
        public static double GetBisectorTwoAngles(double angMin, double angMax)
        {
            // separate sine and cosine part to circumvent boundary problem
            return NormalizeAngle(
                Atan2Deg((SinDeg(angMin) + SinDeg(angMax))/2.0,
                         (CosDeg(angMin) + CosDeg(angMax))/2.0));
        }

        /// <summary>
        ///  ���Ƕ����滯��ʹ���䷶Χ��[-180,180]
        /// </summary>
        /// <param name="angle">��Ҫ���滯�ĽǶ�</param>
        /// <returns>���滯��ĽǶ�</returns>
        public static double NormalizeAngle(double angle)
        {
            while (angle > 180.0) angle -= 360.0;
            while (angle < -180.0) angle += 360.0;

            return (angle);
        }
    }
}