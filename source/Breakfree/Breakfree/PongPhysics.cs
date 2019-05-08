using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine;
using Engine.Core;


namespace Breakfree
{
    public class PongPhysics : Physics2DManager
    {
        protected PongPhysics(Rectangle World)
            : base(World)
        {
        }

        public static void SetupPhysics2DManager(Rectangle World)
        {
            instance = new PongPhysics(World);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Physics2D phys in components)
            {
                if (phys.Parent is Paddle) continue;

                // getto gravity, just tune the vector down ever so slightly.
                float length = phys.Velocity.Length();
                phys.Velocity += new Vector3(0, 0, 0.1f);
                phys.Velocity = Vector3.Normalize(phys.Velocity) * length;
            }

            ObjectOnObjectCollision();
            UpdateObjectsWithVelocity();

            base.Update(gameTime);
        }

        public override void ConstrainToWorld(Physics2D phys)
        {
            if (phys.Parent is Paddle)
            {
                base.ConstrainToWorld(phys);
                return;
            }

            phys.collidedThisFrame = false;

            Rectangle rect = phys.Rectangle;
            Vector3 Velocity = phys.Velocity;
            if (rect.Bottom > world.Bottom)
            {
                if (phys.Parent is Ball)
                {
                    PlayingState state = Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState;
                    state.ResetBall(phys.Parent as Ball);
                }
            }
                    
            else if (rect.Top < world.Top)
            {
                (phys.Parent[Placeable.TypeStatic] as Placeable).Position += new Vector3(0, 0, 10);
                Velocity.Z = -phys.Velocity.Z;
            }
            if (rect.Left < world.Left)
            {
                (phys.Parent[Placeable.TypeStatic] as Placeable).Position += new Vector3(10, 0, 0);
                Velocity.X = -phys.Velocity.X;
            }
            else if (rect.Right > world.Right)
            {
                (phys.Parent[Placeable.TypeStatic] as Placeable).Position += new Vector3(-10, 0, 0);
                Velocity.X = -phys.Velocity.X;
            }
                    
            phys.Velocity = Velocity;

            
        }

        public virtual void UpdateObjectsWithVelocity()
        {
            foreach (Physics2D p in components)
            {
                if (p.Static == false)
                {
                    if (p.Velocity != Vector3.Zero)
                    {
                        if (p.Parent[Placeable.TypeStatic] == null) return;

                        //@@ Limit Velocity Here
                        //@@ probably should add a velocity limiting callback?
                        (p.Parent[Placeable.TypeStatic] as Placeable).Position += p.Velocity;
                        p.Moving = true;

                        if (p.Parent is Paddle)
                            p.Velocity = Vector3.Zero;
                    }
                    else
                    {
                        // If i was moving, I am moving
                    }
                }
                else
                {
                    p.Moving = false;
                }
            }
        }
    }
}
