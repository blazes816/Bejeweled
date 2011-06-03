using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bejeweled.States
{
    class GameMenu : IState
    {
        public void LoadContent(){}
        public void Update(GameTime gameTime){}
        public void Draw(){}
        public ContentManager Content        {
            get;
            set;
        }
        public SpriteBatch SpriteBatch        {
            get;
            set;
        }
    }
}
