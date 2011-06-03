using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bejeweled.States
{
    public interface IState
    {
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw();
        SpriteBatch SpriteBatch
        {
            get;
            set;
        }
        ContentManager Content
        {
            get;
            set;
        }
    }
}
