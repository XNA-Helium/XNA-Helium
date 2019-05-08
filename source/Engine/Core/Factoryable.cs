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
    public interface IFactoryable
    {
        /*
        int IntType
        {
            get;
        }
        */

        Type Type
        {
            get;
        }

        float ID
        {
            //set;
            get;
        }

        bool AddToWorld
        {
            get;
            set;
        }


        void Save(Engine.Persistence.PersistenceManager sw);
        void Load(Engine.Persistence.PersistenceManager sr);
    }

    public abstract class Factoryable
    {
        private float UniqueID = -1;
        public float ID
        {
            internal set
            {
                UniqueID = value;
            }
            get
            {
                return UniqueID;
            }
        }

        public static ReturnsNew ReturnNew
        {
            get
            {
                Engine.DebugHelper.Break(DebugHelper.DebugLevels.Critical);
                return null;
            }
        }

        protected bool addToWorld;

        public virtual bool AddToWorld
        {
            get
            {
                return addToWorld;
            }
            set
            {
                addToWorld = value;
            }
        }

        public Factoryable()
            : this(true, -1)
        {

        }

        public Factoryable( bool InWorld )
            : this( InWorld, -1 )
        {

        }

        public Factoryable(bool InWorld, float uID )
        {
            addToWorld = InWorld;
            if ( addToWorld && uID == -1)
            {
                UniqueID = IDManager.GetNextInstanceID();
            }
            else if ( addToWorld )
            {
                UniqueID = uID;
            }
        }

        public abstract void Save( Engine.Persistence.PersistenceManager sw);

        public abstract void Load( Engine.Persistence.PersistenceManager sr);

        public abstract void HookUpPointers();
    }
}