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

namespace TBS.Menus
{
    class GamePlayMenu : UIBaseMenu
    {
        bool UseInput = false;

        public GamePlayMenu(int x, int y)
            : this()
        {

        }

        public GamePlayMenu()
            : base()
        {
            Engine.GameState.GameStateSystem.Instance.AddGameState(new GameState.PlayingState()); 
            int i, j, k;
            i = j = k = 0;

            //for (i = -5; i <= 5; ++i)
            {
                //for (j = -5; j <= 5; ++j)
                {
                    //for (k = -5; k <= 5; ++k)
                    {
                        TBSObject b = new TBSObject(new Vector3(i, j, k));
                    }
                }
            }

            (CameraManager.Instance.Current as Camera3D).Position = new Vector3(7f, 1f, 0f);
        }

        public override void ProcessInput(GameTime gameTime, ref Engine.TouchCollection touches)
        {
            UseInput = false;
            base.ProcessInput(gameTime, ref touches);
            if (!MenuUsedInput && touches.Count > 0)
            {
                CameraManager.Instance.Current.ProcesInput(gameTime, ref touches);
                UseInput = true;
            }
        }
    }
}
