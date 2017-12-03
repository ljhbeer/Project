namespace CnblogsDotNetSDK.Utility.Geometry
{
    /// <summary>
    /// ��������Լ���Ӧ�ļ���
    /// </summary>
    public struct Rectangle
    {
        private Vector2f _posLeftTop;
        private Vector2f _posRightBottom;

        /// <summary>
        /// ��Ԫ��
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
        /// ���캯����ָ���󶥵���ҵ׵�����
        /// </summary>
        /// <param name="posLeftTop">�󶥵�</param>
        /// <param name="posRightBottom">�ҵ׵�</param>
        public Rectangle(Vector2f posLeftTop, Vector2f posRightBottom)
        {
            _posLeftTop = posLeftTop;
            _posRightBottom = posRightBottom;
        }

        /// <summary>
        /// �ж�P���Ƿ��ھ���֮��
        /// </summary>
        /// <param name="pos">P��</param>
        /// <returns>P���Ƿ��ھ���֮��</returns>
        public bool IsInside(Vector2f pos)
        {
            return pos.IsBetweenX(_posRightBottom, _posLeftTop) && pos.IsBetweenY(_posLeftTop, _posRightBottom);
        }

        /// <summary>
        /// �󶥵�
        /// </summary>
        public Vector2f PosLeftTop
        {
            set { this._posLeftTop = value; }
            get { return this._posLeftTop; }
        }

        /// <summary>
        /// �ҵ׵�
        /// </summary>
        public Vector2f PosRightBottom
        {
            set { this._posRightBottom = value; }
            get { return this._posRightBottom; }
        }
    }
}