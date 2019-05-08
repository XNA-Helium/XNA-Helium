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

namespace Engine
{
    public delegate void Callback();
    public delegate void Update(GameTime gameTime);
    public delegate void Draw(GameTime gameTime,SpriteBatch spriteBatch);
    public delegate String ReturnsString();
}
