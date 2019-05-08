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

using Name.GameState;

namespace Name.Menus
{
    //@@ Refactor AITeamMenu and GamePlayMenu
    //@@ Shared
    class AITeamMenu : SharedGamePlayMenu , IEngineUpdateable
    {
        Vector2 ShootOrigin = new Vector2( 0f, 16f );
        Texture2D ShootTexture = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\ShootingCone" );
        
        BaseAIFSM fsm;
        double starttime;
        GameOptions Options;
        bool ClassicMode = false;
        public AITeamMenu( PlayerObject Player, NameTeam team, double time, GameOptions options )
            : base(Player,team,time,options.TurnLength)
        {
            player = Player;

            if ( !Player.InWorld )
                return;

            Options = options;
            if ( options.ShootMode == ShootMode.Classic )
            {
                ClassicMode = true;
            }
            fsm = new BaseAIFSM( player, Team, options );

            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToUpdateList( this );
        
        }


        public override void Update( GameTime gameTime )
        {
            if ( !player.InWorld )
            {
                NoLongerInWorld();
            }

            fsm.Update( (float) gameTime.TotalGameTime.TotalSeconds );
            base.Update( gameTime );
        }

        public override bool ValidationCallback( )
        {
            return player.InWorld;
        }

        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            base.Draw( gameTime, spriteBatch ); 
            
            Vector2 ZeroPoint = CameraManager.GetPosition( 0.5f, 0.825f ).ToVector2();

            ZeroPoint.X -= Engine.ContentManager.Instance.GetObject<Texture2D>( "MiniMap" ).Width / 2;

            Texture2D YouDot = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\MiniMapYou" );
            Texture2D pack = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\HealthPack" );
            Texture2D fire = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\fire" );
            Texture2D mine = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\mine" );
            Texture2D WhiteDot = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\WhiteDot" );
            String timestring = ( ( int ) TimeLeft ).ToString();
            spriteBatch.DrawString( sf, timestring, CameraManager.GetPosition( 0.5f, 0.075f ).ToVector2() - (sf.MeasureString( timestring )/2), Team.Color );
            spriteBatch.Draw( MiniMap, ZeroPoint, null, Name.GameState.Terrain.BackGround, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.2f );

            foreach ( BaseObject o in Engine.GameState.GameStateSystem.Instance.GetCurrentState.ObjectList )
            {
                if ( o.InWorld == false )
                {

                }
                else if ( o == player )
                    Draw( o, YouDot, ZeroPoint, spriteBatch );
                else if ( o is PlayerObject )
                    Draw( o, WhiteDot, ZeroPoint, spriteBatch, ( o[Drawable2D.TypeStatic] as Drawable2D ).Color );
                else if ( o is HealthPack )
                    Draw( o, pack, ZeroPoint, spriteBatch );
                else if ( o is Napalm )
                    Draw( o, fire, ZeroPoint, spriteBatch );
                else if ( o is LandMine )
                    Draw( o, mine, ZeroPoint, spriteBatch );
            }

            float velocity =  fsm.IsShooting;
            if ( ClassicMode && velocity > 0f)
            {
                Rectangle p_Screen = CameraManager.Instance.Current.Rectangle;
                Vector2 Position = ( player.Weapon[Placeable.TypeStatic] as Placeable ).Position.ToVector2();
                Rectangle target = new Rectangle();
                Vector2 offset = Vector2.Zero;
                float angle = player.Weapon.Rotation;

                float Value = 35;
                if ( player.Facing == Facing.Left )
                {
                    offset = new Vector2( ( float ) -Value * ( float ) Math.Cos( angle ), ( float ) -Value * ( float ) Math.Sin( angle ) );
                    SpriteEffects effects = SpriteEffects.FlipVertically;
                    target = new Rectangle( ( int ) ( Position.X + offset.X - ( float ) p_Screen.X ), ( int ) ( Position.Y + offset.Y - ( float ) p_Screen.Y ), ( int ) 32f + ( int ) ( velocity * 2 ), ( int ) 32 );
                    spriteBatch.Draw( ShootTexture, target, null, Color.White, player.Weapon.Rotation + ( float ) ( Math.PI ), ShootOrigin, effects, 0.3f );
                }
                else if ( player.Facing == Facing.Right )
                {
                    SpriteEffects effects = SpriteEffects.None;
                    offset = new Vector2( ( float ) Value * ( float ) Math.Cos( angle ), ( float ) Value * ( float ) Math.Sin( angle ) );
                    target = new Rectangle( ( int ) ( Position.X + offset.X - ( float ) p_Screen.X ), ( int ) ( Position.Y + offset.Y - ( float ) p_Screen.Y ), ( int ) 32f + ( int ) ( velocity * 2 ), ( int ) 32 );
                    spriteBatch.Draw( ShootTexture, target, null, Color.White, player.Weapon.Rotation, ShootOrigin, effects, 0.3f );
                }
            }
        }
        protected void Draw( BaseObject o, Texture2D texture, Vector2 basePoint, SpriteBatch spriteBatch, Color c )
        {
            Vector2 p = ( o[Placeable.TypeStatic] as Placeable ).Position.ToVector2() / 10;
            spriteBatch.Draw( texture, basePoint + p, null, c, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 0.1f );
        }
        protected void Draw( BaseObject o, Texture2D texture, Vector2 basePoint, SpriteBatch spriteBatch )
        {
            Vector2 p = ( o[Placeable.TypeStatic] as Placeable ).Position.ToVector2() / 10;
            spriteBatch.Draw( texture, basePoint + p, null, Color.White, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 0.1f );
        }

        public override void OnRemove( )
        {
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromUpdateList( this );
            
            base.OnRemove();
        }
    }

}