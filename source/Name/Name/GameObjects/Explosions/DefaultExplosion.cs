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
using Engine.Core;

namespace Name
{
    public abstract class DefaultExplosion<T> : BaseObjectStreamingHelper<T>, IExplosion where T : Factoryable
    {
        protected DefaultExplosion( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToUpdateList( this );
        }

        public DefaultExplosion( )
        {
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToUpdateList( this );
        }
        protected bool collided = false;
        public bool MarkCollided 
        {
            get
            {
                return collided;
            }
            set
            {
                collided = value;
            }
        }

        public override void CleanUp( bool RemoveFromObjectList )
        {
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromUpdateList( this );
            base.CleanUp( RemoveFromObjectList );
        }

        public override void Update( GameTime p_time )
        {
            base.Update( p_time );
            if ( collided )
            {
                ( this[Physics2D.TypeStatic] as Physics2D ).Callback = null;
            }
        }
    }
}
