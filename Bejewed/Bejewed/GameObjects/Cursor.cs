using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Bejeweled.GameObjects
{
    class Cursor : GameObject
    {
        new public float Depth = 0f;

        public Cursor(Texture2D sprite, Vector2 position, float depth) : base(sprite, position, depth) {}
        public Cursor(Texture2D sprite)
        {
            this.Sprite = sprite;
        }

        #region Getters/Setters
        public override Vector2 Position
        {
            get
            {
                return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }
        #endregion
    }
}
