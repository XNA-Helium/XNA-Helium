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



namespace Engine.Core
{
    public sealed class Physics2D : Physics
    {
        internal QuadTreePositionItem<Physics2D> QuadPosition;

        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write(moving);
            bw.Write(collidedThisFrame);
            bw.Write(ref position);
            //bw.Write(Callback); // WTH do i do about a delegate :-/
            bw.Write(Callback == null); // True if callback == null
            bw.Write(isStatic);
            bw.Write(velocity);
            bw.Flush();
            //DebugHelper.Break(DebugHelper.DebugLevels.Explosive);
        }
        public override void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            moving = br.ReadBoolean();
            collidedThisFrame = br.ReadBoolean();
            position = br.ReadRectangle();
            if (br.ReadBoolean())// True if callback == null
            {//Callback = br.Read WHATEVER ???
                Callback = null;
            }
            else
            {
                shouldHaveCallback = true;
            }
            isStatic = br.ReadBoolean();
            velocity = br.ReadVector3();
        }
        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new Physics2D(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public static Type TypeStatic = typeof( Physics2D ); 
        public Physics2D( Rectangle CollisionRect, OnCollision cd ):base(cd)
        {
            position = CollisionRect;
            UniqueClassID = TypeStatic;
        }
        public Physics2D(Rectangle CollisionRect ):base(true,-1)
        {
            this.Moving = true;
            position = CollisionRect;
            UniqueClassID = TypeStatic;
        }
        public Physics2D(bool InWorld, float uID ):base(InWorld,uID)
        {
            this.Moving = true;
            UniqueClassID = TypeStatic;
        }

        protected void PositionSetterCallback( ref Vector3 NewPosition, Placeable component )
        {
            Engine.RectangleExtensions.CenterOn( ref position, NewPosition );
            if (QuadPosition != null)
                QuadPosition.Position = NewPosition.ToVector2();
        }

        public override void OnParentSet( BaseObject Parent )
        {
            Point p = ( Parent[Placeable.TypeStatic] as Placeable ).Position.ToPoint();
            Engine.RectangleExtensions.CenterOn(ref position, ref p);
            ( Parent[Placeable.TypeStatic] as Placeable ).InstallPositionSetCallback( PositionSetterCallback );
            base.OnParentSet( Parent );
            Physics2DManager.Instance.AddToWorld( this );
        }
        public override void OnRemoved( BaseObject Parent )
        {
            ( Parent[Placeable.TypeStatic] as Placeable ).RemovePositionSetCallback( PositionSetterCallback );
            base.OnRemoved( Parent );
            Physics2DManager.Instance.RemoveFromWorld( this );
        }
    }
}
