using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Camera
{
    class AR
    {
        public AR()
        {
            cfg = null;
            cleardata();
        }
        ~AR()
        {

        }


        public int Debug
        {
            set { debug = value; }
        }
        public CFGForm Cfg
        {
            set { cfg = value; }
        }
        public Rectangle Photorect
        {
            set { _photorect = value; }
        }
        public Bitmap  FinishedBmp 
        {
            get
            {
                Bitmap bmp = (Bitmap)_src.Clone();
                DrawRect(bmp);
                DrawOption(bmp);
                return bmp;
            }
        }
        public string GetRectString( )
        {
            return "R(" + _readrect.Left.ToString() + "," + _readrect.Top.ToString() + "," + _readrect.Right.ToString() + "," + _readrect.Bottom.ToString() + ")";
        }
        public bool TestImg(Bitmap bmp)
        {
            cleardata();
            if (_photorect.X < 5 || _photorect.Y < 5 || _photorect.Width < 300 || _photorect.Height < 100 ||
                _photorect.Right > 630 || _photorect.Bottom > 460) return false;
            this._src = bmp;
            this._gdt = (Bitmap )bmp.Clone();
            Fill(_gdt, 0);
            Gray(_src);
            if (!ComputeReadRect()) return false;
            if (!ValidXuehaoLine()) return false;
            if (!ComputeXuehao()) return false;
            // if validxuanzhetir
            ComputXuanzheti();
            return true;
        }

        private bool ComputeReadRect()
        {
            int midx = (_photorect.Left  + _photorect.Right )/2;
            int midy = (_photorect.Top   + _photorect.Bottom )/2;
            Rectangle xr = new Rectangle(_photorect.Left, midy - 50, _photorect.Width, 100);
            Rectangle yr = new Rectangle(midx-50, _photorect.Top , 100, _photorect.Height );
            int[] xcnt, ycnt;
            List<int> xlinepos, ylinepos;
            CountXPixsum(_src,xr,out xcnt);
            CountYPixsum(_src,yr, out ycnt);
            ComputeLinepos(xcnt,out xlinepos);
            ComputeLinepos(ycnt,out ylinepos);

            for (int i = 0; i < xlinepos.Count; i++)
                xlinepos[i] = GetMinPos(xlinepos[i], xcnt);
            for (int i = 0; i < ylinepos.Count; i++)
                ylinepos[i] = GetMinPos(ylinepos[i], ycnt);
            if (xlinepos.Count != 0 && ylinepos.Count != 0)
            {
                _readrect.X = xlinepos[0];
                _readrect.Y = ylinepos[0];
                _readrect.Width = xlinepos[xlinepos.Count - 1] - _readrect.X;
                _readrect.Height = ylinepos[ylinepos.Count - 1] - _readrect.Y;
            }
            if (_readrect.Left > 10 && _readrect.Top > 10 && _readrect.Right < (_photorect.Right - 5) && _readrect.Bottom < (_photorect.Bottom - 5) &&
                       _readrect.Width > (0.7 * _photorect.Width) && _readrect.Height > 100)
            {
                ComputeDetailRect(xlinepos, ylinepos);
                _xuehaorect.Offset(_photorect.Location);
                _xuanzhetirect.Offset(_photorect.Location);
                _tiankongtirect.Offset(_photorect.Location);
                return  true;
            }           
            return false;
        }
        private void ComputeDetailRect(List<int> xlinepos, List<int> ylinepos)
        {
            _xuehaorect = new Rectangle(); 
            _xuanzhetirect = new Rectangle(); 
            _tiankongtirect = new Rectangle();
	        if( xlinepos.Count==0 || ylinepos.Count==0) return;
            int[] xlinedif,ylinedif;
            xlinedif = new int[xlinepos.Count];
            ylinedif = new int[ylinepos.Count];
	        for(int i=1; i<xlinepos.Count; i++)
                xlinedif[i] = xlinepos[i] - xlinepos[i-1];  //xlinedif[0] = 0
            for(int i=1; i<ylinepos.Count; i++)
                ylinedif[i] = ylinepos[i] - ylinepos[i-1];   //ylinedif[0]
            if (xlinedif[1] >= 5 && xlinedif[1] <= 11 && xlinedif[xlinedif.Length - 1] >= 5 && xlinedif[xlinedif.Length - 1] <= 11 &&
                ylinedif[1] >= 5 && ylinedif[1] <= 11 && ylinedif[ylinedif.Length - 1] >= 5 && ylinedif[ylinedif.Length - 1] <= 11)
            {
                var query = from nd in ylinedif where nd > 4 && nd < 12 select nd;
                int n = query.Count() - 2;
                if (n == 1)
                {
                    for (int i = 2; i < ylinedif.Length - 1; i++)
                    {
                        if (ylinedif[i] > 4 && ylinedif[i] < 12)
                        {
                            _xuehaorect = new Rectangle(xlinepos[1], ylinepos[1],
                                xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[i - 1] - ylinepos[1]);
                            _xuanzhetirect = new Rectangle(xlinepos[1], ylinepos[i],
                                xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[ylinepos.Count - 2] - ylinepos[i]);
                            break;
                        }
                    }
                }
                else if (n == 2)
                {
                    int nagain = 0, xzti = 0;
                    for (int i = 2; i < ylinedif.Length - 1; i++)
                    {
                        if (ylinedif[i] > 4 && ylinedif[i] < 9)
                        {
                            nagain++;
                            if (nagain == 1)
                            {
                                _xuehaorect = new Rectangle(xlinepos[1], ylinepos[1],
                                   xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[i - 1] - ylinepos[1]);
                                xzti = i;
                            }
                            if (nagain == 2)
                            {
                                _xuanzhetirect = new Rectangle(xlinepos[1], ylinepos[xzti],
                                    xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[i - 1] - ylinepos[xzti]);
                                _tiankongtirect = new Rectangle(xlinepos[1], ylinepos[i],
                                    xlinepos[xlinepos.Count - 2] - xlinepos[1], ylinepos[ylinepos.Count - 2 - 1] - ylinepos[i]);
                                break;
                            }
                        }
                    }
                }
            }
        }
        private bool ValidXuehaoLine()
        {
            Rectangle xhgdtr = cfg.GetGdtRectangle( _xuehaorect  ,"xuehao");
            Rectangle xhgdtbaker = xhgdtr;
            //gdt.extend(5);

            xhgdtr.X -= 5;
            xhgdtr.Y -= 5;
            xhgdtr.Width += 10;
            xhgdtr.Height += 10;         
            Gdt( xhgdtr, 2, 1, 25); 
            int[] xuehaoxv = CountImgXBlackCnt(_gdt, xhgdtr);            
            List<int> xuehaolinepos = new List<int>();       
	        xuehaoxv[ xuehaoxv.Length -1 ] = 0;  
            xuehaoxv[ xuehaoxv.Length -2 ] = 0;

            var queryResults =  from n in xuehaoxv  select n;
	        int avg = (int)queryResults.Average() + 4; 
	        for(int i=2; i<xuehaoxv.Length-3; i++){
		        if( xuehaoxv[i] > 2*avg ){
			        if(xuehaoxv[i+3] > 2*avg)  {  
				        if( i+15<xuehaoxv.Length){
                            queryResults =  from n in xuehaoxv  select n;
					        int b2avg = queryResults.Count() ;
					        if( b2avg>8 ){
						        i+= b2avg;
						        continue;
					        }
				        }else{
					        //std::printf(" [i+3]>a*avg "); 
					        return false;
				        }
			        }
			        int maxpos = GetMaxPos(xuehaoxv[i], xuehaoxv[i+1], xuehaoxv[i+2]);
			        if(xuehaolinepos.Count==0 && maxpos + i>20)
				        xuehaolinepos.Add( 5 );	
			        xuehaolinepos.Add( maxpos + i );				
			        i+=2;
		        }
	        }
	        if( xuehaolinepos.Count  < 7 || xuehaolinepos.Count  > 11 ) {
                return false;
	        }
	        bool[] lineflag = new bool[11];
	        int[]  linedif  = new int[11];
	        double  len = (xhgdtr.Width  - 10)/10.0;
	        int linecnt = 0;
	        for( int i=0; i<xuehaolinepos.Count ; i++){
		        int pos =(int) ((xuehaolinepos[i]-5 + 2)/len);
		        int dif =(int) ( xuehaolinepos[i] - pos * len - 5 );
		        linedif[pos] = dif;
		        if( dif==-1 ||  dif ==0 || dif == 1){
			        if(lineflag[pos] == false){
				        lineflag[pos] = true;
				        linecnt++;
			        }else
				        return false;
		        }
	        }
	        if(linecnt < 6 ) {  return false; } 	
            return true;
        }
        private bool ComputeXuehao()
        {
            Rectangle gdtr = cfg.GetGdtRectangle(_xuehaorect, "xuehao");
            int[,] blackcnt = new int[10, 4];
            CountGridOption(_gdt,gdtr, blackcnt);
            int a = 0, b = 0, c = 0;
            Rectangle ra = cfg.Xuehaonum[0], rb = cfg.Xuehaonum[1], rc = cfg.Xuehaonum[2];
            ra.X = rb.X = rc.X = 0;

            if (TestRightOption(blackcnt, ra, ref  a) &&
                TestRightOption(blackcnt, rb, ref  b) &&
                TestRightOption(blackcnt, rc, ref  c))
            {
                xuehao = a * 100 + b * 10 + c;
                return true;
            }
            return false;
        }
        private void ComputXuanzheti()
        {
            Rectangle gdtr = cfg.GetGdtRectangle(_xuanzhetirect, "xuanzheti");
            //gdt.extend(5);
            Rectangle xztbaker = gdtr;
            gdtr.X -= 5;
            gdtr.Y -= 5;
            gdtr.Width += 10;
            gdtr.Height += 10;
            //if(!gdtr.valid()) return ;
            Gdt(gdtr, 3, 1, 15);
            Size g = cfg.Xuanzhetitable;
            int[,] blackcnt = new int[g.Width, g.Height];
            CountGridOption(_gdt, xztbaker, blackcnt);

            Rectangle[] question = cfg.Xuanzhetiquestion;
            selectvalue = new int[question.Length];
            for (int i = 0; i < question.Length; i++)
            {
                Rectangle r = question[i];
                r.X++;
                TestRightOption(blackcnt, r, ref selectvalue[i]);
            }
            //Rectangle r = xztbaker;
            //Rectangle[] q = cfg.Xuanzhetiquestion;
            //if (q.Count() == 0) return;
            //if (q[0].Height == 1)
            //{               
            //    for (int i = 0; i < q.Length; i++)
            //    {
            //        Rectangle nr = new Rectangle(q[i].X * r.Width / g.Width + r.X,
            //            q[i].Y * r.Height / g.Height + r.Y,  
            //           10, 
            //           10);  // r  b
            //        CountGridOption( nr, blackcnt, q[i]); //可以修正 T B
            //    }
            //}
            //else
            //{
            //    CountGridOption(r, blackcnt);
            //} 
        }
        private void DrawOption(Bitmap bmp)
        {
            Rectangle r = cfg.GetGdtRectangle(_xuanzhetirect, "xuanzheti");            
            Size grid = cfg.Xuanzhetitable;
            Rectangle[] question = cfg.Xuanzhetiquestion;
            Answer[] answervalue = cfg.Answer;
            
            int ops = answervalue.Length <question.Length? answervalue.Length:question.Length;
            for(int i=0; i<ops; i++){
                Rectangle qr = question[i];
                if (qr.Width == 1) qr.Y+=answervalue[i].option + 1;
                if (qr.Height == 1) qr.X += answervalue[i].option + 1;               
                Rectangle  nr = new Rectangle((int)( (qr.X+0.5)*r.Width/grid.Width + r.X) - 5, 
                    (int)(  (qr.Y+0.5)*r.Height/grid.Height + r.Y)-5,   10,   10); 
                if( selectvalue[i] == answervalue[i].option){
                    DrawRectEdge(bmp,nr,1,Color.Red);
                    nr.X -= 1; nr.Y -= 1; nr.Width += 2; nr.Height += 2;
                    DrawRectEdge(bmp,nr,1,Color.Red);
                    nr.X -= 1; nr.Y -= 1; nr.Width += 2; nr.Height += 2;
                    DrawRectEdge(bmp,nr,1,Color.Red);
                }else{
                    DrawRectEdge(bmp,nr,1,Color.Yellow);
                    nr.X -= 1; nr.Y -= 1; nr.Width += 2; nr.Height += 2;
                    DrawRectEdge(bmp,nr,1,Color.Yellow);                   
                }
            }
        }
   
        private static void Fill(Bitmap bmp, int color)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //循环处理 
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {                        
                        ptr[0] = ptr[1] = ptr[2] = (byte )color;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
        }
        public  static void Gray(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //循环处理 
            unsafe
            {
                byte temp = 0;
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        // write the logic implementation here 
                        temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                        ptr[0] = ptr[1] = ptr[2] = temp;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
        }
        private static int[] CountImgXBlackCnt(Bitmap bmp, Rectangle  r)
        {
           int[] rv = new int[r.Width ];
           BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           unsafe
           {
               byte* ptr = (byte*)(data.Scan0);
               byte* ptr0 = ptr;
               for (int i = 0; i < data.Height; i++)
               {
                   for (int j = 0; j <data.Width; j++)
                   {
                       if( ptr[0] ==0 ) rv[j]++;
                       ptr += 3;
                   }
                   ptr += data.Stride - data.Width * 3;
               }
           }
           bmp.UnlockBits(data);		
		   return rv;
        }
        private static int  CountRectBlackcnt(Bitmap bmp,Rectangle r)
        {
            int cnt = 0;
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                byte* ptr0 = ptr;
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        if (ptr[0] == 0) cnt++;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);		            
            return cnt;
        }
        private static void CountXPixsum(Bitmap bmp, Rectangle r, out int[] xcnt)
        {
            xcnt = new int[r.Size.Width];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    { // write the logic implementation here 
                        xcnt[j] += ptr[0];
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
            for (int i = 0; i < xcnt.Length; i++)
                xcnt[i] /= 255;
        }
        private static void CountYPixsum(Bitmap bmp, Rectangle r, out int[] ycnt)
        {
            ycnt = new int[r.Size.Height];
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    { // write the logic implementation here 
                        ycnt[i] += ptr[0];
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
            for (int i = 0; i < ycnt.Length; i++)
                ycnt[i] /= 255;
        }
        private static void ComputeLinepos(int[] xcnt, out List<int> linepos)
        {//从0开始
            linepos = new List<int>();
            int[] cnt;
            cnt = new int[xcnt.Length];
            //int b = photorect.Left ,e = photorect.Right ; 
            for (int i = 2; i < xcnt.Length; i++)
            {
                cnt[i] = xcnt[i] - xcnt[i - 2];
            }
            cnt[0] = cnt[1] = 0;

            int[] maxpos, minpos, flag;
            maxpos = new int[xcnt.Length / 20];
            minpos = new int[xcnt.Length / 20];
            flag = new int[xcnt.Length];
            for (int i = 0; i < maxpos.Length; i++)
            {
                maxpos[i] = Max(cnt, i * 20, 20);
                minpos[i] = Min(cnt, i * 20, 20);
            }
            var queryResults = from n in maxpos select n;
            int maxavg = (int)queryResults.Average() + 1;
            queryResults = from n in minpos select n;
            int minavg = (int)queryResults.Average() + 1;
            // if(debug&2 && option&1) std::printf("X-maxavg:%d  minavg:%d\n",maxavg,minavg);
            if (maxavg < 5) maxavg = 5;
            if (minavg > -5) minavg = -5;

            for (int x = 2; x < xcnt.Length - 1; x++)
            {
                if ((cnt[x] > maxavg || cnt[x + 1] > maxavg)
                  && (cnt[x] < -minavg || cnt[x - 1] < -minavg))
                {
                    flag[x] = 1;
                    linepos.Add(x);
                    x++;
                }
            }
        }
        private static int Min(int[] cnt, int bindex, int len)
        {
            int min = cnt[bindex];
            for (int i = 1; i < len; i++)
            {
                if (min > cnt[i + bindex]) min = cnt[i + bindex];
            }
            return min;
        }
        private static int Max(int[] cnt, int bindex, int len)
        {
            int max = cnt[bindex];
            for (int i = 1; i < len; i++)
            {
                if (max < cnt[i + bindex]) max = cnt[i + bindex];
            }
            return max;
        }
        private static int GetMaxPos(int a, int b, int c)
        {
            int max = a > b ? a : b;
            max = max > c ? max : c;
            if (a == max) return 0;
            if (b == max) return 1;
            if (c == max) return 2;
            throw new NotImplementedException();
        }
        private static int GetMinPos(int minpos, int[] cnt)
        {
            if (minpos < 3 || minpos > cnt.Length - 3) return minpos;
            int min = Min(cnt, minpos - 3, 6);
            for (int i = minpos - 3; i < minpos + 4; i++)
                if (min == cnt[i]) return i;
            return 0;
        }
        private static void CountGridOption(Bitmap gdt,Rectangle r, int[,] blackcnt)
        {
            int H = blackcnt.GetLength(1);
	        int W = blackcnt.GetLength(0);            
            ShowForm s = new ShowForm();
	        for(int x=0; x<W; x++){
		        for(int y=0; y<H; y++){
			        int xcent = (int)((x+0.5)*r.Width/W + r.X);
			        int ycent = (int)((y+0.5)*r.Height/H + r.Y);
			        Rectangle nr = new Rectangle(xcent-5, ycent-5, 10, 10);//
			        blackcnt[x,y] = CountRectBlackcnt(gdt,nr);
                    if ((debug & 1024) !=0)
                    {
                        BitmapData data = gdt.LockBits(new Rectangle(0, 0, gdt.Width, gdt.Height),
                                            ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        DrawRect( nr, data,Color.Green);//debug
                        gdt.UnlockBits(data);
                        s.ShowImg(gdt);
                    }
		        }
	        }
        }
        private static bool TestRightOption(int[,] blackcnt, Rectangle r,ref  int value)
        {
            List<int> vcnt=new List<int>();
	        if(r.Width==1){ // 选项向下
		        for(int i=0; i<r.Height; i++) vcnt.Add( blackcnt[r.X,i+r.Y] );
	        }else if(r.Height==1){
		        for(int i=0; i<r.Width; i++) vcnt.Add( blackcnt[i+r.X,r.Y] );
	        }else{ value=-1; return false;}
	
	        int max=vcnt.Max();  
	        int sum=vcnt.Sum();  
	        int cnt=vcnt.Count( n => n>50 );            
            int maxpos = vcnt.FindIndex(n => n==max);                      
	        
            value = maxpos;
	        if(cnt==1){ 
		        return true;
	        }	//单选， 暂时不支持多选
	        if(max>=40 && max > sum - max){
		        return true;
	        }else if(max>=25 && max > 2*(sum-max)){
		        return true;
	        }
	        value = -1;
	        return false;
        }
        public static void DrawRect(Rectangle r, BitmapData data,Color c)
        {
            if (r.X > 0 && r.Right < data.Width && r.Y > 0 && r.Bottom < data.Height
                && r.Width > 2 && r.Width < data.Width && r.Height > 2 && r.Height < data.Height)
            {
                unsafe
                {
                    byte* ptr = (byte*)(data.Scan0);
                    byte* ptrfirst = ptr;
                    //TB
                    ptr = ptrfirst + data.Stride * r.Y + r.X * 3;
                    for (int j = 0; j < r.Width; j++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += 3;
                    }
                    ptr = ptrfirst + data.Stride * r.Bottom + r.X * 3;
                    for (int j = 0; j < r.Width; j++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += 3;
                    }
                    //LR   
                    ptr = ptrfirst + data.Stride * r.Y + r.X * 3;
                    for (int i = 0; i < r.Height; i++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += data.Stride;
                    }
                    ptr = ptrfirst + data.Stride * r.Y + r.Right * 3;
                    for (int i = 0; i < r.Height; i++)
                    {
                        ptr[0] = c.B;
                        ptr[1] = c.G;
                        ptr[2] = c.R;
                        ptr += data.Stride;
                    }
                }
            }
        }
        private static int GetstatisticGamma(Bitmap bmp, int pergamma, Rectangle r)
        {
            int[] colorcnt = new int[256];
            int sum = 0;
            int alltotal = 0;
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                for (int y = 0; y < r.Height; y++)
                {
                    for (int x = 0; x < r.Width; x++)
                    {
                        colorcnt[ptr[0]]++;
                        alltotal++;
                        ptr += 3;
                    }
                    ptr += data.Stride - data.Width * 3;
                }
            }
            bmp.UnlockBits(data);
            for (int i = 0; i < 256; i++)
            {
                sum += colorcnt[i];
                int y = (int)(sum * 1.0 / (alltotal * 1.0 / 100.0));
                if (y >= pergamma)
                    return i;
            }
            return 0;
        }
        private static void GammaImg(Bitmap gdt, Bitmap src, int gamma, Rectangle r)
        {
            BitmapData datagdt = gdt.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData databmp = src.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(databmp.Scan0);
                byte* ptrgdt = (byte*)(datagdt.Scan0);
                for (int y = 0; y < r.Height; y++)
                {
                    for (int x = 0; x < r.Width; x++)
                    {
                        ptrgdt[0] = ptrgdt[1] = ptrgdt[2] = (byte)(ptr[0] > gamma ? 255 : 0);
                        ptr += 3;
                        ptrgdt += 3;
                    }
                    ptr += databmp.Stride - databmp.Width * 3;
                    ptrgdt += datagdt.Stride - datagdt.Width * 3;
                }
            }
            src.UnlockBits(databmp);
            gdt.UnlockBits(datagdt);
        }

        private void Gdt(Rectangle r, int xnum, int ynum, int pergamma)
        {
            Rectangle tempr = new Rectangle();
            for (int xi = 0; xi < xnum; xi++)
            {
                for (int yi = 0; yi < ynum; yi++)
                {
                    tempr.X = r.Left + xi * r.Width / xnum;
                    tempr.Y = r.Top + yi * r.Height / ynum;
                    tempr.Width = r.Width / xnum;
                    tempr.Height = r.Height / ynum;
                    int gamma = 0;
                    if (xi == 0 || xi == xnum - 1)
                    {
                        gamma = GetstatisticGamma(_src, pergamma, tempr);
                    }
                    else
                    {
                        gamma = GetstatisticGamma(_src, pergamma, tempr);
                    }
                    if (gamma >= 240)
                        gamma -= 10;
                    GammaImg(_gdt, _src, gamma, tempr);
                }
            }
        }
        private void cleardata()
        {
            this._src = null;
            this._gdt = null;
            _readrect = new Rectangle();
            _xuehaorect = new Rectangle();
            _xuanzhetirect = new Rectangle();
            _tiankongtirect = new Rectangle();
            selectvalue = null;
            xuehao = 0;
        }
        private void DrawRect(Bitmap src)
        {
            BitmapData data = src.LockBits(new Rectangle(0, 0, src.Width, src.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            DrawRect(_xuehaorect,    data, Color.Red);
            DrawRect(_xuanzhetirect, data, Color.Red);
            DrawRect(_tiankongtirect,data, Color.Red);
            src.UnlockBits(data);
        }
        private void DrawRectEdge(Bitmap bmp, Rectangle r, int border, Color c)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //extendR
            DrawRect(r, data, c);
            bmp.UnlockBits(data);
        }
        private Bitmap _src;
        private Bitmap _gdt;
        private Rectangle _photorect;
        private Rectangle _readrect; //ValidImg
        private Rectangle _xuehaorect;
        private Rectangle _xuanzhetirect;
        private Rectangle _tiankongtirect;
        private int[] selectvalue;
        private int xuehao;
        private CFGForm cfg;
        private static int debug;

    }
}
