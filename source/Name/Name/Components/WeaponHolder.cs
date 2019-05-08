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
    public sealed class WeaponHolder : BaseComponent
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
        public static Type TypeStatic = typeof( WeaponHolder );

        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new WeaponHolder( InWorld, uID );
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public WeaponHolder(IWeapon weapon)
            : this(true, -1)
        {
            _Weapon = weapon;
            UniqueClassID = TypeStatic;
        }

        public WeaponHolder(bool InWorld, float uID)
            : base(InWorld, uID)
        {

        }

        IWeapon _Weapon;


        public IWeapon Weapon
        {
            set
            {
                if ( _Weapon != value )
                {
                    _Weapon.CleanUp();
                }
                HasChanged = true;
                _Weapon = value;
            }
            get
            {
                return _Weapon;
            }
        }
    }
}
