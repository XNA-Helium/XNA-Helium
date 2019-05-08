using System;
using System.IO;
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
    public delegate IProjectile ProjectileObjectCallback(Facing facing );

    public sealed class ProjectileHolder : BaseComponent
    {

        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            DebugHelper.Break( DebugHelper.DebugLevels.Explosive );
            bw.Flush();
        }
        public override void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            DebugHelper.Break( DebugHelper.DebugLevels.Explosive );
        }
        public static Type TypeStatic = typeof( ProjectileHolder );

        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new ProjectileHolder( InWorld, uID );
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public ProjectileHolder( ProjectileObjectCallback weapon )
            : this(true,-1)
        {
            _Projectile = weapon;
            UniqueClassID = TypeStatic;
        }

        public ProjectileHolder(bool InWorld, float uID)
            : base(InWorld,uID)
        {

        }

        protected ProjectileObjectCallback _Projectile;
        public ProjectileObjectCallback Projectile
        {
            internal set
            {
                _Projectile = value;
            }
            get
            {
                return _Projectile;
            }
        }
    }
}
