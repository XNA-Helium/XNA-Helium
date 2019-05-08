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
    public class UIBaseMenu : UIElementContainer<Callback>, BaseMenu
    {
        public virtual bool ValidationCallback( )
        {
            return true;
        }

        protected bool MenuUsedInput = false;
        protected MenuSystem MenuSystemInstance
        {
            get
            {
                return MenuSystem.Instance;
            }
        }

        List<BaseUIElement> drawable = new List<BaseUIElement>(12);
        public UIBaseMenu(String BackgroundName, Point Position)
            : base()
        {
            UIBackground _Background = new UIBackground( BackgroundName, Position, 0.99f);
            drawable.Add( _Background );
        }
        public UIBaseMenu( String BackgroundName, Point Position, float DrawLayer )
            :base()
        {
            UIBackground _Background = new UIBackground( BackgroundName, Position, DrawLayer );
            drawable.Add( _Background );
        }
        public UIBaseMenu( )
            : base()
        {

        }
        public virtual void ProcessInput(GameTime gameTime, ref TouchCollection touches)
        {
            MenuUsedInput = false;
            for (int i = 0; i < touches.Count; ++i)
            {
                foreach (KeyValuePair<Callback, BaseUIElement> r in _Elements)
                {
                    if ( base.enabled && r.Value.Extents().Contains( touches[i].Position ) )
                    {
                        if ( ( ( int ) touches[i].state & ( r.Value as UIButton ).ValidStates ) != 0 )
                        {
                            r.Key.Invoke();
                        }
                        MenuUsedInput = true;
                        touches[i].InteractedWith = true;
                        (r.Value as UIButton).Active = true;
                    }
                    else
                    {
                        (r.Value as UIButton).Active = false;
                    }
                }
            }
            if ( touches.Count == 0 )
            {
                foreach ( KeyValuePair<Callback, BaseUIElement> r in _Elements )
                {
                    ( r.Value as UIButton ).Active = false;
                }
            }
            if (InputSystem.Instance.Back() == true)
            {
                Back();
            }
        }
        public virtual void AddUIElement(BaseUIElement element)
        {
            drawable.Add(element);
        }
        public virtual void RemoveUIElement(BaseUIElement element)
        {
            drawable.Remove(element);
        }
        public virtual void Add( BaseUIElement element )
        {
            drawable.Add( element );
        }
        public override void Add(Callback value, BaseUIElement Element)
        {
            base.Add(value, Element);
        }
        public override void Remove(Callback value, BaseUIElement Element)
        {
            base.Remove(value, Element);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < drawable.Count; ++i)
            {
                drawable[i].Draw(gameTime, spriteBatch);
            }
            base.Draw(gameTime,spriteBatch);
        }

        public virtual void OnAdd( ) { }

        public virtual void OnRemove( ) { }

        public virtual void OnHide( ) { }

        public virtual void OnShow( ) { }

        public virtual bool UseUnderlyingMenu()
        {
            return false;
        }
        public virtual void Back( )
        {
            MenuSystem.Instance.PopTopMenu();
        }

        /// <summary>
        /// These need to exist per-menu
        /// </summary>
        //protected static int StaticIDNum;
        //protected static ReturnMenu NewMenu;


        public virtual void Save(Engine.Persistence.PersistenceManager sw)
        {

        }
        public virtual void Load(Engine.Persistence.PersistenceManager sr)
        {

        }
        public virtual void HookUpPointers()
        {

        }
        
        private static Engine.UI.BaseMenu Make<T>() where T : Engine.UI.BaseMenu, new()
        {
            var t = new T();
            return t;
        }

        public static void Setup<T>() where T : Engine.UI.BaseMenu, new()
        {
            Engine.UI.ReturnMenu NewMenu = Make<T>;
            int StaticIDNum = Engine.Core.IDManager.GetNextMenuID();
            Engine.Core.IDManager.AddNewMenu(StaticIDNum, NewMenu, typeof(T));
        }
    }
}   
