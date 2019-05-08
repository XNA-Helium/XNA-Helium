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
using Engine.Core;

namespace Name.GameState
{
    public class Physic2DManagerWithTerrain : Physics2DManager
    {
        public static Vector3 gravity = new Vector2( 0.0f, 1.0f).ToVector3();
        protected Physic2DManagerWithTerrain(Terrain Terrain,Rectangle World):base(World)
        {
            terrain = Terrain;
        }
        protected Terrain terrain;
        public static void SetupPhysics2DManager(Terrain Terrain, Rectangle World)
        {
            instance = new Physic2DManagerWithTerrain(Terrain,World);
        }

        public override void Update( GameTime gametTime )
        {
            foreach ( Physics2D c in NewComponents )
            {
                AddToPhysicsWorld(c);
            }
            NewComponents.Clear();

            
            Iterating = true;
            UpdateObjectsWithVelocity();
            AddGravityToObjects();
            ObjectOnObjectCollision(); //Do Not call base.Update(gametTime); 
            ObjectOnTerrainCollision();
            Iterating = false;

            foreach ( Physics2D c in OldComponents )
            {
                RemoveFromPhysicsWorld(c);
            }
            OldComponents.Clear();
        }

        public virtual void AddGravityToObjects( )
        {
            foreach ( Physics2D p in components )
            {
                if ( p.Moving && !p.Static)
                {
                    p.AddVelocity = gravity;
                }
            }
        }

        public virtual void UpdateObjectsWithVelocity( )
        {
            foreach ( Physics2D p in components )
            {
                if ( p.Static == false)
                {
                    if (p.Velocity != Vector3.Zero)
                    {
                        //@@ Limit Velocity Here
                        //@@ probably should add a velocity limiting callback?
                        (p.Parent[Placeable.TypeStatic] as Placeable).Position += p.Velocity;
                        p.Moving = true;
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

        public virtual void ObjectOnTerrainCollision( )
        {
            foreach ( Physics2D g in components )
            {
                Point p = g.Rectangle.Center;
                IProjectile po = ( g.Parent as IProjectile );
                if(po != null)
                {
                    if ( p.X > world.X + world.Width + 32 )
                    {
                        Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromObjectListLater( g.Parent );
                        g.Moving = false;
                    }
                    if ( p.X < world.X -32 )
                    {
                        Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromObjectListLater( g.Parent );
                        g.Moving = false;
                    }
                    if (p.Y > world.Height + 128 )
                    {
                        Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromObjectListLater( g.Parent );
                        g.Moving = false;
                    }
                    if (p.Y <= world.Y - 300 && po == null && !( g.Parent is GrenadeClusterMask ) )
                    {
                        p.Y = 0;
                    }
                }


                if ( !g.Static && g.Moving )
                {
                    IWeapon j = null;
                    if(g.Parent is PlayerObject)
                        j = ( IWeapon ) ( g.Parent[WeaponHolder.TypeStatic] as WeaponHolder ).Weapon;
                    float pY = p.Y + ( g.Rectangle.Height / 2 );
                    if ( ( ( int ) pY ) + terrain.ExtraHeight > terrain[( int ) p.X] )
                    {
                        if ( j is JetPack && ( j as JetPack ).InUse )
                        {
                            ( g.Parent[Placeable.TypeStatic] as Placeable ).Position = new Vector2( p.X, terrain[( int ) p.X] - ( g.Rectangle.Height / 2 ) ).ToVector3();
                            ClearVelocity(g);
                            g.AddVelocity = new Vector2( 0.0f, -2.0f ).ToVector3();
                        }
                        else
                        {
                            ( g.Parent[Placeable.TypeStatic] as Placeable ).Position = new Vector2( p.X, terrain[( int ) p.X] - ( g.Rectangle.Height / 2 ) ).ToVector3();
                            g.Moving = false;

                            OnCollision cc = ( g.Callback );

                            if ( cc != null )
                            {
                                cc.Invoke( g.Parent );
                            }
                        }
                    }
                    if(  ( ( int ) pY + (g.Rectangle.Height/2)) > terrain[( int ) p.X] && j is JetPack && ( j as JetPack ).InUse )
                    {
                        ( g.Parent[Placeable.TypeStatic] as Placeable ).Position = new Vector2( p.X, terrain[( int ) p.X] - ( g.Rectangle.Height) ).ToVector3();
                        ClearVelocity( g );
                        g.AddVelocity = new Vector2( 0.0f, -2.0f ).ToVector3();
                    }
                }
            }
        }
    }
}
