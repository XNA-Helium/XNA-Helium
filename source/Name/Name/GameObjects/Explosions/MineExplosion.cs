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

    class MineExplosion : DefaultExplosion<MineExplosion>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new MineExplosion(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected MineExplosion( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Placeable gp = new Placeable( Position.ToVector3(), Vector3.Zero, Vector3.One );
            Drawable2D dss = new Drawable2D( true, -1 );
            base.AddComponent( gp );
            base.AddComponent( dss );
            int fps = 3;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\Explosion3" ), new Rectangle( 0, 0, 128, 128 ), "Explosion3", fps );
            dss.PlayAnimation( "Explosion3" );

            Physics2D c = new Physics2D( new Rectangle( 0, 0, 64, 96 ), OnCollision );
            base.AddComponent( c );

            dss.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            EventManager.Instance.AddEvent( RemoveMeInTime, ( float ) deletein / 30.0f );

            SoundManager.Instance.Play( "explosion" );
            */
        }

        int deletein = 24;

        public MineExplosion(Vector2 Position)
            : base()
        {
            Placeable gp = new Placeable(Position.ToVector3(),Vector3.Zero,Vector3.One);
            Drawable2D dss = new Drawable2D(true,-1);
            base.AddComponent(gp);
            base.AddComponent(dss);
            int fps = 3;
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Explosions\Explosion3"), new Rectangle(0, 0, 128, 128), "Explosion3", fps);
            dss.PlayAnimation("Explosion3");

            Physics2D c = new Physics2D( new Rectangle( 0, 0, 64, 96 ), OnCollision );
            base.AddComponent(c);

            dss.DrawLayer = (float) DrawLayer.LayerDepth.Explosions / 100.0f;
            
            EventManager.Instance.AddEvent( RemoveMeInTime, ( float ) deletein / 30.0f );

            SoundManager.Instance.Play("explosion");
        }

        public void OnCollision( BaseObject RHS )
        {
            collided = true;
            if ( RHS[Placeable.TypeStatic] != null )
            {
                if ( RHS[HitPoints.TypeStatic] != null )
                {
                    int Damage = 0;
                    Vector2 RHS_Position = ( RHS[Placeable.TypeStatic] as Placeable ).Position.ToVector2();
                    Vector2 This_Position = ( this[Placeable.TypeStatic] as Placeable ).Position.ToVector2();
                    Vector2 distance = RHS_Position - This_Position;
                    int td = Math.Abs( ( int ) distance.X ); //.Length();
                    if ( td > 64 )
                    {
                        td = 64;
                    }
                    if ( td < 0 )
                    {
                        td = 0;
                    }
                    Damage = 64 - td;
                    Damage /= 3;
                    Damage += 24;

                    Damage /= 2; //Damage gets applied 2x, so we  divide by 2 to "hack-fix" the bug

                    ( RHS[HitPoints.TypeStatic] as HitPoints ).Hitpoints -= Math.Abs( Damage );
                    ( RHS[Physics2D.TypeStatic] as Physics2D ).Moving = true;
                }
            }
        }
    }
}