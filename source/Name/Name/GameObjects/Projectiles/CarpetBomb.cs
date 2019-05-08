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
    class CarpetBomb : ProjectileObject<CarpetBomb>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new CarpetBomb(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        protected CarpetBomb(bool InWorld, float uID)
            : base(InWorld, uID)
        {
            Drawable2D dss = (this[Drawable2D.TypeStatic] as Drawable2D);
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\Napalm"), "Static");
            dss.PlayAnimation("Static");
            dss.FaceVelocity = false;
            (this[Physics2D.TypeStatic] as Physics2D).Rectangle = new Rectangle(0, 0, 16, 96);
        }

        public CarpetBomb(Facing direction)
            : base(direction)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\Napalm" ), "Static" );
            dss.PlayAnimation( "Static" );
            dss.FaceVelocity = false;
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 96 );
        }

        public override void Update(GameTime p_time)
        {
            base.Update(p_time);
            Vector2 position = (base[Placeable.TypeStatic] as Placeable).Position.ToVector2();
            /*
            if ((position.Y) >= (Game1.Instance.GameManager.Level as DynamicLevel)[(int)position.X] - 32)
            {
                SpawnCarpetBombs();
                Engine.GameState.GameStateSystem.Instance.RemoveObject(this);
            }
            */
            float rotation = 11.0f * ((float)p_time.TotalGameTime.TotalSeconds) +  (this[Placeable.TypeStatic] as Placeable).Get2DRotation();
            (this[Placeable.TypeStatic] as Placeable).Set2DRotation(rotation);
        }

        public override void OnCollision(BaseObject rhs)
        {
            if (!collided)
            {

                EventManager.Instance.AddEvent( SpawnCarpetBombs, 0.0f );
                if (rhs == this)
                {
                    //collided with terrain
                }
                base.OnCollision(rhs);
            }
        }

        public void SpawnCarpetBombs()
        {
            Vector2 position = (this[Placeable.TypeStatic] as Placeable).Position.ToVector2();
            SoundManager.Instance.Play("land", 1f);

            int multiplier = 0;

            if (facing == Facing.Left)
            {
                multiplier = -1;
            }
            else if (facing == Facing.Right)
            {
                multiplier = 1;
            }
            CarpetBombSingle wel = new CarpetBombSingle(facing);
            CarpetBombSingle wem = new CarpetBombSingle(facing);
            CarpetBombSingle wer = new CarpetBombSingle(facing);
            CarpetBombSingle www = new CarpetBombSingle(facing);

            ( wem[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 0.0f * multiplier, -2.0f ).ToVector3();
            ( wer[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 2.0f * multiplier, -2.0f ).ToVector3();
            ( wel[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 4.0f * multiplier, -2.0f ).ToVector3();
            ( www[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 6.0f * multiplier, -2.0f ).ToVector3();
            ( wel[Placeable.TypeStatic] as Placeable ).Position = new Vector2( position.X, position.Y - 0 ).ToVector3();
            ( wem[Placeable.TypeStatic] as Placeable ).Position = new Vector2( position.X, position.Y - 0 ).ToVector3();
            ( wer[Placeable.TypeStatic] as Placeable ).Position = new Vector2( position.X, position.Y - 0 ).ToVector3();
            ( www[Placeable.TypeStatic] as Placeable ).Position = new Vector2( position.X, position.Y - 0 ).ToVector3();
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(wel);
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(wer);
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(wem);
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(www);

            Engine.GameState.GameStateSystem.Instance.RemoveObjectLater( this );
        }


        public static IProjectile ReturnNewProjectile( Facing facing )
        {
            return new CarpetBomb(facing);
        }
    }
}
