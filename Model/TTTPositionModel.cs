using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    /* This is a displayable model but since, for tic tac toe, there is
    not really a notion of pieces captured, there is a null implementation 
    of theses methods.*/
    public class TTTPositionModel : IDisplayableModel
    {
        public event EventHandler<BoardChangedEventArgs> BoardChanged;
        public event EventHandler CapturedChanged;
        public event EventHandler PlayerChanged;

        public TileStruct[,] Board { get; }
        public List<EGamePieces> PiecesCapd { get; }
        public Player Player { get; set; }
        
        private int dim;
        private int rowToWin;
        private int turns;


        public TTTPositionModel()
        {
            this.dim = 3;
            this.Board = new TileStruct[dim, dim];
            this.rowToWin = dim;
            this.turns = 0;
            this.PiecesCapd = null;
        }


        /* Populate the board with (empty) pieces. */
        public void Setup()
        {
            for (int row = 0; row < this.dim; row ++)
            {
                for (int col = 0; col < this.dim; col ++)
                {
                    this.Board[row, col] = new TileStruct(new Piece(EGamePieces.empty));
                }
            }
        }


        /* Given a move object, check that the move would be valid against the current
        ttt model. All moves would be valid so long as they dont specify a tile
        which has already been clicked / is not empty. This uses the GamePieces.empty 
        version of empty rather than null checking. */
        public bool IsValidMove(FormedMove move)
        {
            Tuple<int, int> location = move.PosA;
            return this.Board[location.Item1, location.Item2].piece.Val == EGamePieces.empty;
        }


        /* Takes a move (a board coordinate) which has been checked for emptiness.
           Add a piece of the current player to the location in the move.*/
        public void applyMove(FormedMove move)
        {
            Tuple<int, int> location = move.PosA;
            Piece playerPiece;
            if (this.Player.PlayerValue == "O")
                playerPiece = new Piece(EGamePieces.O);
            else
                playerPiece = new Piece(EGamePieces.X);
            updateTileWithPiece(location, playerPiece);
            turns += 1;
        }


        /* Takes a board location and a piece and puts the piece on that location. 
        The boardchanged event is raised for the View to update accordingly. */
        private void updateTileWithPiece(Tuple<int, int> location, Piece newPiece)
        {
            int row = location.Item1;
            int col = location.Item2;
            Board[row, col].piece = newPiece;
            BoardChangedEventArgs e = new BoardChangedEventArgs();
            e.Add(location);
            OnBoardChanged(e);
        }


        /* Checks the board for the existence of a winning position for either player.
        If a winning position is found it returns true and puts the winner in the winner
        variable passed by reference. */
        public bool IsWinningPosition(ref string winner)
        {
            bool isWin = false;
            EGamePieces[] players = new EGamePieces[] { EGamePieces.X, EGamePieces.O };
            foreach (EGamePieces player in players)
            {
                // check verticals
                if ((Board[0, 0].piece.Val == player && Board[1, 0].piece.Val == player && Board[2, 0].piece.Val == player) ||
                    (Board[0, 1].piece.Val == player && Board[1, 1].piece.Val == player && Board[2, 1].piece.Val == player) ||
                    (Board[0, 2].piece.Val == player && Board[1, 2].piece.Val == player && Board[2, 2].piece.Val == player))
                {
                    winner = player.ToString();
                    isWin = true;
                    break;
                }
                // check horizontals
                if ((Board[0, 0].piece.Val == player && Board[0, 1].piece.Val == player && Board[0, 2].piece.Val == player) ||
                    (Board[1, 0].piece.Val == player && Board[1, 1].piece.Val == player && Board[1, 2].piece.Val == player) ||
                    (Board[2, 0].piece.Val == player && Board[2, 1].piece.Val == player && Board[2, 2].piece.Val == player))
                {
                    winner = player.ToString();
                    isWin = true;
                    break;
                }
                // check diagonals
                if ((Board[0, 0].piece.Val == player && Board[1, 1].piece.Val == player && Board[2, 2].piece.Val == player) ||
                    (Board[2, 0].piece.Val == player && Board[1, 1].piece.Val == player && Board[0, 2].piece.Val == player))
                {
                    winner = player.ToString();
                    isWin = true;
                    break;
                }
            }
            return isWin;
        }


        /* Returns true if the turn counter (number of pieces placed) is 
        equal to the number of possible board tiles. */
        public bool IsMaxTurns(ref bool isMaxTurns)
        {
            isMaxTurns = this.turns == (this.dim * this.dim);
            return isMaxTurns;
        }


        /* A wrapper for the change player which also fires the changeevent for the
        View to update its message accordingly. */
        public void ChangePlayer()
        {
            this.Player.change();
            OnPlayerChanged(EventArgs.Empty);
        }


        protected virtual void OnPlayerChanged(EventArgs e)
        {
            if (PlayerChanged != null)
            {
                PlayerChanged(this, e);
            }
        }
        

        protected virtual void OnBoardChanged(BoardChangedEventArgs e)
        {
            if (BoardChanged != null)
            {
                BoardChanged(this, e);
            }
        }
    }
}
