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
    public abstract class BaseObject : Factoryable, IBaseObject, IEngineUpdateable
    {
        public static new ReturnsNew ReturnNew
        {
            get
            {
                Engine.DebugHelper.Break(DebugHelper.DebugLevels.Critical);
                return null;
            }
        }
        
        //public static Type TypeStatic = typeof( BaseObject );

        protected Type UniqueClassID;
        public Type Type
        {
            get
            {
                return UniqueClassID;
            }
        }

        protected bool inWorld;
        public bool InWorld
        {
            get
            {
                return inWorld;
            }
        }
        protected System.Collections.Generic.Dictionary<Type, BaseComponent> _ComponentCollection = new Dictionary<Type,BaseComponent>(16);

        public BaseObject( bool InWorld, float uID )
            : base( InWorld,uID )
        {
            inWorld = InWorld;
            if ( inWorld )
            {
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater( this );
            }
        }
        public BaseObject( bool InWorld )
            : this( InWorld, -1 )
        {
        }
        public BaseObject():this(true, -1)
        {

        }

        public virtual void Update( GameTime p_time ){}

        public virtual bool GetComponent<T>(Type type, out T IsIn) where T : BaseComponent
        {
            T Object = (T)_ComponentCollection[type];
            if (Object != null)
            {
                IsIn = Object;
                return true;
            }
            else
            {
                IsIn = null;
                return false;
            }
        }
        /// <summary>
        /// This will break if the object is null
        /// If you want to do checking in your code use a method that has a Bool parameter. 
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve</typeparam>
        /// <returns>The component you requested</returns>
        public virtual T GetComponent<T>() where T : BaseComponent
        {
            T Object = (T)_ComponentCollection[typeof(T)];
            DebugHelper.Break(Object == null, DebugHelper.DebugLevels.Critical);
            return Object;
        }
        public virtual T GetComponent<T>(out bool IsIn) where T : BaseComponent
        {
            IsIn = false;
            T Object = (T)_ComponentCollection[typeof(T)];
            if (Object != null)
            {
                IsIn = true;
            }
            return Object;
        }

        public virtual T GetComponent<T>( Type type, out bool IsIn ) where T : BaseComponent
        {
            IsIn = false;
            T Object = ( T ) _ComponentCollection[type];
            if ( Object != null )
            {
                IsIn = true;
            }
            return Object;
        }

        public virtual BaseComponent this[Type p_ComponentName]
        {
            set
            {
                if ( _ComponentCollection.ContainsKey( p_ComponentName ) )
                {
                    if (! (_ComponentCollection[p_ComponentName] == value ) )
                    {
                        RemoveComponent( p_ComponentName );
                        ( value as BaseComponent ).Parent = this;
                        ( value as BaseComponent ).Init();
                        _ComponentCollection[p_ComponentName] = value;
                    }
                }
                else
                {
                    ( value as BaseComponent ).Parent = this;
                    ( value as BaseComponent ).Init();
                    _ComponentCollection[p_ComponentName] = value;
                }
            }
            get
            {
                if ( _ComponentCollection.ContainsKey( p_ComponentName ) )
                {
                    return _ComponentCollection[p_ComponentName] as BaseComponent;
                }
                else
                {
                    return null;
                }
            }
        }
        public virtual void AddComponent( BaseComponent p_Component )
        {
            this[p_Component.Type] = p_Component;
        }
        public virtual void AddComponent( Type p_ComponentName, BaseComponent p_Component )
        {
            this[p_ComponentName] = p_Component;
        }
        public virtual void RemoveComponent(Type p_ComponentType)
        {
            ( _ComponentCollection[p_ComponentType] as BaseComponent ).OnRemoved( this );
            _ComponentCollection.Remove( p_ComponentType );
        }

        /// <summary>
        /// This should only be called from the event manager, or outside of any game loops
        /// </summary>
        public void CleanUp()
        {
            CleanUp( true );
        }

        /// <summary>
        /// This should only be called from the event manager, or outside of any game loops
        /// </summary>
        public virtual void RemoveMeInTime( )
        {
            this.CleanUp();
        }

        public virtual void CleanUp(bool RemoveFromObjectList )
        {
            if ( inWorld )
            {
                foreach ( BaseComponent component in _ComponentCollection.Values )
                {
                    component.OnRemoved( this );
                }
                _ComponentCollection.Clear();

                if(RemoveFromObjectList && Engine.GameState.GameStateSystem.Instance.Count > 0 )
                    Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromObjectListLater( this );

            }
            inWorld = false;
        }


        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write( ID );
            bw.Write( inWorld );
            bw.Write( (int) IDManager.MapType(this.Type ) );
            bw.Write( _ComponentCollection.Count );
            foreach ( BaseComponent c in _ComponentCollection.Values )
            {
                bw.Write( c.ID );
                bw.Write( InWorld );
                bw.Write( IDManager.MapType(c.Type) );
                c.Save(sw);
            }
            bw.Flush();
        }

        public override void Load(Engine.Persistence.PersistenceManager sr)
        {
            System.IO.BinaryReader br = sr.BinaryReader;
            
            int componentcount = br.ReadInt32();

            for(int i = 0; i < componentcount; ++i)
            {
                float uID = br.ReadSingle();
                bool InWorld = br.ReadBoolean();
                int Type = br.ReadInt32();
                Type t = IDManager.MapType(Type);

                BaseComponent component = (BaseComponent) IDManager.ReturnClass(Type, InWorld, uID);
                component.Load(sr);

                if(!_ComponentCollection.ContainsKey(t))
                {
                    AddComponent(component);
                }
            }
        }

        public static BaseObject Load( Engine.Persistence.PersistenceManager sr, float UniqueID)
        {
            System.IO.BinaryReader br = sr.BinaryReader;
            //BaseObject b = new BaseObject();
            float SecondUniqueID = br.ReadSingle();
            DebugHelper.Break(UniqueID != SecondUniqueID, DebugHelper.DebugLevels.Explosive);
            bool InWorld = br.ReadBoolean();
            int ObjectType = br.ReadInt32();

            BaseObject NewObject = ( BaseObject ) IDManager.ReturnClass( ObjectType, InWorld, UniqueID );

            DebugHelper.Break( ( NewObject == null ), DebugHelper.DebugLevels.HighlyCritical );
            
            br.BaseStream.Flush();

            NewObject.Load( sr );

            return NewObject as BaseObject;
        }
    }
}
