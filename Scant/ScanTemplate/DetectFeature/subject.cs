using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScanTemplate
{
    public  class subject
    {
        public subject(string name, Rectangle rectangle)
        {
            this.Name = name;
            this.Rect = rectangle;
        }
        public override string ToString()
        {
            return Name;
        }
        public Rectangle Rect { get; set; }
        private string Name;

        public int BitmapdataLength { get; set; }
    }
    public class LoadBitmapData
    {
        public LoadBitmapData(string path)
        {
             _nameList = Tools.FileTools.NameListFromDir(path);
             position = 0;
        }
        public int Count(bool used) //
        {
            if (!used)
                return _nameList.Count;
            return _nameList.Count - position; ;
        }

        public Bitmap GetBitmap(string kh)
        {
            Bitmap bmp =(Bitmap) Bitmap.FromFile(kh);
            return bmp.Clone(Rect, bmp.PixelFormat);
        }
        public string GetNextKh()
        {
            if (position >= 0 && position < _nameList.Count)
            {
                string re =_nameList[position];
                position++;
                return re;
            }
            return "";
        }
        public void SetBitMapRectangle(Rectangle rectangle)
        {
            this.Rect = rectangle;
        }
        public void GoHead()
        {
            if(position<0 || position>= _nameList.Count)
            position = 0;
        }
        private List<string> _nameList;
        private int position;
        private Rectangle Rect;

    }
}
