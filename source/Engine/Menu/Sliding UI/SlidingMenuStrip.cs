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
    class SlidingMenuStrip : BaseMenu
    {
        public virtual bool ValidationCallback( )
        {
            return true;
        }

        List<SlidingMenuItem> MenuItems;

        public SlidingMenuStrip()
        {
            MenuItems = new List<SlidingMenuItem>();
        }

        public void AddMenuItem(SlidingMenuItem item)
        {
            MenuItems.Add(item);
        }

        public void RemoveMenuItem(SlidingMenuItem item)
        {
            MenuItems.Remove(item);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (SlidingMenuItem item in MenuItems)
            {
                item.Draw(gameTime,spriteBatch);
            }
        }

        public virtual void ProcessInput(GameTime gameTime,ref TouchCollection touches)
        {
            for (int i = 0; i < touches.Count; ++i)
            {
                ProcessInput(touches[i].Position);
            }
        }

        public bool ProcessInput(Point point)
        {
            bool Triggered = false;
            foreach (SlidingMenuItem item in MenuItems)
            {
                if (item.ProcessInput(point))
                {
                    Triggered = true;
                }
            }
            return Triggered;
        }



        public virtual void Update(GameTime gameTime) { }


        public virtual void OnAdd( ) { }

        public virtual void OnRemove( ) { }

        public virtual void OnHide( ) { }

        public virtual void OnShow( ) { }

        public virtual bool UseUnderlyingMenu()
        {
            return false;
        }

        public virtual void Save(Engine.Persistence.PersistenceManager sw)
        {

        }
        public virtual void Load(Engine.Persistence.PersistenceManager sr)
        {

        }
        public virtual void HookUpPointers()
        {

        }

        public virtual void Back()
        {
            DebugHelper.Break(DebugHelper.DebugLevels.Explosive);
        }

        public int GetStaticID()
        {
            return -1;
        }
    }
}
