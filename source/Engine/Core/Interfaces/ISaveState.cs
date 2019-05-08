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

    public interface ISaveState
    {
        int Type{  get; }

        bool HasChanged{ get; }
        
        void SaveState(BinaryWriter writer, bool CompleteState, bool ClearFlag);

    	void LoadState(BinaryReader reader);

    	void PlayState( );

        void LoadAndPlayState( BinaryReader reader );
    }
}
