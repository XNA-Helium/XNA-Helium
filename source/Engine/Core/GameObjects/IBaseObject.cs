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

    public interface IBaseObject : IEngineUpdateable, IFactoryable
    {
        void RemoveMeInTime( );
        bool InWorld
        {
            get;
        }
        void Update( GameTime p_time );

        bool GetComponent<T>( Type type, out T IsIn ) where T : BaseComponent;

        T GetComponent<T>( Type type, out bool IsIn ) where T : BaseComponent;

        BaseComponent this[Type p_ComponentName]
        {
            set;
            get;
        }

        void AddComponent( BaseComponent p_Component );
        void AddComponent( Type p_ComponentName, BaseComponent p_Component );

        void RemoveComponent( Type p_ComponentType );

        void CleanUp( );
        void CleanUp( bool RemoveFromObjectList );

        void Save(Engine.Persistence.PersistenceManager sw);
        void Load(Engine.Persistence.PersistenceManager sw);
    }


}
