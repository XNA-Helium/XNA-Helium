using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelBoard
{
    class TicTacToeMiniGame : MiniGameState
    {
        SpriteFont font;
        public override void Initialize()
        {
            font = Engine.ContentManager.Instance.GetObject<SpriteFont>(@"Content\Arial");
            base.Initialize();
        }


    }
}
