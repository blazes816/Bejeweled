using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bejeweled.GameObjects
{
    class Selected : GameObject
    {
        public Selected(Texture2D sprite, Vector2 position, float depth) : base(sprite, position, depth) {}

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.X != -1) base.Draw(spriteBatch);
        }

        #region Getters/Setters
        // A tiles Position is different in real space than in game memory
        public override Vector2 Position
        {
            get
            {
                return new Vector2(21 + (this.X * Tile._tileSize), 21 + (this.Y * GameObjects.Tile._tileSize));
            }
            set
            {
                this.X = (int)value.X;
                this.Y = (int)value.Y;
            }
        }
        #endregion
    }
}
