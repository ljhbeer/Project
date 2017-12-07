using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
//using AForge.Math.Geometry;
using MovetoCTL;
using ScanTemplate;

namespace ARTemplate
{
    [Flags]
    enum Act : short { None = 0, DefinePoint = 1, DefineId = 2, DefineChoose = 4, DefineUnChoose = 8,DefineName=16, Zoomin, Zoomout, SeclectionToWhite, SeclectionToDark, ZoomMouse };
    public delegate void CompleteMouseMove(bool bcompleted);

    public partial class FormTemplate : Form
    {
        public FormTemplate( Template t)
        {
            InitializeComponent();
            template = t;
            Init();
            Reset();
            template.SetDataToNode(m_tn);
        }
        private void Init()
        {
            m_tn = new TreeNode();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            zoombox = new ZoomBox();
        }
        private void FormTemplate_Load(object sender, EventArgs e)
        {
            SetImage(template.Image);
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
            //template.ResetData(false);
            m_tn.Nodes.Clear();
            //MT.ClearEvent();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            _OriginWith = pictureBox1.Width;
            zoombox.Reset();
            m_act = Act.None;
            treeView1.Nodes.Clear();


            m_tn.Text = "网上阅卷";
            TreeNode[] vt = new TreeNode[7];
            for (int i = 0; i < vt.Count(); i++)
                vt[i] = new TreeNode();
            vt[0].Name = vt[0].Text = "特征点";
            vt[1].Name = vt[1].Text = "姓名";
            vt[2].Name = vt[2].Text = "考号";
            vt[3].Name = vt[3].Text = "选择题";
            vt[4].Name = vt[4].Text = "非选择题";
            vt[5].Name = vt[5].Text = "选区变白";
            vt[6].Name = vt[6].Text = "选区变黑";
            m_tn.Nodes.AddRange(vt);
            treeView1.Nodes.Add(m_tn);
            treeView1.ExpandAll();
        }       

