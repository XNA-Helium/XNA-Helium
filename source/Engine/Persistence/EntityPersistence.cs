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

namespace Engine.Persistence
{
    public abstract class Persistable
    {
        protected float MyID = Engine.Core.IDManager.GetNextInstanceID();
        public enum BitMask : int{ One = 1, Two = 2, Three = 4, Four = 8,Five = 16, Six = 32, Seven = 64, Eight = 128}; 
       
        /// <summary>
        /// SaveDeltaState
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="EntireState">True to save entire state, False to just save Changes</param>
        public abstract void Save( System.IO.Stream sw, bool EntireState );

        /// <summary>
        /// LoadLastState
        /// </summary>
        /// <param name="s"></param>
        public abstract void ReloadEntireState( System.IO.Stream s );

        /// <summary>
        /// ReplayToPresent
        /// </summary>
        /// <param name="s"></param>
        public abstract void LoadFrameForReplay( System.IO.Stream s );

        public bool BitSet( BitMask mask,ref int data )
        {
            return ((int)mask & data) == (int)mask;
        }

        public void SetBit( BitMask mask,ref int data )
        {
            data |= (int)mask;
        }


    }

}
