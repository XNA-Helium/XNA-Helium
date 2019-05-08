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

using Engine.Core;
using Engine;
using Engine.UI;
using Name.GameState;

namespace Name.Menus
{
    public class AimAndShootMenu : TimeDisplay
    {
        Vector2 ShootOrigin = new Vector2(0f,16f);
        Texture2D ShootTexture = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\ShootingCone");
        SpriteFont sf = Engine.ContentManager.Instance.GetObject<SpriteFont>(@"SharedContent\TextboxFont");
        Texture2D MiniMap = Engine.ContentManager.Instance.GetObject<Texture2D>("MiniMap");
        
        System.Random rand = new Random();
        Texture2D ie;
        bool wasTouched;
        bool ThisFrame;
        PlayerObject player;
        NameTeam Team;
        bool HasShot = false;
        bool ClassicMode = false;

        public AimAndShootMenu(bool classicMode, PlayerObject Player, NameTeam team, double time, float TurnLength )
            : base( @"Content\Menu\BottomBackground", CameraManager.GetPosition( 0.0f, 0.8125f ), time,TurnLength )
        {
            ClassicMode = classicMode;
            Team = team;
            player = Player;
            wasTouched = false;
            ThisFrame = false;

            ie =  Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\Reticle" );
            origin = new Vector2( ie.Width / 2, ie.Width / 2 );

            UIButton b = new UIButton( @"Content\Menu\Reticle", CameraManager.GetPosition( 0.28125f, 0.88125f ) );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( LaunchShootMenu, b );

            b = new UIButton( @"Content\Menu\Up", CameraManager.GetPosition( 0.61f, 0.85f ) );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Up, b );

            b = new UIButton( @"Content\Menu\Down", CameraManager.GetPosition( 0.61f, 0.94f ) );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Down, b );


            b = new UIButton( @"Content\Menu\BackSmall", CameraManager.GetPosition( 0.03f, 0.89f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );

            Point p = player.Position.ToPoint();
            CameraManager.Instance.Current.CenterOn(ref p);
        }

        public override void OnAdd( )
        {
            player.DeployAndShot();
            base.OnAdd();
        }

        public override void OnRemove( )
        {
            player.Retract();
            base.OnRemove();
        }

        public override void ProcessInput( GameTime gameTime, ref Engine.TouchCollection touches )
        {
            ThisFrame = false;
            base.ProcessInput( gameTime, ref touches );
            if ( wasTouched  && !ThisFrame)
            {
                Team.WeaponList.Decriment( player.Weapon );
                IProjectile po = player.Weapon.Fire( currentFiringVelocity );
                Engine.DebugHelper.Break( MenuSystemInstance.CurrentMenu != this, DebugHelper.DebugLevels.Critical );
                HasShot = true;
                MenuSystemInstance.PushMenu( new ProjectileTracker( po ) );
            }
        }

        float currentFiringVelocity = 0.0f;
        void LaunchShootMenu( )
        {


            if ( FiringVelocity >= 90.0f || player.Weapon is FlameThrower )
            {
                SoundManager.Instance.Play("flamethrower");
                float jitter = ( float ) ( rand.NextDouble() * ( 3.0 * Utility.DegreeToRadian ) - ( 1.5 * Utility.DegreeToRadian ) );
                player.Weapon.Rotation += jitter;
                ThisFrame = false;
                wasTouched = true;
                return;
            }

            ThisFrame = true;
            wasTouched = true;
            currentFiringVelocity  += FiringVelocity;
            if (currentFiringVelocity > 89.0f)
            {
                currentFiringVelocity = 89.0f;
            }
            {
                //If the launcher is not yet deployed this looks bad
                position = ( player.Weapon[Placeable.TypeStatic] as Placeable ).Position.ToVector2();
                Terrain t = ( Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState ).Terrain;
                Vector2 t_gravity = Physic2DManagerWithTerrain.gravity.ToVector2();
                Vector2 t_power = Vector2.Zero;
                {
                    Facing t_f = player.Facing;
                    float angle = player.Weapon.Rotation;
                    if ( t_f == Facing.Right )
                    {
                        t_power = new Vector2( ( float ) currentFiringVelocity * ( float ) Math.Cos( angle ), ( float ) currentFiringVelocity * ( float ) Math.Sin( angle ) );
                    }
                    if ( t_f == Facing.Left )
                    {
                        t_power = new Vector2( ( float ) -currentFiringVelocity * ( float ) Math.Cos( angle ), ( float ) -currentFiringVelocity * ( float ) Math.Sin( angle ) );
                    }
                }

                while ( t[( int ) position.X] > position.Y )
                {
                    t_power += t_gravity;
                    position += t_power;
                }
                int total = 0;
                for ( int i = -10; i <= 10; ++i )
                {
                    int xthing = ( int ) ( i * 7.5 ) + ( int ) position.X;
                    total += t[xthing];
                }
                total /= 21;
                position.Y = total;
                Point point = position.ToPoint();
                if(!ClassicMode)
                    CameraManager.Instance.Current.CenterOn(ref point );
            }
        }

