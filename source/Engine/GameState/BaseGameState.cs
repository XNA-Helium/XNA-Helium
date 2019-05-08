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
    public class FakePointer<T> where T : class, IBaseObject, IFactoryable 
    {
        protected float ID;

        public FakePointer(T ob)
        {
            ID = ob.ID;
        }

        public FakePointer(float id)
        {
            ID = id;
        }

        public bool Valid
        {
            get
            {
                return Engine.GameState.GameStateSystem.Instance.GetCurrentState.HasObject(ID);
            }
        }
        public float IDNum
        {
            get
            {
                return ID;
            }
        }
        public T Value
        {
            get
            {
                if (ID == -1)
                {
                    Engine.DebugHelper.Break(DebugHelper.DebugLevels.Curious);
                    return null;
                }
#if DEBUG
                BaseObject o = Engine.GameState.GameStateSystem.Instance.GetCurrentState[ID];
                Engine.DebugHelper.Break(!(o is T), DebugHelper.DebugLevels.Explosive);
#endif
                return (T) (IBaseObject) Engine.GameState.GameStateSystem.Instance.GetCurrentState[ID];
            }
        }

        public void Clear()
        {
            ID = -1;
        }
    }
}
namespace Engine.GameState
{
    public abstract class BaseGameState
    {

        public bool Paused = false;

        protected ICollection<BaseObject> objectList;
        protected System.Collections.Generic.Dictionary<float, BaseObject> Dictionary = new Dictionary<float, BaseObject>();

        protected List<BaseObject> addLater = new List<BaseObject>( 128 );
        /// <summary>
        /// Adds the object immediatly if possible, or Later if the object list is currently iterating
        /// </summary>
        /// <param name="baseObject">Object to add</param>
        protected List<BaseObject> removeLater = new List<BaseObject>( 128 );
        protected virtual void AddObject(BaseObject baseObject)
        {
#if DEBUG   // If your adding an object 2x, then something is wrong
            Engine.DebugHelper.Break(objectList.Contains(baseObject), DebugHelper.DebugLevels.Explosive); 
#endif
            objectList.Add( baseObject );
            Dictionary[baseObject.ID] = baseObject;
#if DEBUG
            Engine.LoggingSystem.Instance.Log("GameState - Added:", baseObject, LoggingSystem.LoggingLevels.Important);
#endif
        }
        public virtual void AddToObjectListLater(BaseObject baseObject)
        {
            if (IteratingAddList)
                addLater.Add(baseObject);
            else
                AddObject(baseObject);
        }

        /// <summary>
        /// Remove an object from teh object list
        /// </summary>
        /// <param name="baseObject">Object to remove</param>
        public virtual void RemoveObject( BaseObject baseObject )
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("GameState -Removed:", baseObject, LoggingSystem.LoggingLevels.Important);
            //Engine.DebugHelper.Break(IteratingUpateList, DebugHelper.DebugLevels.Curious);
#endif
            Dictionary.Remove( baseObject.ID );
            objectList.Remove(baseObject);
            baseObject.CleanUp(false);
        }
        public virtual void RemoveFromObjectListLater(BaseObject baseObject)
        {
            removeLater.Add(baseObject);
        }


        /// <summary>
        /// called when the gamestate has been added to the GameStateSystem
        /// </summary>
        public virtual void Initialize()
        {
        }

        protected bool IteratingAddList = false;
        /// <summary>
        /// called when the game state has been removed from the GameStateSystem
        /// This does not mean the gamestate will be deleted, but it will not receive updates
        /// unless readded back to the GameStateSystem
        /// </summary>
        public virtual void Shutdown()
        {
        }


        /// <summary>
        /// The GameState queues itself to be removed from the GameStateSystem if present
        /// </summary>
        public void RemoveSelf()
        {
            GameStateSystem.Instance.RemoveGameStateLater(this as PrimaryGameState);
        }

        protected bool IteratingUpateList = false;
        protected List<IEngineUpdateable> AddToUpdateListLater = new List<IEngineUpdateable>( 12 );
        protected List<IEngineUpdateable> RemoveFromUpdateListLater = new List<IEngineUpdateable>( 12 );
        protected List<IEngineUpdateable> updateList = new List<IEngineUpdateable>( 128 );
        public virtual void Update( GameTime gameTime )
        {
            IteratingAddList = true;
            foreach ( BaseObject b in addLater )
            {
                AddObject( b );
            }
            addLater.Clear();
            IteratingAddList = false;
            foreach ( IEngineUpdateable u in AddToUpdateListLater )
            {
                updateList.Add( u );
            }
            AddToUpdateListLater.Clear();

            IteratingUpateList = true;
            foreach ( IEngineUpdateable i in updateList )
            {
                i.Update( gameTime );
            }
            IteratingUpateList = false;

            foreach ( IEngineUpdateable u in RemoveFromUpdateListLater )
            {
                updateList.Remove( u );
            }
            RemoveFromUpdateListLater.Clear();


            foreach ( BaseObject b in removeLater )
            {
                RemoveObject( b );
            }
            removeLater.Clear();
        }

        protected List<IEngineDrawable> drawList = new List<IEngineDrawable>( 128 );
        public abstract void Draw( GameTime gameTime, SpriteBatch spriteBatch );

        protected List<IEngineDebugDrawable> debugDrawList = new List<IEngineDebugDrawable>( 128 );
        public virtual void DebugDraw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            foreach ( IEngineDebugDrawable i in debugDrawList )
            {
                i.DebugDraw( gameTime, spriteBatch );
            }
        }
        public BaseObject this[float ID]
        {
            internal set
            {
                Dictionary[ID] = value;
            }
            get
            {
                if (!Dictionary.ContainsKey(ID))
                    DebugHelper.Break(DebugHelper.DebugLevels.Explosive);
                return Dictionary[ID];
            }
        }
        public bool HasObject(float ID)
        {
            return Dictionary.ContainsKey(ID);
        }
        public virtual BaseObject FindObject( float ID )
        {
            return Dictionary[ID];
        }
        public virtual bool FindObject( ref BaseObject FindMe )
        {
            if ( objectList.Contains( FindMe ) )
                return true;
            else
                return false;
        }
        public ICollection<BaseObject> ObjectList
        {
            get
            {
                return objectList;
            }
        }
    }
}
