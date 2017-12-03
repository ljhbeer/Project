using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using yj.formimg;

namespace yj
{
	public class LoadBitmapData{
		FileStream fs;
		BufferedStream bs;
		private byte[] buffer;
		private int floorid;
		private string bmpdatapath;
		private List<subject>  sublist;
		private subject activesj;
		private Dictionary<string,int> khindex = new Dictionary<string, int>();
		public LoadBitmapData(string bmpdatapath,int floorid,List<subject>  sublist){
			this.floorid = floorid;
			this.bmpdatapath = bmpdatapath;
			this.sublist = sublist;
			this.activesj = null;
			bs = null;
			fs = null;
			Init();
			buffer  = new byte[102400];
		}
		 ~LoadBitmapData(){
			Clear();
		}
		public void Clear(){
			if(bs!=null || fs!=null){
				bs.Flush();
				bs.Close();
	//				fs.Flush();
				fs.Close();
				bs = null;
				fs = null;
			}
		}
		private void Init(){
			List<List<datainfo>> di = new List<List<datainfo>>();
			if(Directory.Exists(bmpdatapath)){
				string difullpath = bmpdatapath+"datainfo[fid].json".Replace("[fid]",floorid.ToString());
				if(File.Exists( difullpath)){
					 string str = File.ReadAllText(difullpath);
					 di = JsonConvert.DeserializeObject<List<List<datainfo>>>(str);
					 khindex.Clear();
					 for(int i=0; i<di.Count; i++)
					 {
					 	if(di[i].Count>0)
					 		sublist[i].BitmapdataLength =(int)( di[i][0].count);
					 }
					 if(di.Count>0)
					 for(int i=0; i<di[0].Count; i++){
					 	string kh = di[0][i].kh;
					 	int index =(int)( di[0][i].startpos / di[0][i].count);
					 	khindex[kh] = index;
					 }
					
				}
			}
			di.Clear();
		}
		public bool  SetActiveSubject(subject activesj){
			if(!sublist.Contains(activesj))
				return false;
			if(this.activesj == null || this.activesj!=activesj ){
				if(this.activesj!=null){
					if(bs!=null || fs!=null){
						bs.Flush();
						bs.Close();
//						fs.Flush();
						fs.Close();
					}						
				}
				string imgdatafilename = bmpdatapath + "roomdata_[roomid].data".Replace("[roomid]",activesj.Subid.ToString());
				fs = new FileStream(imgdatafilename, FileMode.Open, FileAccess.Read);
	            	bs = new BufferedStream(fs, 512000);
				this.activesj = activesj;
			}else if( this.activesj==activesj ){
				// donothing
			}
			return true;
		}
		public Bitmap GetBitmap(string kh){
			int index = khindex[kh];
			int length = activesj.BitmapdataLength;
			Rectangle r = activesj.Rect;

            if (length > buffer.Length)
                buffer = new byte[length];
	        bs.Position = index * length;
	        bs.Read(buffer, 0, buffer.Length);
            Bitmap bmp = new Bitmap(r.Width, r.Height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, r.Width, r.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            IntPtr ptr = bmpData.Scan0; 
            int bytes = bmpData.Stride * bmp.Height; 
            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, ptr, bytes); 
            bmp.UnlockBits(bmpData);
	        return  bmp;
		}
	}
}
