using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class TTTPositionModel : IDisplayableModel
    {
        private int dim;
        private TileStruct[,] board;
        private Player player;
        private int rowToWin;
        private int turns;

        public event EventHandler<BoardChangedEventArgs> BoardChanged;
        public event EventHandler CapturedChanged;
        public event EventHandler PlayerChanged;

        public TTTPositionModel()
        {
            this.dim = 3;
            this.board = new TileStruct[dim, dim];
            this.player = new Player("X"); //fix player class
            this.rowToWin = dim;
            this.turns = 0;
        
        }

        public void Setup()
        {
            for (int row = 0; row < this.dim; row ++)
            {
                for (int col = 0; col < this.dim; col ++)
                {
                    this.board[row, col] = new TileStruct(new Piece(GamePieces.empty));
                }
            }
        }

        public void SetPlayer()
        {
            //
        }


        /// <summary>
        /// Given a move object, check that the move would be valid against the current
        /// ttt model. All moves would be valid so long as they dont specify a a tile
        /// which has already been clicked. eg so long as they are empty.
        /// This uses the GamePieces.empty version of empty
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool validateMove(FormedMove move)
        {
            Tuple<int, int> location = move.PosA;
            return this.board[location.Item1, location.Item2].piece.Val == GamePieces.empty;
        }



        /* Takes a move (a board coordinate) which has been checked for emptiness.
           Add a piece of the current player to the location in the move.*/
        public void applyMove(FormedMove move, string moveType)
        {
            Tuple<int, int> location = move.PosA;
            Piece playerPiece;
            if (this.player.CurPlayer == "O")
                playerPiece = new Piece(GamePieces.O);
            else
                playerPiece = new Piece(GamePieces.X);
            ;
            updateTileWithPiece(location, playerPiece);
            turns += 1;
        }



        private void updateTileWithPiece(Tuple<int, int> location, Piece newPiece)
        {
            ;
            int row = location.Item1;
            int col = location.Item2;
            ;
            board[row, col].piece = newPiece;
            // finally fire the BoardChanged event!
            // EventArgs could be the tile coordinates which have changed
            BoardChangedEventArgs e = new BoardChangedEventArgs();
            e.Add(location);
            OnBoardChanged(e);
        }




        /// <summary>
        /// Checks the board for the existence of a winning position for either player.
        /// If a winning position is found it returns true and puts the winner in the winner
        /// variable passed by reference. 
        /// 
        /// </summary>
        public bool IsWinningPosition(ref string winner)
        {
            bool isWin = false;
            GamePieces[] players = new GamePieces[] { GamePieces.X, GamePieces.O };
            foreach (GamePieces player in players)
            {
                // check verticals
                if ((board[0, 0].piece.Val == player && board[1, 0].piece.Val == player && board[2, 0].piece.Val == player) ||
                    (board[0, 1].piece.Val == player && board[1, 1].piece.Val == player && board[2, 1].piece.Val == player) ||
                    (board[0, 2].piece.Val == player && board[1, 2].piece.Val == player && board[2, 2].piece.Val == player))
                {
                    winner = player.ToString();
                    isWin = true;
                    break;
                }

                // check horizontals

                if ((board[0, 0].piece.Val == player && board[0, 1].piece.Val == player && board[0, 2].piece.Val == player) ||
                    (board[1, 0].piece.Val == player && board[1, 1].piece.Val == player && board[1, 2].piece.Val == player) ||
                    (board[2, 0].piece.Val == player && board[2, 1].piece.Val == player && board[2, 2].piece.Val == player))
                {
                    winner = player.ToString();
                    isWin = true;
                    break;
                }

                // check diagonals

                if ((board[0, 0].piece.Val == player && board[1, 1].piece.Val == player && board[2, 2].piece.Val == player) ||
                    (board[2, 0].piece.Val == player && board[1, 1].piece.Val == player && board[0, 2].piece.Val == player))
                {
                    winner = player.ToString();
                    isWin = true;
                    break;
                }



            }
            
            return isWin;
        }


        public Player Player
        {
            get
            {
                return this.player;
            }
            set
            {
                this.player = value;
                // add the event
            }
        }



        public TileStruct[,] Board
        {
            get
            {
                return this.board;
            }
        }

        public List<GamePieces> PiecesCapd
        {
            get
            {
                return null;
            }
        }
        public bool IsMaxTurns(ref bool isMaxTurns)
        {
            isMaxTurns = this.turns == (this.dim * this.dim);
            return isMaxTurns;
        }



        public void ChangePlayer()
        {
            this.player.change();
            OnPlayerChanged(EventArgs.Empty);
        }

        protected virtual void OnPlayerChanged(EventArgs e)
        {
            if (PlayerChanged != null)
            {
                PlayerChanged(this, e);
            }
        }
        



        // this method is called by some code (when the code changes the board) and raises the event 
        protected virtual void OnBoardChanged(BoardChangedEventArgs e)
        {
            // eg makes sure the Event has a delegate attached
            if (BoardChanged != null)
            {
                BoardChanged(this, e);
            }
        }


    }
}
