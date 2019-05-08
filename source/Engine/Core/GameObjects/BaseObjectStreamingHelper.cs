using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using Engine;

namespace Engine.Core
{
    public abstract class BaseObjectStreamingHelper<T> : Engine.Core.BaseObject, Engine.Core.IBaseObject where T : Factoryable
    {

        public static void Initialize(ReturnsNew retnew)
        {
            IntType = IDManager.GetNextID<T>( retnew );
        }
        public static int IntType;
        public static Type TypeStatic = typeof( T );
        public BaseObjectStreamingHelper( bool InWorld, float uID )
            : base(InWorld,uID)
        {
            UniqueClassID = TypeStatic; // Not in BaseObjects   
        }

        public BaseObjectStreamingHelper( bool InWorld )
            : base( InWorld )
        {
            UniqueClassID = TypeStatic; // Not in BaseObjects   
        }
        public BaseObjectStreamingHelper()
            : base( )
        {
            UniqueClassID = TypeStatic; // Not in BaseObjects   
        }

        /// <summary>
        /// This function for Post-Load use only
        /// </summary>
        public override void HookUpPointers()
        {
        }
    }
}