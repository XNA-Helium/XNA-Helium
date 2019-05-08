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
    class SmallExplosion : DefaultExplosion<SmallExplosion>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new SmallExplosion(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected SmallExplosion( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Placeable p = new Placeable( true, -1 );
            Drawable2D r = new Drawable2D( true, -1 );
            base.AddComponent( p );
            base.AddComponent( r );
            p.Position = ( Position + new Vector2( 0, -25 ) ).ToVector3();
            r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            int fps = 3;
            deletein = 10 * fps;
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\Explosion3" ), new Rectangle( 0, 0, 128, 128 ), "Explosion3", fps );
            r.PlayAnimation( "Explosion3" );

            EventManager.Instance.AddEvent( RemoveMeInTime, ( float ) deletein / 30.0f );

            Physics2D ph = new Physics2D( new Rectangle( 0, 0, 64, 96 ), OnCollision );
            ph.Static = true;
            base.AddComponent( ph );

            SoundManager.Instance.Play( "explosion" );
            */
        }

        int deletein = 24;

        public SmallExplosion(Vector2 Position)
            : base()
        {
            Placeable p = new Placeable(true,-1);
            Drawable2D r = new Drawable2D(true,-1);
            base.AddComponent( p );
            base.AddComponent( r );
            p.Position = ( Position + new Vector2( 0, -25 ) ).ToVector3();
            r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            int fps = 3;
            deletein = 10 * fps;
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\Explosion3" ), new Rectangle( 0, 0, 128, 128 ), "Explosion3", fps );
            r.PlayAnimation( "Explosion3" );

            EventManager.Instance.AddEvent( RemoveMeInTime, ( float ) deletein / 30.0f );

            Physics2D ph = new Physics2D( new Rectangle( 0, 0, 64, 96 ), OnCollision );
            ph.Static = true;
            base.AddComponent(ph);

            SoundManager.Instance.Play("explosion");
        }
        public void OnCollision( BaseObject RHS )
        {
            collided = true;
            if ( RHS[Physics2D.TypeStatic] != null && !(RHS is SmallExplosion))
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
                    Damage /= 10;
                    Damage += 5;
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