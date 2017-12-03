using System;

namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// 2ά����ϵ�е��������
    /// </summary>
    /// <remarks>�벻Ҫʹ��==��=�������жϣ���Equals</remarks>
    public struct Vector2f
    {
        private double _x;
        private double _y;

        /// <summary>
        /// ����������һ����
        /// </summary>
        public static readonly Vector2f NotExist = new Vector2f(-99, -99);

        /// <summary>
        /// �յĵ�
        /// </summary>
        public static readonly Vector2f Empty = new Vector2f(-1, -1);        

        #region Construction Functions

        /// <summary>
        /// ���캯����ָ��X��Y��ֵ
        /// </summary>
        /// <param name="x">X��ֵ</param>
        /// <param name="y">Y��ֵ</param>
        public Vector2f(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// ���캯����������ֱ��ָ��X��Y��ֵ��ͨ�������귽ʽ
        /// </summary>
        /// <param name="length">�����ֱ��ָ����X    �����귽ʽ������</param>
        /// <param name="angle">�����ֱ��ָ����Y    �����귽ʽ���Ƕ�</param>
        /// <param name="isPOLAR">�Ƿ���ü�����ķ�ʽָ��X��Y��ֵ</param>
        public Vector2f(double length, double angle, bool isPOLAR)
        {
            if (!isPOLAR)
            {
                _x = length;
                _y = angle;
            }
            else
            {
                _x = length * AngleHelper.CosDeg(angle);
                _y = length * AngleHelper.SinDeg(angle);
            }
        }

        /// <summary>
        /// ���캯����������һ��Vector2f��ֵ
        /// </summary>
        /// <param name="vector2f">vector2f��ֵ</param>
        public Vector2f(Vector2f vector2f)
        {
            _x = vector2f._x;
            _y = vector2f._y;
        }

        #endregion

        /// <summary>
        /// �����Xֵ
        /// </summary>
        public double X
        {
            set { _x = value; }
            get { return _x; }
        }

        /// <summary>
        /// �����Yֵ
        /// </summary>
        public double Y
        {
            set { _y = value; }
            get { return _y; }
        }

        #region Overload Opertor

        public static Vector2f operator +(Vector2f lRel, Vector2f rRel)
        {
            return new Vector2f(lRel._x + rRel._x, lRel._y + rRel._y);
        }

        public static Vector2f operator -(Vector2f lRel, Vector2f rRel)
        {
            return new Vector2f(lRel._x - rRel._x, lRel._y - rRel._y);
        }

        public static Vector2f operator *(Vector2f lRel, double p)
        {
            return new Vector2f(lRel._x * p, lRel._y * p);
        }

        public static Vector2f operator *(double p, Vector2f rRel)
        {
            return new Vector2f(p * rRel._x, p * rRel._y);
        }

        public static Vector2f operator /(Vector2f lRel, double p)
        {
            return new Vector2f(lRel._x / p, lRel._y / p);
        }

        public static bool operator ==(Vector2f lRel, Vector2f rRel)
        {
            return lRel.X == rRel.X && lRel.Y == rRel.Y; 
        }

         public static bool operator !=(Vector2f lRel, Vector2f rRel)
        {
            return lRel.X != rRel.X || lRel.Y != rRel.Y; 
        }      

        public override bool Equals(object obj)
        {
            if (obj is Vector2f)
            {
                Vector2f v = (Vector2f)obj;

                if (_x == v._x && _y == v._y)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        /// <summary>
        /// �õ��������ĳ��ȣ���ԭ�㵽�õ�ĳ��ȣ�
        /// </summary>
        /// <returns>�õ��������ĳ���</returns>
        public double GetLength()
        {
            return Math.Sqrt(_x * _x + _y * _y);
        }

        /// <summary>
        /// �õ��õ㵽��һ��ľ���
        /// </summary>
        /// <param name="pos">��һ��</param>
        /// <returns>�õ㵽��һ��ľ���</returns>
        public double GetDistanceTo(Vector2f pos)
        {
            return (this - pos).GetLength();
        }

        /// <summary>
        /// �õ��õ���ԭ��ĽǶ�
        /// </summary>
        /// <returns>�õ���ԭ��ĽǶ�</returns>
        public double GetDirection()
        {
            return (AngleHelper.Atan2Deg(_y, _x));
        }

        /// <summary>
        /// �õ��õ㵽p�����ΪdFrac�ĵ�
        /// example: this --- 0.25 ---------  p
        /// </summary>
        /// <param name="p">p��</param>
        /// <param name="dFrac">����ֵ</param>
        /// <returns>�õ㵽p�����ΪdFrac�ĵ�</returns>
        public Vector2f GetVector2fOnLineFraction(Vector2f p, double dFrac)
        {
            // formula: this + dFrac * ( p - this ) = this - dFrac * this + dFrac * p =
            //          ( 1 - dFrac ) * this + dFrac * p
            return ((this) * (1.0 - dFrac) + (p * dFrac));
        }

        /// <summary>
        /// �ж�P���Ƿ��ڸõ��ǰ��
        /// </summary>
        /// <param name="p">P��</param>
        /// <returns>P���Ƿ��ڸõ��ǰ��</returns>
        public bool IsInFrontOf(Vector2f p)
        {
            return ((_x > p.X) ? true : false);
        }

        /// <summary>
        /// �ж�P���Ƿ��ڸõ�ĺ�
        /// </summary>
        /// <param name="p">P��</param>
        /// <returns>P���Ƿ��ڸõ�ĺ�</returns>
        public bool IsBehindOf(Vector2f p)
        {
            return ((this._x < p.X) ? true : false);
        }

        /// <summary>
        /// �ж�P���Ƿ��ڸõ�����
        /// </summary>
        /// <param name="p">P��</param>
        /// <returns>P���Ƿ��ڸõ�����</returns>
        public bool IsLeftOf(Vector2f p)
        {
            return ((this._y < p.Y) ? true : false);
        }

        /// <summary>
        /// �ж�P���Ƿ��ڸõ���ұ�
        /// </summary>
        /// <param name="p">P��</param>
        /// <returns>P���Ƿ��ڸõ���ұ�</returns>
        public bool IsRightOf(Vector2f p)
        {
            return ((this._y > p.Y) ? true : false);
        }

        /// <summary>
        /// �жϸõ��Xֵ�Ƿ���P1��P2֮��
        /// </summary>
        /// <param name="p1">P1��</param>
        /// <param name="p2">P2��</param>
        /// <returns>
        /// �õ��Xֵ�Ƿ���P1��P2֮��
        /// </returns>
        public bool IsBetweenX(Vector2f p1, Vector2f p2)
        {
            return (_x < p2.X && _x > p1.X) || (_x < p1.X && _x > p2.X);
        }

        /// <summary>
        /// �жϸõ��Xֵ�Ƿ���P1��P2֮��
        /// </summary>
        /// <param name="p1">P1</param>
        /// <param name="p2">P2</param>
        /// <returns>
        /// �õ��Xֵ�Ƿ���P1��P2֮��
        /// </returns>
        public bool IsBetweenX(double p1, double p2)
        {
            return (_x < p2 && _x > p1) || (_x < p1 && _x > p2);
        }

        /// <summary>
        /// �жϸõ��Yֵ�Ƿ���P1��P2֮��
        /// </summary>
        /// <param name="p1">P1��</param>
        /// <param name="p2">P2��</param>
        /// <returns>
        /// �õ��Yֵ�Ƿ���P1��P2֮��
        /// </returns>
        public bool IsBetweenY(Vector2f p1, Vector2f p2)
        {
            return (_y < p2.Y && _y > p1.Y) || (_y < p1.Y && _y > p2.Y);
        }

        /// <summary>
        /// �жϸõ��Yֵ�Ƿ���P1��P2֮��
        /// </summary>
        /// <param name="p1">P1</param>
        /// <param name="p2">P2</param>
        /// <returns>
        /// �õ��Yֵ�Ƿ���P1��P2֮��
        /// </returns>
        public bool IsBetweenY(double p1, double p2)
        {
            return (_y < p2 && _y > p1) || (_y < p1 && _y > p2);
        }

        /// <summary>
        /// �������øõ�ĳ��ȣ����ı�Ƕȡ�
        /// </summary>
        /// <param name="length">�����ĳ���</param>
        /// <remarks>��0��0��������Ч�ġ�</remarks>
        public void SetLength(double length)
        {
            if (this.GetLength() > Double.Epsilon)
            {
                Vector2f v = new Vector2f(this);
                v *= (length / this.GetLength());
                _x = v._x;
                _y = v._y;
            }
        }
    }
}