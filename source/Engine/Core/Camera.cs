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
    public enum CameraMode { TopDown, Follow, None };

    public class Camera : IRectangle, IEngineUpdateable
    {
        protected float aspectRatio;
        
        protected Rectangle rect;

        public Camera( )
        {
            name = "BaseCamera";
            aspectRatio = CameraManager.Instance.GraphicsDevice.GraphicsDevice.DisplayMode.AspectRatio;
            rect = new Rectangle(0, 0, CameraManager.Instance.GraphicsDevice.GraphicsDevice.DisplayMode.Width, CameraManager.Instance.GraphicsDevice.GraphicsDevice.DisplayMode.Height);
        }

        public virtual void ReSize(int width, int height)
        {
            Point NewCenter = rect.Center;
            rect.Width = width;
            rect.Height = height;
            centerOn(ref NewCenter); // Might need tweaking
        }

        /// <summary>
        /// Offset the 2D camera by the input 
        /// </summary>
        /// <param name="input">The offset from the current position</param>
        public virtual void CameraMovement( Point input )
        {
            rect.Offset(input);
        }

        public Rectangle TwoDDrawView
        {
            get
            {
                return rect;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return rect;
            }
        }

        public void Update( GameTime gameTime )
        {
            if ( !DoNotFollowTarget )
            {
                Point p = Target.Position.ToPoint();
                centerOn(ref p);
            }
        }

        public void CenterOn(ref Point point )
        {
            centerOn(ref point );
        }

        public Point GetPosition(float X, float Y)
        {
            return new Point( (int)(X * (float)rect.Width), (int)(Y * (float)rect.Height));
        }


        Point NewCenter = Point.Zero;
        protected void centerOn(ref Point point )
        {
            Engine.RectangleExtensions.CenterOn( ref rect,ref point );
            if(ConstrainToRect && !cRect.Contains(rect) )
            {
                NewCenter = rect.Center;
                if ( rect.Bottom >= cRect.Bottom )
                {
                    NewCenter.Y = cRect.Bottom - rect.Height / 2;
                }
                else if ( rect.Top <= cRect.Top)
                {
                    NewCenter.Y = cRect.Top + rect.Height / 2;
                }
                if ( rect.Left <= cRect.Left )
                {
                    NewCenter.X = cRect.Left + rect.Width / 2;
                }
                else if ( rect.Right >= cRect.Right )
                {
                    NewCenter.X = cRect.Right - rect.Width / 2;
                }
                Engine.RectangleExtensions.CenterOn( ref rect, ref NewCenter );
            }
        }

        protected string name;
        public virtual string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        protected Engine.Core.Placeable Target = null;
        protected bool DoNotFollowTarget = true;
        public void SetTarget( Engine.Core.Placeable target )
        {
            if ( target != null )
            {
                DoNotFollowTarget = false;
                Target = target;
            }
            else
            {
                DoNotFollowTarget = true;
            }
        }
        protected Rectangle cRect;
        protected bool ConstrainToRect = false;
        public void SetConstrainRectangle( ref Rectangle rect )
        {
            cRect = rect;
            ConstrainToRect = true;
        }
        public void SetConstrainRectangle(bool constrain )
        {
            ConstrainToRect = constrain;
        }



        TimeSpan starttime;
        Point StartPoint;
        protected Vector2 velocity = Vector2.Zero;
        public virtual void ProcesInput(GameTime gameTime, ref Engine.TouchCollection touches )
        {
            if ( touches.Count == 1 )
            {
                velocity = Vector2.Zero;
                Touch location = touches[0];
                switch ( location.State )
                {
                    case TouchStates.Pressed:
                        {
                            StartPoint = location.Position;
                            starttime = gameTime.ElapsedGameTime;
                        }
                        break;
                    case TouchStates.Moved:
                        {
                            if (  location.PreviousTouch != null)
                            {
                                Vector2 temp = location.PreviousTouch.Position.ToVector2() - location.Position.ToVector2();
                                rect.Offset( ( int ) temp.X, ( int ) temp.Y );
                                Point p =  rect.Center;
                                centerOn(ref p);
                            }
                        }
                        break;
                    case TouchStates.Released:
                        {
                            if ( location.PreviousTouch != null )
                            {
                                velocity = StartPoint.ToVector2() - location.Position.ToVector2();
                                if ( velocity.Length() < 50 )
                                {
                                }
                                //TimeSpan time = gameTime.ElapsedGameTime - starttime;
                                //velocity /= time.Ticks;
                            }
                        }
                        break;
                }
            }

            /*
            if(velocity != Vector2.Zero && !touching)
            {
                ScreenPosition.Offset((int)velocity.X, (int)velocity.Y);
                
                if(velocity.X > 1 && velocity.X < -1)
                {
                    velocity.X /= 10;
                }else{
                    velocity.X = 0;
                }

                if(velocity.Y > 1 && velocity.Y < -1)
                {
                    velocity.Y /= 10;
                }else{
                    velocity.Y = 0;
                }
            }
            */
        }
    }
}
