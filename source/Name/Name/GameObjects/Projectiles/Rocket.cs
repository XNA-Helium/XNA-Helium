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
    class Rocket : ProjectileObject<Rocket>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new Rocket(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected Rocket( bool InWorld, float uID )
            : base(InWorld, uID)
        {

        }

        public Rocket(Facing direction)
            : base(direction)
        {

        }

        public static IProjectile ReturnNewProjectile( Facing facing )
        {
            return new Rocket(facing);
        }

        public override void OnCollision(BaseObject rhs)
        {
            if (!collided)
            {
                Vector2 position = (this[Placeable.TypeStatic] as Placeable).Position.ToVector2();
                WeaponExplosion we = new WeaponExplosion(new Vector2(position.X, position.Y));
                if (rhs == this)
                {
                    //collided with terrain
                }

                base.OnCollision(rhs);
                if (rhs[HitPoints.TypeStatic] != null)
                {
                    (rhs[HitPoints.TypeStatic] as HitPoints).Hitpoints -= 40;
                }
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater( we );
                Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
            }
        }
    }
}
