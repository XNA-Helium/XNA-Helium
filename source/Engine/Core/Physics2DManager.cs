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
    public class Physics2DManager
    {
        protected Physics2DManager( Rectangle World )
        {
            world = World;
            QuadTree = new QuadTree<Physics2D>(new FRect(world), 128); //@@ come back and change this number to optimize for the number of objects you have
        }

        protected Rectangle world;
        protected static Physics2DManager instance;
        public static Physics2DManager Instance
        {
            get
            {
                return instance;
            }
        }

        protected List<Physics2D> OldComponents = new List<Physics2D>();
        protected List<Physics2D> NewComponents = new List<Physics2D>();
        protected List<Physics2D> components = new List<Physics2D>();

        protected QuadTree<Physics2D> QuadTree;


        public virtual void ConstrainToWorld(Physics2D g1)
        {
            g1.collidedThisFrame = false;
            if (! g1.Static )
            {
                Point NewCenter = g1.Rectangle.Center;
                Rectangle rect = g1.Rectangle;
                bool Changes = false;
                {
                    if ( rect.Bottom > world.Bottom )
                    {
                        NewCenter.Y = world.Bottom - 1 - rect.Height / 2;
                        Changes = true;
                    }
                    else if ( rect.Top < world.Top )
                    {
                        NewCenter.Y = 1 + world.Top + rect.Height / 2;
                        Changes = true;
                    }
                    if ( rect.Left < world.Left )
                    {
                        NewCenter.X = 1 + world.Left + rect.Width / 2;
                        Changes = true;
                    }
                    else if ( rect.Right > world.Right )
                    {
                        NewCenter.X = world.Right - 1 - rect.Width / 2;
                        Changes = true;
                    }
                    if(Changes)
                        ( g1.Parent[Placeable.TypeStatic] as Placeable ).Position = NewCenter.ToVector3();
                }
            }
        }


        public virtual void ObjectOnObjectCollision( )
        {
            List<QuadTreePositionItem<Physics2D>> Collides = new List<QuadTreePositionItem<Physics2D>>();
            foreach (Physics2D phys in components)
            {
                if (phys == null) continue;

                ConstrainToWorld(phys);

                Collides.Clear();
                
                QuadTree.GetItems(new FRect(phys.Rectangle), ref Collides);

                        if (Collides.Count > 1)
                        {
                            OnCollision cc1 = phys.Callback;
                            foreach (var collider in Collides)
                            {
                                if (collider.Parent != phys) // This is not the original object
                                {
                                    if (cc1 != null)
                                    {
                                        cc1.Invoke(collider.Parent.Parent);
                                    }

                                    OnCollision cc2 = collider.Parent.Callback;
                                    if (cc2 != null)
                                    {
                                        cc2.Invoke(phys.Parent);
                                    }
                                }
                            }
                            phys.collidedThisFrame = true;
                        }
            }
                 
        }

        protected void AddToPhysicsWorld(Physics2D physicsObject)
        {
            Vector3 Position = (physicsObject.Parent[Placeable.TypeStatic] as Placeable).Position;
            physicsObject.QuadPosition = QuadTree.Insert(new QuadTreePositionItem<Physics2D>(physicsObject, Position.ToVector2(),
                new Vector2(physicsObject.Rectangle.Width, physicsObject.Rectangle.Height)));

            components.Add(physicsObject);
            if (!physicsObject.Static)
                physicsObject.Moving = true;

#if DEBUG
            Engine.LoggingSystem.Instance.Log("Physics - Added:", physicsObject, LoggingSystem.LoggingLevels.Important);
#endif
        }

        protected void RemoveFromPhysicsWorld(Physics2D physicsObject)
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("Physics - Removed:", physicsObject, LoggingSystem.LoggingLevels.Important);
#endif
            if (physicsObject.QuadPosition != null)
            {
                physicsObject.QuadPosition.Delete();
                physicsObject.QuadPosition = null;
            }

            components.Remove(physicsObject);
        }

        public void AddToWorld(Physics2D physicsObject)
        {
            if (Iterating)
            {
                NewComponents.Add(physicsObject);
            }
            else
            {
                AddToPhysicsWorld(physicsObject);
            }
        }
        

        public void RemoveFromWorld( Physics2D physicsObject )
        {
            if (Iterating)
            {
                OldComponents.Add(physicsObject);
            }
            else
            {
                RemoveFromPhysicsWorld(physicsObject);
            }
        }

        protected bool Iterating = false;
        /// <summary>
        /// Eithor completely override this method, or use it as is. Do not do both
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update( GameTime gameTime )
        {
            foreach ( Physics2D c in NewComponents )
            {
                AddToPhysicsWorld( c );
            }
            NewComponents.Clear();

            Iterating = true;
            ObjectOnObjectCollision();
            Iterating = false;

            foreach ( Physics2D c in OldComponents )
            {
                RemoveFromPhysicsWorld( c );
            }
            OldComponents.Clear();
        }

        protected void ClearVelocity( Physics2D component )
        {
            component.Velocity = Vector3.Zero;
        }

    }
}
