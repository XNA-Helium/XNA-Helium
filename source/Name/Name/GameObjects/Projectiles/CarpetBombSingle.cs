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
    class CarpetBombSingle : ProjectileObject<CarpetBombSingle>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new CarpetBombSingle(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        protected CarpetBombSingle(bool InWorld, float uID)
            : base(InWorld, uID)
        {
            Drawable2D dss = (this[Drawable2D.TypeStatic] as Drawable2D);
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\NapalmPiece"), "Static");
            dss.PlayAnimation("Static");
            dss.FaceVelocity = false;
            (this[Physics2D.TypeStatic] as Physics2D).Rectangle = new Rectangle(0, 0, 16, 16);
        }

        public CarpetBombSingle(Facing direction)
            : base(direction)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\NapalmPiece" ), "Static" );
            dss.PlayAnimation( "Static" );
            dss.FaceVelocity = false;
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 16 );
        }

        public override void OnCollision(BaseObject rhs)
        {
            if (rhs == this)
            {
                //collided with terrain
                Vector2 position = (this[Placeable.TypeStatic] as Placeable).Position.ToVector2();
                Napalm we = new Napalm(new Vector2(position.X, position.Y));
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(we);
                base.OnCollision(rhs);
                Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
            }
        }

        public static IProjectile ReturnNewProjectile( Facing facing )
        {
            return new CarpetBombSingle(facing);
        }

    }
}
