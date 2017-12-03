using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AR
{
    
    public class ActivePage
    {
        public ActivePage(Size size )
        {
            PageSize = size;
            _paperblocks = new List<PaperBlock>();
            _scorerectangles = new List<Rectangle>();
            _Image = new Bitmap(PageSize.Width, PageSize.Height);
            DetectChoiceArea.Fill(_Image, 200);
        }
        internal void CreateImage(List<PaperBlock> list,PaperBlocks pbs)
        {
            DetectChoiceArea.Fill(_Image, 200);  //clear
            if (list.Count == 0) return;

            Graphics g = Graphics.FromImage(_Image);
            int imgpos = 0;
            _paperblocks = list;
            _scorerectangles.Clear();
            
            Rectangle scorer=new Rectangle(0,0,0,0);
            foreach (PaperBlock pb in list)
            {
                Rectangle src = pb.ImageRectangle;
                Rectangle pagesrc = pb.ImagePageRectangle;
                Rectangle dstr = new Rectangle( pb.Location.X * pagesrc.Width, pb.Location.Y*pagesrc.Height, src.Width, src.Height);
                Point point = new Point(pb.Location.X * pagesrc.Width + src.Width, pb.Location.Y * pagesrc.Height);
                g.DrawImage(pb.Paper.Img, dstr, src, GraphicsUnit.Pixel);
                ScoresImg.DrawBitmap((Bitmap)_Image, pbs.IntScore, point, src.Height,ref scorer);
                _scorerectangles.Add(scorer);
                imgpos++;
            }
        }
        public Rectangle TestScore(Point e)
        {
            foreach (Rectangle r in _scorerectangles)
                if (r.Contains(e))
                    return r;
            return new Rectangle(0, 0, 30, 30);
        }
        public void SetScore(Point e,int score)
        {
            for (int i = 0; i < _scorerectangles.Count; i++)
                if (_scorerectangles[i].Contains(e))
                {
                    //if( _paperblocks[i].BlockScore.GetScore() == -1)
                    //    _paperblocks[i].
                    _paperblocks[i].BlockScore.SetScore(score);
                }
        }
        public void SetScore(int score)
        {
            for (int i = 0; i < _paperblocks.Count; i++)
            {
                _paperblocks[i].BlockScore.SetScore(score);
            }
        }
        public Rectangle[] ScoreSelection()
        {
            List<Rectangle> _ScoreSelection = new List<Rectangle>();
            for (int i = 0; i < _paperblocks.Count; i++)
                if (_paperblocks[i].BlockScore.HasScore())
                {
                    int Score = _paperblocks[i].BlockScore.GetScore();
                    int Y = Score*30/_scorerectangles[i].Width;
                    int X = Score * 30 % _scorerectangles[i].Width;                  
                    _ScoreSelection.Add(
                        new Rectangle(_scorerectangles[i].X + X,
                            _scorerectangles[i].Y + Y*30,30, 30)
                        );
                }
            return _ScoreSelection.ToArray();
        }

        public Bitmap _Image;
        public Size PageSize;
        private List<PaperBlock> _paperblocks;
        private List<Rectangle> _scorerectangles;
    }
}
