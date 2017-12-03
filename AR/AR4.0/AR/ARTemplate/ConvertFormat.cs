using System;
using System.Collections.Generic;
using System.Drawing;   
using System.Drawing.Imaging;   
using System.Runtime.InteropServices; 

//pictureBox1.Image = Image.FromFile(@"c:\temp\1.jpg");

//            pictureBox2.Image = MyConvert.Convert((Bitmap)pictureBox1.Image, PixelFormat.Format8bppIndexed, true);
//            pictureBox3.Image = MyConvert.Convert((Bitmap)pictureBox1.Image, PixelFormat.Format4bppIndexed, true);
//            pictureBox4.Image = MyConvert.Convert((Bitmap)pictureBox1.Image, PixelFormat.Format1bppIndexed, true);

namespace AR
{
    class ConvertFormat
    { 
       ///    
       /// 转换图片为彩色转换索引色成可以绘制的图形   
       public static Bitmap ConvertToRGB(Bitmap p_Image)   
       {   
           Bitmap newImage = new Bitmap(p_Image.Width, p_Image.Height, PixelFormat.Format32bppArgb);   
           newImage.SetResolution(p_Image.HorizontalResolution, p_Image.VerticalResolution);   
           Graphics g = Graphics.FromImage(newImage);   
           g.DrawImageUnscaled(p_Image, 0, 0);   
           g.Dispose();   
           return newImage;   
       }   
  
       ///    
       /// Convet图形为索引图形   
       ///    
       /// 原始图形   
       /// 格式 之支持 Format4bppIndexed  Format1bppIndexed Format8bppIndexed   
       /// 原始图形   
       /// 图形   
       public static Bitmap Convert(Bitmap p_SourceImage, PixelFormat p_PixelFormat, bool p_Dithering)   
       {   
           int _Width = p_SourceImage.Width;   
           int _Height = p_SourceImage.Height;   
  
           PixelFormat _SetPixFormat = PixelFormat.Format1bppIndexed;   
           if (p_PixelFormat == PixelFormat.Format4bppIndexed || p_PixelFormat == PixelFormat.Format1bppIndexed || p_PixelFormat == PixelFormat.Format8bppIndexed) _SetPixFormat = p_PixelFormat;   
  
  
           //创建颜色索引表 为2个颜色   
           Bitmap _NewBitmap = new Bitmap(_Width, _Height, _SetPixFormat);   
           ColorPalette _Palette = null;   
           switch (_SetPixFormat)   
           {   
               case PixelFormat.Format1bppIndexed:   
                   _Palette = _NewBitmap.Palette;   
                   _Palette.Entries[0] = Color.FromArgb(255, 0, 0, 0);   
                   _Palette.Entries[1] = Color.FromArgb(255, 255, 255, 255);   
                   _NewBitmap.Palette = _Palette;   
                   break;   
               case PixelFormat.Format4bppIndexed:   
                   _Palette = SetColorPalette.CreatePalette(p_SourceImage, 16, 4);   
                   _NewBitmap.Palette = _Palette;   
                   break;   
               case PixelFormat.Format8bppIndexed:   
                   _Palette = SetColorPalette.CreatePalette(p_SourceImage, 256, 8);   
                   _NewBitmap.Palette = _Palette;   
                   break;   
           }   
  
           //重新获取一个原始图形 色彩设置为24色 R G B   
           Bitmap _Source = new Bitmap(_Width, _Height, PixelFormat.Format24bppRgb);   
           Graphics _Graphics = Graphics.FromImage(_Source);   
           _Graphics.DrawImage(p_SourceImage, 0, 0, _Width, _Height);   
           _Graphics.Dispose();   
  
  
           //获取两个图形的数据   
           BitmapData _SourceData = _Source.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);   
           BitmapData _NewData = _NewBitmap.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.WriteOnly, _SetPixFormat);   
  
