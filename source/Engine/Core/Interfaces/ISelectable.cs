using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Engine
{
    interface ISelectable
    {
        Rectangle Position
        {
            get;
            set;
        }
        bool Selected
        {
            get;
            set;
        }
        void IGotSelected();
        void ILostSelection();
        ISelectable NewObjectSelected(ISelectable newObject);
        bool IsSelectable();
    }
}
