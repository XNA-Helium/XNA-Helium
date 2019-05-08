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

namespace TBS
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main( string[] args )
        {
            using ( EngineGame game = new TBSGame() )
            {
                game.Run();
            }
        }
    }
#endif

    public class TBSGame : EngineGame
    {
        protected override void LoadGameContent( )
        {
            CameraManager.Instance.Current = new Camera3D();

            Engine.ContentManager.Instance.Load<Texture2D>( @"SharedContent\Background" );
        }

        protected override Engine.UI.BaseMenu FirstMenu( )
        {
            return new TBS.Menus.GamePlayMenu();
            //return new TBS.Menus.MainMenu();
        }

    }
}
