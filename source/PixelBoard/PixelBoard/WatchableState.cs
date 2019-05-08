using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.GameState;

using Microsoft.Xna.Framework;

namespace PixelBoard
{
    class WatchableState : PrimaryGameState
    {
        public delegate void WatchableStateCallback(WatchableState state);

        public WatchableStateCallback GameStateStarted;
        public WatchableStateCallback GameStateEnded;

        public override void Initialize()
        {
            if (GameStateStarted != null)
                GameStateStarted(this);

            base.Initialize();
        }

        public override void Shutdown()
        {
            if (GameStateEnded != null)
                GameStateEnded(this);

            base.Shutdown();
        }
    }
}
