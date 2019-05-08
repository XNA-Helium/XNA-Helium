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
    class WeaponExplosion :  DefaultExplosion<WeaponExplosion>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new WeaponExplosion(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected WeaponExplosion( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Placeable p = new Placeable(true, -1);
            base.AddComponent( p );
            Drawable2D r = new Drawable2D(true, -1);
            p.Position = ( Position + new Vector2( 0, -25 ) ).ToVector3();
            base.AddComponent( r );

            int fps = 3;
            deletein = 10 * fps;
            r.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Explosions\Explosion2"), new Rectangle(0, 0, 128, 128), "Explosion2", fps);
            r.PlayAnimation("Explosion2");
            r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            EventManager.Instance.AddEvent( RemoveMeInTime, (float)deletein / 30.0f);
            SoundManager.Instance.Play("explosion");
            */
        }

        int deletein;

        public WeaponExplosion(Vector2 Position): base()
        {
            Placeable p = new Placeable(true, -1);
            base.AddComponent( p );
            Drawable2D r = new Drawable2D(true, -1);
            p.Position = ( Position + new Vector2( 0, -25 ) ).ToVector3();
            base.AddComponent( r );

            int fps = 3;
            deletein = 10 * fps;
            r.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Explosions\Explosion2"), new Rectangle(0, 0, 128, 128), "Explosion2", fps);
            r.PlayAnimation("Explosion2");
            r.DrawLayer = ( float ) DrawLayer.LayerDepth.Explosions / 100.0f;

            EventManager.Instance.AddEvent( RemoveMeInTime, (float)deletein / 30.0f);
            SoundManager.Instance.Play("explosion");
        }
    }
}
