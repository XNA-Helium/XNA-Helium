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
    public class LaunchingRod : BaseObjectStreamingHelper<LaunchingRod>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new LaunchingRod(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected LaunchingRod( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            Placeable p = new Placeable(true,-1);
            base.AddComponent( p );

            Drawable2D r = new Drawable2D(true,-1);
            base.AddComponent(r);
            r.Visible = false;
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodLeft" ), new Rectangle( 0, 0, 32, 64 ), DeployLeft, 3 );
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodRight" ), new Rectangle( 0, 0, 32, 64 ), DeployRight, 3 );
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodExtendedLeft" ), Left );
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodExtendedRight" ), Right );

            Placeable target = player[Placeable.TypeStatic] as Placeable;


            if (player.Facing == Facing.Right)
            {
                r.PlayAnimation(Right, Right);
            }
            if (player.Facing == Facing.Left)
            {
                r.PlayAnimation(Left, Left);
            }
            r.DrawLayer = (float)DrawLayer.LayerDepth.HealthPacks / 100.0f;

            faom = new FacingAwareOffsetManager(player, this, LeftOffset.ToVector3(), RightOffset.ToVector3());
            */
        }

        Vector2 RightOffset = new Vector2(-27, -53);
        Vector2 LeftOffset = new Vector2(27, -53);

        public static string DeployLeft = "DeployLeft";
        public static string DeployRight = "DeployRight";
        public static string Left = "Left";
        public static string Right = "Right";

        protected FacingAwareOffsetManager faom;
        public LaunchingRod(PlayerObject player):base()
        {

            Placeable p = new Placeable(true,-1);
            base.AddComponent( p );

            Drawable2D r = new Drawable2D(true,-1);
            base.AddComponent(r);
            r.Visible = false;
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodLeft" ), new Rectangle( 0, 0, 32, 64 ), DeployLeft, 3 );
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodRight" ), new Rectangle( 0, 0, 32, 64 ), DeployRight, 3 );
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodExtendedLeft" ), Left );
            r.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\RodExtendedRight" ), Right );

            Placeable target = player[Placeable.TypeStatic] as Placeable;


            if (player.Facing == Facing.Right)
            {
                r.PlayAnimation(Right, Right);
            }
            if (player.Facing == Facing.Left)
            {
                r.PlayAnimation(Left, Left);
            }
            r.DrawLayer = (float)DrawLayer.LayerDepth.HealthPacks / 100.0f;

            faom = new FacingAwareOffsetManager(player, this, LeftOffset.ToVector3(), RightOffset.ToVector3());
        }
        public override void CleanUp(bool RemoveFromObjectList)
        {
            faom.CleanUp();
            faom = null;
            base.CleanUp(RemoveFromObjectList);
        }
        public void Deploy(Facing facing)
        {
            Drawable2D r;
            this.GetComponent<Drawable2D>(Drawable2D.TypeStatic, out r);
            if (facing == Facing.Right)
            {
                r.PlayAnimation(DeployRight, Right);
            }
            if (facing == Facing.Left)
            {
                r.PlayAnimation(DeployLeft, Left);
            }
            r.Visible = true;
        }
        public void Retract(Facing facing)
        {
            Drawable2D r;
            this.GetComponent<Drawable2D>(Drawable2D.TypeStatic, out r);
            if (facing == Facing.Right)
            {
                r.PlayAnimation(Right);
            }
            if (facing == Facing.Left)
            {
                r.PlayAnimation(Left);
            }
            r.Visible = false;
        }
    }
}
