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
    public abstract class WeaponObject<T> : BaseObjectStreamingHelper<T>, IWeapon where T : Factoryable
    {
        protected Vector2 Offset = new Vector2( 0, -75 );
        FacingAwareOffsetManager o;
        private PlayerObject _PO;
        public PlayerObject Player
        {
            get
            {
                return _PO;
            }
        }
        protected Facing facing;
        protected WeaponObject(bool InWorld, float uID)
            : base(InWorld, uID)
        {
            DebugHelper.Break(DebugHelper.DebugLevels.Explosive);
            /*
            facing = _po.Facing;
            _PO = _po;
            Drawable2D r = new Drawable2D(true, -1);
            r.DrawLayer = (float)DrawLayer.LayerDepth.Weapons / 100.0f;
            Placeable p = new Placeable(true, -1);
            base.AddComponent(p);
            base.AddComponent(r);
            (this[Placeable.TypeStatic] as Placeable).Position = (_po.Position + Offset).ToVector3();
            r.Visible = false;

            o = new FacingAwareOffsetManager(_po, this, Offset.ToVector3(), Offset.ToVector3());
            */
        }

        public WeaponObject( PlayerObject _po )
        {
            facing = _po.Facing;
            _PO = _po;
            Drawable2D r = new Drawable2D( true, -1 );
            r.DrawLayer = ( float ) DrawLayer.LayerDepth.Weapons / 100.0f;
            Placeable p = new Placeable( true, -1 );
            base.AddComponent( p );
            base.AddComponent( r );
            ( this[Placeable.TypeStatic] as Placeable ).Position = ( _po.Position + Offset ).ToVector3();
            r.Visible = false;

            o = new FacingAwareOffsetManager( _po, this, Offset.ToVector3(), Offset.ToVector3() );
        }
        public override void CleanUp( bool RemoveFromObjectList )
        {
            o.CleanUp();
            base.CleanUp( RemoveFromObjectList );
        }

        public bool Visible
        {
            get
            {
                return ( this[Drawable2D.TypeStatic] as Drawable2D ).Visible;
            }
            set
            {
                ( this[Drawable2D.TypeStatic] as Drawable2D ).Visible = value;
            }
        }

        public virtual void SetFacing( Facing Facing )
        {
            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            facing = Facing;
            if ( facing == Facing.Left )
            {
                dss.PlayAnimation( "Left" );
            }
            if ( facing == Facing.Right )
            {
                dss.PlayAnimation( "Right" );
            }
        }

        public virtual IProjectile Fire( float Velocity )
        {
            Vector2 StartPosition = ( this[Placeable.TypeStatic] as Placeable ).Position.ToVector2();

            BaseObject rocket = (BaseObject) ( this[ProjectileHolder.TypeStatic] as ProjectileHolder ).Projectile( facing );

            ( rocket[Drawable2D.TypeStatic] as Drawable2D ).Visible = true;

            ( rocket[Placeable.TypeStatic] as Placeable ).Position = StartPosition.ToVector3();

            float angle = ( this[Placeable.TypeStatic] as Placeable ).Get2DRotation();
            if ( facing == Facing.Right )
            {
                ( rocket[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( ( float ) Velocity * ( float ) Math.Cos( angle ), ( float ) Velocity * ( float ) Math.Sin( angle ) ).ToVector3();
            }
            if ( facing == Facing.Left )
            {
                ( rocket[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( ( float ) -Velocity * ( float ) Math.Cos( angle ), ( float ) -Velocity * ( float ) Math.Sin( angle ) ).ToVector3();
            }
            return rocket as IProjectile;
        }
        public float Rotation
        {
            get
            {
                return ( this[Placeable.TypeStatic] as Placeable ).Get2DRotation();
            }
            set
            {
                ( this[Placeable.TypeStatic] as Placeable ).Set2DRotation( value );
            }
        }

    }
    
}
