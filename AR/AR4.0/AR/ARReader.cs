using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace AR
{ 
    public class ARReader
    {
        public ARReader(ComboBox comboBoxQustiongroup, Button buttonPageUp, Button buttonPageDown,PictureBox pictureBox1, ComboBox comboBoxsetscore, Button buttonupdate,MainForm mainForm)
        {
            this.m_cbxqg = comboBoxQustiongroup;
            this.m_btnup = buttonPageUp;
            this.m_btndown = buttonPageDown;
            this.pictureBox1 = pictureBox1;
            this.comboBoxSetScore = comboBoxsetscore;
            this.buttonupdate = buttonupdate;
            this.mainForm = mainForm;
        }
        public void Read(bool bread, ActivePage page)
        {
            if (bread)
            {
                this.m_activepage = page;
                pictureBox1.Image = page._Image;
                HandEvent();
            }
            else
            {
                ClearEvent();
            }
        }      

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_cbxqg.SelectedIndex == -1) return;
                if(m_scorearea)
                m_activepage.SetScore(e.Location,m_activescore);
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_activerect.Contains(e.Location) 
                || !pictureBox1.ClientRectangle.Contains(e.Location)
                || pictureBox1.Image == null)
                return;
            
            Color c = ((Bitmap)(pictureBox1.Image)).GetPixel(e.X, e.Y);
            if (c.B == 128 && c.G > 127)
            {
                if (m_cbxqg.SelectedIndex == -1) return;
                m_scorearea = true;
                m_activescore = c.G - 128;
                m_activerect = GetActiveRect(e.Location,m_activescore);

                pictureBox1.Cursor = Cursors.Hand;
                pictureBox1.Invalidate(m_activerect);
            }
            else
            {
                m_scorearea = false;
                pictureBox1.Cursor = Cursors.Arrow;
            }
            pictureBox1.Invalidate();

        }

        private Rectangle GetActiveRect(Point e, int score)
        {
            if (score == -1)
            {
                return new Rectangle(0, 0, 0, 0);
            }
            else
            {
                Rectangle sr = m_activepage.TestScore(e);
                int x = (e.X - sr.X) / 30;
                int y = (e.Y - sr.Y) / 30;
                sr.Offset(x * 30, y * 30);
                Rectangle ret = new Rectangle( sr.Location,new Size(30,30));
                return ret;               
            }            
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int index = m_cbxqg.SelectedIndex;
            if (m_cbxqg.SelectedIndex == -1) return;
            Rectangle[] lr =  m_activepage.ScoreSelection();
            if(lr.Length>0)
                e.Graphics.FillRectangles(Brushes.GreenYellow, lr);
            if (m_scorearea)
            {
                e.Graphics.FillRectangle(Brushes.Red, m_activerect);
            }
        }
        private void comboBoxQustiongroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox qg = (ComboBox)sender;
            if (qg.SelectedIndex != -1)
            {
                PaperBlocks pbs = (PaperBlocks)qg.SelectedItem;
                pictureBox1.Image = null;
                if (pbs.HasPapers())
                {                    
                    m_activepage.CreateImage( pbs.GetPaperBlocks(m_activepage.PageSize ),pbs );
                    pictureBox1.Image = m_activepage._Image;
                    this.comboBoxSetScore.Items.Clear();
                    for (int i = 0; i < pbs.IntScore + 1; i++)
                        this.comboBoxSetScore.Items.Add(i + "分");
                }
                else
                {
                    MessageBox.Show("本小题已改完");
                }
            }
        }

        private void buttonPageUp_Click(object sender, EventArgs e)
        {
            //if (ARConfig.back)
            //{
            //    pictureBox1.Image = null;
            //    page.CreateImage( m_cbxqg.SelectedIndex);
            //    pictureBox1.Image = page.Image;
            //}
        }
        private void buttonPageDown_Click(object sender, EventArgs e) //目标没有达成
        {
            PaperBlocks pbs = (PaperBlocks)m_cbxqg.SelectedItem;
            pictureBox1.Image = null;
            if (pbs.HasPapers())
            {
                m_activepage.CreateImage(pbs.GetPaperBlocks(m_activepage.PageSize), pbs);
                pictureBox1.Image = m_activepage._Image;
            }
            else
            {
                MessageBox.Show("本小题已改完");
            }
        }
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //none //Hide this button
        }
        private void comboBoxSetScore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSetScore.SelectedIndex != -1)
                m_activepage.SetScore(comboBoxSetScore.SelectedIndex);
        }
        private void HandEvent()
        {
            ClearEvent();
            m_cbxqg.SelectedIndexChanged +=  new EventHandler(comboBoxQustiongroup_SelectedIndexChanged);
            m_btnup.Click += new EventHandler(buttonPageUp_Click);
            m_btndown.Click += new EventHandler(buttonPageDown_Click);
            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);

            buttonupdate.Click += new EventHandler(buttonUpdate_Click);
            comboBoxSetScore.SelectedIndexChanged += new EventHandler(comboBoxSetScore_SelectedIndexChanged);
        }
        private void ClearEvent()
        {
            if(m_cbxqg!=null)
                m_cbxqg.SelectedIndexChanged -=
                new System.EventHandler(comboBoxQustiongroup_SelectedIndexChanged);
            if(m_btnup!=null)
                m_btnup.Click -= new EventHandler(buttonPageUp_Click);
            if(m_btndown!=null)
                m_btndown.Click -= new EventHandler(buttonPageDown_Click);
            if (pictureBox1 != null)
            {
                pictureBox1.MouseDown -= new MouseEventHandler(pictureBox1_MouseDown);
                pictureBox1.MouseMove -= new MouseEventHandler(pictureBox1_MouseMove);
                pictureBox1.Paint -= new PaintEventHandler(pictureBox1_Paint);
            }
            if(buttonupdate !=null)
                buttonupdate.Click -= new EventHandler(buttonUpdate_Click);
            if (comboBoxSetScore != null)
                comboBoxSetScore.SelectedIndexChanged -= new EventHandler(comboBoxSetScore_SelectedIndexChanged);
        }  
        //private Cursor cursor;
        private ActivePage m_activepage;
        private Rectangle m_activerect;
     

        private int m_activescore;
        private bool m_scorearea;

        private Button m_btnup;
        private Button m_btndown;
        private Button buttonupdate;
        private PictureBox pictureBox1;
        private ComboBox m_cbxqg;
        private ComboBox comboBoxSetScore;
        private MainForm mainForm;
    }
}
