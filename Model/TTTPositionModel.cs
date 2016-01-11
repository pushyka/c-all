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

        public event EventHandler<BoardChangedEventArgs> BoardChanged;
        public event EventHandler CapturedChanged;
        public event EventHandler PlayerChanged;

        public TTTPositionModel()
        {
            this.dim = 3;
            this.board = new TileStruct[dim, dim];
            this.player = new Player("white"); //fix player class
            this.rowToWin = dim;
        
        }

        public void Setup()
        {
            for (int row = 0; row < this.dim; row ++)
            {
                for (int col = 0; col < this.dim; col ++)
                {
                    this.board[row, col] = new TileStruct();
                }
            }
        }

        public void SetPlayer()
        {
            //
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




        /* Takes a move (a board coordinate) which has been checked for emptiness.
           Add a piece of the current player to the location in the move.*/
        public void applyMove(FormedMove move, string moveType)
        {
            Tuple<int, int> location = move.PosA;
            Piece playerPiece;
            if (this.player.CurPlayer == "white")
                playerPiece = new Piece(GamePieces.O);
            else
                playerPiece = new Piece(GamePieces.X);
            ;
            updateTileWithPiece(location, playerPiece);
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
        /// </summary>
        public bool IsWinningPosition(ref string winner)
        {
            bool isWin = false;
            // rows
            
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
