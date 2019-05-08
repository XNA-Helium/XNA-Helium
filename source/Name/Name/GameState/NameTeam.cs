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
using Engine.GameState;
using Engine.Core;

namespace Name.GameState
{
    public class NameTeam
    {
        protected AvailableWeaponList weaponList = new AvailableWeaponList();
        public AvailableWeaponList WeaponList
        {
            get
            {
                return weaponList;
            }
            set
            {
                weaponList = value;
            }
        }
        public int HP
        {
            get
            {
                return TotalHP;
            }
        }
        protected bool deadTeam = false;
        public bool DeadTeam
        {
            get
            {
                return deadTeam;
            }
        }
        public Color Color
        {
            get
            {
                return color;
            }
        }
        protected CircularObjectList<PlayerObject> PlayableObjects = new CircularObjectList<PlayerObject>();
        protected int TotalHP = 0;
        protected String team = Team.NoTeam;
        protected int playerNumber = -1;
        protected bool controlledLocally = true;
        protected bool ai = false;
        protected Color color;
        protected Dictionary<HitPoints, int> HPmap = new Dictionary<HitPoints, int>( );
        public NameTeam(String Team, bool AI, int PlayerNumber,bool ControledLocally, Color c)
        {
            color = c;
            team = Team;
            ai = AI;
            playerNumber = PlayerNumber;
            controlledLocally = ControledLocally;
        }

        public PlayerObject AddPlayer(Point position)
        {
            return AddPlayer(position, new PlayerObject() );
        }

        public PlayerObject AddPlayer(Point position, PlayerObject playableObject)
        {
            (playableObject[TeamMember.TypeStatic] as TeamMember).Team = team;
            HitPoints hp = (playableObject[HitPoints.TypeStatic] as HitPoints);
            hp.InstallHitpointsSetCallback(HitPointsChanged);
            (playableObject[Placeable.TypeStatic] as Placeable).Set2DPosition(position);
            HPmap.Add( hp, hp.Hitpoints );
            TotalHP += hp.Hitpoints;

            (playableObject.HP[DrawableText.TypeStatic] as DrawableText).Color = color;
            (playableObject[Drawable2D.TypeStatic] as Drawable2D).Color = color;

            PlayableObjects.Add( playableObject );

            return playableObject;
        }

        void HitPointsChanged(ref int HP, HitPoints component)
        {
            if ( HP <= 0 )
            {
                component.RemoveHitpointsSetCallback( HitPointsChanged );
                PlayableObjects.SafeRemove( component.Parent as PlayerObject);
                HPmap.Remove( component );
                if ( PlayableObjects.Current == component.Parent && !PlayableObjects.SafeRemove(component.Parent as PlayerObject))
                {
                    deadTeam = true;
                    Engine.DebugHelper.Break( deadTeam, DebugHelper.DebugLevels.Informative );
                }
            }
            else
            {
                HPmap[component] = HP;
            }
            TotalHP = 0;
            foreach ( int i in HPmap.Values )
            {
                TotalHP += i;
            }
        }

        public void Next( )
        {
            PlayableObjects.Next();
        }

        public CircularObjectList<PlayerObject> TeamMembers
        {
            get
            {
                return PlayableObjects;
            }
        }

        public PlayerObject CurrentlyActive
        {
            get
            {
                Engine.DebugHelper.Break( !PlayableObjects.Current.InWorld, DebugHelper.DebugLevels.Informative ); 
                return PlayableObjects.Current;
            }
            /*set
            {
                CurrentIndex = PlayableObjects.IndexOf(value);
            }*/
        }
        public String TeamName
        {
            get
            {
                return team;
            }
        }
        public bool AITeam
        {
            get
            {
                return ai;
            }
        }
        public bool Human
        {
            get
            {
                return !ai;
            }
        }
        public bool Local
        {
            get
            {
                return controlledLocally;
            }
        }
    }
}
