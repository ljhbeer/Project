using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Camera
{
    public partial class Camera : Form
    {
        private object obj = new object();

        WebCamera camera;
        AR ar;
        DeviceCapabilityInfo _DeviceCapabilityInfo;
        DeviceInfo _DeviceInfo;
        private CFGForm cfg;
        private int capframe;
        private int debug;
        private Bitmap imgclone;
        public delegate void myDelegate(int anInteger, string aString);
        public delegate void SetCameraDelegate(Image img);
        public void SetCamera(Image img)
        {
            if (checkBoxImgRotate.Checked)
                img.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            imgclone = (Bitmap)img.Clone();

            Bitmap src = (Bitmap)img;
            BitmapData data = src.LockBits(new Rectangle(0, 0, src.Width, src.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            AR.DrawRect(cfg.Photorect, data,Color.Green);           
            src.UnlockBits(data); 
            
            pictureBox1.Image = src ;
        }
        public void SetText(int anInteger, string aString)
        {
            if (anInteger == 1)
                textBoxState.Text = aString;
            else if (anInteger == 2)
                textBoxR.Text = aString;
        }
        public Camera()
        {
            debug = 0;
            bcapture = false;
            capframe = 3;
            framecnt = 0;
            cfg = new CFGForm();
            ar = new AR();
            ar.Photorect = cfg.Photorect;
            ar.Cfg = cfg;
            InitializeComponent();
            textBoxPX.Text = cfg.Photorect.X.ToString();
            textBoxPY.Text = cfg.Photorect.Y.ToString();
            textBoxPWidth.Text = cfg.Photorect.Width.ToString();
            textBoxPHeight.Text = cfg.Photorect.Height.ToString();
            camera = new WebCamera();
            foreach (DeviceInfo info in camera.GetCameras())
            {
                comboBox1.Items.Add(info);
            }
            camera.NewFrameEvent += new NewFrameEventHandler(camera_NewFrameEvent);
        }
        void camera_NewFrameEvent(object sender, EventArgs e)
        {
            this.Invoke(new SetCameraDelegate(SetCamera), new object[] { camera.NewFrame });
            string str = framecnt.ToString() + " " + capframe.ToString() + bcapture.ToString() + ar.GetRectString();
            textBoxState.Invoke(new myDelegate(SetText), new object[] { 1, str });
            framecnt++;
            framecnt %= 8192;
            if (bcapture)
            {
                long nowframe = framecnt % capframe;
                if (capframe == 15)
                {
                    if (nowframe == 13 || nowframe == 0)
                        pictureBox2.Image = null;
                }
                if (nowframe == 0)
                {
                    capframe = 5;
                    if (ar.TestImg(imgclone))
                    {
                        pictureBox2.Image = ar.FinishedBmp;
                        //判断是否Save 
                        //ar.Save(path);
                        //ar.save(result)
                    }
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            _DeviceCapabilityInfo = null;
            _DeviceInfo = (DeviceInfo)comboBox1.SelectedItem;
            foreach (DeviceCapabilityInfo info in camera.GetDeviceCapability(_DeviceInfo))
            {
                comboBox2.Items.Add(info);
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _DeviceCapabilityInfo = (DeviceCapabilityInfo)comboBox2.SelectedItem;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (_DeviceInfo != null && _DeviceCapabilityInfo != null)
            {
                if (camera.StartVideo(_DeviceInfo, _DeviceCapabilityInfo))
                    button2.Enabled = true;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (camera.DeviceExist)
            {
                if (camera.CloseVideo())
                    button2.Enabled = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bcapture = !bcapture;
            if (bcapture)
            {
                framecnt = 0;
                button2.Text = "正在截图";
            }
            else
            {
                button2.Text = "完成截图";
            }
        }
        private void Camera_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (camera.DeviceExist)
                camera.CloseVideo();
            this.Dispose();
        }
        private void buttonApply_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(textBoxPX.Text);
            int y = Convert.ToInt32(textBoxPY.Text);
            int w = Convert.ToInt32(textBoxPWidth.Text);
            int h = Convert.ToInt32(textBoxPHeight.Text);
            if (x > 5 && y > 5 && w > 400 && h > 100 && x + w < 630 && y + h < 450)
            {
                ar.Photorect = new Rectangle(x, y, w, h);
            }
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;
        }
        private void openimg_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.Filter = "所有图像文件 | *.bmp; *.pcx; *.png; *.jpg; *.gif;" +
                "*.tif; *.ico; *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf|" +
                "位图( *.bmp; *.jpg; *.png;...) | *.bmp; *.pcx; *.png; *.jpg; *.gif; *.tif; *.ico|" +
                "矢量图( *.wmf; *.eps; *.emf;...) | *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf";
            opnDlg.Title = "打开图像文件";
            opnDlg.ShowHelp = true;
            Bitmap curBitmap = null;
            if (opnDlg.ShowDialog() == DialogResult.OK)
            {
                string curFileName = opnDlg.FileName;
                try
                {
                    curBitmap = (Bitmap)Image.FromFile(curFileName);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            if (curBitmap != null)
            {
                Image imgload = curBitmap;
                if (curBitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    imgload = new Bitmap(curBitmap.Width, curBitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    Graphics g1 = Graphics.FromImage(imgload);
                    g1.DrawImageUnscaled(curBitmap, 0, 0);
                }
                SetCamera(imgload);
                if (ar.TestImg(imgclone))
                {
                    pictureBox2.Image = ar.FinishedBmp;                    
                }
            }

        }
        private void btncfg_Click(object sender, EventArgs e)
        {
            cfg.ShowDialog();
        }
        private void textBoxdebug_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxdebug.Text;
            foreach(char c in text ) 
            {
                if (!Char.IsNumber(c))
                {
                    textBoxdebug.Text = "";
                    return;
                }
            }
            debug = Convert.ToInt32(text);
            ar.Debug = debug;
        }
    }
}
