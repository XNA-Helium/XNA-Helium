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
using Engine.Core;

namespace Engine
{
    public class Camera3D : Camera
    {

        //@@  need to add support to this for Orthognail
        public bool Orthogonal = false;

        public struct ThirdPersonCam
        {
            public float viewDist;
            public float MinViewDistance;
            public float height;
            public Vector3 myUp;
            public Vector3 myRight;
            public Vector3 myForward;
            public Vector3 myCameraRotation;
        };

        public Camera3D( ):base()
        {
            nearClip = 0.01f;
            farClip = 10000.0f;
            position = new Vector3( 0, 0, 0 );
            thirdPersonCam.viewDist = 100.0f;
            thirdPersonCam.height = 4.0f;
            thirdPersonCam.MinViewDistance = 1.0f; //8.0f;
            thirdPersonCam.myUp = Vector3.Up;
            thirdPersonCam.myRight = -Vector3.UnitZ;
            thirdPersonCam.myForward = Vector3.Cross( thirdPersonCam.myUp, thirdPersonCam.myRight );
            thirdPersonCam.myCameraRotation = Vector3.Zero;

            aspectRatio = CameraManager.Instance.GraphicsDevice.GraphicsDevice.DisplayMode.AspectRatio;

            // may have to move this to update if aspect ratio or clipping planes change during runtime
            projection = Matrix.CreatePerspectiveFieldOfView( 1, aspectRatio, nearClip, farClip );
        }

        public void Reset( )
        {
            nearClip = 0.01f;
            farClip = 10000.0f;
            position = new Vector3( 0, 0, 0 );
            thirdPersonCam.viewDist = 10.0f;
            thirdPersonCam.height = 4.0f;
            thirdPersonCam.MinViewDistance = 1.0f; //8.0f;
            thirdPersonCam.myUp = Vector3.Up;
            thirdPersonCam.myRight = -Vector3.UnitZ;
            thirdPersonCam.myForward = Vector3.Cross( thirdPersonCam.myUp, thirdPersonCam.myRight );
            thirdPersonCam.myCameraRotation = Vector3.Zero;

            aspectRatio = CameraManager.Instance.GraphicsDevice.GraphicsDevice.DisplayMode.AspectRatio;

            // may have to move this to update if aspect ratio or clipping planes change during runtime
            projection = Matrix.CreatePerspectiveFieldOfView( 1, aspectRatio, nearClip, farClip );
        }

        protected Vector3 Offset = Vector3.Zero;
        ThirdPersonCam thirdPersonCam;

        public void UpdateMatricies( )
        {
            if ( !DoNotFollowTarget )
            {
                Matrix matRotation = Matrix.CreateFromYawPitchRoll( Target.Rotation.Y * Engine.Core.Utility.DegreeToRadian, Target.Rotation.X * Utility.DegreeToRadian, Target.Rotation.Z * Utility.DegreeToRadian );

                matRotation = matRotation * Matrix.CreateFromYawPitchRoll( thirdPersonCam.myCameraRotation.Y, thirdPersonCam.myCameraRotation.X, thirdPersonCam.myCameraRotation.Z );

                Position = Vector3.Transform( new Vector3( -thirdPersonCam.viewDist, thirdPersonCam.height, 0 ), matRotation ) + Target.Position;

                view = Matrix.CreateLookAt( Position, Target.Position, Vector3.Up );
            }
            else
            {
                view = Matrix.CreateLookAt( Position, Position + thirdPersonCam.myForward, thirdPersonCam.myUp );
            }
        }

        /// <summary>
        /// In radians.
        /// </summary>
        /// <param name="input">X controls rotation around Up axis. Y controls zooming in/out.</param>
        public override void CameraMovement( Point input )
        {
            
            base.CameraMovement(input);

            thirdPersonCam.myCameraRotation = thirdPersonCam.myCameraRotation + new Vector3( 0.0f, input.X * .05f, 0.0f );

            if ( thirdPersonCam.height > 3.5f || -input.Y > 0 )
                thirdPersonCam.height = thirdPersonCam.height + -input.Y * .05f;

            if ( thirdPersonCam.viewDist > thirdPersonCam.MinViewDistance || -input.Y > 0 )
                thirdPersonCam.viewDist = thirdPersonCam.viewDist + -input.Y * .05f;
        }

        public virtual void LookAt(Vector3 lookat, Vector3 up)
        {
            view = Matrix.CreateLookAt(position,lookat,up);
            thirdPersonCam.myForward = view.Forward;
            thirdPersonCam.myUp = view.Up;
            thirdPersonCam.myRight = view.Right;
        }

        protected float nearClip;
        protected float farClip;

        protected Vector3 position;


        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                Point p = value.ToPoint();
                base.CenterOn(ref p);
                position = value;
            }
        }
        public Vector3 Rotation
        {
            get
            {
                return thirdPersonCam.myForward;
            }
        }

        protected Matrix view;
        protected Matrix projection;

        public Matrix View
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
            }
        }
        public Matrix Projection
        {
            get
            {
                return projection;
            }
            set
            {
                projection = value;
            }
        }
        public Matrix World
        {
            get
            {
                Matrix matRotation = Matrix.CreateFromYawPitchRoll( thirdPersonCam.myCameraRotation.Y, thirdPersonCam.myCameraRotation.X, thirdPersonCam.myCameraRotation.Z );

                return matRotation * Matrix.CreateTranslation( position );
            }
        }


        public float FarClip
        {
            get
            {
                return farClip;
            }
            set
            {
                farClip = value;
            }
        }
        public float NearClip
        {
            get
            {
                return nearClip;
            }
            set
            {
                nearClip = value;
            }
        }

        public float AspectRatio
        {
            get
            {
                return aspectRatio;
            }
            set
            {
                aspectRatio = value;

                // may have to move this to update if aspect ratio or clipping planes change during runtime
                projection = Matrix.CreatePerspectiveFieldOfView( 1, aspectRatio, nearClip, farClip );
            }
        }

        public ThirdPersonCam ThirdPersonCamera
        {
            get
            {
                return thirdPersonCam;
            }
            set
            {
                thirdPersonCam = value;
            }
        }
    }
}
