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
    sealed public class DrawableText : Drawable
    {

        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write( drawLayer );
            bw.Write( color );
            bw.Write( ref position );
            bw.Write(ContentManager.Instance.ReturnName(sf));
            bw.Flush();
        }
        public override void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            drawLayer = br.ReadSingle();
            color = br.ReadColor();
            position = br.ReadPoint();
            sf = ContentManager.Instance.GetObject<SpriteFont>(br.ReadString());
        }
        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new DrawableText(InWorld,uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public SpriteFont SpriteFont
        {
            get
            {
                return sf;
            }
        }

        SpriteFont sf;
        
        public static Type TypeStatic = typeof( DrawableText ); 

        protected ReturnsString text;
        public ReturnsString Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        protected Point position;
        protected float Rotation = 0.0f;
        protected float Scale = 1.0f;
        protected Color color = Color.White;
        protected void PositionSetterCallback(ref Vector3 NewPosition, Placeable component)
        {
            position = NewPosition.ToPoint();
        }

        public override void OnParentSet(BaseObject Parent)
        {
            GameState.GameStateSystem.Instance.GetCurrentState.AddDrawableText(this);
            (Parent[Placeable.TypeStatic] as Placeable).InstallPositionSetCallback(PositionSetterCallback);
            base.OnParentSet(Parent);
        }
        public override void OnRemoved(BaseObject Parent)
        {
            GameState.GameStateSystem.Instance.GetCurrentState.RemoveDrawable(this);
            (Parent[Placeable.TypeStatic] as Placeable).RemovePositionSetCallback(PositionSetterCallback);
            base.OnRemoved(Parent);
        }
        public DrawableText(bool InWorld, float uID)
            : base(InWorld,uID)
        {
            sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
            UniqueClassID = TypeStatic;
            drawLayer = 0.5f;
        }

        public DrawableText( ReturnsString Text ):this(Engine.ContentManager.Instance.GetObject<SpriteFont>(@"SharedContent\TextboxFont"),Text)
        {
        }

        public DrawableText( SpriteFont SF, ReturnsString Text )
            : this( true, -1 )
        {
            sf = SF;
            //Position is the CENTER point
            text = Text;
        }

        protected float drawLayer;
        public float DrawLayer
        {
            get
            {
                return drawLayer;
            }
            set
            {
                drawLayer = value;
            }
        }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        Vector2 target;
        Vector2 origin;
        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch, bool paused )
        {
            Rectangle CurrentCamera = (CameraManager.Instance.Current.Rectangle);
            string drawme = text.Invoke();
            Vector2 boxSize = sf.MeasureString(drawme);
            target = new Vector2( ( int ) position.X - CurrentCamera.X, ( int ) position.Y - CurrentCamera.Y );
            origin = new Vector2( ( int ) boxSize.X / 2, ( int ) boxSize.Y / 2 );

            spriteBatch.DrawString(sf, drawme, target, color, Rotation, origin, Scale, SpriteEffects.None, drawLayer);
        }
    }
}
