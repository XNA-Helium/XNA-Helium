using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using Engine;
using Engine.GameState;
using Engine.Core;

namespace Name.GameState
{
    public class AvailableWeaponList
    {
        public const int INFINITY = -1;

        // Actual Fields
        public int FlameThrower = 5;
        public int Rocket = -1;
        public int Cluster = -1;
        public int CarpetBomb = 4;
        public int Grenade = 1;
        public int LandMine = 2;
        public int JetPack = 6;
        public int SpiderMine = 3;
  
        public int GetAvailable( IWeapon wobject )
        {
            if ( wobject is CarpetBombLauncher )
            {
                return CarpetBomb;
            }
            if ( wobject is ClusterLauncher )
            {
                return Cluster;
            }
            if ( wobject is FlameThrower )
            {
                return FlameThrower;
            }
            if ( wobject is GrenadeLauncher )
            {
                return Grenade;
            }
            if ( wobject is JetPack )
            {
                return JetPack;
            }
            if ( wobject is LandMineLauncher )
            {
                return LandMine;
            }
            if ( wobject is RocketLauncher )
            {
                return Rocket;
            }
            if ( wobject is SpiderMineLauncher )
            {
                return SpiderMine;
            }
            return 0;
        }

        public void Decriment( IWeapon wobject )
        {
            if ( wobject is CarpetBombLauncher && CarpetBomb != -1 )
            {
                --CarpetBomb;
            }
            if ( wobject is ClusterLauncher && Cluster != -1 )
            {
                --Cluster;
            }
            if ( wobject is FlameThrower && FlameThrower != -1 )
            {
                --FlameThrower;
            }
            if ( wobject is GrenadeLauncher && Grenade != -1 )
            {
                --Grenade;
            }
            if ( wobject is JetPack && JetPack != -1 )
            {
                --JetPack;
            }
            if ( wobject is LandMineLauncher && LandMine != -1 )
            {
                --LandMine;
            }
            if ( wobject is RocketLauncher && Rocket != -1 )
            {
                --Rocket;
            }
            if ( wobject is SpiderMineLauncher && SpiderMine != -1 )
            {
                --SpiderMine;
            }
        }
 
    }
}