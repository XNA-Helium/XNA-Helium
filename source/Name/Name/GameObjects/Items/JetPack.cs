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

    class JetPack : WeaponObject<JetPack>
    {
        
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new JetPack(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected JetPack( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            totalEnergy = 100.0f;
            Drawable2D dsa = new Drawable2D( true, -1 );
            AddComponent( dsa );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackActiveLeft" ), new Rectangle( 0, 0, 48, 96 ), "JetPackActiveLeft", 5 );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackActiveRight" ), new Rectangle( 0, 0, 48, 96 ), "JetPackActiveRight", 5 );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackInactiveLeft" ), "JetPackInactiveLeft" );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackInactiveRight" ), "JetPackInactiveRight" );
            dsa.DrawLayer = ( float ) DrawLayer.LayerDepth.HealthPacks / 100.0f;
            dsa.Visible = false;
            if ( _po.Facing == Facing.Left )
            {
                dsa.PlayAnimation( "JetPackInactiveLeft" );
            }
            if ( _po.Facing == Facing.Right )
            {
                dsa.PlayAnimation( "JetPackInactiveRight" );
            }

            faom = new OffsetManager( _po, this, new Point( 0, -10 ).ToVector3() );

            //Drawable_Text dt = new Drawable_Text(GetTotalEnergy, new Vector2(-3,47));
            //base.AddComponent(new Game_Velocity_Limiting(new Vector2(10,10)));
            */
        }




        protected float totalEnergy;
        OffsetManager faom;
        public JetPack(PlayerObject _po) :base(_po)
        {
            totalEnergy = 100.0f;
            Drawable2D dsa = new Drawable2D(true,-1);
            AddComponent( dsa );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackActiveLeft" ), new Rectangle( 0, 0, 48, 96 ), "JetPackActiveLeft", 5 );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackActiveRight" ), new Rectangle( 0, 0, 48, 96 ), "JetPackActiveRight", 5 );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackInactiveLeft" ), "JetPackInactiveLeft");
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\JetPackInactiveRight" ), "JetPackInactiveRight" );
            dsa.DrawLayer = ( float ) DrawLayer.LayerDepth.HealthPacks / 100.0f;
            dsa.Visible = false;
            if ( _po.Facing == Facing.Left )
            {
                dsa.PlayAnimation( "JetPackInactiveLeft" );
            }
            if ( _po.Facing == Facing.Right )
            {
                dsa.PlayAnimation( "JetPackInactiveRight" );
            }
            
            faom = new OffsetManager( _po, this, new Point( 0, -10).ToVector3() );

            //Drawable_Text dt = new Drawable_Text(GetTotalEnergy, new Vector2(-3,47));
            //base.AddComponent(new Game_Velocity_Limiting(new Vector2(10,10)));
        }
        public override void CleanUp(bool RemoveFromObjectList)
        {
            faom.CleanUp();
            base.CleanUp(RemoveFromObjectList);
        }
        public void SetAnimation(bool Active)
        {
            Drawable2D dsa = (Drawable2D) base[Drawable2D.TypeStatic];
            if ( Active )
            {
                if ( Player.Facing == Facing.Left )
                {
                    dsa.PlayAnimation( "JetPackActiveLeft" );
                }
                if ( Player.Facing == Facing.Right )
                {
                    dsa.PlayAnimation( "JetPackActiveRight" );
                }
            }
            else
            {
                if ( Player.Facing == Facing.Left )
                {
                    dsa.PlayAnimation( "JetPackInactiveLeft" );
                }
                if ( Player.Facing == Facing.Right )
                {
                    dsa.PlayAnimation( "JetPackInactiveRight" );
                }
            }
        }

        public override IProjectile Fire( float Velocity )
        {
            /*
            AvailableWeaponList weaponlist = (Game1.Instance.GameManager as GameManagerTeamedInterface).CurrentUTP().AvailableWeapons;
            if (weaponlist.JetPack != AvailableWeaponList.INFINITY)
            {
                weaponlist.JetPack -= 1;
                
            } */
            return null;
        }

        public override void SetFacing(Facing facing)
        {}

        public float TotalEnergy
        {
            get
            {
                return totalEnergy;
            }
        }
        public string GetTotalEnergy()
        {
            return "" + (int)totalEnergy;
        }

        public bool Use(float energy )
        {
            totalEnergy -= energy;
            return totalEnergy > 0;
        }

        protected bool inUse;
        public bool InUse
        {
            get
            {
                return inUse;
            }
            set
            {
                inUse = value;
            }
        }

    }
}