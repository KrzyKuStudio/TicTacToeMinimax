using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAsyncMessageBox;

namespace TicTacToeMinimax
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private Fields
        //------------------------------------------------------
        //
        //  private Fields
        //
        //------------------------------------------------------
        Player turn;
        Player initialTurn;
        System.Windows.Threading.DispatcherTimer timer;
        TTTBoard board;
        bool inProgress;
        bool wait;
        Random rnd = new Random();
        Player aiPlayer = Player.PLAYERX;
        Player humanPlayer = Player.PLAYERO;
        #endregion private Fields

        /// <summary>
        /// Main Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            btnNewGame_Click(null, null);
            this.turn = RandomizePlayer();
            this.initialTurn = turn;

            //clock timer
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
        }

        /// <summary>
        /// New game button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            board = new TTTBoard(3);
            this.turn = this.initialTurn == Player.PLAYERO ? Player.PLAYERX : Player.PLAYERO;
            this.initialTurn = this.initialTurn == Player.PLAYERO ? Player.PLAYERX : Player.PLAYERO;
            this.wait = false;
            this.inProgress = true;
        }

        /// <summary>
        /// Find control by string and change image to source string
        /// </summary>
        /// <param name="button"></param>
        /// <param name="source"></param>
        private void ChangeButtonImage(string button, string source)
        {
            object item = ButtonGrid.FindName(button);
            Button knefel = (Button)item;
            knefel.Background = new ImageBrush { ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(source) };
        }

        /// <summary>
        /// Find control by string and chage image to source
        /// </summary>
        /// <param name="button"></param>
        /// <param name="source"></param>
        private void ChangeRoundImage()
        {
            if (this.turn == Player.PLAYERX)
            {
                imagePlayerRound.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/krzyzyk_m.png");
            }
            else
            {
                imagePlayerRound.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/kolko_m.png");
            }
        }

        /// <summary>
        /// Redraws gamefield
        /// </summary>
        private void DrawBoard(TTTBoard board)
        {
            for (int row = 0; row < board.Dim; row++)
            {
                for (int col = 0; col < board.Dim; col++)
                {
                    if (board.Square(row, col) == (int)Player.PLAYERO)
                    {
                        ChangeButtonImage("btn" + row.ToString() + col.ToString(), "Images/kolko.png");
                    }
                    else if (board.Square(row, col) == (int)Player.PLAYERX)
                    {
                        ChangeButtonImage("btn" + row.ToString() + col.ToString(), "Images/krzyzyk.png");
                    }
                    else if (board.Square(row, col) == (int)Player.EMPTY)
                    {
                        ChangeButtonImage("btn" + row.ToString() + col.ToString(), "Images/nic.png");
                    }
                }
            }
        }

        /// <summary>
        /// Main Game Loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            DrawBoard(board);
            if (this.inProgress)
            {
                ChangeRoundImage();

                // Run AI, if necessary
                if (!this.wait) AiMove();
                else this.wait = false;
            }

        }

        /// <summary>
        /// Put X to correct place in board from view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerMakeMove(object sender, RoutedEventArgs e)
        {
            string rowString = (sender as Button).Name[3].ToString();
            string colString = (sender as Button).Name[4].ToString();
            int row = Convert.ToInt32(rowString);
            int col = Convert.ToInt32(colString);

            if (this.inProgress &&
                this.turn == this.humanPlayer &&
                this.board.Square(row, col) == (int)Player.EMPTY
                )
            {
                board.Move(row, col, humanPlayer);
                this.turn = this.aiPlayer;

                Player winner = this.board.CheckWin();
                if (winner != Player.NONE)
                {
                    GameOver(winner);
                }
                this.wait = true;
            }

            //    def click(self, position):
            //        """
            //        Make human move.
            //        """
            //        if self._inprogress and (self._turn == self._humanplayer):        
            //            row, col = self.get_grid_from_coords(position)
            //            if self._board.square(row, col) == provided.EMPTY:
            //                self._board.move(row, col, self._humanplayer)
            //                self._turn = self._aiplayer
            //                winner = self._board.check_win()
            //                if winner is not None:
            //                    self.game_over(winner)
            //                self._wait = True

        }

        /// <summary>
        /// Choose random player
        /// </summary>
        /// <returns></returns>
        private Player RandomizePlayer()
        {
            // Random rnd = new Random();
            float random = rnd.Next(100);
            if (random > 50)
            {
                return Player.PLAYERO;
            }
            return Player.PLAYERX;
        }

        /// <summary>
        /// Make Ai Move
        /// </summary>
        private void AiMove()
        {
            if (this.inProgress && (this.turn == aiPlayer))
            {
                int[] rowCol = MoveWrapper(this.board, this.aiPlayer);
                if ((Player)this.board.Square(rowCol[0], rowCol[1]) == Player.EMPTY) this.board.Move(rowCol[0], rowCol[1], aiPlayer);
                this.turn = this.humanPlayer;
                Player winner = this.board.CheckWin();
                if (winner != Player.NONE)
                {
                    GameOver(winner);
                }
            }
        }

        /// <summary>
        /// Get Score based on player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private int Score(Player player)
        {
            if (player == Player.PLAYERO)
            {
                return -1;
            }
            else if (player == Player.PLAYERX)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private int[] MoveWrapper(TTTBoard board, Player player)
        {
            int[] move = mmMove(board, player).Item2;
            if (move[0] == -1 && move[1] == -1) throw new Exception("ilegal move -1,-1");
            return move;
        }

        /// <summary>
        ///  Make a move on the board
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private Tuple<int, int[]> mmMove(TTTBoard board, Player player)
        {
            if ((int)board.CheckWin() == 1)
            {
                return new Tuple<int, int[]>(Score(board.CheckWin()), new int[] { -1, -1 });
            }
            else if ((board.GetEmptySquares()).Count == (board.Dim * board.Dim))
            {
                return new Tuple<int, int[]>(0, new int[] { rnd.Next(board.Dim), rnd.Next(board.Dim) });
            }
            else if (board.GetEmptySquares().Count == 0)
            {
                return new Tuple<int, int[]>(Score(board.CheckWin()), new int[] { -1, -1 });
            }

            List<int> listScore = new List<int>();
            List<int[]> listMove = new List<int[]>();
            Player newPlayer = player == Player.PLAYERO ? Player.PLAYERX : Player.PLAYERO;

            List<int[]> moves = board.GetEmptySquares();
            foreach (int[] move in moves)
            {
                TTTBoard newBoard = board.Clone();
                newBoard.Move(move[0], move[1], player);

                int newScore = mmMove(newBoard, newPlayer).Item1;
                listScore.Add(newScore);
                listMove.Add(move);

                if (player == Player.PLAYERX && newScore == 1) break;
                else if (player == Player.PLAYERO && newScore == -1) break;
            }
            int[] moveNew;
            int newScoreNew;
            if (player == Player.PLAYERX)
            {
                newScoreNew = listScore.Max();
                moveNew = listMove[listScore.IndexOf(newScoreNew)];
            }
            else
            {
                newScoreNew = listScore.Min();
                moveNew = listMove[listScore.IndexOf(newScoreNew)];
            }
            return new Tuple<int, int[]>(newScoreNew, moveNew);
        }

        private void GameOver(Player winner)
        {
            if (winner == Player.DRAW)
            {
                AsyncMessageBox.BeginMessageBoxAsync(
                "Remis",
                "Tic Tac Toe",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            else if (winner == Player.PLAYERO)
            {
                AsyncMessageBox.BeginMessageBoxAsync(
              "O wygrywa",
               "Tic Tac Toe",
              MessageBoxButton.OK,
              MessageBoxImage.Information);
            }
            else if (winner == Player.PLAYERX)
            {
                AsyncMessageBox.BeginMessageBoxAsync(
                "X Wygrywa",
                "Tic Tac Toe",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            //Game is no longer in progress
            this.inProgress = false;

        }
    }
}
