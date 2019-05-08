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
    class Napalm : DefaultExplosion<Napalm>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new Napalm(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected Napalm( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Placeable gp = new Placeable( Position.ToVector3(), Vector3.Zero, Vector3.One );
            base.AddComponent( gp );
            Drawable2D dss = new Drawable2D(true,-1);
            base.AddComponent( dss );

            int fps = 3;

            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\NapalmFireBig" ), new Rectangle( 0, 0, 64, 64 ), "Level1", fps);
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\NapalmFireMed" ), new Rectangle( 0, 0, 64, 64 ), "Level2", fps );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\NapalmFireSmall" ), new Rectangle( 0, 0, 64, 64 ), "Level3", fps );

            dss.PlayAnimation("Level1", true);
            base.AddComponent(dss);
            Physics2D c = new Physics2D( new Rectangle( 0, 0, 48, 58 ), OnCollision );
            dss.DrawLayer = (float) DrawLayer.LayerDepth.Explosions / 100.0f;

            //Position.Y = (Game1.Instance.GameManager.Level as DynamicLevel)[(int)Position.X] - (c.Height / 2) - 5;
            //gp.Position = Position;

            base.AddComponent( c );
            SoundManager.Instance.Play("napalm5");
            */ 
        }

        int totalDamage = 30;

        public Napalm(Vector2 Position)
            : base()
        {
            Placeable gp = new Placeable( Position.ToVector3(), Vector3.Zero, Vector3.One );
            base.AddComponent( gp );
            Drawable2D dss = new Drawable2D(true,-1);
            base.AddComponent( dss );

            int fps = 3;

            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\NapalmFireBig" ), new Rectangle( 0, 0, 64, 64 ), "Level1", fps);
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\NapalmFireMed" ), new Rectangle( 0, 0, 64, 64 ), "Level2", fps );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\NapalmFireSmall" ), new Rectangle( 0, 0, 64, 64 ), "Level3", fps );

            dss.PlayAnimation("Level1", true);
            base.AddComponent(dss);
            Physics2D c = new Physics2D( new Rectangle( 0, 0, 48, 58 ), OnCollision );
            dss.DrawLayer = (float) DrawLayer.LayerDepth.Explosions / 100.0f;

            //Position.Y = (Game1.Instance.GameManager.Level as DynamicLevel)[(int)Position.X] - (c.Height / 2) - 5;
            //gp.Position = Position;

            base.AddComponent( c );
            SoundManager.Instance.Play("napalm5");
        }
        public void OnCollision(BaseObject RHS)
        {
            if (RHS[Placeable.TypeStatic] != null)
            {
                if (RHS[HitPoints.TypeStatic] != null && (RHS[Physics2D.TypeStatic] as Physics2D).Moving)
                {
                    totalDamage -= 1;
                    (RHS[HitPoints.TypeStatic] as HitPoints).Hitpoints -= 1;
                    
                    switch(totalDamage)
                    {
                        case 20:
                            (base[Drawable2D.TypeStatic] as Drawable2D).PlayAnimation("Level2");
                            break;
                        case 10:
                            (base[Drawable2D.TypeStatic] as Drawable2D).PlayAnimation("Level3");
                            break;
                        case 0:
                            
                            Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
                            break;
                    }
                }
                else if (RHS is LargeExplosion)
                {
                    totalDamage -= 20;
                    if (totalDamage <= 0)
                    {
                       Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
                    }
                    else if (totalDamage <= 10)
                    {
                        (base[Drawable2D.TypeStatic] as Drawable2D).PlayAnimation("Level3");
                    }
                    else if (totalDamage <= 20)
                    {
                        (base[Drawable2D.TypeStatic] as Drawable2D).PlayAnimation("Level2");
                    }
                    else if (totalDamage <= 30)
                    {
                        (base[Drawable2D.TypeStatic] as Drawable2D).PlayAnimation("Level1");
                    }
                }
            }
        }
    }
}