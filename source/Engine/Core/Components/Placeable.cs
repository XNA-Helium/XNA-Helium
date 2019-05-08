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

    public sealed class Placeable : BaseComponent
    {

        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write( position );
            bw.Write( rotation );
            bw.Write( scale );
            bw.Flush();
        }
        public override void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            position = br.ReadVector3();
            rotation = br.ReadVector3();
            scale = br.ReadVector3();
        }
        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new Placeable(InWorld,uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public static Type TypeStatic = typeof( Placeable ); 
        
        public Placeable(bool InWorld, float uID ):this(Vector3.Zero,Vector3.Zero,Vector3.One,InWorld,uID)
        {
        }
        public Placeable(Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : this(Position, Rotation, Scale, true,-1)
        {

        }
        public Placeable(Vector3 Position, Vector3 Rotation, Vector3 Scale, bool InWorld, float uID)
            : base(InWorld,uID)
        {
            UniqueClassID = TypeStatic;
            position = Position;
            rotation = Rotation;
            scale = Scale;
        }

        #region Position
        private SetterValidatorDelegate<Vector3,Placeable> PositionValidator = null;
        private List<SetterCallback<Vector3, Placeable>> PositionSetCallbacks = new List<SetterCallback<Vector3, Placeable>>(1);
        public void InstallPositionSetCallback(SetterCallback<Vector3, Placeable> Callback)
        {
            PositionSetCallbacks.Add(Callback);
        }
        public void RemovePositionSetCallback(SetterCallback<Vector3, Placeable> Callback)
        {
            PositionSetCallbacks.Remove(Callback);
        }
        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                if (PositionValidator != null)
                {
                    Vector3 RequiredOffset;
                    if (PositionValidator.Invoke(ref value, out RequiredOffset, null))
                    {
                        value += RequiredOffset;
                    }
                }
                {
                    HasChanged = true;
                    position = value;
                }
                for (int i = 0; i < PositionSetCallbacks.Count; ++i)
                {
                    PositionSetCallbacks[i].Invoke(ref position, this);
                }
            }
        }
        #endregion 
    
        
        #region Rotation
        private SetterValidatorDelegate<Vector3, Placeable> RotationValidator = null;
        private List<SetterCallback<Vector3, Placeable>> RotationSetCallbacks = new List<SetterCallback<Vector3, Placeable>>(1);
        public void InstallRotationSetCallback(SetterCallback<Vector3, Placeable> Callback)
        {
            RotationSetCallbacks.Add(Callback);
        }
        public void RemoveRotationSetCallback(SetterCallback<Vector3, Placeable> Callback)
        {
            RotationSetCallbacks.Remove(Callback);
        }
        private Vector3 rotation;
        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                if (RotationValidator != null)
                {
                    Vector3 RequiredOffset;
                    if (RotationValidator.Invoke(ref value, out RequiredOffset, null))
                    {
                        value += RequiredOffset;
                    }
                }
                {
                    rotation = value;
                    HasChanged = true;
                }
                for (int i = 0; i < RotationSetCallbacks.Count; ++i)
                {
                    RotationSetCallbacks[i].Invoke(ref rotation, this);
                }
            }
        }
        #endregion 

        
        #region Scale
        private SetterValidatorDelegate<Vector3, Placeable> ScaleValidator = null;
        private List<SetterCallback<Vector3, Placeable>> ScaleSetCallbacks = new List<SetterCallback<Vector3, Placeable>>(1);
        public void InstallScaleSetCallback(SetterCallback<Vector3, Placeable> Callback)
        {
            ScaleSetCallbacks.Add(Callback);
        }
        public void RemoveScaleSetCallback(SetterCallback<Vector3, Placeable> Callback)
        {
            ScaleSetCallbacks.Remove(Callback);
        }
        private Vector3 scale;
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                if (ScaleValidator != null)
                {
                    Vector3 RequiredOffset;
                    if (ScaleValidator.Invoke(ref value, out RequiredOffset, null))
                    {
                        value += RequiredOffset;
                    }
                }
                {
                    scale = value;
                    HasChanged = true;
                }
                for (int i = 0; i < ScaleSetCallbacks.Count; ++i)
                {
                    ScaleSetCallbacks[i].Invoke(ref scale, this);
                }
            }
        }
        #endregion 
    }
}
