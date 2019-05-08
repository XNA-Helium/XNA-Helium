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
    public class Drawable3D : Drawable
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
            return new Drawable3D( InWorld, uID );
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        public string ModelName
        {
            set
            {
                HasChanged = true;
                model = Engine.ContentManager.Instance.GetObject<Model>(value);
            }
        }

        Model model;
        public Model Model
        {
            get
            {
                return model;
            }
            set
            {
                HasChanged = true;
                model = value;
            }
        }


        public static Type TypeStatic = typeof( Drawable3D ); 
        
        public Drawable3D(bool InWorld, float uID ):base(InWorld,uID)
        {
            UniqueClassID = TypeStatic;
        }
        public override void OnParentSet(BaseObject Parent)
        {
            GameState.GameStateSystem.Instance.GetCurrentState.Add3DDrawable(this);
            //(Parent[Placeable.TypeStatic] as Placeable).InstallRotationSetCallback(RotationSetterCallback);
            //(Parent[Placeable.TypeStatic] as Placeable).InstallPositionSetCallback(PositionSetterCallback);
            base.OnParentSet(Parent);
        }
        public override void OnRemoved(BaseObject Parent)
        {
            GameState.GameStateSystem.Instance.GetCurrentState.RemoveDrawable(this);
            //(Parent[Placeable.TypeStatic] as Placeable).RemoveRotationSetCallback(RotationSetterCallback);
            //(Parent[Placeable.TypeStatic] as Placeable).RemovePositionSetCallback(PositionSetterCallback);
            base.OnRemoved(Parent);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool paused)
        {
            Placeable p = Parent.GetComponent<Placeable>();
            Matrix I, S, R, O, T;
            I = Matrix.Identity;
            S = Matrix.CreateScale(p.Scale.X, p.Scale.Y, p.Scale.Z);
            R = Matrix.CreateFromYawPitchRoll(p.Rotation.Y * Utility.DegreeToRadian, p.Rotation.X * Utility.DegreeToRadian, p.Rotation.Z * Utility.DegreeToRadian);
            O = Matrix.Identity;
            T = Matrix.CreateTranslation(p.Position);

            Matrix ISROT = I * S * R * O * T;
            Camera3D c = CameraManager.Instance.Current as Camera3D;
            model.Draw(ISROT, c.View, c.Projection);
        }
    }
}
