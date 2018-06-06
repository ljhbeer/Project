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
using Tools;

namespace ARTemplate
{
    [Flags]
    enum Act : short { None = 0, DefinePoint = 1, DefineId = 2, DefineChoose = 4, DefineUnChoose = 8, DefineName = 16, Zoomin, Zoomout, SeclectionToWhite, SeclectionToDark, ZoomMouse, SelectionToGroup, DefineCustom };
    public delegate void CompleteMouseMove(bool bcompleted);

    public partial class FormTemplate : Form
    {
        //public FormTemplate( Template t)
        //{
        //    InitializeComponent();
        //    Init(t);
        //}
        public FormTemplate()
        {
            InitializeComponent();
            Init(null);
            _ActiveEditArea = null;
            _ActiveEditMode = false;
            _ControlRects = null;
            _TestR = new Rectangle();
        }
        ~FormTemplate()
        {
            Clear();
        }
        public void Clear()
        {
            if (_fs != null)
            {
                _fs.Close();
                _fs = null;
            }
        }
        public FormTemplate(TemplateShow templateShow)
        {
            InitializeComponent();
            _fs = null;
            _src = null;
            Init(templateShow.Template);
            InitSrc(templateShow);
        }

        private void InitSrc(TemplateShow templateShow)
        {
            if (templateShow != null && templateShow.Template != null)
            {
                if (File.Exists( templateShow.SrcFileName) )
                {
                    _fs = new System.IO.FileStream(templateShow.SrcFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    Bitmap orgsrc = (Bitmap)System.Drawing.Image.FromStream(_fs);
                    _src = orgsrc.Clone(_template.CorrectRect, orgsrc.PixelFormat);
                    if (_src != null)
                        SetImage(_src);
                }
            }
        }
        private void Init(Template t)
        {
            if (t == null)
            {
                _fs = null;
                _src = null;
            }
            _template = t;
            m_tn = new TreeNode();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            zoombox = new ZoomBox();
            Reset();
            if (t != null)
                m_tn = t.GetTreeNode();
            InitComboBoxKH(t);
            treeView1.Nodes.Add(m_tn);
            treeView1.ExpandAll();
        }
        private void InitComboBoxKH(Template t)
        {

            toolStripComboBoxKHFormat.SelectedIndex = 0;
            if (t!=null && t.Dic.ContainsKey("考号") && t.Dic["考号"].Count == 1)
            {
                KaoHaoChoiceArea kh = (KaoHaoChoiceArea)t.Dic["考号"][0];
                switch (kh.Type) //数字means横向 涂卡
                {
                    case "条形码": toolStripComboBoxKHFormat.SelectedIndex = 0; break;
                    case "无": toolStripComboBoxKHFormat.SelectedIndex = 9; break;
                    case "3": toolStripComboBoxKHFormat.SelectedIndex = 1; break;
                    case "4": toolStripComboBoxKHFormat.SelectedIndex = 2; break;
                    case "5": toolStripComboBoxKHFormat.SelectedIndex = 3; break;
                    case "6": toolStripComboBoxKHFormat.SelectedIndex = 4; break;
                    case "7": toolStripComboBoxKHFormat.SelectedIndex = 5; break;
                    case "8": toolStripComboBoxKHFormat.SelectedIndex = 6; break;
                    case "9": toolStripComboBoxKHFormat.SelectedIndex = 7; break;
                    case "10": toolStripComboBoxKHFormat.SelectedIndex = 8; break;
                    default: toolStripComboBoxKHFormat.SelectedIndex = 0; break;
                }
            }
        }
        private void FormTemplate_Load(object sender, EventArgs e)
        {
        }
        private void SetImage(Bitmap image)
        {
            //image.Save("F:\\setimage.tif");
            pictureBox1.Image = image;
            ReSetPictureBoxImage();
            MT = new MovetoCTL.MovetoTracker(pictureBox1);
            MT.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            MT.StartDraw(true);
        }
        private void Reset()
        {
            m_tn.Nodes.Clear();
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            _OriginWith = pictureBox1.Width;
            zoombox.Reset();
            m_act = Act.None;
            treeView1.Nodes.Clear();
        }       

        private void toolStripButtonSaveTemplate_Click(object sender, EventArgs e)
        {
            if (_template == null) return;
            UpdateTemplate();

            string filename =  _template.GetTemplateName() + ".json";
            if (_template.FileName != "")
            {
                if (File.Exists(_template.FileName))
                {
                    filename = _template.FileName;
                }
                else if(Directory.Exists(_template.FileName))
                {
                    filename = _template.FileName +"\\"+ filename;
                }
            }
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.FileName = filename;
            saveFileDialog2.Filter = "Json files (*.json)|*.json";
            saveFileDialog2.Title = "Save json file";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _template.Save(saveFileDialog2.FileName);
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void toolStripButtonImportTemplate_Click(object sender, EventArgs e)
        {

            OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            OpenFileDialog2.FileName = "OpenFileDialog2";
            OpenFileDialog2.Filter = "Json files (*.json)|*.json";
            OpenFileDialog2.Title = "Open Json file";
            if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (_template.Load(OpenFileDialog2.FileName))
                    {
                        Init(_template);
                        RefreshTemplate();
                        //pictureBox1.Image.Save("F:\\b.tif");
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
                m_act == Act.SeclectionToDark || m_act == Act.SeclectionToWhite ||
                m_act == Act.SelectionToGroup || m_act == Act.DefineCustom  )
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
        private void toolStripButtonSetGroup_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.SelectionToGroup;
            toolStripButton_Click(sender, e);
        }
        private void toolStripButtonCustomDefine_Click(object sender, EventArgs e)
        {
            if (!((ToolStripButton)sender).Checked)
                m_act = Act.DefineCustom;
            toolStripButton_Click(sender, e);
        }
        private void CompleteSelection(bool bcomplete)
        {
            if (bcomplete)
            {
                ShowMessage("Complete: " + m_act);
                m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                if (_ActiveEditMode && _ActiveEditArea != null)
                {
                    //_TestR = Rectangle.Union(_ActiveEditArea.Rect, m_Imgselection);
                    _ActiveEditArea.Rect = m_Imgselection;// Rectangle.Union(_ActiveEditArea.Rect, m_Imgselection);
                    _ActiveEditArea.EditMode = false;
                    _ActiveEditMode = false;
                    _ActiveEditArea = null;
                }
                else
                {
                    switch (m_act)
                    {
                        case Act.DefinePoint: CompleteDeFinePoint(); break;
                        case Act.DefineChoose: CompleteDeFineChoose(); break;
                        case Act.DefineUnChoose: CompleteDeFineUnChoose(); break;
                        case Act.DefineId: CompleteDeFineId(); break;
                        case Act.SeclectionToWhite: CompleteSelectionToWhite(); break;
                        case Act.SeclectionToDark: CompleteSelectionToDark(); break;
                        case Act.DefineName: CompleteDeFineName(); break;
                        case Act.SelectionToGroup: CompleteSelectionGroup(); break;
                        case Act.DefineCustom: CompleteDefineCustom(); break;
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (_ActiveEditMode && _ActiveEditArea != null)
            //{
            //    int index = 0;
            //    foreach (Rectangle r in _ControlRects)
            //    {
            //        if (r.Contains(e.Location))
            //        {
            //            if(index==1||index==2)
            //            this.Cursor = Cursors.SizeNESW;
            //            else 
            //            this.Cursor = Cursors.SizeNWSE;
            //            return;
            //        }
            //        index++;
            //    }
            //}
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
                foreach (string s in new string[] { "特征点", "考号","校对", "选择题", "非选择题", "选区变黑", "选区变白","题组","自定义" })
                {
                    if( m_tn.Nodes.ContainsKey(s))
                    foreach (TreeNode t in m_tn.Nodes[s].Nodes)
                    {
                        if (t.Tag != null)
                        {
                            Area I = (Area)(t.Tag);
                            if (I.EditMode )
                                continue;
                            DrawArea(e, font, I);
                            if(I.HasSubAreas())
                                foreach (Area sI in I.SubAreas)
                                {
                                    if (sI.EditMode)
                                        continue;
                                    DrawArea(e, font, sI);
                                }
                        }
                    }
                }

                //return;
                if (_ActiveEditMode && _ActiveEditArea != null)
                {
                    Pen pen1 = Pens.DarkBlue;
                    Area I = _ActiveEditArea;
                    Rectangle r = zoombox.ImgToBoxSelection(I.ImgArea);
                    e.Graphics.DrawRectangle(pen1, r);
                    _ControlRects = DetectImageTools.DetectCorrect.GetLrbRtb(r, 5, 5);
                    e.Graphics.DrawRectangles(pen1, _ControlRects.ToArray());
                }
                if (_TestR.Width > 0 && _TestR.Height > 0)
                {
                    Rectangle r = zoombox.ImgToBoxSelection(_TestR);
                    e.Graphics.DrawRectangle(Pens.Yellow, r);
                }
            }
        }

        private void DrawArea(PaintEventArgs e,  Font font,  Area I)
        {
            e.Graphics.DrawRectangle(Pens.Red, zoombox.ImgToBoxSelection(I.ImgArea));
            if (I.HasImgSubArea())
            {
                foreach (Rectangle r in I.ImgSubArea())
                {
                    r.Offset(I.ImgArea.Location);
                    e.Graphics.DrawRectangle(Pens.Red, zoombox.ImgToBoxSelection(r));
                }
            }
            if (I.NeedFill())
            {
                e.Graphics.FillRectangle(I.FillPen(), zoombox.ImgToBoxSelection(I.ImgArea));
                //e.Graphics.DrawString(t.Name, font, Red, zoombox.ImgToBoxSelection(I.ImgArea).Location);
            }
            if (I.ShowTitle)
            {
                e.Graphics.DrawString(I.Title, font, Brushes.Red, zoombox.ImgToBoxSelection(I.ImgArea).Location);
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
            String keyname = "考号";
            int cnt = m_tn.Nodes[keyname].GetNodeCount(false) ;
            if (!ExistDeFineSelection(keyname))
            {
                string type = toolStripComboBoxKHFormat.SelectedItem.ToString();
                type = type.Substring(3);
                if (type.Contains("涂卡"))
                {
                    type = type.Substring(2,type.Length-3);
                    if(!"1023456789".Contains(type))
                        return;
                }
                if (cnt == 0)
                {

                    Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                    TreeNode t = new TreeNode();
                    t.Name = t.Text = keyname;
                    if (type == "条形码" || type == "无")
                    {
                        t.Tag = new KaoHaoChoiceArea(m_Imgselection, t.Name, type);
                        m_tn.Nodes[keyname].Nodes.Add(t);
                    }
                    else
                    {
                        int count = Convert.ToInt32(type); //位数
                        Bitmap bitmap = GetDrawedbyBlackWhiteBitMap();
                        DetectChoiceArea dca = new DetectChoiceArea(bitmap, count);
                        if (dca.DetectKH(false))
                        {                        
                            t.Tag = new KaoHaoChoiceArea(m_Imgselection, t.Name, type, dca.Choicepoint, dca.Choicesize);  
                            m_tn.Nodes[keyname].Nodes.Add(t);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先删除，考号区域只有一项");
                }
            }
        }
        private void CompleteDeFinePoint()
        {
            //TODO: add 特征点
            String keyname = "特征点";
            int cnt = m_tn.Nodes[keyname].GetNodeCount(false);
            if (cnt >= 3)
            {
                MessageBox.Show("只能有3个特征点");
                return;
            }
            if (!ExistDeFineSelection(keyname) )
            {
                //Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                //Bitmap bmp = (Bitmap)pictureBox1.Image;
                //m_Imgselection.Intersect( new Rectangle(0,0,bmp.Width,bmp.Height));
                //Image img = bmp.Clone(m_Imgselection, bmp.PixelFormat);
                ////Bitmap img = ConvertFormat.Convert((Bitmap)cropimg, PixelFormat.Format8bppIndexed, true);
                //MyDetectFeatureRectAngle dr = new MyDetectFeatureRectAngle(null);
                //Rectangle r = dr.Detected(new Rectangle(0, 0, img.Width, img.Height), (Bitmap)img);   
                
                //if (r.Width > 0 )
                //{
                //    TreeNode t = new TreeNode(); 
                //    cnt++ ;
                //    t.Name = t.Text = keyname + cnt;
                //    r.Offset(m_Imgselection.Location);
                //    // FeaturePoint(r, new Point(0, 0));
                //    t.Tag = new FeaturePoint(r, new Point(bmp.Width/2, bmp.Height/2));
                //    //m_Imgselection = tf.ImgSelection();
                //    m_tn.Nodes[keyname].Nodes.Add(t);
                //}
            }
        }
        private void CompleteDeFineName()
        {
            //TODO: 每个条目一项
             String keyname = "校对";
             TreeNode t = new TreeNode();
             int cnt = m_tn.Nodes[keyname].GetNodeCount(false);
             if(InputBox.Input( keyname ) && InputBox.strValue!="")
             {
                 t.Name = t.Text = InputBox.strValue;
                 Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
                 t.Tag = new NameArea(m_Imgselection,t.Name);
                 m_tn.Nodes[keyname].Nodes.Add(t);
             }
             else
             {
                 MessageBox.Show("请重新输入");
             }
        }
        private void CompleteDeFineChoose()
        {
            String keyname = "选择题";
            bool Hengpai = true;
            if (toolStripComboBoxChooseType.SelectedIndex != -1)
            {
                if (toolStripComboBoxChooseType.SelectedItem.ToString() == "竖排")
                    Hengpai = false;
            }
            if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                //t.Name = t.Text = ;
                string choosename = keyname + cnt;
                int count = 0;
                if (InputBox.Input("选择题"))
                    count = InputBox.IntValue;
                else
                {
                    return;
                }
                
                Bitmap bitmap = GetDrawedbyBlackWhiteBitMap();
                if (Hengpai)
                {//TODO:仅支持 横向填涂
                    DetectChoiceArea dca = new DetectChoiceArea(bitmap, count);
                    if (dca.Detect())
                    {
                        t.Name = t.Text = choosename;
                        t.Tag = new SingleChoiceArea(m_Imgselection,
                            t.Name, dca.Choicepoint, dca.Choicesize);
                        m_tn.Nodes[keyname].Nodes.Add(t);
                    }
                }
                else
                {
                    DetectChoiceArea dca = new DetectChoiceArea(bitmap, count);
                    if (dca.Detect(Hengpai))
                    {
                        t.Name = t.Text = choosename;
                        t.Tag = new SingleChoiceArea(m_Imgselection,
                            t.Name, dca.Choicepoint, dca.Choicesize);
                        m_tn.Nodes[keyname].Nodes.Add(t);
                    }
                }
            }
        }
        private Bitmap GetDrawedbyBlackWhiteBitMap()
        {
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
                                g.FillRectangle(I.FillPen(), r);
                            }
                        }
            }
            return bitmap;
        }
        private void CompleteDeFineUnChoose()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "非选择题";
            //if (!ExistDeFineSelection(keyname))
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;
                string unchoosename = "T-" + cnt;
                if (cnt == 1)
                {
                    _defaultunchoosescore = 1;
                    if (InputBox.Input("非选择题"))
                        _defaultunchoosescore = InputBox.IntValue;
                }
                else
                {
                    if (_defaultunchoosescore == 0)
                        _defaultunchoosescore = 2;
                }
                    String name = unchoosename;
                    t.Name = unchoosename;
                    t.Text = unchoosename + "(" + _defaultunchoosescore + "分)";
                    t.Tag = new UnChoose(_defaultunchoosescore, name, m_Imgselection);
                    m_tn.Nodes[keyname].Nodes.Add(t);
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
        private void CompleteSelectionGroup()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "题组";
            if (!ExistDeFineSelection("非选择题"))
            {
                MessageBox.Show("题组必须包含非选择题");
            }
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;

                string Tzname = "TZ-" + cnt;
                {
                    String name = Tzname;
                    t.Name = Tzname;
                    t.Text = Tzname;
                    t.Tag = new TzArea(m_Imgselection, name);
                    m_tn.Nodes[keyname].Nodes.Add(t);
                    //TODO: //重命名非选择题
                }
            }
        }
        private void CompleteDefineCustom()
        {
            Rectangle m_Imgselection = zoombox.BoxToImgSelection(MT.Selection);
            String keyname = "自定义";
            // 区域不做选择
            int count = 0;
            string name = "座位号";
            if (InputBox.Input("自定义"))
            {
                count = InputBox.IntValue;
                name = InputBox.strValue;
            }
            else
            {
                return;
            }

            if(name == "座位号")
            {
                TreeNode t = new TreeNode();
                int cnt = m_tn.Nodes[keyname].GetNodeCount(false) + 1;

                Bitmap bitmap = GetDrawedbyBlackWhiteBitMap();
                DetectChoiceArea dca = new DetectChoiceArea(bitmap, count);
                if (dca.DetectCustomDF(false))
                {
                    string type = count.ToString();
                    t.Name = t.Text = name + cnt;
                    t.Tag = new CustomArea(m_Imgselection, t.Name,type, dca.Choicepoint, dca.Choicesize);
                    m_tn.Nodes[keyname].Nodes.Add(t);
                }
               
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
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Parent == null) return;
            if (e.KeyCode == Keys.Delete)
            {
                if (treeView1.SelectedNode.Parent.Text != "网上阅卷")
                {
                    if (treeView1.SelectedNode.Parent != null)
                    {
                        Area I = (Area)treeView1.SelectedNode.Parent.Tag;
                        Area sI = (Area)treeView1.SelectedNode.Tag;
                        if (I != null && I.HasSubAreas() && sI!=null)
                        {
                            if (I.SubAreas.Contains(sI))
                                I.SubAreas.Remove(sI);
                        }
                    }
                    TreeNode t = treeView1.SelectedNode.NextNode;
                    if (t == null)
                        t = treeView1.SelectedNode.PrevNode;
                    treeView1.SelectedNode.Remove();
                    treeView1.SelectedNode = t;
                }
                pictureBox1.Invalidate();
            } else if (e.KeyCode == Keys.R)
            {
                if (treeView1.SelectedNode.Text == "题组")
                {
                    int cnt = _template.Manageareas.SinglechoiceAreas.Count+1;
                    AddUnChooseToTzArea();
                    ReNameUnChooseByTzArea1(cnt);
                    ReNameAreaByIncreace("选区变黑");
                    ReNameAreaByIncreace("选区变白");
                    UpdateTemplate();
                }
                pictureBox1.Invalidate();
            }
            else if (e.KeyCode == Keys.E)
            {
                 if (treeView1.SelectedNode.Parent.Text == "非选择题" ||
                        (treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Text == "题组"))
                {
                    Area I = (Area)treeView1.SelectedNode.Tag;
                    I.EditMode = true;
                    if (_ActiveEditArea != null)
                        _ActiveEditArea.EditMode = false;
                    _ActiveEditArea = I;
                    _ActiveEditMode = true;
                }
                else if(treeView1.SelectedNode.Text == "非选择题" || treeView1.SelectedNode.Text == "题组" ) //clear EditMode
                {
                    if (_ActiveEditArea != null)
                        _ActiveEditArea.EditMode = false;
                    _ActiveEditArea = null;
                    _ActiveEditMode = false;
                }
                pictureBox1.Invalidate();
            }
        }
        private void AddUnChooseToTzArea()
        {
            for (int i = 0; i < m_tn.Nodes["题组"].Nodes.Count; i++)
            {
                TreeNode Tntz = m_tn.Nodes["题组"].Nodes[i];
                TzArea Itz = (TzArea)Tntz.Tag;
                List<TreeNode> removenodes = new List<TreeNode>();
                foreach (TreeNode tnuc in m_tn.Nodes["非选择题"].Nodes)
                {
                    UnChoose uc = (UnChoose)tnuc.Tag;
                    if (Itz.ImgArea.Contains(uc.ImgArea))
                    {
                        Itz.SubAreas.Add(uc);
                        removenodes.Add(tnuc);
                        //TODO: ReName UnChoose
                    }
                }
                foreach (TreeNode tn in removenodes)
                {
                    m_tn.Nodes.Remove(tn);
                    Tntz.Nodes.Add(tn);
                }
            }
        }
        private bool CheckUnChooseNotInTzarea()
        {
            return m_tn.Nodes["非选择题"].Nodes.Count > 0;
        }
        private void ReNameUnChooseByTzArea1(int cnt)
        {
            for (int i = 0; i < m_tn.Nodes["题组"].Nodes.Count; i++)
            {
                TreeNode t = m_tn.Nodes["题组"].Nodes[i];
                t.Name = "TZ-" + cnt;
                t.Text = t.Name;
                TzArea tr = (TzArea)t.Tag;

                int subcnt = 1;
                foreach (TreeNode tn in t.Nodes)
                {
                    UnChoose uc = (UnChoose)tn.Tag;
                    tn.Name = tn.Text = cnt + "-" + subcnt;
                    uc.SetName(tn.Name);
                    subcnt++;
                }
                cnt++;
            }
        }
        private void ReNameUnChooseByTzArea(int cnt)
        {
            for (int i = 0; i < m_tn.Nodes["题组"].Nodes.Count; i++)
            {
                TreeNode t = m_tn.Nodes["题组"].Nodes[i];
                t.Name = "TZ-" + cnt;
                t.Text = t.Name;
                TzArea tr = (TzArea)t.Tag;

                int subcnt = 1;
                foreach (TreeNode tn in m_tn.Nodes["非选择题"].Nodes)
                {
                    UnChoose uc = (UnChoose)tn.Tag;
                    if (tr.ImgArea.Contains(uc.ImgArea))
                    {
                        tn.Name = tn.Text = cnt + "-" + subcnt;
                        uc.SetName(tn.Name);
                        subcnt++;
                    }
                }
                cnt++;
            }
        }
        private bool ReNameAreaByIncreace(string keyname )
        {
            if (!m_tn.Nodes.ContainsKey(keyname))
                return false;
            int cnt = 0;
            for (int i = 0; i < m_tn.Nodes[keyname].Nodes.Count; i++)
            {
                TreeNode t = m_tn.Nodes[keyname].Nodes[i];
                t.Name =   cnt.ToString();
                t.Text = t.Name;
                Area I = (Area)t.Tag;
                //TODO: Area.SetName(name) UnImpl;
                //I.SetName(t.Name);
                cnt++;
            }
            return true;
        }
        private void RefreshTemplate()
        {
            m_act = Act.None;
            m_Imgselection = new Rectangle(0, 0, 0, 0);
            //m_tn.Nodes.Clear();
            //m_tn = _template.GetTreeNode();
            if (pictureBox1.Image != null)
                zoombox.UpdateBoxScale(pictureBox1);
        }
        private void UpdateTemplate()
        {
            _template.UpdateTreeNodes(m_tn);           
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
        private double _OriginWith;
        private float  _defaultunchoosescore;
        private Template _template;
        private Bitmap _src;
        private FileStream _fs;
        private Area _ActiveEditArea;
        private bool _ActiveEditMode;
        private List<Rectangle> _ControlRects;
        private Rectangle _TestR;

    }
}
