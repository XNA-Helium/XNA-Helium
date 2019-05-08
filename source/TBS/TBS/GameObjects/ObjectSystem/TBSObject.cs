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

namespace TBS
{
    public class TBSObject : Engine.Core.BaseObject
    {
        // Should this impliment the attachment system from
        // APE  and IEngineUpdatable?

        // Knows its extents
        // Center position
        bool AllowPassthrough;

        public TBSObject()
            : this(Vector3.Zero,true)
        {

        }
        public TBSObject(bool InWorld)
            : this(Vector3.Zero)
        {

        }

        public TBSObject(Vector3 Position)
            : this(Position,true)
        {
        }

        public TBSObject(Vector3 Position, bool InWorld)
            : base(true)
        {
            base.AddComponent(new Placeable(true,-1));
            (this[Placeable.TypeStatic] as Placeable).Position = Position;

            base.AddComponent(new Drawable3D(true,-1));
            (this[Drawable3D.TypeStatic] as Drawable3D).ModelName = @"Content\Models\goblinSword";
        }

        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            base.Save(sw);
        }
        public override void Load(Engine.Persistence.PersistenceManager sr)
        {
            base.Load(sr);
        }
        public override void HookUpPointers()
        {
            //throw new NotImplementedException();
        }
    }
}