        private void toolStripButtonSaveTemplate_Click(object sender, EventArgs e)
        {
        	UpdateTemplate();
        	FileInfo fi = new FileInfo(template.Filename);        	
        	string path = fi.Directory.Parent.Parent.FullName + "\\template\\";
        	if(!Directory.Exists(path))
        		Directory.CreateDirectory(path);
        	string filename = path+fi.Directory.Name+"_"+template.GetTemplateName()+".xml";
       
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.FileName = filename;
            saveFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            saveFileDialog2.Title = "Save xml file";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    template.Save(saveFileDialog2.FileName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void toolStripButtonImportTemplate_Click(object sender, EventArgs e)
        {

            OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            OpenFileDialog2.FileName = "OpenFileDialog2";
            OpenFileDialog2.Filter = "Xml files (*.xml)|*.xml";
            OpenFileDialog2.Title = "Save xml file";
            if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (template.Load(OpenFileDialog2.FileName))
                    {
                        RefreshTemplate();
                        pictureBox1.Invalidate();
                    }
                }
                catch
                {
                    MessageBox.Show("模板加载失败", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            toolStripButton_Click(sender, e);
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
            if (m_act == Act.DefinePoint || m_act == Act.DefineId || m_act == Act.DefineName||
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
        private void toolStripButtonName_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.DefineName;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonZoomNone_Click(object sender, EventArgs e)
        {
            // zoombox.Reset();
            if (!((ToolStripButton)sender).Checked)
                ((ToolStripButton)sender).Checked = false;
            m_act = Act.None;
            double rat =   _OriginWith / zoombox.ImageWith(pictureBox1) ; 
            Zoomrat(rat, new Point(pictureBox1.Width / 2, pictureBox1.Height / 2));
        }
        private void toolStripButtonCloseAndOutImages_Click(object sender, EventArgs e)
        {
        	UpdateTemplate();
            this.Close();
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
                    case Act.DefineName: CompleteDeFineName(); break;
                }
            }
            pictureBox1.Invalidate();
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
                foreach (string s in new string[] { "特征点", "考号","姓名", "选择题", "非选择题", "选区变黑", "选区变白" })
                {
                    foreach (TreeNode t in m_tn.Nodes[s].Nodes)
                    {
                        if (t.Tag != null)
                        {
                            Area I = (Area)(t.Tag);
                            e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(I.ImgArea));
                            if (I.HasSubArea())
                            {
                                foreach (Rectangle r in I.ImgSubArea())
                                {
                                    r.Offset(I.ImgArea.Location);
                                    e.Graphics.DrawRectangle(pen, zoombox.ImgToBoxSelection(r));
                                }
                            }
                            if (I.NeedFill())
                            {
                                e.Graphics.FillRectangle(I.FillPen(), zoombox.ImgToBoxSelection(I.ImgArea));
                                e.Graphics.DrawString(t.Name, font, Red, zoombox.ImgToBoxSelection(I.ImgArea).Location);
                            }
                        }
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
            _OriginWith = zoombox.ImageWith(pictureBox1);
            pictureBox1.Invalidate();
        }

        private void CompleteDeFineId()
        {
            //TODO: 考号  实现条形码，未实现 填涂
            String keyname = "考号";
            int cnt = m_tn.Nodes[keyname].GetNodeCount(false) ;
            if (!ExistDeFineSelection(keyname))
            {
                if (cnt == 0)
                {
                    Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                    TreeNode t = new TreeNode();
                    t.Name = t.Text = keyname;
                    t.Tag = new KaoHaoChoiceArea( m_Imgselection,t.Name,"条形码");
                    m_tn.Nodes[keyname].Nodes.Add(t);
                }
                else
                {
                    MessageBox.Show("请先删除，考号区域只有一项");
                }
            }
        }
        private void CompleteDeFinePoint()
        {//TODO: "特征点"
            String keyname = "特征点";
            int cnt = m_tn.Nodes[keyname].GetNodeCount(false);
            if (cnt >= 3)
            {
                MessageBox.Show("只能有3个特征点");
                return;
            }
            if (!ExistDeFineSelection(keyname) )
            {
                Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                Bitmap bmp = (Bitmap)pictureBox1.Image;
                m_Imgselection.Intersect( new Rectangle(0,0,bmp.Width,bmp.Height));
                Image img = bmp.Clone(m_Imgselection, bmp.PixelFormat);
                //Bitmap img = ConvertFormat.Convert((Bitmap)cropimg, PixelFormat.Format8bppIndexed, true);
                MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(null);
                Rectangle r = dr.Detected(new Rectangle(0, 0, img.Width, img.Height), (Bitmap)img);               
                if (r.Width > 0 )
                {
                    TreeNode t = new TreeNode(); 
                    cnt++ ;
                    t.Name = t.Text = keyname + cnt;
                    r.Offset(m_Imgselection.Location);
                    // FeaturePoint(r, new Point(0, 0));
                    t.Tag = new FeaturePoint(r, new Point(bmp.Width/2, bmp.Height/2));
                    //m_Imgselection = tf.ImgSelection();
                    m_tn.Nodes[keyname].Nodes.Add(t);
                }
            }
        }
        private void CompleteDeFineName()
        {
             String keyname = "姓名";
             TreeNode t = new TreeNode();
             int cnt = m_tn.Nodes[keyname].GetNodeCount(false);
             if (cnt == 0)
             {
                 t.Name = t.Text = keyname;
                 Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                 t.Tag = new NameArea(m_Imgselection);
                 m_tn.Nodes[keyname].Nodes.Add(t);
             }
             else
             {
                 MessageBox.Show("请先删除，姓名区域只有一项");
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
                    Bitmap bitmap = ((Bitmap)(pictureBox1.Image)).Clone(m_Imgselection, PixelFormat.Format24bppRgb);
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        foreach (string s in new string[] { "选区变黑", "选区变白" })
                            foreach (TreeNode tt in m_tn.Nodes[s].Nodes)
                                if (tt.Tag != null)
                                {
                                    Area I = (Area)(tt.Tag);
                                    Rectangle r = I.ImgArea;
                                    r.Intersect(m_Imgselection);
                                    if (r.Width > 0)
                                    {
                                        r.Offset(-m_Imgselection.X, -m_Imgselection.Y);
                                        g.FillRectangle(Brushes.Black, r);
                                    }
                                }
                    }

                    DetectChoiceArea dca = new DetectChoiceArea(bitmap, (int)count);
                    if (dca.Detect())
                    {
                        t.Name = t.Text = choosename;
                        t.Tag = new SingleChoiceArea(m_Imgselection,
                            t.Name, dca.Choicepoint, dca.Choicesize);
                        m_tn.Nodes[keyname].Nodes.Add(t);
                    }
                    //else 
                    //{
                    //    bitmap.Save("f:\\" + choosename + ".jpg");
                    //}
                }
            }
        }
        private void CompleteDeFineUnChoose()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "非选择题";
            //if (!ExistDeFineSelection(keyname))
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
                t.Tag = new TempArea(m_Imgselection, stringname);
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
                t.Tag = new TempArea(m_Imgselection, stringname);
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
            if (keyname == "特征点" || keyname == "选择题" || keyname =="非选择题")
            {
                foreach (TreeNode t in m_tn.Nodes[keyname].Nodes)
                {
                    if (t.Tag != null)
                    {
                        if (((Area)(t.Tag)).IntersectsWith(rect))
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
            if (pictureBox1.Image == null)
            {
                pictureBox1.Image = template.Image;
            }         
            zoombox.UpdateBoxScale(pictureBox1);
        }
        private void UpdateTemplate()
        {
            template.ResetData();
            // "特征点", "考号","姓名", "选择题", "非选择题", "选区变黑", "选区变白" 
            foreach (TreeNode t in m_tn.Nodes)
            {
                foreach (TreeNode n in m_tn.Nodes[t.Name].Nodes)
                {
                    if (n.Tag != null)
                    {
                        template.AddArea((Area)n.Tag,t.Name);
                    }
                }
            }
        }
        private void ShowMessage(string message)
        {
            textBoxMessage.Text = message;
        }

        private Rectangle m_Imgselection;
        private TreeNode m_tn;
        private MovetoTracker MT;
        private Act m_act;
        private Point crop_startpoint;
        private ZoomBox zoombox;
        private Template template;
        private double _OriginWith;

    }
}
