using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bejeweled.GameObjects
{
    class Tile : GameObject
    {
        public const int _tileSize = 56;
        private bool selected = false;
        public Texture2D selectedSprite;

        public Tile(Texture2D sprite, Vector2 position, float depth, Texture2D selectedSprite) : base(sprite, position, depth) {
            this.selectedSprite = selectedSprite;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
         
            if (this.Selected == true)
            {
                base.Draw(spriteBatch, this.selectedSprite, new Vector2(this.Position.X - 4, this.Position.Y - 4), this.Depth+0.05f);
            }
        }

        public void Move(GameTime time)
        {
            Vector2 a = this.Velocity * time.ElapsedGameTime.Milliseconds;
            Vector2 b = this.Position + a;
            this.Position += this.Velocity * time.ElapsedGameTime.Milliseconds;
        }

        #region Getters/Setters
        // A tiles Position is different in real space than in game memory
        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
            }
        }
        #endregion
    }
}
