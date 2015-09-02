using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToeMinimax
{
    public enum Player
    {
        EMPTY = 1,
        PLAYERX = 2,
        PLAYERO = 3,
        DRAW = 4,
        NONE = 5
    }
    /// <summary>
    ///  Class to represent a Tic-Tac-Toe board.
    /// </summary>
    public class TTTBoard
    {
        #region Private Fields
        //------------------------------------------------------
        //
        //  private Fields
        //
        //------------------------------------------------------

        const int EMPTY = 1;
        const int PLAYERX = 2;
        const int PLAYERO = 3;
        const int DRAW = 4;

        bool reverse;
        int dim;
        int[,] board;

        #endregion Private Fields

        #region Public Properties
        //------------------------------------------------------
        //
        //  Public Properies
        //
        //------------------------------------------------------

        public int Dim
        {
            get { return dim; }
        }

        /// <summary>
        /// Returns one of the three constants EMPTY, PLAYERX, or PLAYERO 
        /// that correspond to the contents of the board at position (row, col)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int Square(int row, int col)
        {
            return this.board[row, col];
        }

        #endregion Public Properties

        #region Constructor
        //------------------------------------------------------
        //
        //  Constructor
        //
        //------------------------------------------------------

        /// <summary>
        /// Initialize the TTTBoard object with the given dimension and 
        /// whether or not the game should be reversed.
        /// </summary>
        /// <param name="dim"></param>
        /// <param name="reverse"></param>
        /// <param name="board"></param>
        public TTTBoard(int dim, bool reverse = false, int[,] board = null)
        {
            this.dim = dim;
            this.reverse = reverse;
            this.board = board;

            if (board == null)
            {
                //create empty board
                this.board = new int[dim, dim];
                for (int row = 0; row < this.dim; row++)
                {
                    for (int col = 0; col < this.dim; col++)
                    {
                        this.board[row, col] = EMPTY;
                    }
                }
            }
            else
            {
                //copy board
                for (int row = 0; row < dim; row++)
                {
                    for (int col = 0; col < dim; col++)
                    {
                        this.board[row, col] = board[row, col];
                    }
                }
            }
        }

        #endregion Constructor

        #region Private Methods
        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        #endregion Private Methods

        #region Public Methods
        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        /// <summary>
        /// Return a list of (row, col) tuples for all empty squares
        /// </summary>
        /// <returns></returns>
        public List<int[]> GetEmptySquares()
        {
            List<int[]> emptyList = new List<int[]>();

            for (int row = 0; row < this.dim; row++)
            {
                for (int col = 0; col < this.dim; col++)
                {
                    if (this.board[row, col] == EMPTY)
                    {
                        emptyList.Add(new int[] { row, col });
                    }
                }
            }
            return emptyList;
        }

        /// <summary>
        /// Returns a constant associated with the state of the game
        /// If PLAYERX wins, returns PLAYERX.
        /// If PLAYERO wins, returns PLAYERO.
        /// If game is drawn, returns DRAW.
        /// If game is in progress, returns None.
        /// </summary>
        /// <returns></returns>
        public Player CheckWin()
        {
            int[,] board = this.board;
            int dim = this.dim;
            int dimRng = this.dim - 1;

            List<int[]> lines = new List<int[]>();

            //rows
            for (int row = 0; row < this.dim; row++)
            {
                int[] rows = new int[dim];
                int index = 0;
                for (int col = 0; col < this.dim; col++)
                {
                    rows[index] = this.board[row, col];
                    index++;
                }
                lines.Add(rows);
            }

            //cols
            for (int col = 0; col < this.dim; col++)
            {
                int[] cols = new int[dim];
                int index = 0;
                for (int row = 0; row < this.dim; row++)
                {
                    cols[index] = this.board[row, col];
                    index++;
                }
                lines.Add(cols);
            }

            //diags1
            {
                int[] diags = new int[dim];
                int index = 0;
                for (int row = 0; row < this.dim; row++)
                {
                    diags[index] = this.board[row, row];
                    index++;
                }
                lines.Add(diags);
            }

            //diags2
            {
                int[] diags = new int[dim];
                int index = 0;
                for (int row = 0; row < this.dim; row++)
                {
                    diags[index] = this.board[row, dim - row - 1];
                    index++;
                }
                lines.Add(diags);
            }

            //check all lines
            foreach (int[] line in lines)
            {
                if (line.Length == 3 && line[0] != (int)Player.EMPTY)
                {
                    var groups = line.GroupBy(v => v);
                    foreach (var group in groups)
                    {
                        if (group.Count() == 3)
                        {
                            if (this.reverse)
                            {
                                return SwitchPlayer((Player)line[0]);
                            }
                            else
                            {
                                return (Player)line[0];
                            }
                        }
                    }

                }
            }
            //no winner, check for draw
            if (this.GetEmptySquares().Count == 0)
            {
                return Player.DRAW;
            }
            //else
            return Player.NONE;
        }

        /// <summary>
        /// Place player on the board at position (row, col).
        //  player should be either the constant PLAYERX or PLAYERO.
        //  Does nothing if board square is not empty.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="player"></param>
        public void Move(int row, int col, Player player)
        {
            if (this.board[row, col] == (int)Player.EMPTY)
            {
                this.board[row, col] = (int)player;
            }
        }
        /// <summary>
        /// Return a copy of the board.
        /// </summary>
        /// <returns></returns>
        public TTTBoard Clone()
        {
            //    def clone(self):
            //        """
            //        Return a copy of the board.
            //        """
            //using object extensions.cs to new copy
            TTTBoard newBoard = this.Copy();
            return newBoard;
        }

        /// <summary>
        ///  Convenience function to switch players.
        /// </summary>
        /// <returns></returns>
        public Player SwitchPlayer(Player player)
        {
            if (player == Player.PLAYERX)
            {
                return Player.PLAYERO;
            }
            else
            {
                return Player.PLAYERX;
            }
        }
        #endregion Public Methods
    }
}
