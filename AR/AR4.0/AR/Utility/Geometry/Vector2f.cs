using System;

namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// 2维坐标系中的坐标点类
    /// </summary>
    /// <remarks>请不要使用==或！=来进行判断，用Equals</remarks>
    public struct Vector2f
    {
        private double _x;
        private double _y;

        /// <summary>
        /// 不存在这样一个点
        /// </summary>
        public static readonly Vector2f NotExist = new Vector2f(-99, -99);

        /// <summary>
        /// 空的点
        /// </summary>
        public static readonly Vector2f Empty = new Vector2f(-1, -1);        

        #region Construction Functions

        /// <summary>
        /// 构造函数，指定X和Y的值
        /// </summary>
        /// <param name="x">X的值</param>
        /// <param name="y">Y的值</param>
        public Vector2f(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// 构造函数，可以用直接指定X和Y的值或通过极坐标方式
        /// </summary>
        /// <param name="length">如何是直接指定：X    极坐标方式：长度</param>
        /// <param name="angle">如何是直接指定：Y    极坐标方式：角度</param>
        /// <param name="isPOLAR">是否采用极坐标的方式指定X和Y的值</param>
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
        /// 构造函数，复制另一个Vector2f的值
        /// </summary>
        /// <param name="vector2f">vector2f的值</param>
        public Vector2f(Vector2f vector2f)
        {
            _x = vector2f._x;
            _y = vector2f._y;
        }

        #endregion

        /// <summary>
        /// 坐标的X值
        /// </summary>
        public double X
        {
            set { _x = value; }
            get { return _x; }
        }

        /// <summary>
        /// 坐标的Y值
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
        /// 得到该坐标点的长度（从原点到该点的长度）
        /// </summary>
        /// <returns>得到该坐标点的长度</returns>
        public double GetLength()
        {
            return Math.Sqrt(_x * _x + _y * _y);
        }

        /// <summary>
        /// 得到该点到另一点的距离
        /// </summary>
        /// <param name="pos">另一点</param>
        /// <returns>该点到另一点的距离</returns>
        public double GetDistanceTo(Vector2f pos)
        {
            return (this - pos).GetLength();
        }

        /// <summary>
        /// 得到该点与原点的角度
        /// </summary>
        /// <returns>该点与原点的角度</returns>
        public double GetDirection()
        {
            return (AngleHelper.Atan2Deg(_y, _x));
        }

        /// <summary>
        /// 得到该点到p点比例为dFrac的点
        /// example: this --- 0.25 ---------  p
        /// </summary>
        /// <param name="p">p点</param>
        /// <param name="dFrac">比例值</param>
        /// <returns>该点到p点比例为dFrac的点</returns>
        public Vector2f GetVector2fOnLineFraction(Vector2f p, double dFrac)
        {
            // formula: this + dFrac * ( p - this ) = this - dFrac * this + dFrac * p =
            //          ( 1 - dFrac ) * this + dFrac * p
            return ((this) * (1.0 - dFrac) + (p * dFrac));
        }

        /// <summary>
        /// 判断P点是否在该点的前方
        /// </summary>
        /// <param name="p">P点</param>
        /// <returns>P点是否在该点的前方</returns>
        public bool IsInFrontOf(Vector2f p)
        {
            return ((_x > p.X) ? true : false);
        }

        /// <summary>
        /// 判断P点是否在该点的后方
        /// </summary>
        /// <param name="p">P点</param>
        /// <returns>P点是否在该点的后方</returns>
        public bool IsBehindOf(Vector2f p)
        {
            return ((this._x < p.X) ? true : false);
        }

        /// <summary>
        /// 判断P点是否在该点的左边
        /// </summary>
        /// <param name="p">P点</param>
        /// <returns>P点是否在该点的左边</returns>
        public bool IsLeftOf(Vector2f p)
        {
            return ((this._y < p.Y) ? true : false);
        }

        /// <summary>
        /// 判断P点是否在该点的右边
        /// </summary>
        /// <param name="p">P点</param>
        /// <returns>P点是否在该点的右边</returns>
        public bool IsRightOf(Vector2f p)
        {
            return ((this._y > p.Y) ? true : false);
        }

        /// <summary>
        /// 判断该点的X值是否在P1和P2之间
        /// </summary>
        /// <param name="p1">P1点</param>
        /// <param name="p2">P2点</param>
        /// <returns>
        /// 该点的X值是否在P1和P2之间
        /// </returns>
        public bool IsBetweenX(Vector2f p1, Vector2f p2)
        {
            return (_x < p2.X && _x > p1.X) || (_x < p1.X && _x > p2.X);
        }

        /// <summary>
        /// 判断该点的X值是否在P1和P2之间
        /// </summary>
        /// <param name="p1">P1</param>
        /// <param name="p2">P2</param>
        /// <returns>
        /// 该点的X值是否在P1和P2之间
        /// </returns>
        public bool IsBetweenX(double p1, double p2)
        {
            return (_x < p2 && _x > p1) || (_x < p1 && _x > p2);
        }

        /// <summary>
        /// 判断该点的Y值是否在P1和P2之间
        /// </summary>
        /// <param name="p1">P1点</param>
        /// <param name="p2">P2点</param>
        /// <returns>
        /// 该点的Y值是否在P1和P2之间
        /// </returns>
        public bool IsBetweenY(Vector2f p1, Vector2f p2)
        {
            return (_y < p2.Y && _y > p1.Y) || (_y < p1.Y && _y > p2.Y);
        }

        /// <summary>
        /// 判断该点的Y值是否在P1和P2之间
        /// </summary>
        /// <param name="p1">P1</param>
        /// <param name="p2">P2</param>
        /// <returns>
        /// 该点的Y值是否在P1和P2之间
        /// </returns>
        public bool IsBetweenY(double p1, double p2)
        {
            return (_y < p2 && _y > p1) || (_y < p1 && _y > p2);
        }

        /// <summary>
        /// 重新设置该点的长度，不改变角度。
        /// </summary>
        /// <param name="length">期望的长度</param>
        /// <remarks>（0，0）点是无效的。</remarks>
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