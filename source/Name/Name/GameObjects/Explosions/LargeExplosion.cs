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

    class LargeExplosion : DefaultExplosion<LargeExplosion>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new LargeExplosion(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected LargeExplosion( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Placeable p = new Placeable( true, -1 );
            Drawable2D r = new Drawable2D( true, -1 );
            base.AddComponent( p );
            base.AddComponent( r );
            p.Position = ( Position ).ToVector3();
            r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            int fps = 3;

            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\Explosion1" ), new Rectangle( 0, 0, 256, 256 ), "Explosion1", fps );

            r.PlayAnimation( "Explosion1" );

            base.AddComponent( r );

            Physics2D c = new Physics2D( new Rectangle( 0, 0, 192, 256 ), OnCollision );
            base.AddComponent( c );
            c.Moving = true;

            EventManager.Instance.AddEvent( RemoveMeInTime, ( float ) deletein / 30.0f );
            SoundManager.Instance.Play( "explosion" );
            */
        }

        int deletein = 33;
        public LargeExplosion(Vector2 Position)
            : base()
        {

            Placeable p = new Placeable(true,-1);
            Drawable2D r = new Drawable2D(true,-1);
            base.AddComponent( p );
            base.AddComponent( r );
            p.Position = ( Position ).ToVector3();
            r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            int fps = 3;

            r.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Explosions\Explosion1"), new Rectangle(0, 0, 256, 256), "Explosion1", fps);

            r.PlayAnimation("Explosion1");

            base.AddComponent(r);

            Physics2D c = new Physics2D( new Rectangle( 0, 0, 192, 256 ), OnCollision );
            base.AddComponent(c);
            c.Moving = true;

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
                    if ( td > 196 )
                    {
                        td = 196;
                    }
                    if ( td < 0 )
                    {
                        td = 0;
                    }
                    Damage = 196 - td;
                    Damage /= 4; // 0 to 49
                    ++Damage; //out of 50
                    
                    // --------------------------------
                    //@@ This goes off 2x
                    Damage /= 2;
                    // --------------------------------

                    ( RHS[HitPoints.TypeStatic] as HitPoints ).Hitpoints -= Math.Abs( Damage );
                    ( RHS[Physics2D.TypeStatic] as Physics2D ).Moving = true;
                }
            }
        }
    } 
}