using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace yj.formimg
{
	public class MoveTracker
	{
        public event CompleteMouseMove completevent;		
		public MoveTracker(PictureBox pbox){
			pictureBox1=pbox;
            ptStart = new Point(0, 0);
            m_bdraw = false;
            m_selecting = false;
            m_selection = new Rectangle(0,0,0,0);            
            //private Cursor cursor;
		}

        internal void ClearEvent()
        {
            this.completevent = null;
            pictureBox1.MouseDown -= new MouseEventHandler(pictureBox1_MouseDown);
            pictureBox1.MouseMove -= new MouseEventHandler(pictureBox1_MouseMove);
            pictureBox1.MouseUp -= new MouseEventHandler(pictureBox1_MouseUp);
            pictureBox1.Paint -= new PaintEventHandler(pictureBox1_Paint);
            pictureBox1.Cursor = cursor;
        }
        internal void StartDraw(bool bdraw)
        {
            m_bdraw = bdraw;
            if (m_bdraw && pictureBox1!=null)
            {
                pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
                pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
                pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
                pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
                cursor = pictureBox1.Cursor;
                pictureBox1.Cursor = Cursors.Cross;
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
           if(m_bdraw)
                if (pictureBox1.Image != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        m_selecting = true;
                        m_selection = new Rectangle(new System.Drawing.Point(e.X, e.Y), new Size());
                    }
                }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_selecting)
            {
                m_selection.Width = e.X - m_selection.X;
                m_selection.Height = e.Y - m_selection.Y;
                pictureBox1.Refresh();
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bool susscess = false;
            if (e.Button == MouseButtons.Left && m_selecting)
            {
                m_selecting = false;
                if (m_selection.Width > 10 && m_selection.Height > 10)
                {
                    susscess = true;
                }
            }
            if (completevent != null)
                completevent(susscess);
            //ClearEvent();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (m_selecting)
            {
                Pen pen = Pens.Green;
                e.Graphics.DrawRectangle(pen, m_selection);
            }
        }

        public Rectangle Selection
        {
            get { return m_selection; }
        }

        private PictureBox pictureBox1;
        private bool m_selecting;
        private Point ptStart;
        private bool m_bdraw;
        private Cursor cursor;
        private Rectangle m_selection;

    }
}