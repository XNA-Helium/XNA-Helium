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


namespace Engine.Core
{
    public delegate Object ReturnsNew(bool InWorld, float uID);

    //This factory can be plugged in on the END
    // of the inheritance tree if need be
    // If we do that move all components into Engine.Core.Components
    // And leave the new component in Engine.Core
    // With the same name

    public abstract class IFactory
    {
        public abstract Object ReturnNew(bool InWorld,float uID);

        public static Type StaticType;
        public static int TypeINT;
    }

    public class Factory<T> : IFactory where T : Factoryable
    {
        private ReturnsNew ReturnsNewDelegate;
        public Factory(ReturnsNew retnewdeg):base()
        {
            ReturnsNewDelegate = retnewdeg;
        }
        public int IntType = Factory<T>.TypeINT;
        public Type Type = StaticType;

        public override Object ReturnNew(bool InWorld, float uID)
        {
            
            return ReturnsNewDelegate(InWorld,uID);
        }

        public static new Type StaticType = typeof( T );
        public static new int TypeINT = IDManager.GetNextID();

    }
}