        public static float RotValue = .10f;
        public static float MaxValue = 1.5f;
        public static float MinValue = -1.5f;
        void Up( )
        {
            if (player.Facing == Engine.Core.Facing.Left)
            {
                player.Weapon.Rotation += RotValue;

                if ( player.Weapon.Rotation >= MaxValue )
                {
                    player.Weapon.Rotation = MaxValue;
                }
                else if ( player.Weapon.Rotation <= MinValue )
                {
                    player.Weapon.Rotation = MinValue;
                }

                if ( player.Weapon is FlameThrower ) // to prevent torching self
                {
                    if ( player.Weapon.Rotation >= Math.PI / 7.35f )
                    {
                        player.Weapon.Rotation = ( float ) Math.PI / 7.35f;
                    }
                    else if ( player.Weapon.Rotation <= -1.5f )
                    {
                        player.Weapon.Rotation = -1.5f;
                    }
                }
            }
            else if (player.Facing == Engine.Core.Facing.Right)
            {
                player.Weapon.Rotation -= RotValue;
                if (player.Weapon.Rotation >= MaxValue)
                {
                    player.Weapon.Rotation = MaxValue;
                }
                else if (player.Weapon.Rotation <= MinValue)
                {
                    player.Weapon.Rotation = MinValue;
                }

                if (player.Weapon is FlameThrower) // to prevent torching self
                {
                    if (player.Weapon.Rotation >= Math.PI / 7.35f)
                    {
                        player.Weapon.Rotation = (float)Math.PI / 7.35f;
                    }
                    else if ( player.Weapon.Rotation <= Math.PI / -7.35f )
                    {
                        player.Weapon.Rotation = ( float ) Math.PI / -7.35f;
                    }
                }
            }
        }

        public static float FiringVelocity = 0.15f;
        void Down( )
        {
            if ( player.Facing == Engine.Core.Facing.Left )
            {
                player.Weapon.Rotation -= RotValue;

                if ( player.Weapon.Rotation >= MaxValue )
                {
                    player.Weapon.Rotation = MaxValue;
                }
                else if ( player.Weapon.Rotation <= MinValue )
                {
                    player.Weapon.Rotation = MinValue;
                }

                if ( player.Weapon is FlameThrower ) // to prevent torching self
                {
                    if ( player.Weapon.Rotation >= Math.PI / 7.35f )
                    {
                        player.Weapon.Rotation = ( float ) Math.PI / 7.35f;
                    }
                    else if ( player.Weapon.Rotation <= Math.PI / -7.35f )
                    {
                        player.Weapon.Rotation = (float) Math.PI / -7.35f;
                    }
                }
            }
            else if ( player.Facing == Engine.Core.Facing.Right )
            {
                player.Weapon.Rotation += RotValue;
                if ( player.Weapon.Rotation >= MaxValue )
                {
                    player.Weapon.Rotation = MaxValue;
                }
                else if ( player.Weapon.Rotation <= MinValue )
                {
                    player.Weapon.Rotation = MinValue;
                }

                if ( player.Weapon is FlameThrower ) // to prevent torching self
                {
                    if ( player.Weapon.Rotation >= Math.PI / 7.35f )
                    {
                        player.Weapon.Rotation = ( float ) Math.PI / 7.35f;
                    }
                    else if ( player.Weapon.Rotation <= MinValue )
                    {
                        player.Weapon.Rotation = MinValue;
                    }
                }
            }
        }

