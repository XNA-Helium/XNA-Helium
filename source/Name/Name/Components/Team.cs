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
    public static class Team
    {
        public static string NoTeam = "NoTeam";
        public static string Team1 = "Team1";
        public static string Team2 = "Team2";
        public static string Team3 = "Team3";
        public static string Team4 = "Team4";


        public static string TeamName(int num )
        {
            switch ( num )
            {
                case 0:
                    return Team1;
                case 1:
                    return Team2;
                case 2:
                    return Team3;
                case 3:
                    return Team4;
            }
            return NoTeam;
        }

        public static Color TeamColor(int num)
        {
            switch (num)
            {
                case 0:
                    return Color.Cyan;
                case 1:
                    return Color.Pink;
                case 2:
                    return Color.LightGreen;
                case 3:
                    return new Color(255, 255, 100);
            }
            return Color.White;
        }
    }
}
