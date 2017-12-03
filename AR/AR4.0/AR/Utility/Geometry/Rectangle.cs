namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// 代表矩形以及相应的计算
    /// </summary>
    public struct Rectangle
    {
        private Vector2f _posLeftTop;
        private Vector2f _posRightBottom;

        /// <summary>
        /// 空元素
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle(Vector2f.Empty, Vector2f.Empty);

        public static bool operator ==(Rectangle lRel, Rectangle rRel)
        {
            return lRel._posLeftTop == rRel._posLeftTop && lRel._posRightBottom == rRel._posRightBottom;
        }

        public static bool operator !=(Rectangle lRel, Rectangle rRel)
        {
            return !(lRel == rRel);
        }

        public override bool Equals(object obj)
        {
            if (obj is Rectangle)
            {
                Rectangle v = (Rectangle)obj;

                if (_posLeftTop == v._posLeftTop && _posRightBottom == v._posRightBottom)
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

        /// <summary>
        /// 构造函数，指定左顶点和右底点的左边
        /// </summary>
        /// <param name="posLeftTop">左顶点</param>
        /// <param name="posRightBottom">右底点</param>
        public Rectangle(Vector2f posLeftTop, Vector2f posRightBottom)
        {
            _posLeftTop = posLeftTop;
            _posRightBottom = posRightBottom;
        }

        /// <summary>
        /// 判断P点是否在矩形之中
        /// </summary>
        /// <param name="pos">P点</param>
        /// <returns>P点是否在矩形之中</returns>
        public bool IsInside(Vector2f pos)
        {
            return pos.IsBetweenX(_posRightBottom, _posLeftTop) && pos.IsBetweenY(_posLeftTop, _posRightBottom);
        }

        /// <summary>
        /// 左顶点
        /// </summary>
        public Vector2f PosLeftTop
        {
            set { this._posLeftTop = value; }
            get { return this._posLeftTop; }
        }

        /// <summary>
        /// 右底点
        /// </summary>
        public Vector2f PosRightBottom
        {
            set { this._posRightBottom = value; }
            get { return this._posRightBottom; }
        }
    }
}