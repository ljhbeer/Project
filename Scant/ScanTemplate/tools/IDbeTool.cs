using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class IDbeTool
    {
        public IDbeTool()
        {
            B = E = activeindex = -1;
        }
        public void MoveCircleNext()
        {
            activeindex++;
            if (activeindex == Count)
                activeindex = 0;
        }
        public void MoveNext()
        {
            activeindex++;
        }
        public void MovePrevious()
        {
            activeindex--;
        }
        public void MoveToTop()
        {
            activeindex = 0;
        }
        public bool OK()
        {
            return B > 0 && E > 0 && B < E;
        }
        public bool HasNext
        {
            get
            {
                return activeindex >= 0 && activeindex < E - B;
            }
        }
        public int Count { get { return E - B; } }
        public int B { get; set; }
        public int E { get; set; }
        public int ActiveIndex { get { return activeindex + B; } }
        private int activeindex;
    }
}
