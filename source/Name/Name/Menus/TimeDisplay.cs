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
    public abstract class TimeDisplay : UIBaseMenu
    {
        protected double starttime;
        protected float TURNLENGTH;
        protected float TimeLeft = 0.0f;
        SpriteFont sf;

        public TimeDisplay(string BackgroundName, Point Position, double time, float TurnLength ):base(BackgroundName,Position)
        {
            starttime = time;
            TURNLENGTH = TurnLength;
            sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
        
        }
        public TimeDisplay( double time, float TurnLength ):base()
        {
            time = starttime;
            TURNLENGTH = TurnLength;
            sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
        
        }


        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            if ( TimeLeft == 0 )
            {
                starttime = gameTime.TotalGameTime.TotalSeconds;
                TimeLeft = TURNLENGTH;
            }
            TimeLeft = TURNLENGTH - ( float ) ( gameTime.TotalGameTime.TotalSeconds - starttime );
            String timestring = ( ( int ) TimeLeft ).ToString();
            spriteBatch.DrawString( sf, timestring, CameraManager.GetPosition( 0.5f, 0.075f ).ToVector2() - (sf.MeasureString( timestring )/2), Color.White,0.0f,Vector2.Zero,1.0f,SpriteEffects.None,1.0f );
           
            base.Draw( gameTime, spriteBatch );
        }
    }
}
