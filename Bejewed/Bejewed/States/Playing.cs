using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bejeweled.States
{
    class Playing : IState
    {
        private Board board;
        private ContentManager content;
        private SpriteBatch spriteBatch;

        public Playing()
        {
            
        }

        public void LoadContent()
        {
            // Initialize our game board
            this.board = new Board(Content , this.SpriteBatch);
            this.board.Load();
        }
        public void Update(GameTime gameTime)
        {
            this.board.Update(gameTime);
        }
        public void Draw()
        {
            this.board.Draw();
        }

        public ContentManager Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
            set
            {
                this.spriteBatch = value;
            }
        }
    }
}
