using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    public class SafeList<T> : List<T> where T : class 
    {
        protected List<T> AddMe = new List<T>(12);
        protected List<T> RemoveMe = new List<T>(12);

        public SafeList( )
            : base()
        {

        }
        public SafeList( int i )
            : base( i )
        {

        }
        public SafeList( IEnumerable<T> collection )
            : base( collection )
        {

        }

        public void SafeRemove( T obj )
        {
            RemoveMe.Add( obj );
        }

        public void SafeAdd( T obj )
        {
            AddMe.Add( obj );
        }

        public void AddAll( )
        {
            foreach ( T a in AddMe )
            {
                Add( a );
            }
            AddMe.Clear();
        }

        public void RemoveAll( )
        {
            foreach ( T a in RemoveMe )
            {
                Remove( a );
            }
            RemoveMe.Clear();
        }
    }
}
