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
    class GrenadeClusterMask : BaseObjectStreamingHelper<GrenadeClusterMask>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new GrenadeClusterMask(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected GrenadeClusterMask( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            this.AddComponent( new Placeable( true, -1 ) );
            this.AddComponent( new Drawable2D( true, -1 ) );
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\ClusterGrenadeShine" ), "Static" );
            dss.PlayAnimation( "Static" );
            om = new OffsetManager( parent, this, Vector3.Zero );
            */
        }

        OffsetManager om;
        public GrenadeClusterMask(BaseObject parent)
        {
            this.AddComponent( new Placeable(true,-1) );
            this.AddComponent( new Drawable2D(true,-1));
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\ClusterGrenadeShine" ), "Static" );
            dss.PlayAnimation( "Static" );
            om = new OffsetManager( parent, this, Vector3.Zero );
        }
        public override void CleanUp(bool RemoveFromObjectList)
        {
            om.CleanUp();
            base.CleanUp(RemoveFromObjectList);
        }
    }

    class GrenadeCluster : ProjectileObject<GrenadeCluster>
    {
        
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new GrenadeCluster(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected GrenadeCluster( bool InWorld, float uID )
            : base(InWorld, uID)
        {

            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\ClusterGrenade" ), "Static" );
            dss.PlayAnimation( "Static" );
            dss.FaceVelocity = false;
            dss.Spin = true;
            dss.SpinSpeed = 5.0f;

            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 16 );

            mask = new GrenadeClusterMask( this );
        }

        BaseObject mask;

        public BaseObject GraphicalOverlay
        {
            get
            {
                return mask;
            }
        }

        public GrenadeCluster(Facing direction)
            : base(direction)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\ClusterGrenade" ), "Static" );
            dss.PlayAnimation( "Static" );
            dss.FaceVelocity = false;
            dss.Spin = true;
            dss.SpinSpeed = 5.0f;

            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 16 );
        
            mask = new GrenadeClusterMask(this);
        }

        public override void OnCollision(BaseObject rhs)
        {
            if (!collided)
            {
                SoundManager.Instance.Play("land", 1f);
                Vector2 position = ( this[Placeable.TypeStatic] as Placeable ).Position.ToVector2();
                GrenadeSingle wem = new GrenadeSingle(Facing.Right);
                //GrenadeSingle wer = new GrenadeSingle(Facing.Right);
                //GrenadeSingle wel = new GrenadeSingle(Facing.Left);
                GrenadeSingle wer2 = new GrenadeSingle(Facing.Right);
                GrenadeSingle wel2 = new GrenadeSingle(Facing.Left);

                Random rand1 = new Random();
                Random rand2 = new Random();

                ( wem[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 0.0f, -10.0f ).ToVector3();

                //(wer[Physics2D.TypeStatic] as Physics2D).AddVelocity = new Vector2(0.6f, -7.0f).ToVector3();
                //(wel[Physics2D.TypeStatic] as Physics2D).AddVelocity = new Vector2(-0.6f, -7.0f).ToVector3();
                ( wer2[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 0.3f, -5.0f ).ToVector3();
                ( wel2[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( -0.3f, -5.0f ).ToVector3();
                ( wem[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( 0.0f, -7.0f ).ToVector3();

                ( wel2[Placeable.TypeStatic] as Placeable ).Position = new Vector2( position.X - 14, position.Y - 14 ).ToVector3();
                //(wel[Placeable.TypeStatic] as Placeable).Position = new Vector2(position.X - 12, position.Y - 10).ToVector3();
                ( wem[Placeable.TypeStatic] as Placeable ).Position = new Vector2( position.X, position.Y - 3 ).ToVector3();
                ( wer2[Placeable.TypeStatic] as Placeable ).Position = new Vector2( position.X + 14, position.Y - 14 ).ToVector3();
                //(wer[Placeable.TypeStatic] as Placeable).Position = new Vector2(position.X + 12, position.Y - 10).ToVector3();
                //Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(wel);
                //Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(wer);
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater( wem );
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater( wel2 );
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater( wer2 );
                if (rhs == this)
                {
                    //collided with terrain
                }
                base.OnCollision(rhs);
               Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
               Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(mask);
            }
        }


        public static IProjectile ReturnNewProjectile( Facing facing )
        {
            return new GrenadeCluster(facing);
        }
    }
}
