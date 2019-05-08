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

namespace Engine.GameState
{
    public class PrimaryGameState : BaseGameState
    {

        public virtual void OnObjectLoaded(BaseObject bobject) 
        {
            bobject.HookUpPointers();
        }

        public virtual void Save(Engine.Persistence.PersistenceManager sw)
        {
            // Clear out all the pending objects
            {
                // Make sure the game state we are saving is COMPLETELY clean
                // We don't want any objects to reference other objects
                // that are half dead..
                while (addLater.Count > 0 || AddToUpdateListLater.Count > 0 || RemoveFromUpdateListLater.Count > 0 || removeLater.Count > 0)
                {
                    IteratingAddList = true;
                    foreach (BaseObject b in addLater)
                    {
                        AddObject(b);
                    }
                    addLater.Clear();
                    IteratingAddList = false;
                    foreach (IEngineUpdateable u in AddToUpdateListLater)
                    {
                        updateList.Add(u);
                    }
                    AddToUpdateListLater.Clear();

                    foreach (IEngineUpdateable u in RemoveFromUpdateListLater)
                    {
                        updateList.Remove(u);
                    }
                    RemoveFromUpdateListLater.Clear();
                    foreach (BaseObject b in removeLater)
                    {
                        RemoveObject(b);
                    }
                    removeLater.Clear();
                }
            }
            // Begin Saving
            {
                System.IO.BinaryWriter bw = sw.BinaryWriter;
                bw.Write(Dictionary.Count);
                bw.Flush();
            }
            foreach (BaseObject b in Dictionary.Values)
            {
                {
                    System.IO.BinaryWriter bw = sw.BinaryWriter;
                    bw.Write(b.ID);
                    bw.Flush();
                }
                b.Save(sw);
#if DEBUG
                Engine.DebugHelper.Break(IDManager.MapType(b.Type) == null, DebugHelper.DebugLevels.Explosive);
#endif
            }
        }

        public virtual void Load(Engine.Persistence.PersistenceManager sw)
        {
            int count = 0;
            {
                System.IO.BinaryReader br = sw.BinaryReader;
                count = br.ReadInt32();
            }

            List<BaseObject> Objects = new List<BaseObject>();

            for ( int i = 0; i < count; ++i )
            {
                float ID = -1;
                {
                    System.IO.BinaryReader br = sw.BinaryReader;
                    ID = br.ReadSingle();
                }
                Engine.DebugHelper.Break( ID == -1, DebugHelper.DebugLevels.Explosive );
                if ( Dictionary.ContainsKey( ID ) )
                {
                    {
                        System.IO.BinaryReader br = sw.BinaryReader;
                        
                        // This data is loaded in BaseObject.Load if we don't clear it out there will be trouble
                        //BaseObject b = new BaseObject();
                        float SecondUniqueID = br.ReadSingle();
                        DebugHelper.Break(ID != SecondUniqueID, DebugHelper.DebugLevels.Explosive);
                        bool InWorld = br.ReadBoolean();
                        int ObjectType = br.ReadInt32();
                    }
                    Dictionary[ID].Load(sw);
                }
                else
                {
                     BaseObject.Load( sw, ID ) ; // This call adds it to world
                }
                Objects.Add(Dictionary[ID]);
            }
            float LargestID  = 10000.0f;
            foreach (BaseObject b in Objects)
            {
                OnObjectLoaded(b);
                if (b.ID > LargestID)
                    LargestID = b.ID;
            }
            Objects.Clear();

            LargestID += 1000;
            IDManager.SetNextInstanceID(LargestID);
        }

        public virtual bool Contains( BaseObject gameobject )
        {
            return objectList.Contains( gameobject );
        }

        public virtual void AddToUpdateList( Engine.IEngineUpdateable iupdatable )
        {
            if ( !IteratingUpateList )
                updateList.Add( iupdatable );
            else
                AddToUpdateListLater.Add( iupdatable );
        }
        public virtual void AddToUpdateList( Engine.Core.BaseComponent component )
        {
            if ( !IteratingUpateList )
                updateList.Add( component );
            else
                AddToUpdateListLater.Add( component );
        }
        public virtual void AddToUpdateList( Engine.Core.BaseObject Object )
        {
            if ( !IteratingUpateList )
                updateList.Add( Object );
            else
                AddToUpdateListLater.Add( Object );

        }
        public virtual void RemoveFromUpdateList( IEngineUpdateable updatable )
        {
            if ( !IteratingUpateList )
                updateList.Remove( updatable );
            else
                RemoveFromUpdateListLater.Add( updatable );
        }

        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend );
            foreach ( Drawable2D d in TwoDList )
            {
                d.Draw( gameTime, spriteBatch, this.Paused );
            }
            foreach (DrawableText d in TextList)
            {
                d.Draw(gameTime, spriteBatch, this.Paused);
            }
            spriteBatch.End();
            MenuSystem.Instance.GraphicsDevice.GraphicsDevice.Clear( ClearOptions.DepthBuffer, Color.Black, 1.0f, 0 ); //Clear the DepthBuffer only
            foreach ( Drawable3D d in ThreeDList )
            {
                d.Draw(gameTime, spriteBatch, this.Paused);
            }
            MenuSystem.Instance.GraphicsDevice.GraphicsDevice.Clear( ClearOptions.DepthBuffer , Color.Black, 1.0f, 0 ); //Clear the DepthBuffer only
        }

        List<Drawable2D> TwoDList = new List<Drawable2D>(128);
        public virtual void Add2DDrawable( Drawable2D drawable )
        {
            TwoDList.Add( ( Drawable2D ) drawable );
        }

        List<DrawableText> TextList = new List<DrawableText>(128);
        public virtual void AddDrawableText(DrawableText drawable)
        {
            TextList.Add((DrawableText)drawable);
        }

        List<Drawable25D> Two5DList = new List<Drawable25D>(128);
        public virtual void Add25DDrawable( Drawable25D drawable )
        {
            Two5DList.Add( ( Drawable25D ) drawable );
        }

        List<Drawable3D> ThreeDList = new List<Drawable3D>(128);
        public virtual void Add3DDrawable( Drawable3D drawable )
        {
            ThreeDList.Add( ( Drawable3D ) drawable );
        }
        public virtual void RemoveDrawable(Drawable drawable)
        {
            if(drawable is Drawable2D)
            {
                TwoDList.Remove( drawable as Drawable2D );
            }else if(drawable is Drawable25D)
            {
                Two5DList.Remove( drawable as Drawable25D );
            }else if(drawable is Drawable3D)
            {
                ThreeDList.Remove( drawable as Drawable3D);
            }
            else if (drawable is DrawableText)
            {
                TextList.Remove( drawable as DrawableText);
            }
        }
    }
}
