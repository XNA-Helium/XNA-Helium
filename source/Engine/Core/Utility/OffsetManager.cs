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

namespace Engine.Core
{
    public class OffsetManager
    {
        protected Placeable target;
        protected Placeable me;
        protected Vector3 offset;
        public OffsetManager( BaseObject Target, BaseObject Me, Vector3 Offset )
        {
            target = Target[Placeable.TypeStatic] as Placeable;
            me = Me[Placeable.TypeStatic] as Placeable;
            offset = Offset;
            me.Position = target.Position + offset;
            target.InstallPositionSetCallback(OnChange);
        }
        public virtual void OnChange(ref Vector3 newPosition, Placeable component)
        {
            me.Position = newPosition + offset;
        }
        public void CleanUp( )
        {
            target.RemovePositionSetCallback( OnChange );
        }
    }
}
