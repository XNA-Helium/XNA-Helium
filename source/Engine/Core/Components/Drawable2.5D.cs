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
    public class Drawable25D: Drawable
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
        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new Drawable25D(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public static Type TypeStatic = typeof(Drawable25D); 
        
        public Drawable25D(bool InWorld, float uID ):base(InWorld, uID)
        {
            UniqueClassID = TypeStatic;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool paused)
        {
            throw new NotImplementedException();
        }
    }
}
