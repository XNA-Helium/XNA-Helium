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
    public abstract class BaseComponent : Factoryable, IEngineUpdateable
    {
        public static void Initialize<T>( ReturnsNew retnew ) where T : Factoryable
        {
            Engine.Core.IDManager.AddClass<T>( retnew );
        }

        protected Type UniqueClassID;
        public Type Type
        {
            get
            {
                return UniqueClassID;
            }
        }

        private bool HasChanges = false;
        public bool HasChanged
        {
            set
            {
                DebugHelper.Break( parent == null, DebugHelper.DebugLevels.Informative );
                HasChanges = value;
            }
            get
            {
                return HasChanges;
            }
        }


        BaseObject parent;

        public virtual void OnParentSet( BaseObject Parent ) { }
        public virtual void OnRemoved( BaseObject Parent ) { }
        public override void HookUpPointers() { }
        public virtual void Init( ) { }

        public BaseComponent(bool InWorld, float uID)
            : base(InWorld,uID)
        {
        }

        public BaseObject Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                OnParentSet( parent );
            }
        }

        public virtual void Update( GameTime p_time ) { }

    }
}
