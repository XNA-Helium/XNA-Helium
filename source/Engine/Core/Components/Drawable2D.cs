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
    
    public enum Facing {Left, Right, Up, Down };

    sealed public class Drawable2D : Drawable
    {
        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write(visible);
            bw.Write(SpriteSheets.Count);
            foreach (KeyValuePair<string,SpriteSheet> sheet in SpriteSheets)
            {
                bw.Write(sheet.Key);
                bw.Write( ContentManager.Instance.ReturnName( sheet.Value.spriteSheet ) );
                bw.Write(ref sheet.Value.boxSize);
                bw.Write(sheet.Value.framesPerSprite);
                bw.Write(ref sheet.Value.currentSquare);
                //bw.Write( sheet.Value.sheetName);
                bw.Write( sheet.Value.boxesWide);
                bw.Write( sheet.Value.boxesTall);
                bw.Write( sheet.Value.currentIndex);
                bw.Write( sheet.Value.maxIndex);
                bw.Write( sheet.Value.counter);
               //sheet.Value
            }
            bw.Write(currentAnimation.name);
            bw.Write(currentAnimation.Frame);
            
            bw.Write(AnimationFollowingThisOne);
            if(AnimationFollowingThisOne)
                bw.Write(NextAnimation);

            bw.Write(color);
            bw.Write(drawLayer);
            bw.Write(ref position);
            bw.Write(Rotation);
            bw.Write(Scale);
            bw.Write(SpinSpeed);
            bw.Write(spin);
            bw.Write(RotateToFaceVelocity);
            bw.Write(UseOrigin);
            bw.Write(origin);

            bw.Flush();
        }
        public override void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            visible = br.ReadBoolean();
            int sheets = br.ReadInt32();
            for (int i = 0; i < sheets; ++i)
            {
                string name = br.ReadString();
                Texture2D texture = ContentManager.Instance.GetObject<Texture2D>(br.ReadString());
                Rectangle boxsize = br.ReadRectangle();
                int fps = br.ReadInt32();
                if (!SpriteSheets.ContainsKey(name))
                {
                    SpriteSheets.Add(name, new SpriteSheet(texture,boxsize, name, fps));
                }
                else
                {
                    SpriteSheets[name].spriteSheet = texture;
                    SpriteSheets[name].boxSize = boxsize;
                    SpriteSheets[name].framesPerSprite = fps;
                }
                SpriteSheets[name].currentSquare = br.ReadRectangle();
                //SpriteSheets[name].sheetName = br.ReadString();
                SpriteSheets[name].boxesWide = br.ReadInt32();
                SpriteSheets[name].boxesTall = br.ReadInt32();
                SpriteSheets[name].currentIndex = br.ReadInt32();
                SpriteSheets[name].maxIndex = br.ReadInt32();
                SpriteSheets[name].counter = br.ReadInt32();
            }

            string ca = br.ReadString();
            currentAnimation = SpriteSheets[ca];
            currentAnimation.Frame = br.ReadInt32();

            AnimationFollowingThisOne = br.ReadBoolean();
            if(AnimationFollowingThisOne)
                NextAnimation = br.ReadString();
    
            color = br.ReadColor();
            drawLayer = br.ReadSingle();
            position = br.ReadRectangle();
            Rotation = br.ReadSingle();
            Scale = br.ReadSingle();
            SpinSpeed = br.ReadSingle();
            spin = br.ReadBoolean();
            RotateToFaceVelocity = br.ReadBoolean();
            UseOrigin = br.ReadBoolean();
            origin = br.ReadVector2();
        }
        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new Drawable2D(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }


        bool AnimationFollowingThisOne = false;
        string NextAnimation;

        System.Collections.Generic.Dictionary<String,SpriteSheet> SpriteSheets = new Dictionary<string,SpriteSheet>();
        SpriteSheet currentAnimation;

        public static Type TypeStatic = typeof( Drawable2D ); 
        

        // Facing concept
        public Drawable2D(SpriteSheet DefaultSheet):this(true, -1)
        {
            SpriteSheets.Add(DefaultSheet.name,DefaultSheet);
            currentAnimation = DefaultSheet;
            SetInitialPosition();
        }

        public Drawable2D(bool InWorld, float uID ):base(InWorld,uID)
        {
            UniqueClassID = TypeStatic;
            drawLayer = 0.5f;
            //Position is the CENTER point
            spin = false;
            spinSpeed = 1.0f;
            RotateToFaceVelocity = false; 
        }

        protected void SetInitialPosition()
        {
            position = currentAnimation.BoxSize;
            Engine.RectangleExtensions.CenterOn( ref position, ( Parent[Placeable.TypeStatic] as Placeable ).Position );
        }

        public int PlayTime
        {
            get
            {
                return currentAnimation.Time;
            }
        }

        public string CurrentlyPlaying
        {
            get
            {
                if ( currentAnimation == null )
                    return "NoCurrentAnimation";
                else
                    return currentAnimation.name;
            }
        }

        public int Frame
        {
            get
            {
                return currentAnimation.Frame;
            }
        }

        protected Color color = Color.White;
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
        public void PlayAnimation(string current, string Next)
        {
            AnimationFollowingThisOne = true;
            NextAnimation = Next;
            if ( currentAnimation != null && currentAnimation.name == current )
            {
                return;
            }
            if ( currentAnimation != null )
                currentAnimation.Reset();
    
            currentAnimation = SpriteSheets[current];
            SetInitialPosition();

        }
        public void PlayAnimation( string Name )
        {
            if (currentAnimation != null && currentAnimation.name == Name )
                return;

            if(currentAnimation != null)
                currentAnimation.Reset();
    
            currentAnimation = SpriteSheets[Name];
            SetInitialPosition();
        }

        public void PlayAnimation( string Name, bool RandomSeed )
        {
            if ( currentAnimation != null && currentAnimation.name == Name )
                return;

            if ( currentAnimation != null )
                currentAnimation.Reset();

            currentAnimation = SpriteSheets[Name];
            SetInitialPosition();
            currentAnimation.Reset( RandomSeed );
        }

        public void SetupAnimation( Texture2D texture, String name )
        {
            SpriteSheets.Add( name, new SpriteSheet( texture, name ) );
        }

        public void SetupAnimation( SpriteSheet sheet, String name )
        {
            SpriteSheets.Add( name, sheet );
        }

        public void SetupAnimation( Texture2D texture, Rectangle rect, string name, int fps )
        {
            SpriteSheets.Add( name, new SpriteSheet( texture, rect, name, fps ) );
        }

        public void SetupAnimation( SpriteSheet sheet )
        {
            SpriteSheets.Add( sheet.name, sheet );
        }


        public Rectangle Rectangle
        {
            get { return position; }
        }

        protected Rectangle position;
        protected float Rotation = 0.0f;
        protected float Scale = 1.0f;
        protected void RotationSetterCallback( ref Vector3 NewRotation, Placeable component )
        {
            Rotation = NewRotation.Y;
        }

        protected void PositionSetterCallback( ref Vector3 NewPosition, Placeable component )
        {
            Engine.RectangleExtensions.CenterOn( ref position, NewPosition );
        }

        public override void OnParentSet( BaseObject Parent )
        {
            GameState.GameStateSystem.Instance.GetCurrentState.Add2DDrawable( this );
            ( Parent[Placeable.TypeStatic] as Placeable ).InstallRotationSetCallback( RotationSetterCallback );
            ( Parent[Placeable.TypeStatic] as Placeable ).InstallPositionSetCallback( PositionSetterCallback );
            base.OnParentSet( Parent );
        }
        public override void OnRemoved( BaseObject Parent )
        {
            GameState.GameStateSystem.Instance.GetCurrentState.RemoveDrawable( this );
            ( Parent[Placeable.TypeStatic] as Placeable ).RemoveRotationSetCallback( RotationSetterCallback );
            ( Parent[Placeable.TypeStatic] as Placeable ).RemovePositionSetCallback( PositionSetterCallback );
            base.OnRemoved( Parent );
        }

        // This removes all per-Draw allocations from the drawable 2d component
        private float rotation = 0.0f;
        private Rectangle CurrentCamera;
        private Vector2 speed;
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool paused)
        {
            if ( !visible )
                return;

            if (paused)
            {
                //don't change the rotation
            }
            else if (RotateToFaceVelocity)
            {
                speed = (Parent[Physics2D.TypeStatic] as Physics2D).Velocity.ToVector2();
                if(speed != Vector2.Zero)
                    rotation = (float)Math.Atan(speed.Y / speed.X);
            }
            else if (spin)
            {
                speed = (Parent[Physics2D.TypeStatic] as Physics2D).Velocity.ToVector2();
                rotation = spinSpeed * ((float)gameTime.TotalGameTime.TotalSeconds) * speed.X / Math.Abs(speed.X);
            }
            else
            {
                rotation = Rotation;
            }
            if (!paused & AnimationFollowingThisOne  && currentAnimation.AtLastFrame() )
            {
                AnimationFollowingThisOne = false;
                PlayAnimation( NextAnimation );
                NextAnimation = "";
            }
            if( !paused)
                currentAnimation.Update( gameTime );
    
            CurrentCamera = (CameraManager.Instance.Current.Rectangle);
            if(UseOrigin)
                currentAnimation.Draw( spriteBatch, gameTime, ref  CurrentCamera, color, Scale, drawLayer,ref position, rotation, origin);
            else
                currentAnimation.Draw( spriteBatch, gameTime, ref  CurrentCamera, color, Scale, drawLayer,ref position, rotation );
        }

        protected float spinSpeed;
        public float SpinSpeed
        {
            get
            {
                return spinSpeed;
            }
            set
            {
                spinSpeed = value;
            }
        }

        protected bool spin;
        public bool Spin
        {
            get
            {
                return spin;
            }
            set
            {
                spin = value;
            }
        }
        protected bool RotateToFaceVelocity;
        public bool FaceVelocity
        {
            get
            {
                return RotateToFaceVelocity;
            }
            set
            {
                RotateToFaceVelocity = value;
            }
        }

        bool UseOrigin = false;
        Vector2 origin = Vector2.Zero;
        public void SetOrigin( Vector2 NewOrigin )
        {
            UseOrigin = true;
            origin = NewOrigin;
        }
    }
}
