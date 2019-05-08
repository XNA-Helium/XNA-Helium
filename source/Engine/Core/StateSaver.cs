using System;
using System.IO;
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


namespace Engine.Core
{
	public class StateSaver
	{
        public static bool StateSaverEnabled = false;
		protected List<ISaveState> NewItems = new List<ISaveState>();
		protected List<ISaveState> DeletedItems = new List<ISaveState>();
        protected static StateSaver instance = new StateSaver();
        public static StateSaver Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new StateSaver();
                }
                return instance;
            }
        }

        protected Dictionary<int, ReturnsNew> creation = new Dictionary<int, ReturnsNew>();

        public void AddToMap( int ID, ReturnsNew newObject )
        {

        }
	}
}
