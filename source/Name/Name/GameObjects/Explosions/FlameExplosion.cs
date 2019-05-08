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
    class FlameExplosion : BaseObjectStreamingHelper<FlameExplosion>
    {
        
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new FlameExplosion(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected FlameExplosion( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Placeable p = new Placeable( true, -1 );
            //Drawable2D r = new Drawable2D(true,-1);
            base.AddComponent( p );
            //base.AddComponent( r );
            p.Position = ( Position + new Vector2( 0, 0 ) ).ToVector3();
            //r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            //r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\Reticle" ), "FlameDefault" );
            //r.PlayAnimation( "FlameDefault" );


            Physics2D ph = new Physics2D( new Rectangle( 0, 0, 128, 64 ), OnCollision );
            ph.Static = true;
            base.AddComponent( ph );
            */
        }

        //int deletein = 24;
        int totaldamage = 35;
        public FlameExplosion(Vector2 Position)
            : base()
        {

            Placeable p = new Placeable(true,-1);
            //Drawable2D r = new Drawable2D(true,-1);
            base.AddComponent( p );
            //base.AddComponent( r );
            p.Position = ( Position + new Vector2( 0, 0 ) ).ToVector3();
            //r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            //r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\Reticle" ), "FlameDefault" );
            //r.PlayAnimation( "FlameDefault" );


            Physics2D ph = new Physics2D( new Rectangle( 0, 0, 128, 64 ), OnCollision );
            ph.Static = true;
            base.AddComponent( ph );
            
        }

        public void OnCollision(BaseObject rhs)
        {
            if ( rhs[HitPoints.TypeStatic] != null && totaldamage > 0 )
            {
                ( rhs[HitPoints.TypeStatic] as HitPoints ).Hitpoints -= 1;
                --totaldamage;
            }
        }
    }
}