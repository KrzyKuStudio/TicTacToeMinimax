using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToeMinimax;
using System.Collections.Generic;

namespace TicTacToeMinimaxTest
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// "Empty Squares"
        /// </summary>
        [TestMethod]
        public void InvariantTests0()
        {
            TTTBoard board = new TTTBoard(3);
            List<int[]> squares = board.GetEmptySquares();
            int count = squares.Count;
            Assert.AreEqual(9, count, "Empty squares");
        }

        /// <summary>
        /// "TTboard Clone"
        /// </summary>
        [TestMethod]
        public void TTboardClone()
        {
            TTTBoard board = new TTTBoard(3);
            TTTBoard newBoard = board.Clone();
            board.Move(0, 0, Player.PLAYERX);
            board.Move(0, 1, Player.PLAYERO);
            int emptyBoard = board.GetEmptySquares().Count;
            int emptyNewBoard = newBoard.GetEmptySquares().Count;
            Assert.AreNotEqual(emptyNewBoard, emptyBoard, "Bad clone");
        }

        /// <summary>
        /// "TTboard CheckWin"
        /// </summary>
        [TestMethod]
        public void TTboardCheckWin()
        {
            TTTBoard board = new TTTBoard(3);
            TTTBoard newBoard = board.Clone();
            Assert.AreEqual(Player.NONE, board.CheckWin(), "Check Win game in progres 1");
            board.Move(0, 0, Player.PLAYERO);
            board.Move(0, 1, Player.PLAYERX);
            Assert.AreEqual(Player.NONE, board.CheckWin(), "Check Win game in progres 2");
            board.Move(1, 0, Player.PLAYERO);
            board.Move(0, 2, Player.PLAYERX);
            Assert.AreEqual(Player.NONE, board.CheckWin(), "Check Win game in progres 3");
            board.Move(2, 0, Player.PLAYERO);
            Assert.AreEqual(Player.PLAYERO, board.CheckWin(), "Check Win should be game winner  4");

        }

        /// <summary>
        /// "TTboard CheckWinfull"
        /// </summary>
        [TestMethod]
        public void TTboardCheckWin2()
        {
            TTTBoard board = new TTTBoard(3);
            TTTBoard newBoard = board.Clone();
            Assert.AreEqual(Player.NONE, board.CheckWin(), "Check Win game in progres 1");
            board.Move(0, 0, Player.PLAYERO);
            board.Move(0, 1, Player.PLAYERO);
            Assert.AreEqual(Player.NONE, board.CheckWin(), "Check Win game in progres 2");
            board.Move(1, 0, Player.PLAYERX);
            board.Move(1, 1, Player.PLAYERX);
            Assert.AreEqual(Player.NONE, board.CheckWin(), "Check Win game in progres 3");
            board.Move(1, 2, Player.PLAYERO);
            board.Move(0, 2, Player.PLAYERX);
            Assert.AreEqual(Player.NONE, board.CheckWin(), "heck Win game in progres 4");
            board.Move(2, 0, Player.PLAYERO);
            board.Move(2, 1, Player.PLAYERO);
            board.Move(2, 2, Player.PLAYERX);
            Assert.AreEqual(Player.DRAW, board.CheckWin(), "heck Win game DRAW");

        }
    }
}
