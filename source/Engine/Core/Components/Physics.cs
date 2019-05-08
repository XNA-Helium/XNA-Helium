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
    public abstract class Physics : BaseComponent , IRectangle
    {
        OnCollision callback;
        protected bool moving = true;
        protected bool shouldHaveCallback = false;
        public bool collidedThisFrame = false;
        public bool Moving
        {
            get
            {
                return moving;
            }
            set
            {
                HasChanged = true;
                moving = value;
                if ( moving == false )
                {
                    velocity = Vector3.Zero;
                }
            }
        }
        /// <summary>
        /// Only ever set by Load/Save
        /// </summary>
        public bool ShouldHaveCallback
        {
            get
            {
                return shouldHaveCallback;
            }
        }

        public Physics(bool InWorld,float uID):base(InWorld,uID)
        {
        }

        public Physics( OnCollision p_Delegate ):base(true, -1) 
        {
            callback = p_Delegate;
            isStatic = false;
        }

        public void OnCollision( BaseObject p_Collider )
        {
            if(callback != null)
                callback.Invoke( p_Collider );
        }

        protected Rectangle position;
        public Rectangle Rectangle 
        {
            set
            {
                HasChanged = true;
                position = value;
            }
            get
            {
                return position;
            }
        }
        public OnCollision Callback
        {
            get
            {
                return callback;
            }
            set
            {
                HasChanged = true;
                callback = value;
            }
        }
        protected bool isStatic;
        public bool Static
        {
            get
            {
                return isStatic;
            }
            set
            {
                HasChanged = true;
                isStatic = value;
            }
        }
        public Vector3 AddVelocity
        {
            set
            {
                HasChanged = true;
                velocity += value;
            }
        }
        protected Vector3 velocity;
        public Vector3 Velocity
        {
            set
            {
                HasChanged = true;
                velocity = value;
            }
            get
            {
                return velocity;
            }
        }
    }
}
