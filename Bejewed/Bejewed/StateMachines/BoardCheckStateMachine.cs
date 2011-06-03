using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Bejeweled.GameObjects;

namespace Bejeweled.StateMachines
{
    class BoardCheckStateMachine : StateMachine
    {
        public enum States { SwapTiles, RemoveTiles, DropTiles }
        private States curentState = States.SwapTiles;

        private int[][] board;
        private List<List<Tile>> tiles;

        public bool abortMachine = false;
        public bool isFinished = false;
        private bool finishState = false;

        private List<List<Vector2>> matches;

        // State SwapTiles
        private Vector2[] swapTiles;
        private Vector2[] swapDirections = new Vector2[2];
        private Vector2[] swapPositions = new Vector2[2];
        private Vector2[] swapDeltas = new Vector2[2];


        // State RemoveTiles
        private TimeSpan removeLastTime;


        public BoardCheckStateMachine(ref int[][] board, ref List<List<Tile>> tiles, Vector2[] swapTiles)
        {
            this.board = board;
            this.tiles = tiles;
            this.swapTiles = swapTiles;

            // Start the machine
            this.SwapTiles();
        }

        public void Update(GameTime gameTime)
        {
            switch (this.curentState)
            {
                case States.SwapTiles:
                    {
                        this.SwapTilesUpdate(gameTime);
                        break;
                    }
                case States.RemoveTiles:
                    {
                        this.RemoveTilesUpdate(gameTime);
                        break;
                    }
            }
        }

        #region Swap
        private void SwapTiles()
        {
            // Get our direction vectors
            this.swapDirections[0] = this.swapTiles[1] - this.swapTiles[0];
            this.swapDirections[1] = this.swapDirections[0] * -1;

            // Set our velocity vectors
            this.getTileObj(this.swapTiles[0]).Velocity = this.swapDirections[0] * this.getTileObj(this.swapTiles[0]).Speed;
            this.getTileObj(this.swapTiles[1]).Velocity = this.swapDirections[1] * this.getTileObj(this.swapTiles[1]).Speed;

            // Get our new positions
            this.swapPositions[0] = this.getTileObj(this.swapTiles[1]).Position;
            this.swapPositions[1] = this.getTileObj(this.swapTiles[0]).Position;

        }

        private void SwapTilesUpdate(GameTime gameTime)
        {
            this.swapDeltas[0] = (this.swapPositions[0] - this.getTileObj(this.swapTiles[0]).Position) * this.swapDirections[0];
            this.swapDeltas[1] = (this.swapPositions[1] - this.getTileObj(this.swapTiles[1]).Position) * this.swapDirections[1];

            for (int i = 0; i < 2; i++)
            {
                if (this.swapDeltas[i].X < 0 || this.swapDeltas[i].Y < 0)
                {
                    this.getTileObj(this.swapTiles[i]).Position = this.swapPositions[i];
                    this.getTileObj(this.swapTiles[i]).Velocity = Vector2.Zero;

                    if(i == 1)
                    {
                        this.finishState = true;
                        int swap = this.board[(int)this.swapTiles[0].X][(int)this.swapTiles[0].Y];
                        this.board[(int)this.swapTiles[0].X][(int)this.swapTiles[0].Y] = this.board[(int)this.swapTiles[1].X][(int)this.swapTiles[1].Y];
                        this.board[(int)this.swapTiles[1].X][(int)this.swapTiles[1].X] = swap;
                    }
                }
                else
                {
                    this.getTileObj(this.swapTiles[i]).Move(gameTime);
                }
            }

            // Finish up the state
            if (this.finishState)
            {
                int bSwap = this.board[(int)this.swapTiles[0].X][(int)this.swapTiles[0].Y];
                this.board[(int)this.swapTiles[0].X][(int)this.swapTiles[0].Y] = this.board[(int)this.swapTiles[1].X][(int)this.swapTiles[1].Y];
                this.board[(int)this.swapTiles[1].X][(int)this.swapTiles[1].Y] = bSwap;

                if (this.abortMachine)
                {
                    this.isFinished = true;
                    return;
                }

                if (this.checkGameBoard())
                {
                    this.curentState = States.RemoveTiles;
                    RemoveTiles(gameTime);
                }
                else
                {
                    Vector2 vSwap = this.swapPositions[0];
                    this.swapPositions[0] = this.swapPositions[1];
                    this.swapPositions[1] = vSwap;

                    vSwap = this.swapDirections[0];
                    this.swapDirections[0] = this.swapDirections[1];
                    this.swapDirections[1] = vSwap;

                    // Set our velocity vectors
                    this.getTileObj(this.swapTiles[0]).Velocity = this.swapDirections[0] * this.getTileObj(this.swapTiles[0]).Speed;
                    this.getTileObj(this.swapTiles[1]).Velocity = this.swapDirections[1] * this.getTileObj(this.swapTiles[1]).Speed;

                    this.finishState = false;
                    this.abortMachine = true;
                }
            }
        }
        #endregion Swap

        #region Remove
        private void RemoveTiles(GameTime gameTime)
        {
            this.removeLastTime = gameTime.TotalGameTime;

            foreach (List<Vector2> group in this.matches)
            {
                group.Sort(new Vector2Comparer());
            }
        }

        private void RemoveTilesUpdate(GameTime gameTime)
        {
            if (this.removeLastTime.Subtract(gameTime.TotalGameTime).Duration().Seconds > 0.5)
            {
                Vector2 tile = this.matches.ElementAt(0).ElementAt(0);
                this.board[(int)tile.X][(int)tile.Y] = -1;
                this.tiles[(int)tile.X][(int)tile.Y] = null;

                this.matches.ElementAt(0).RemoveAt(0);

                if (this.matches.ElementAt(0).Count < 1)
                    this.matches.RemoveAt(0);


                this.removeLastTime = gameTime.TotalGameTime;
            }

            if (this.matches.Count < 1)
            {
                this.curentState = States.DropTiles;
            }
        }
        #endregion Remove

        #region Utility Methods
        private void scoreBoard()
        {
            //findGroups();
        }

        private List<List<Vector2>> findGroups()
        {
            List<List<Vector2>> groups = new List<List<Vector2>>();
            for (int i = 0; i < this.board.Length; i++)
            {
                for (int j = 0; j < this.board[0].Length; j++)
                {
                    //groups.Add(findGroup(i,j));
                }
            }
            return groups;
        }

        /*private List<Vector2> findGroup(int i, int j)
        {

        }*/

        private Tile getTileObj(Vector2 tile)
        {
            return this.tiles.ElementAt((int)tile.X).ElementAt((int)tile.Y);
        }

        private bool checkGameBoard()
        {
            this.matches = findGroups();

            for(int i = 0; i < this.matches.Count; i++)
                if (this.matches.ElementAt(i).Count < 3)
                    this.matches.RemoveAt(i);

            return (this.matches.Count > 0) ? true : false;
        }
        #endregion
    }
}

public class Vector2Comparer : IComparer<Vector2>
{
    public int Compare(Vector2 x, Vector2 y)
    {
        int xx = (int)x.X, xy = (int)x.Y, yx = (int)y.X, yy = (int)y.Y;

        if (xy != yy)
            return yy.CompareTo(xy);

        return xx.CompareTo(yx);
    }
}