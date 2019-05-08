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
    public class FacingAwareOffsetManager : OffsetManager
    {
        protected Vector3 rightPoint;
        PlayerObject PO;
        public FacingAwareOffsetManager( PlayerObject Target, BaseObject Me, Vector3 LeftPoint, Vector3 RightPoint )
            : base( Target, Me, LeftPoint )
        {
            PO = Target;
            rightPoint = RightPoint;
            if ( (PO as PlayerObject).Facing == Facing.Left )
            {
                me.Position = target.Position + offset;
            }
            if ( (PO as PlayerObject ).Facing == Facing.Right )
            {
                me.Position = target.Position + rightPoint;
            }
        }
        public override void OnChange( ref Vector3 newPosition, Placeable component )
        {
            if ( PO.Facing == Facing.Left )
            {
                me.Position = newPosition + offset;
            }
            if ( PO.Facing == Facing.Right )
            {
                me.Position = newPosition + rightPoint;
            }
        }
    }
}
