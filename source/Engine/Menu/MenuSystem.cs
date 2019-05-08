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
using Engine.UI;

namespace Engine
{
    public class MenuSystem : IEngineUpdateable
    {

        public static bool PointIsCenter = false; // UI should center on where you put it, make things easier 

        TouchCollection touches;
        Stack<BaseMenu> Menus = new Stack<BaseMenu>( 10 );

        protected GraphicsDeviceManager device;
        public GraphicsDeviceManager GraphicsDevice
        {
            get
            {
                return device;
            }
        }

        protected static MenuSystem instance = null;
        public static MenuSystem Instance
        {
            get
            {
                return instance;
            }
        }
        public static void SetupMenuSystem(GraphicsDeviceManager device)
        {
            instance = new MenuSystem(device);
        }
        protected MenuSystem(GraphicsDeviceManager graphicsDevice)
        {
            this.device = graphicsDevice;
        }

        public virtual void Update(GameTime gameTime)
        {
            touches = InputSystem.Instance.Touches;
            if ( Menus.Count > 0 )
            {
                Menus.Peek().ProcessInput( gameTime, ref touches );
                Menus.Peek().Update( gameTime );
            }
            /*if (Menus.Peek().UseUnderlyingMenu())
            {

            }*/
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend );
            Menus.Peek().Draw(gameTime, spriteBatch);
            /*if (Menus.Peek().UseUnderlyingMenu())
            {
                
            }*/
            spriteBatch.End();
        }


        public virtual void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw all the menus
        }

        public void PushMenu( BaseMenu ms )
        {
            if ( Menus.Count > 0 )
                Menus.Peek().OnHide();

            ms.OnAdd();
            Menus.Push( ms );
            ShowNextValidMenu();
        }

        public BaseMenu CurrentMenu
        {
            get
            {
                return Menus.Peek();
            }
        }

        public void AddMenu(BaseMenu ms)
        {
            PushMenu( ms );
        }

        public void PopTopMenu()
        {
            BaseMenu ms = Menus.Pop();
            ms.OnHide();
            ms.OnRemove();
            ShowNextValidMenu();
        }

        public void ShowNextValidMenu( )
        {
            while( Menus.Count > 0 )
            {
                if ( Menus.Peek().ValidationCallback() )
                {
                    Menus.Peek().OnShow();
                    break;
                }
                else
                {
                    BaseMenu ms = Menus.Pop();
                    ms.OnHide();
                    ms.OnRemove();
                }
            }
        }

        public virtual void Save(Engine.Persistence.PersistenceManager sw, int SkipMenuCount)
        {
            sw.BinaryWriter.Write(Menus.Count - SkipMenuCount);
            for (int i = (Menus.Count - 1 - SkipMenuCount); i >= 0; --i)
            {
                int id = Engine.Core.IDManager.GetMenuID( Menus.ElementAt(i).GetType() );
                sw.BinaryWriter.Write(id);
                Menus.ElementAt(i).Save(sw);
            }
        }

        public virtual void Load(Engine.Persistence.PersistenceManager sr)
        {
            int count = sr.BinaryReader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                int id = sr.BinaryReader.ReadInt32();
                BaseMenu m = Engine.Core.IDManager.GetNewMenu(id);
                m.Load(sr);
                m.HookUpPointers();
                Menus.Push(m);
            }
        }
    }
}
