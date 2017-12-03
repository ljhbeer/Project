using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace MovetoCTL
{
	public enum MOUSE_MODE	// mouse mode
	{
		MM_NONE,
		MM_DRAWING,
		MM_OBJ_SELECTED
	}	   
	public enum RESIZE_BORDER	// keep track of which border of the box is to be resized.
	{
		RB_NONE = 0,
		RB_TOP = 1,
		RB_RIGHT = 2,
		RB_BOTTOM = 3,
		RB_LEFT = 4,
		RB_LEFTTOP = 5,
		RB_LEFTBOTTOM = 6,
		RB_RIGHTTOP = 7,
		RB_RIGHTBOTTOM = 8
	}
	public class MovetoTracker
	{
		public MOUSE_MODE MouseMode{
			get { return m_enMM;   }
			set { m_enMM = value; }
		}
		public MovetoTracker(PictureBox pbox){
			pictureBox1=pbox;
            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
            MouseUp = null;
            //DrawingImage=new Bitmap(picturebox1.Image,picturebox1.ClientRectangle.Width,
            //    picturebox1.ClientRectangle.Height);
            //Graphics GraphicsTemp = Graphics.FromImage(DrawingImage);
            //GraphicsTemp.DrawImage(picturebox1.Image,0,0);
            //if(picturebox1.Image!=null)
            //    GraphicsTemp.DrawImage(picturebox1.Image,0,0);
            //else
            //    GraphicsTemp.FillRectangle(new SolidBrush(picturebox1.BackColor),picturebox1.ClientRectangle);
            //GraphicsTemp.Dispose();
            //HandleMenu();
			m_enMM = MOUSE_MODE.MM_NONE;
            //m_enRBorder = RESIZE_BORDER.RB_NONE;
			//this.m_enMM = MOUSE_MODE.MM_DRAWING;
			pictureBox1.Cursor = Cursors.Cross;
            Selection = new Rectangle(0,0,0,0);
		}
        public Rectangle Selection;
        public MouseEventHandler MouseUp { get; set; }
        public void StartDraw(bool bdraw)
        {
            m_bdraw = bdraw;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Selection = new Rectangle(0, 0, 0, 0);
           if(m_bdraw)
                if (pictureBox1.Image != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        _selecting = true;
                        _selection = new Rectangle(new System.Drawing.Point(e.X, e.Y), new Size());
                    }
                }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selecting)
            {
                _selection.Width = e.X - _selection.X;
                _selection.Height = e.Y - _selection.Y;
                pictureBox1.Refresh();
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _selecting)
            {
                _selecting = false;
                Selection = _selection;
                //Selection = MyFunction.PictureBoxZoomSelectionToImageSelection(_selection, pictureBox1);                 
            }
            if (MouseUp != null)
                MouseUp(sender, e);
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (_selecting)
            {
                Pen pen = Pens.Green;
                e.Graphics.DrawRectangle(pen, _selection);
            }
        }

        private PictureBox pictureBox1;
        private bool _selecting;
        private Rectangle _selection;
        private Point ptStart = new Point(0, 0);
        private MOUSE_MODE m_enMM;
        //private RESIZE_BORDER m_enRBorder;
        //private int MouseX, MouseY;        
        //private bool bMouseDown = false;
        private bool m_bdraw = false;

        //internal void ClearEvent()
        //{
        //    this.completevent = null;
        //    pictureBox1.MouseDown -= new MouseEventHandler(pictureBox1_MouseDown);
        //    pictureBox1.MouseMove -= new MouseEventHandler(pictureBox1_MouseMove);
        //    pictureBox1.MouseUp -= new MouseEventHandler(pictureBox1_MouseUp);
        //    pictureBox1.Paint -= new PaintEventHandler(pictureBox1_Paint);
        //    pictureBox1.Cursor = cursor;
        //}
    }
}