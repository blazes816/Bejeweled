using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bejeweled
{
    class GameObject
    {
        protected int x = 0;
        protected int y = 0;
        protected Vector2 position;
        protected float depth = 0.0f;
        protected float scale = 1.0f;
        protected float speed = 0.1f;
        protected Vector2 velocity;
        protected Texture2D sprite;
        protected SpriteBatch spriteBatch;

        public GameObject()
        {

        }
        public GameObject(Texture2D sprite, Vector2 position)
        {
            this.Sprite = sprite;
            this.Position = position;
        }
        public GameObject(Texture2D sprite, Vector2 position, float depth)
        {
            this.Sprite = sprite;
            this.Position = position;
            this.Depth = depth;
        }

        #region Draw methods
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, this.Position, null, Color.White, 0, Vector2.Zero, this.Scale, SpriteEffects.None, this.Depth);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D sprite)
        {
            spriteBatch.Draw(sprite, this.Position, null, Color.White, 0, Vector2.Zero, this.Scale, SpriteEffects.None, this.Depth);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 position)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, this.Scale, SpriteEffects.None, this.Depth);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 position, float depth)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, this.Scale, SpriteEffects.None, depth);
        }

        

        public virtual void Draw()
        {
            this.Draw(this.spriteBatch);
        }
        #endregion

        #region Getters/Setters
        public virtual Texture2D Sprite
        {
            get
            {
                return this.sprite;
            }
            set
            {
                this.sprite = value;
            }
        }

        public virtual int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public virtual int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public virtual Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public virtual Vector2 Velocity
        {
            get
            {
                return this.velocity;
            }
            set
            {
                this.velocity = value;
            }
        }

        public virtual float Depth
        {
            get
            {
                return this.depth;
            }
            set
            {
                this.depth = value;
            }
        }

        public virtual float Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
            }
        }

        public virtual float Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                this.speed = value;
            }
        }

        public virtual SpriteBatch SpriteBatch
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
        #endregion
    }
}
