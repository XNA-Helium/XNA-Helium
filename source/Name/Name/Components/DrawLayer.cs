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
using Engine.Core;

namespace Name
{
    class DrawLayer
    {
        public enum LayerDepth { Background = 99, PlayerHitPoints = 95, HealthPacks = 85, Players = 80, Projectiles = 75, Explosions = 70, Weapons = 65, ArrowAbovePlayer = 64, MiniMap = 59, MiniMapRectangle = 57, HUD = 56, GreenPlayerDot = 54, MiniMapDot = 53, CollectablesDot = 52, MainScreenOverlay = 50, TeamHealthBar = 49, MenuBackground = 39, TutorialBackground = 38, TutorialText = 37, BetweenMenu = 35, MenuButton = 30, ScreenText = 25, DebugLayer = 19 };
    }
}
