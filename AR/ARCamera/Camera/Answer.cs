using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camera
{
    public class Answer
    {
        public Answer(int id, int typeid, float score, int option)
        {
            this.id = id;
            this.typeid = typeid;
            this.score = score;
            this.option = option;
        }
        public int id;
        public int typeid;
        public float score;
        public int option;
    }
}
