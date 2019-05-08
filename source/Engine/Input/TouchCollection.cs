using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class TouchCollection
    {
        
        public static int MaxTouches = 10;
        protected int count = 0;
        public TouchCollection()
        {
            for ( int i = 0; i < MaxTouches; ++i )
            {
                Touches.Add(new Touch());
            }
        }
        
        public int Count
        {
            get
            {
                return count;
            }
            internal set
            {
                count = value;
            }
        }
        protected List<Touch> Touches = new List<Touch>(MaxTouches);
        public virtual Touch this[int Index]
        {
            internal set
            {
                //if (Index > Touches.Count)
                //    throw new IndexOutOfRangeException("This touch does not exist");
                Touches[Index] = value;
            }
            get
            {
                if (Index > Touches.Count)
                    throw new IndexOutOfRangeException("This touch does not exist");
                return Touches[Index];
            }
        }

    }
}
