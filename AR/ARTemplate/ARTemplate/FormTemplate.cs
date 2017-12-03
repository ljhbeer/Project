using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using AForge.Math.Geometry;
using MovetoCTL;

namespace ARTemplate
{
    [Flags]
    enum Act : short { None = 0, DefinePoint = 1, DefineId = 2, DefineChoose = 4, DefineUnChoose = 8, Zoomin, Zoomout, SeclectionToWhite, SeclectionToDark, ZoomMouse };
    public delegate void CompleteMouseMove(bool bcompleted);

    public partial class FormTemplate : Form
    {
        public FormTemplate()
        {
            InitializeComponent();
            Init();
            Reset();
        }
        private void Init()
        {
            template = new Template();
            m_tn = new TreeNode();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            zoombox = new ZoomBox();
        }
        private void SetImage(Bitmap image)
        {
            pictureBox1.Image = image;
            ReSetPictureBoxImage();
            MT = new MovetoCTL.MovetoTracker(pictureBox1);
            MT.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            MT.StartDraw(true);
        }
        private void Reset()
        {
            template.Reset();
            m_tn.Nodes.Clear();
            //MT.ClearEvent();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            zoombox.Reset();
            m_act = Act.None;
            treeView1.Nodes.Clear();

            m_tn.Text = "网上阅卷";
            TreeNode[] vt = new TreeNode[6];
            for (int i = 0; i < vt.Count(); i++)
                vt[i] = new TreeNode();
            vt[0].Name = vt[0].Text = "特征点";
            vt[1].Name = vt[1].Text = "考号";
            vt[2].Name = vt[2].Text = "选择题";
            vt[3].Name = vt[3].Text = "非选择题";
            vt[4].Name = vt[4].Text = "选区变白";
            vt[5].Name = vt[5].Text = "选区变黑";
            m_tn.Nodes.AddRange(vt);
            treeView1.Nodes.Add(m_tn);
            treeView1.ExpandAll();
        }
        private void 导入模板IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
            OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            OpenFileDialog2.FileName = "OpenFileDialog2";
            OpenFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            OpenFileDialog2.Title = "Open xml file";
            if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Template tc = new Template();
                if (tc.Load(OpenFileDialog2.FileName))
                {
                    template = tc;
                    RefreshTemplate();
                    ////////////////////////////////////////
                    MT = new MovetoTracker(pictureBox1);
                    MT.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
                }
            }
        }
        private void 导出模板OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.FileName = "saveFileDialog2";
            saveFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            saveFileDialog2.Title = "Save xml file";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    UpdateTemplate();
                    template.Save(saveFileDialog2.FileName);
                }
                catch
                {
                    MessageBox.Show("Failed loading selected image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void 导入图片IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
            if (!template.CheckEmpty())
            {
                DialogResult dr = MessageBox.Show("当前模板还有数据，导入图片需要清除原有数据，是否清除", "清除模板", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    template.Reset();
                    RefreshTemplate();
                }
                else
                {
                    return;
                }
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap  bitmap_src =(Bitmap) Bitmap.FromFile(openFileDialog1.FileName);                      
                    SetImage(bitmap_src);                  
                    template.SetImagePath(openFileDialog1.FileName);
                    template.SetImageSize(pictureBox1.Image.Size);
                }
                catch
                {
                    MessageBox.Show("导入图片失败，重新选择", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    template.Reset();
                }
            }
        }
        private void 定义考号KToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonDId.PerformClick();
        }
        private void 定义特征点TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonDP.PerformClick();
        }
        private void 定义选择题XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonDX.PerformClick();
        }
        private void 定义非选择题FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonDF.PerformClick();
        }
        private void toolStripMenuItemExportImg_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;
            Brush dark = Brushes.Black;
            Brush white = Brushes.White;
            Image img = (Image)pictureBox1.Image.Clone();


            if (IsPixelFormatIndexed(img.PixelFormat))
            {
                Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.DrawImage(img,0,0,img.Width,img.Height);
                }
                img = bmp;
            }
            {
                Graphics g = Graphics.FromImage(img);
                bool bchange = false;
                foreach (TreeNode t in m_tn.Nodes["选区变黑"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        bchange = true;
                        Rectangle r = (Rectangle)t.Tag;
                        g.FillRectangle(dark, r);
                    }
                }
                foreach (TreeNode t in m_tn.Nodes["选区变白"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        bchange = true;
                        Rectangle r = (Rectangle)t.Tag;
                        g.FillRectangle(white, r);
                    }
                }

                ///////////
                if (!bchange)
                {
                    MessageBox.Show("图片没有改动");
                    return;
                }
                SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                saveFileDialog2.FileName = "导出修改后的图片";
                saveFileDialog2.Filter = "JPEG files (*.jpg)|*.jpg";
                saveFileDialog2.Title = "导出修改后的图片";
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    img.Save(saveFileDialog2.FileName);
                }
            }
        }
        private void toolStripButtonWhite_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.SeclectionToWhite;
            toolStripButton_Click(sender, e);
        }

        private void toolStripButtonToDark_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.SeclectionToDark;
            toolStripButton_Click(sender, e);
        }

        private void toolStripButtonZoomin_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
            Zoomrat(1.1, new Point(pictureBox1.Width / 2, pictureBox1.Height / 2));
        }
        private void toolStripButtonZoomMouse_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
            m_act = Act.ZoomMouse;
        }
        private void toolStripButtonZoomout_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
            Zoomrat(0.9, new Point(pictureBox1.Width / 2, pictureBox1.Height / 2));
        }
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton b in toolStrip1.Items)
                b.Checked = false;
            ToolStripButton click = (ToolStripButton)sender;
            if (click.Checked)
                m_act = Act.None;
            ShowMessage("act:" + m_act);
            click.Checked = !click.Checked;
            //MT.ClearEvent();
            if (m_act == Act.DefinePoint || m_act == Act.DefineId ||
                m_act == Act.DefineChoose || m_act == Act.DefineUnChoose||
                m_act == Act.SeclectionToDark || m_act == Act.SeclectionToWhite )
            {
                MT.StartDraw(true);
                //MT.completevent += CompleteSelection;
            }
        }
        private void toolStripButtonDP_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.DefinePoint;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonDId_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.DefineId;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonDX_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.DefineChoose;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonDF_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.DefineUnChoose;
            toolStripButton_Click(sender, e);
        }
       
        private void CompleteSelection(bool bcomplete)
        {
            if (bcomplete)
            {
                ShowMessage("Complete: " + m_act);
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                switch (m_act)
                {
                    case Act.DefinePoint: CompleteDeFinePoint(); break;
                    case Act.DefineChoose: CompleteDeFineChoose(); break;
                    case Act.DefineUnChoose: CompleteDeFineUnChoose(); break;
                    case Act.DefineId: CompleteDeFineId(); break;
                    case Act.SeclectionToWhite: CompleteSelectionToWhite(); break;
                    case Act.SeclectionToDark: CompleteSelectionToDark(); break;
                }
            }
            pictureBox1.Invalidate();
        }

        private void toolStripButtonZoomNone_Click(object sender, EventArgs e)
        {
            //
        }
       
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            if (MT != null && MT.Selection.Width > 3)
            {
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                CompleteSelection(true);
            }
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if(m_act == Act.ZoomMouse)
            pictureBox1.Focus();
        }
        private void pictureBox1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            double f = 0.0;
            if (numberOfTextLinesToMove > 0)
            {
                for (int i = 0; i < numberOfTextLinesToMove; i++)
                {
                    f += 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
            else if (numberOfTextLinesToMove < 0)
            {
                for (int i = 0; i > numberOfTextLinesToMove; i--)
                {
                    f -= 0.05;
                }
                Zoomrat(f + 1, e.Location);
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null && m_tn.Nodes.Count != 0)
            {
                Pen pen = Pens.Red;
                Brush dark = Brushes.Black;
                Brush white = Brushes.White;
                Brush Red = Brushes.Red;
                Font font = DefaultFont;
                foreach (TreeNode t in m_tn.Nodes["特征点"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        ISelectionInterface I = (ISelectionInterface)(t.Tag);
                        e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(I.ImgSelection()));
                        if (I.HasSubSelection())
                        {
                            foreach (Rectangle r in I.ImgSubSelection())
                            {
                                e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(r));
                            }
                        }
                    }
                }
                foreach (TreeNode t in m_tn.Nodes["考号"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        ISelectionInterface I = (ISelectionInterface)(t.Tag);
                        e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(I.ImgSelection()));
                        if (I.HasSubSelection())
                        {
                            foreach (Rectangle r in I.ImgSubSelection())
                            {
                                r.Offset(I.ImgSelection().Location);
                                e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(r));
                            }
                        }
                    }
                }
                foreach (TreeNode t in m_tn.Nodes["选择题"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        ISelectionInterface I = (ISelectionInterface)(t.Tag);
                        e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(I.ImgSelection()));
                        if (I.HasSubSelection())
                        {
                            foreach (Rectangle r in I.ImgSubSelection())
                            {
                                r.Offset(I.ImgSelection().Location);
                                e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(r));
                            }
                        }
                    }
                }
                foreach (TreeNode t in m_tn.Nodes["非选择题"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        ISelectionInterface I = (ISelectionInterface)(t.Tag);
                        e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(I.ImgSelection()));
                    }
                }

                foreach (TreeNode t in m_tn.Nodes["选区变黑"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        Rectangle r = zoombox.ImgToBoxSelection((Rectangle)t.Tag);
                        e.Graphics.FillRectangle(dark,r );
                        e.Graphics.DrawString(t.Name, font, Red, r.Location);
                    }
                }
                foreach (TreeNode t in m_tn.Nodes["选区变白"].Nodes)
                {
                    if (t.Tag != null)
                    {
                        Rectangle r = zoombox.ImgToBoxSelection((Rectangle)t.Tag);
                        e.Graphics.FillRectangle(white,r);
                        e.Graphics.DrawString(t.Name, font, Red, r.Location);
                    }
                }
            }
        }
        private void buttonzoomout_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;
            Point p = new Point(panel1.Width / 2, panel1.Height / 2);
            p.Offset(-pictureBox1.Location.X, -pictureBox1.Location.Y);
            Zoomrat(1.1F, p);
        }
        private void buttonzoomin_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;
            Point p = new Point(panel1.Width / 2, panel1.Height / 2);
            p.Offset(-pictureBox1.Location.X, -pictureBox1.Location.Y);
            Zoomrat(0.9F, p);
        }
        private void Zoomrat(double rat, Point e)
        {
            Bitmap  bitmap_show =(Bitmap )pictureBox1.Image;
            Point L = pictureBox1.Location;
            Point S = panel1.AutoScrollPosition;
            int w = (int)(pictureBox1.Width * rat);
            int h = w * bitmap_show.Height / bitmap_show.Width;
            L.Offset((int)(e.X * (rat - 1)), (int)(e.Y * (rat - 1)));
            pictureBox1.SetBounds(S.X, S.Y, w, h);
            zoombox.UpdateBoxScale(pictureBox1);

            S.Offset((int)(e.X * (1 - rat)), (int)(e.Y * (1 - rat)));
            panel1.Invalidate();
            panel1.AutoScrollPosition = new Point(-S.X, -S.Y);
        }
        private void ReSetPictureBoxImage()
        {
            Bitmap bitmap_show = (Bitmap)pictureBox1.Image;
            crop_startpoint.X = crop_startpoint.Y = 0;
            int width = pictureBox1.Height * bitmap_show.Width / bitmap_show.Height;
            int height = pictureBox1.Width * bitmap_show.Height / bitmap_show.Width;
            if (pictureBox1.Height > height)
                pictureBox1.Height = height;
            if (pictureBox1.Width > width)
                pictureBox1.Width = width;
            // pictureBox1.Image = bitmap_show;
            zoombox.UpdateBoxScale(pictureBox1);
            pictureBox1.Invalidate();
        }

        /// <summary>
        /// ///////////////////////未完成
        /// </summary>
        private void CompleteDeFineId()
        {
            String keyname = "考号";
            //if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                t.Name = t.Text = keyname + cnt;
                string choosename = "";
                float count = 0;
                if (InputBox.Input("设置考号", "考号排列横1纵2", ref choosename, "考号位数", ref count))
                {//仅支持 横向
                    Bitmap bitmap = ((Bitmap)(pictureBox1.Image)).Clone(m_Imgselection, pictureBox1.Image.PixelFormat);
                    DetectChoiceArea dca = new DetectChoiceArea(bitmap, (int)count);
                    Boolean bvertical = false;
                    if (choosename == "1")
                        bvertical = false;
                    else if (choosename == "2")
                        bvertical = true;
                    else
                    {
                        MessageBox.Show("错误参数");
                        return;
                    }

                    if (dca.DetectKH(bvertical))
                    {
                        t.Name = t.Text = "考号";
                        t.Tag = new KaoHaoChoiceArea(m_Imgselection,
                            t.Name, dca.Choicepoint, dca.Choicesize);
                        m_tn.Nodes[keyname].Nodes.Add(t);
                        //pictureBox1.Invalidate();
                    }
                    else
                    {
                        bitmap.Save("f:\\" + choosename + ".jpg");
                    }
                }
            }
        }
        private void CompleteDeFinePoint()
        {
            String keyname = "特征点";
            if (!ExistDeFineSelection(keyname))
            {
                List<Point> corners = new List<Point>();
                Bitmap bmp = (Bitmap)pictureBox1.Image;
                Image cropimg = bmp.Clone(m_Imgselection, bmp.PixelFormat);
                Bitmap img = ConvertFormat.Convert((Bitmap)cropimg, PixelFormat.Format8bppIndexed, true);
                if (CheckImageRectangledTriangle(img, out corners))
                {
                    TreeNode t = new TreeNode();
                    int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                    t.Name = t.Text = keyname + cnt;
                    TriAngleFeature tf = new TriAngleFeature(corners, m_Imgselection.Location);
                    t.Tag = tf;
                    m_Imgselection = tf.ImgSelection();
                    m_tn.Nodes[keyname].Nodes.Add(t);
                }
            }
        }
        private void CompleteDeFineChoose()
        {
            String keyname = "选择题";
            if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                t.Name = t.Text = keyname + cnt;
                string choosename = "";
                float count = 0;
                if (InputBox.Input("设置选择题", "标题", ref choosename, "小题数", ref count))
                {//仅支持 横向
                    Bitmap bitmap = ((Bitmap)(pictureBox1.Image)).Clone(m_Imgselection, pictureBox1.Image.PixelFormat);
                    DetectChoiceArea dca = new DetectChoiceArea(bitmap, (int)count);
                    if (dca.Detect())
                    {
                        t.Name = t.Text = choosename;
                        t.Tag = new SingleChoiceArea(m_Imgselection,
                            t.Name, dca.Choicepoint, dca.Choicesize);
                        m_tn.Nodes[keyname].Nodes.Add(t);
                        //pictureBox1.Invalidate();
                    }
                    else
                    {
                        bitmap.Save("f:\\" + choosename + ".jpg");
                    }
                }
            }
        }
        private void CompleteDeFineUnChoose()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "非选择题";
            if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string unchoosename = keyname + cnt;
                float score = 1;
                if (InputBox.Input("设置非选择题", "标题", ref unchoosename, "分值", ref score))
                {
                    String name = unchoosename;
                    t.Name = unchoosename;
                    t.Text = unchoosename + "(" + score + "分)";
                    t.Tag = new UnChoose(score, name, m_Imgselection);
                    m_tn.Nodes[keyname].Nodes.Add(t);
                }
            }
        }
        private void CompleteSelectionToDark()
        {
            String keyname = "选区变黑";
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            //if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string stringname = keyname + cnt;
                t.Tag = m_Imgselection;
                t.Name = cnt.ToString();
                t.Text = stringname;
                m_tn.Nodes[keyname].Nodes.Add(t);
            }
        }
        private void CompleteSelectionToWhite()
        {
            String keyname = "选区变白";
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            //if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string stringname = keyname + cnt;
                t.Tag = m_Imgselection;
                t.Name = cnt.ToString();
                t.Text = stringname;
                m_tn.Nodes[keyname].Nodes.Add(t);
            }
        }      
        private bool ExistDeFineSelection(String keyname)
        {
            Rectangle rect = m_Imgselection;
            int reduceH = rect.Height / 8;
            rect.Y += reduceH;
            rect.Height -= reduceH * 2;
            if (keyname == "特征点")
            {
                foreach (TreeNode t in m_tn.Nodes[keyname].Nodes)
                {
                    if (t.Tag != null)
                    {
                        if (((TriAngleFeature)(t.Tag)).IntersectsWith(rect))
                            return true;
                    }
                }
            }
            else if (keyname == "选择题")
            {
                foreach (TreeNode t in m_tn.Nodes[keyname].Nodes)
                {
                    if (t.Tag != null)
                    {
                        if (((SingleChoiceArea)(t.Tag)).IntersectsWith(rect))
                            return true;
                    }
                }

            }
            else if (keyname == "非选择题")
            {
                foreach (TreeNode t in m_tn.Nodes[keyname].Nodes)
                {
                    if (t.Tag != null)
                    {
                        if (((UnChoose)(t.Tag)).IntersectsWith(rect))
                            return true;
                    }
                }
            }
            return false;
        }
        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (treeView1.SelectedNode.Parent.Text != "网上阅卷")
                {
                    TreeNode t = treeView1.SelectedNode.NextNode;
                    if (t == null)
                        t = treeView1.SelectedNode.PrevNode;
                    treeView1.SelectedNode.Remove();
                    treeView1.SelectedNode = t;
                }
                pictureBox1.Invalidate();
            }
        }

        private void RefreshTemplate()
        {
            m_act = Act.None;
            //MT.ClearEvent();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            for (int i = 0; i < m_tn.Nodes.Count; i++)
                m_tn.Nodes[i].Nodes.Clear();
            template.SetDataToNode(m_tn);
            if (template.Filename != null)
            {
                pictureBox1.Image = Bitmap.FromFile(template.Filename);
            }
            else
            {
                pictureBox1.Image = null;
            }
            zoombox.UpdateBoxScale(pictureBox1);
        }
        private void UpdateTemplate()
        {
            template.ResetData();
            foreach (TreeNode n in m_tn.Nodes["考号"].Nodes)
            {
                if (n.Tag != null)
                {
                    template.AddKaoHaoSingleChoiceArea((KaoHaoChoiceArea)n.Tag);
                }
            }
            foreach (TreeNode n in m_tn.Nodes["选择题"].Nodes)
            {
                if (n.Tag != null)
                {
                    template.AddSingleChoiceArea((SingleChoiceArea)n.Tag);
                }
            }
            foreach (TreeNode n in m_tn.Nodes["非选择题"].Nodes)
            {
                if (n.Tag != null)
                {
                    template.AddUnChoose((UnChoose)n.Tag);
                }
            }
            if (m_tn.Nodes["特征点"].Nodes.Count >= 3)
            {
                TriAngleFeature p0, p1, p2;
                p0 = p1 = p2 = null;
                foreach (TreeNode n in m_tn.Nodes["特征点"].Nodes)
                {
                    if (n.Tag != null)
                    {
                        TriAngleFeature t = (TriAngleFeature)n.Tag;
                        if (t.Direction == 0)
                            p0 = t;
                        else if (t.Direction == 1)
                            p1 = t;
                        else if (t.Direction == 2)
                            p2 = t;
                    }
                }
                if (p0 == null || p1 == null || p2 == null || template == null)
                    return;
                template.AddFeaturePoints(p0, p1, p2);
            }
        }
        private void ShowMessage(string message)
        {
            textBoxMessage.Text = message;
        }
        private static bool CheckImageRectangledTriangle(Bitmap bitmap, out List<System.Drawing.Point> drawcorners)
        {
            // lock image
            List<AForge.IntPoint> corners;
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);
            // step 1 - turn background to black
            unsafe
            {
                byte* ptr = (byte*)(bitmapData.Scan0);
                for (int i = 0; i < bitmapData.Height; i++)
                {
                    for (int j = 0; j < bitmapData.Width; j++)
                    {
                        if (*ptr == 0)
                            *ptr = 255;
                        else *ptr = 0;
                        ptr++;
                    }
                    ptr += bitmapData.Stride - bitmapData.Width;
                }
            }
            // step 2 - locating objects
            AForge.Imaging.BlobCounter blobCounter = new AForge.Imaging.BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;
            blobCounter.ProcessImage(bitmapData);
            AForge.Imaging.Blob[] blobs = blobCounter.GetObjectsInformation();
            bitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<AForge.IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                if (shapeChecker.IsTriangle(edgePoints, out corners))
                {
                    PolygonSubType subType = shapeChecker.CheckPolygonSubType(corners);
                    if (subType == PolygonSubType.RectangledTriangle || subType == PolygonSubType.RectangledIsoscelesTriangle)
                    {
                        drawcorners = new List<System.Drawing.Point>();
                        foreach (AForge.IntPoint p in corners)
                            drawcorners.Add(new System.Drawing.Point(p.X, p.Y));
                        return true;
                        //MessageBox.Show("triangle" + corners[0] + corners[1] + corners[2]);
                        //g.DrawPolygon(greenPen, ToPointsArray(corners));
                    }
                }
            }
            drawcorners = null;
            return false;
        }

        private Rectangle m_Imgselection;
        private TreeNode m_tn;
        private MovetoTracker MT;
        private Act m_act;
        private Point crop_startpoint;
        private ZoomBox zoombox;
        private Template template;



        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare, PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed, PixelFormat.Format8bppIndexed };

        /// <summary>
        /// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
        /// 无法从带有索引像素格式的图像创建graphics对象
        /// </summary>
        /// <param name="imgPixelFormat">原图片的PixelFormat</param>
        /// <returns></returns>
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }

     
    }
}
