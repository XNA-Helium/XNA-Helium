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


namespace Engine.UI
{
    public delegate BaseMenu ReturnMenu();

    public interface BaseMenu
    {
        bool ValidationCallback( );

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void Update(GameTime gameTime);

        void ProcessInput(GameTime gameTime,ref TouchCollection touches);

        void OnAdd();

        void OnRemove();

        bool UseUnderlyingMenu();

        void OnShow( );

        void OnHide( );

        void Back( );
        
        void Save(Engine.Persistence.PersistenceManager sw);

        void Load(Engine.Persistence.PersistenceManager sr);

        void HookUpPointers();

    }
}