        Vector2 origin = new Vector2();
        Vector2 position = new Vector2();
        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            base.Draw( gameTime, spriteBatch );
            if (ThisFrame)
            {
                if (!ClassicMode)
                {
                    spriteBatch.Draw(ie, position - CameraManager.Instance.Current.Rectangle.Location.ToVector2(), null, Color.White, 0.0f, origin, 1 + (currentFiringVelocity / 30.0f), SpriteEffects.None, 0.01f);
                }
                else //ClassicMode  == true
                {
                    Rectangle p_Screen = CameraManager.Instance.Current.Rectangle;
                    Vector2 Position = (player.Weapon[Placeable.TypeStatic] as Placeable).Position.ToVector2();
                    Rectangle target = new Rectangle();
                    Vector2 offset = Vector2.Zero;
                    float angle = player.Weapon.Rotation;

                    float Value = 35;
                    if (player.Facing == Facing.Left)
                    {
                        offset = new Vector2((float)-Value * (float)Math.Cos(angle), (float)-Value * (float)Math.Sin(angle));
                        SpriteEffects effects = SpriteEffects.FlipVertically;
                        target = new Rectangle((int)(Position.X + offset.X - (float)p_Screen.X), (int)(Position.Y + offset.Y - (float)p_Screen.Y), (int)32f + (int)(currentFiringVelocity * 2), (int)32);
                        spriteBatch.Draw(ShootTexture, target, null, Color.White, player.Weapon.Rotation + (float)(Math.PI), ShootOrigin, effects, 0.3f);
                    }
                    else if (player.Facing == Facing.Right)
                    {
                        SpriteEffects effects = SpriteEffects.None;
                        offset = new Vector2((float)Value * (float)Math.Cos(angle), (float)Value * (float)Math.Sin(angle));
                        target = new Rectangle((int)(Position.X + offset.X - (float)p_Screen.X), (int)(Position.Y + offset.Y - (float)p_Screen.Y), (int)32f + (int)(currentFiringVelocity * 2), (int)32);
                        spriteBatch.Draw(ShootTexture, target, null, Color.White, player.Weapon.Rotation, ShootOrigin, effects, 0.3f);
                    }

                }
            }
            Vector2 ZeroPoint = CameraManager.GetPosition(0.5f, 0.675f).ToVector2();

            ZeroPoint.X -= Engine.ContentManager.Instance.GetObject<Texture2D>("MiniMap").Width / 2;

            Texture2D YouDot = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\MiniMap\MiniMapYou");
            Texture2D pack = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\MiniMap\HealthPack");
            Texture2D fire = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\MiniMap\fire");
            Texture2D mine = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\MiniMap\mine");
            Texture2D WhiteDot = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\MiniMap\WhiteDot");
            String timestring = ((int)TimeLeft).ToString();
            spriteBatch.DrawString(sf, timestring, CameraManager.GetPosition(0.5f, 0.075f).ToVector2() - (sf.MeasureString(timestring) / 2), Team.Color);
            spriteBatch.Draw(MiniMap, ZeroPoint, null, Name.GameState.Terrain.BackGround, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.2f);

            foreach ( BaseObject o in Engine.GameState.GameStateSystem.Instance.GetCurrentState.ObjectList )
            {
                if (o.InWorld == false)
                {

                }
                else if (o == player)
                    Draw(o, YouDot, ZeroPoint, spriteBatch);
                else if (o is PlayerObject)
                    Draw(o, WhiteDot, ZeroPoint, spriteBatch, (o[Drawable2D.TypeStatic] as Drawable2D).Color);
                else if (o is HealthPack)
                    Draw(o, pack, ZeroPoint, spriteBatch);
                else if (o is Napalm)
                    Draw(o, fire, ZeroPoint, spriteBatch);
                else if (o is LandMine)
                    Draw(o, mine, ZeroPoint, spriteBatch);
            }
        }
        protected void Draw( BaseObject o, Texture2D texture, Vector2 basePoint, SpriteBatch spriteBatch, Color c )
        {
            Vector2 p = (o[Placeable.TypeStatic] as Placeable).Position.ToVector2() / 10;
            spriteBatch.Draw(texture, basePoint + p, null, c, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 0.1f);
        }
        protected void Draw(BaseObject o, Texture2D texture, Vector2 basePoint, SpriteBatch spriteBatch)
        {
            Vector2 p = (o[Placeable.TypeStatic] as Placeable).Position.ToVector2() / 10;
            spriteBatch.Draw(texture, basePoint + p, null, Color.White, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 0.1f);
        }

        public override bool ValidationCallback()
        {
            return !HasShot;// base.ValidationCallback();
        }
    }
}
