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
    class HealthPack : BaseObjectStreamingHelper<HealthPack>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new HealthPack(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected HealthPack( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Used = false;
            _Hitpoints = points;


            Placeable gp = new Placeable( true, -1 );
            base.AddComponent( gp );
            gp.Position = Position.ToVector3();

            Drawable2D dsa = new Drawable2D( true, -1 );
            base.AddComponent( dsa );
            int fps = 1;
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Items\HealthPack" ), new Rectangle( 0, 0, 64, 64 ), "healthPackAnim", fps );
            dsa.PlayAnimation( "healthPackAnim" );
            dsa.DrawLayer = ( float ) DrawLayer.LayerDepth.HealthPacks / 100.0f;

            Physics2D c = new Physics2D( new Rectangle( 0, 0, 64, 64 ), OnCollision );
            base.AddComponent( c );
            */ 
        }

        int _Hitpoints;
        bool Used;
        
        public HealthPack(int points,Vector2 Position):base()
        {
            Used = false;
            _Hitpoints = points;


            Placeable gp = new Placeable(true,-1);
            base.AddComponent( gp );
            gp.Position = Position.ToVector3();

            Drawable2D dsa = new Drawable2D( true, -1 );
            base.AddComponent( dsa );            
            int fps = 1;
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Items\HealthPack" ), new Rectangle( 0, 0, 64, 64 ), "healthPackAnim", fps );
            dsa.PlayAnimation("healthPackAnim");
            dsa.DrawLayer = (float) DrawLayer.LayerDepth.HealthPacks / 100.0f;

            Physics2D c = new Physics2D(new Rectangle(0,0,64,64),OnCollision);
            base.AddComponent(c);
           
        }

        public void OnCollision(BaseObject Collider)
        {
            if (!Used)
            {
                if (Collider[Physics2D.TypeStatic] != null)
                {
                    if (Collider[HitPoints.TypeStatic] != null)
                    {
                        (Collider[HitPoints.TypeStatic] as HitPoints).Hitpoints += _Hitpoints;
                        SoundManager.Instance.Play("pickup");
                        EventManager.Instance.AddEvent( RemoveMeInTime, 0.0f );
                        Used = true;
                    }
                }
            }
        }
    }

}
