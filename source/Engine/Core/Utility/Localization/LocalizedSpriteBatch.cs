
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;


namespace DataTypes.Core
{
    class LocalizedSpriteBatch : SpriteBatch
    {
        System.Collections.Generic.Dictionary<string,Texture2D> hashtable;

        public LocalizedSpriteBatch( GraphicsDevice graphicsDevice )
            : base( graphicsDevice )
        {
            hashtable = new System.Collections.Generic.Dictionary<string, Texture2D>();
            MapTextToObject( "A", @"Content\Art/GUI/A_Button" );
            MapTextToObject( "B", @"Content\Art/GUI/B_Button" );
            MapTextToObject( "X", @"Content\Art/GUI/X_Button" );
            MapTextToObject( "Y", @"Content\Art/GUI/Y_Button" );
        }


        public new void DrawString( SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth )
        {
            string firstPart, button, secondPart;

            if ( text != "" && text.Contains( "[" ) )
            {
                firstPart = text.Remove( text.IndexOf( "[" ), text.Length - text.IndexOf( "[" ) );
                button = text.Substring( text.IndexOf( "[" ) + 1, 1 );
                Texture2D buttonImage = ( Texture2D ) hashtable[button];

                secondPart = text.Substring( firstPart.Length + 3 );

                base.DrawString( spriteFont, firstPart, position, color, rotation, origin, scale, effects, layerDepth );

                if ( spriteFont.Equals( Engine.ContentManager.Instance.GetObject<SpriteFont>( @"Content/Art/Fonts/TitleFont" ) ) )
                    base.Draw( buttonImage, position + new Vector2( spriteFont.MeasureString( firstPart ).X, 0 ), new Color( color.R, color.G, color.B, 255 ) );
                else
                    base.Draw( buttonImage, position + new Vector2( spriteFont.MeasureString( firstPart ).X, -10 ), new Color( color.R, color.G, color.B, 255 ) );

                base.DrawString( spriteFont, secondPart, position + new Vector2( spriteFont.MeasureString( firstPart ).X + buttonImage.Width, 0 ), color, rotation, origin, scale, effects, layerDepth );
            }
            else
            {
                base.DrawString( spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth );
            }
        }

        public void MapTextToObject( String text, String image )
        {
            hashtable.Add( text, Engine.ContentManager.Instance.GetObject<Texture2D>( image ) );
        }
    }
}