           //复制出图形数据的   
           byte[] _SourceByte = new byte[_SourceData.Stride * _Height];   
           byte[] _NewByte = new byte[_NewData.Stride * _Height];   
           Marshal.Copy(_SourceData.Scan0, _SourceByte, 0, _SourceByte.Length);   
           Marshal.Copy(_NewData.Scan0, _NewByte, 0, _NewByte.Length);   
  
           int _SourceIndex = 0;   
           int _NewIndex = 0;   
           //颜色表列   
           Dictionary<int, byte> _ColorTable = new Dictionary<int, byte>();   
  
           Color _SetColor;   
              
           for (int i = 0; i != _Height; i++)   
           {   
               _NewIndex = i * _NewData.Stride;   
               _SourceIndex = i * _SourceData.Stride;   
               for (int z = 0; z != _Width; z++)   
               {   
                   #region 获取色彩索引   
                   //获取原始图形色彩   
                    _SetColor = Color.FromArgb(0, _SourceByte[_SourceIndex + 2], _SourceByte[_SourceIndex + 1], _SourceByte[_SourceIndex]);   
                    _SourceIndex += 3;   
   
                    byte _TableIndex = 0;   
                    if (!(_ColorTable.TryGetValue(_SetColor.ToArgb(), out _TableIndex)))   
                    {   
                        //判断颜色是否在颜色表里   
                        _TableIndex = (byte)ConvertTobppFindNearestColor(_SetColor, _Palette.Entries);   
                        _ColorTable.Add(_SetColor.ToArgb(), _TableIndex);   
                    }   
                    #endregion   
  
                    #region 填充新图形   
                    switch (_SetPixFormat)   
                    {   
                        case PixelFormat.Format1bppIndexed:   
                            byte _Mask = (byte)(0x80 >> ((z - 1) & 0x7));   
                            if (_TableIndex == 1)   
                            {   
                                _NewByte[_NewIndex] |= _Mask;   
                            }   
                            else   
                            {   
                                _NewByte[_NewIndex] &= (byte)(_Mask ^ 0xff);   
                            }   
                            if ((z % 8) == 0 && (z != 0)) _NewIndex++;   
                            break;   
                        case PixelFormat.Format4bppIndexed:   
                            int _SetIndex = z / 2;   
                            if ((z % 2) == 0)   
                            {   
                                _NewByte[_NewIndex + _SetIndex] = (byte)(_TableIndex << 4);   
                            }   
                            else   
                            {   
                                _NewByte[_NewIndex + _SetIndex] = (byte)(_NewByte[_NewIndex + _SetIndex] | _TableIndex);   
                            }   
                            break;   
                        case PixelFormat.Format8bppIndexed:   
                            _NewByte[_NewIndex + z] = _TableIndex;   
                            break;   
                    }   
                    #endregion   
  
                    #region 先获取右边 然后获取下边 获取下边的左变和右边的色彩 设置数据   
   
                    if (!p_Dithering) continue;   
                    int _R = _SetColor.R - _Palette.Entries[_TableIndex].R;   
                    int _G = _SetColor.G - _Palette.Entries[_TableIndex].G;   
                    int _B = _SetColor.B - _Palette.Entries[_TableIndex].B;               
   
                    int _StarIndex = i * _SourceData.Stride;   
                    int _SourceIndexRVA = 0;   
                    if (z + 1 < _Width)   
                    {                         
                        _SourceIndexRVA = (_StarIndex) + ((z + 1) * 3);   
                        //_SourceIndexRVA = _SourceIndex;   
                        _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_B * 7) >> 4);   
                        _SourceIndexRVA++;   
                        _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_G * 7) >> 4);   
                        _SourceIndexRVA++;   
                        _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_R * 7) >> 4);   
                           
                    }   
                    if (i + 1 < _Height)   
                    {   
                        if (z - 1 > 0)   
                        {                              
                            _SourceIndexRVA = (_StarIndex) + ((z - 1) * 3) + _SourceData.Stride;   
                            //_SourceIndexRVA = (_SourceIndex - 6) + _SourceData.Stride;   
                            _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_B * 3) >> 4);   
                            _SourceIndexRVA++;   
                            _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_G * 3) >> 4);   
                            _SourceIndexRVA++;   
                            _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_R * 3) >> 4);   
                        }                          
                        _SourceIndexRVA = (_StarIndex) + ((z + 0) * 3) + _SourceData.Stride;   
                        //_SourceIndexRVA = (_SourceIndex - 3) + _SourceData.Stride;                          
                        _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_B * 5) >> 4);   
                        _SourceIndexRVA++;   
                        _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_G * 5) >> 4);   
                        _SourceIndexRVA++;   
                        _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_R * 5) >> 4);   
   
                        if (z + 1 < _Width)   
                        {                             
                            _SourceIndexRVA = (_StarIndex) + ((z + 1) * 3) + _SourceData.Stride;   
                           // _SourceIndexRVA = _SourceIndex + _SourceData.Stride;   
                            _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_B * 1) >> 4);   
                            _SourceIndexRVA++;   
                            _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_G * 1) >> 4);   
                            _SourceIndexRVA++;   
                            _SourceByte[_SourceIndexRVA] = ConvertTobppLimits(_SourceByte[_SourceIndexRVA], (_R * 1) >> 4);   
                        }   
                    }   
                    #endregion   
                }   
            }   
          
            _Source.UnlockBits(_SourceData);   
            Marshal.Copy(_NewByte, 0, _NewData.Scan0, _NewByte.Length);   
            _NewBitmap.UnlockBits(_NewData);   
   
            return _NewBitmap;   
        }   
        private static int ConvertTobppFindNearestColor(Color p_SetColor, Color[] p_PaletteEntries)   
        {   
            int _MinDistanceSquared = 195076; // 255 * 255 + 255 * 255 + 255 * 255 + 1    
            int _BestIndex = 0;   
   
            for (int i = 0; i < p_PaletteEntries.Length; i++)   
            {   
                int _ColorR = p_SetColor.R - p_PaletteEntries[i].R;   
                int _ColorG = p_SetColor.G - p_PaletteEntries[i].G;   
                int _ColorB = p_SetColor.B - p_PaletteEntries[i].B;   
   
                int _DistanceSquared = _ColorR * _ColorR + _ColorG * _ColorG + _ColorB * _ColorB;   
   
                if (_DistanceSquared < _MinDistanceSquared)   
                {   
                    _MinDistanceSquared = _DistanceSquared;   
                    _BestIndex = i;   
                }   
            }   
            return _BestIndex;   
        }   
        private static byte ConvertTobppLimits(int p_ColorA, int p_ColorB)   
        {              
            return (p_ColorA + p_ColorB) < 0 ? (byte)0 : (p_ColorA + p_ColorB) > 255 ? (byte)255 : (byte)(p_ColorA + p_ColorB);   
        }   
   
        ///    
        /// 获取颜色索引表   
        ///    
        public class SetColorPalette   
        {   
            public static ColorPalette CreatePalette(Bitmap p_Bitmap, int p_MaxColors, int p_BitsPerPixel)   
            {   
   
                ColorPalette _ReturnPalette;   
                Node _Tree;   
                int _LeafCount, _Index;   
                Node[] _ReducibleNodes = new Node[9];   
   
                if (p_MaxColors > Math.Pow(2, p_BitsPerPixel)) return null;   
   
                _Tree = null;   
                _LeafCount = 0;   
                if (p_BitsPerPixel > 8) return null;   
                for (int i = 0; i <= (int)p_BitsPerPixel; i++)   
                {   
                    _ReducibleNodes[i] = null;   
                }   
   
                Bitmap _SetBitmap = new Bitmap(p_Bitmap.Width, p_Bitmap.Height, PixelFormat.Format32bppArgb);   
                Graphics _Graphcis = Graphics.FromImage(_SetBitmap);   
                _Graphcis.DrawImage(p_Bitmap, 0, 0, p_Bitmap.Width, p_Bitmap.Height);   
                _Graphcis.Dispose();   
   
                BitmapData _BitmapDataSource = _SetBitmap.LockBits(new Rectangle(0, 0, _SetBitmap.Width, _SetBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);   
   
                int[] _DataInt = new int[_BitmapDataSource.Width * _BitmapDataSource.Height];   
   
                Marshal.Copy(_BitmapDataSource.Scan0, _DataInt, 0, _DataInt.Length);   
                try   
                {   
                    int _Pad = _BitmapDataSource.Stride - (((_SetBitmap.Width * Bitmap.GetPixelFormatSize(_SetBitmap.PixelFormat)) + 7) / 8);   
   
                    int _ReadIndex = 0;   
   
                    for (int i = 0; i < _SetBitmap.Height; i++)   
                    {   
                        for (int j = 0; j < _SetBitmap.Width; j++)   
                        {   
                            byte _ColorBlue = (byte)(((uint)_DataInt[_ReadIndex] & 0x000000FF) >> 0);   
                            byte _ColorGreen = (byte)(((uint)_DataInt[_ReadIndex] & 0x0000FF00) >> 8);   
                            byte _ColorRed = (byte)(((uint)_DataInt[_ReadIndex] & 0x00FF0000) >> 16);   
                            _ReadIndex++;   
                            AddColor(ref _Tree, _ColorRed, _ColorGreen, _ColorBlue, p_BitsPerPixel, 0, ref _LeafCount, ref _ReducibleNodes);   
   
                            while (_LeafCount > p_MaxColors)   
                                ReduceTree(p_BitsPerPixel, ref _LeafCount, ref _ReducibleNodes);   
                        }   
                    }   
   
                    if (_LeafCount > p_MaxColors) _Tree = null;   
                    Bitmap _Bmp = null;   
                    switch (p_BitsPerPixel)   
                    {   
                        case 1:   
                            _Bmp = new Bitmap(1, 1, PixelFormat.Format1bppIndexed);   
                            break;   
                        case 4:   
                            _Bmp = new Bitmap(1, 1, PixelFormat.Format4bppIndexed);   
                            break;   
                        case 8:   
                            _Bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);   
                            break;   
                    }   
                    _ReturnPalette = _Bmp.Palette;   
                    _Bmp.Dispose();   
   
                    _Index = 0;   
                    GetPaletteColors(_Tree, ref _ReturnPalette, ref _Index);   
   
                    Color[] _EntriesList = _ReturnPalette.Entries;   
                    for (int i = _Index + 1; i < _EntriesList.Length; i++)   
                    {   
                        _EntriesList[i] = Color.FromArgb(0, 0, 0, 0);   
                    }   
                }   
                finally   
                {   
                    if (_BitmapDataSource != null) _SetBitmap.UnlockBits(_BitmapDataSource);   
                }   
                return _ReturnPalette;   
            }   
   
            private static void GetPaletteColors(Node p_Tree, ref ColorPalette p_Entries, ref int p_Index)   
            {   
                  
                Color[] _EntriesList = p_Entries.Entries;   
                if (p_Tree.IsLeaf)   
                {   
                    _EntriesList[p_Index] = Color.FromArgb(   
                                        (byte)((p_Tree.RedSum) / (p_Tree.PixelCount)),   
                                        (byte)((p_Tree.GreenSum) / (p_Tree.PixelCount)),   
                                        (byte)((p_Tree.BlueSum) / (p_Tree.PixelCount)));   
                    p_Index++;   
                }   
                else   
                {   
                    for (int i = 0; i < 8; i++)   
                    {   
                        if (p_Tree.Child[i] != null) GetPaletteColors(p_Tree.Child[i], ref p_Entries, ref p_Index);   
                    }   
                }   
            }   
   
            private static void ReduceTree(int p_ColorBits, ref int p_LeafCount, ref Node[] p_ReducibleNodes)   
            {   
                int _i;   
                Node _Node;   
                uint _RedSum, _GreenSum, _BlueSum;   
                int _Children;   
   
                // Find the deepest level containing at least one reducible node   
                for (_i = p_ColorBits - 1; (_i > 0) && (p_ReducibleNodes[_i] == null); _i--) ;   
   
                // Reduce the node most recently added to the list at level i   
                _Node = p_ReducibleNodes[_i];   
                p_ReducibleNodes[_i] = _Node.Next;   
   
                _RedSum = _GreenSum = _BlueSum = 0;   
                _Children = 0;   
                for (_i = 0; _i < 8; _i++)   
                {   
                    if (_Node.Child[_i] != null)   
                    {   
                        _RedSum += _Node.Child[_i].RedSum;   
                        _GreenSum += _Node.Child[_i].GreenSum;   
                        _BlueSum += _Node.Child[_i].BlueSum;   
                        _Node.PixelCount += _Node.Child[_i].PixelCount;   
                        _Node.Child[_i] = null;   
                        _Children++;   
                    }   
                }   
   
                _Node.IsLeaf = true;   
                _Node.RedSum = _RedSum;   
                _Node.GreenSum = _GreenSum;   
                _Node.BlueSum = _BlueSum;   
                p_LeafCount -= (_Children - 1);   
            }   
   
            private static void AddColor(ref Node p_Node, byte p_Red, byte p_Green, byte p_Blue, int p_ColorBits, int p_Level, ref int p_LeafCount, ref Node[] p_ReducibleNodes)   
            {                  
                byte[] _Mask = new byte[8] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };   
   
                if (p_Node == null)   
                {   
                    p_Node = CreateNode(p_Level, p_ColorBits, ref p_LeafCount, ref p_ReducibleNodes);   
                }   
                if (p_Node.IsLeaf)   
                {   
                    p_Node.PixelCount++;   
                    p_Node.RedSum += p_Red;   
                    p_Node.GreenSum += p_Green;   
                    p_Node.BlueSum += p_Blue;   
                }   
                else   
                {   
                   int _Shift = 7 - p_Level;   
                   int _Index = (((p_Red & _Mask[p_Level]) >> _Shift) << 2) |   
                        (((p_Green & _Mask[p_Level]) >> _Shift) << 1) |   
                        ((p_Blue & _Mask[p_Level]) >> _Shift);   
                    AddColor(ref p_Node.Child[_Index], p_Red, p_Green, p_Blue, p_ColorBits, p_Level + 1, ref p_LeafCount, ref p_ReducibleNodes);   
                }   
            }   
   
            private static Node CreateNode(int p_Level, int p_ColorBits, ref int p_LeafCount, ref Node[] p_ReducibleNodes)   
            {   
                Node _Node = new Node();   
   
                _Node.IsLeaf = (p_Level == p_ColorBits);   
                if (_Node.IsLeaf)   
                    p_LeafCount++;   
                else   
                {   
                    // Add the node to the reducible list for this level   
                    _Node.Next = p_ReducibleNodes[p_Level];   
                    p_ReducibleNodes[p_Level] = _Node;   
                }   
                return _Node;   
            }   
   
            private class Node   
            {   
                public bool IsLeaf;               // TRUE if node has no children   
                public int PixelCount;            // Number of pixels represented by this leaf   
                public uint RedSum;               // Sum of red components   
                public uint GreenSum;             // Sum of green components   
                public uint BlueSum;              // Sum of blue components   
                public Node[] Child = new Node[8]; // Pointers to child nodes   
                public Node Next;                  // Pointer to next reducible node   
            }   
   
        }   
   
   
         
    }   
}   
