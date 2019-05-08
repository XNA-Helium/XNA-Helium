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
    class GrenadeSingle : ProjectileObject<GrenadeSingle>
    {
        
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new GrenadeSingle(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected GrenadeSingle( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\Cluster" ), "Static" );
            dss.PlayAnimation( "Static" );
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 16 );
        }

        public GrenadeSingle(Facing direction)
            : base(direction)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\Cluster" ), "Static" );
            dss.PlayAnimation( "Static" );
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 16 );
        }
        
        public override void OnCollision(BaseObject rhs)
        {
            if ( (rhs is GrenadeSingle && rhs != this) || rhs is SmallExplosion)
            {
                return; 
            }
            if (!collided)
            {
                Vector2 position = (this[Placeable.TypeStatic] as Placeable).Position.ToVector2();
                SmallExplosion we = new SmallExplosion(new Vector2(position.X, position.Y));
                if (rhs == this)
                {
                    //collided with terrain
                }
                base.OnCollision( rhs );

                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater( we );
                Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
            }
        }

        public static IProjectile ReturnNewProjectile( Facing facing )
        {
            return new GrenadeSingle(facing);
        }        
    }
}
