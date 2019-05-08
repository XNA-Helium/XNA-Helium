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

namespace Breakfree
{
    public class MainMenu : UIBaseMenu
    {
        public MainMenu()
            : base(@"Content\MainMenuBackground", new Point(0, 0))
        {
            UIButton b = new UIButton(@"Content\Start", CameraManager.GetPosition(0.025f, 0.25f));
            b.ValidStates = (int)TouchStates.Released;
            Add(Start, b);
        }

        protected void Start()
        {
            MenuSystem.Instance.PopTopMenu();
            PlayingState ps = new PlayingState();
            Engine.GameState.GameStateSystem.Instance.AddGameState(ps);
            ps.Setup();

            Engine.MenuSystem.Instance.PushMenu(new GameMenu());
        }
    }
}
