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

namespace Engine
{
    public class CameraManager: IEngineUpdateable
    {
        protected Camera camera;
        public Camera Current
        {
            get
            {
                return camera;
            }
            set
            {
                camera = value;
            }
        }
        
        protected GraphicsDeviceManager device;
        public GraphicsDeviceManager GraphicsDevice
        {
            get
            {
                return device;
            }
        }

        protected static CameraManager instance = null;
        public static CameraManager Instance
        {
            get
            {
                return instance;
            }
        }
        public static Point GetPosition(Vector2 vector)
        {
            return instance.Current.GetPosition(vector.X, vector.Y);
        }
        public static Point GetPosition(float X, float Y)
        {
            return instance.Current.GetPosition(X, Y);
        }
        public static void SetupCameraManager(GraphicsDeviceManager device)
        {
            instance = new CameraManager(device);
            instance.camera = new Camera();
        }
        protected CameraManager( GraphicsDeviceManager graphicsDevice )
        {
            this.device = graphicsDevice;
        }
        public void Update( GameTime gameTime )
        {
            camera.Update( gameTime );
        }

        void ProcessInput( GameTime gameTime, ref TouchCollection touches )
        {
            if ( touches.Count == 1 )
            {
                Point p = touches[0].PreviousTouch.Position;
                Point p2 = touches[0].Position;
                PointExtensions.Subract(ref p,ref p2 );
                camera.CameraMovement( p );
            }
        }

    }
}
