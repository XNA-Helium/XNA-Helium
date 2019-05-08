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
using Engine.UI;
using Engine.Core;

namespace Name.Menus
{
    public class JetpackMenu : TimeDisplay
    {
        PlayerObject player;
        JetPack j;
        bool active = false;
        public JetpackMenu( PlayerObject Player, double time, float TurnLength )
            : base( @"Content\Menu\BottomBackground", CameraManager.GetPosition( 0.0f, 0.8125f ), time, TurnLength )
        {
            player = Player;
            player.SwapWeapon( new JetPack( player ) );
            j = player.Weapon as JetPack;

            Point p = CameraManager.GetPosition( 0.46875f, 0.89375f );
            int width = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\Previous_active" ).Width;

            p.X -= width;
            UIButton b = new UIButton( @"Content\Menu\Previous", p );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Left, b );

            p.X += width + width;
            b = new UIButton( @"Content\Menu\Next", p);
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Right, b );


            b = new UIButton( @"Content\Menu\Up", CameraManager.GetPosition( 0.46875f, 0.84375f ) );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Up, b );

            b = new UIButton( @"Content\Menu\Down", CameraManager.GetPosition( 0.46875f, 0.94375f ) );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Down, b );


            b = new UIButton( @"Content\Menu\BackSmall", CameraManager.GetPosition( 0.0f, 0.9125f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
        }

        public override void OnShow( )
        {
            player.DeployAndShot();
            j.InUse = true;
            base.OnShow();
        }

        public override void OnHide( )
        {
            player.Retract();
            j.InUse = false;
            base.OnHide();
        }

        public override void Update( GameTime gameTime )
        {
            if ( !player.InWorld )
            {
                MenuSystemInstance.PopTopMenu();
                return;
            }

            if ( !active )
            {
                if ( j.TotalEnergy <= 0  || !player.InWorld)
                {
                    MenuSystemInstance.PopTopMenu();
                    return;
                }
                ( player[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 0.0f, -0.9f ).ToVector3();
            }

            base.Update( gameTime );
            j.SetAnimation(true); // active);
            active = false;

            if ( player.InWorld )
            {
                Point point = player.Position.ToPoint();
                CameraManager.Instance.Current.CenterOn(ref point );
            }
        }

        void Down( )
        {
            if ( j.Use( 1.0f ) )
            {
                ( player[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 0.0f, 2.0f ).ToVector3();
                active = true;
            }
            else
            {
                MenuSystemInstance.PopTopMenu();
            }
        }

        void Up( )
        {
            if ( j.Use( 1.0f ) )
            {
                ( player[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 0.0f, -2.0f ).ToVector3();
                active = true;
            }
            else
            {
                MenuSystemInstance.PopTopMenu();
            }
        }

        void Left( )
        {
            if ( j.Use( 1.0f ) )
            {
                player.Stand( Facing.Left );
                ( player[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( -2.0f, 0.0f ).ToVector3();
                active = true;
            }
            else
            {
                MenuSystemInstance.PopTopMenu();
            }
        }

        void Right( )
        {
            if ( j.Use( 1.0f ) )
            {
                player.Stand( Facing.Right );
                ( player[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 2.0f,0.0f ).ToVector3();
                active = true;
            }
            else
            {
                MenuSystemInstance.PopTopMenu();
            }
        }
    }
}
