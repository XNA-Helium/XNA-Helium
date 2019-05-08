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

    public class PlayerHitpoints : BaseObjectStreamingHelper<PlayerHitpoints> , IEngineUpdateable
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new PlayerHitpoints(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected PlayerHitpoints( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            _Object = HasHitpoints;
            gh = ( HasHitpoints[HitPoints.TypeStatic] as HitPoints );
            gh.InstallHitpointsSetCallback( OnHPChange );
            currentHP = gh.Hitpoints;
            this.AddComponent( new Placeable( true, -1 ) );
            dt = new DrawableText( Engine.ContentManager.Instance.GetObject<SpriteFont>( @"Content\Menu\TitleFont" ), RetHP );
            spacewidth = ( int ) dt.SpriteFont.MeasureString( " " ).X;
            this.AddComponent( dt );

            om = new OffsetManager( HasHitpoints, this, new Vector2( 0.0f, 64.0f ).ToVector3() );
            */
        }

        private BaseObject _Object;
        private HitPoints gh;

        int currentHP;

        OffsetManager om;

        public BaseObject GetRelated
        {
            get
            {
                return _Object;
            }
        }
        DrawableText dt;
        int spacewidth;
        public PlayerHitpoints( BaseObject HasHitpoints )
            : base()
        {
            _Object = HasHitpoints;
            gh = (HasHitpoints[HitPoints.TypeStatic] as HitPoints);
            gh.InstallHitpointsSetCallback( OnHPChange );
            currentHP = gh.Hitpoints;
            this.AddComponent(new Placeable(true,-1));
            dt = new DrawableText(Engine.ContentManager.Instance.GetObject<SpriteFont>(@"Content\Menu\TitleFont"), RetHP);
            spacewidth = (int) dt.SpriteFont.MeasureString(" ").X;
            this.AddComponent(dt);

            om = new OffsetManager( HasHitpoints, this, new Vector2( 0.0f, 64.0f).ToVector3()  );
        }
        public override void CleanUp(bool RemoveFromObjectList)
        {
            if(om != null)
                om.CleanUp();
            base.CleanUp(RemoveFromObjectList);
        }

        protected string RetHP()
        {
            if (_Object is PlayerObject)
            {
                String s = (_Object as PlayerObject).MyName + "\n";
                String s2 = currentHP.ToString();
                int width = (int) ((dt.SpriteFont.MeasureString(s).X/2) - (dt.SpriteFont.MeasureString(s2).X/2));
                for (int i = spacewidth; (i + spacewidth) <= width; i += spacewidth)
                {
                    s += " ";
                }
                return s + s2;
            }
            else
            {
                return currentHP.ToString();
            }
        }

        protected void OnHPChange(ref int hp, HitPoints components )
        {
            currentHP = hp;
            if ( hp <= 0 )
            {
                om = null;
                gh.RemoveHitpointsSetCallback( OnHPChange );
                if ( _Object is PlayerObject )
                {
                    ( _Object as PlayerObject ).Retract(false);
                }
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromObjectListLater( _Object );
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromObjectListLater( this );
            }
        }

    }
}