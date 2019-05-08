using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    public class CircularObjectList<T> : LinkedList<T> where T : class 
    {
        LinkedListNode<T> CurrentNode = new LinkedListNode<T>(null);
        
        public CircularObjectList()
        {
        }

        public virtual T this[int index]
        {
            get
            {
                return this.ElementAt<T>( index );
            }
        }

        public virtual void Add(T obj)
        {
            AddLast(obj);
            if (Count == 1)
            {
                CurrentNode = First;
            } 
        }

        public virtual  T Current
        {
            get
            {
                return CurrentNode.Value;
            }
        }

        public virtual void Next()
        {
            if (CurrentNode == Last)
            {
                CurrentNode = First;
            }else{
                CurrentNode = CurrentNode.Next;
            }
        }

        protected new void Remove(T obj)
        {
            base.Remove(obj);
        }

        public bool SafeRemove(T obj)
        {
            if (CurrentNode.Value == obj)
            {
                if (Count == 1)
                {
                    return false;
                }
                else
                {
                    Next();
                    Remove(obj);
                }
            }
            else
            {
                Remove(obj);
            }
            return true;
        }
    }
}
