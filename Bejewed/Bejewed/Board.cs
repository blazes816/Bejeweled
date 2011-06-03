using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bejeweled.GameObjects;
using Bejeweled.StateMachines;

namespace Bejeweled
{
    class Board
    {
        private ContentManager content;
        private SpriteBatch spriteBatch;

        private int[][] board;
        private string[] colors;
        private List<List<Tile>> tiles = new List<List<Tile>>();
        public GameObject cursor;

        private Vector2[] swapping = null;
        private Vector2 selected = new Vector2(-1,-1);
        private Vector2 hover = new Vector2(0, 0);
        private ButtonState currentMouseState;
        private BoardCheckStateMachine boardCheckStateMachine = null;


        public Board(ContentManager content, SpriteBatch spriteBatch)
        {
            this.content = content;
            this.SpriteBatch = spriteBatch;
            this.board = level1();
        }

        #region Hook methods
        public void Load()
        {
            // Load tile sprites.  When expanding later we'll need something more sophisticated
            colors = new string[6] { "green", "red", "blue", "yellow", "purple", "orange" };

            for (int i = 0; i < board.Length; i++)
            {
                tiles.Add(new List<Tile>());
                for (int j = 0; j < board[0].Length; j++)
                {
                    tiles.ElementAt(i).Add(
                        new Tile(content.Load<Texture2D>("Images/" + colors[board[i][j]]),
                            new Vector2(25 + (i * Tile._tileSize), 25 + (j * Tile._tileSize)), 
                            0.5f,
                            content.Load<Texture2D>("Images/selected")
                        )
                    );
                }
            }

            // Load cursor
            this.cursor = new Cursor(content.Load<Texture2D>("Images/cursor"), Vector2.Zero, 0f);
        }

        public void Update(GameTime gameTime)
        {
            // Update currently hovered tile
            if (!this.Selected.Equals(this.hover))
                this.tiles.ElementAt((int)this.hover.X).ElementAt((int)this.hover.Y).Selected = false;

            this.hover = getTileSelection();
            this.tiles.ElementAt((int)this.hover.X).ElementAt((int)this.hover.Y).Selected = true;

            // Check mouse click
            if ((Mouse.GetState().LeftButton == ButtonState.Released) &&
                (currentMouseState == ButtonState.Pressed)) fireMouseClick();

            currentMouseState = Mouse.GetState().LeftButton;

            // Handle Swapping state
            if (this.Swapping != null)
            {
                if(this.boardCheckStateMachine == null){
                    this.boardCheckStateMachine = new BoardCheckStateMachine(ref this.board, ref this.tiles, this.Swapping);
                }
                else{
                    this.boardCheckStateMachine.Update(gameTime);
                 /*  if(int points = this.boardCheckStateMachine.hasPoints()){
                 *      // Update points ya'll
                 *  }
                 */
                    if(this.boardCheckStateMachine.isFinished == true){
                        //this.board = newBoard;
                        this.boardCheckStateMachine = null;

                        this.Swapping = null;
                    }
                }
                 
            }
        }

        public void Draw()
        {
            // Draw tiles, hover, selected, and the cursor
            foreach (List<Tile> row in this.tiles) foreach (Tile tile in row) if(tile != null) tile.Draw(this.SpriteBatch);
            cursor.Draw(this.SpriteBatch);
        }
        #endregion

        private bool Swap(Vector2 a, Vector2 b)
        {
            return false;
        }

        #region Utility methods
        // Return the currently hovered over tile
        private Vector2 getTileSelection()
        {
            Vector2 ret = new Vector2((uint)((Mouse.GetState().X - 25) / Tile._tileSize), (uint)((Mouse.GetState().Y - 25) / Tile._tileSize));
            
            ret.X = (ret.X <= (board[0].Length - 1)) ? ret.X : board[0].Length - 1;
            ret.Y = (ret.Y <= (board.Length - 1)) ? ret.Y : board.Length - 1;
            return ret;
        }

        // Handle a single click
        private void fireMouseClick()
        {
            if (Swapping != null) return;

            // Not in swap and no tile is selected
            if (this.Selected.X == -1)
            {
                this.Selected = getTileSelection();
            }
            else
            {
                Vector2 hoveredTile = getTileSelection();
                
                int vDiff = (int)((uint)hoveredTile.X - selected.X);
                int hDiff = (int)((uint)hoveredTile.Y - selected.Y);
                // A swap has been tried.
                if ((-1 <= vDiff) && (vDiff <= 1) && (-1 <= hDiff) && (hDiff <= 1))
                {
                    this.Swapping = new Vector2[2] { hoveredTile, this.Selected };
                }

                this.Selected = new Vector2(-1,-1);
            }
        }
        #endregion

        #region Getters/Setters
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

        public Vector2[] Swapping
        {
            get
            {
                return this.swapping;
            }
            set
            {
                this.swapping = value;
            }
        }

        public Vector2 Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if ((int)this.selected.X != -1)
                    this.tiles.ElementAt((int)this.selected.X).ElementAt((int)this.selected.Y).Selected = false;
                
                this.selected = value;

                if ((int)this.selected.X != -1)
                    this.tiles.ElementAt((int)this.selected.X).ElementAt((int)this.selected.Y).Selected = true;
            }
        }
        #endregion

        #region Levels
        private int[][] level1()
        {
            int[][] level = new int[8][] {  new int[8] { 1, 4, 4, 2, 2, 3, 4, 5 },
                                            new int[8] { 5, 3, 5, 1, 1, 2, 4, 5 },
                                            new int[8] { 1, 1, 4, 5, 2, 4, 2, 2 },
                                            new int[8] { 0, 0, 2, 2, 0, 2, 0, 3 },
                                            new int[8] { 3, 2, 0, 0, 5, 0, 0, 4 },
                                            new int[8] { 1, 1, 3, 5, 4, 2, 5, 0 },
                                            new int[8] { 2, 2, 3, 4, 2, 1, 1, 4 }, 
                                            new int[8] { 3, 4, 0, 0, 1, 2, 3, 0 }};
            return level;
        }
        #endregion

    }
}
