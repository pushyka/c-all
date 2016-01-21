using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public delegate void BoardChanged(object sender, BoardChangedEventArgs e);


    /* This is a specialised type of ChessPosition. This is the single chess position which is the 'good copy'
    which the view is 'bound' to. This represents the chess Model. When this object is changed, the view is notified by the
    Events and is updated to match. The other ChessPosition type does not have events bound to it and is used for testing of moves 
    and position generation. Multiple ChessPosition objects will be made. One ChessPositionModel object will be made. */
    public class ChessPositionModel : ChessPosition, IDisplayableModel
    {
        public event EventHandler<BoardChangedEventArgs> BoardChanged;
        public event EventHandler CapturedChanged;
        public event EventHandler PlayerChanged;

        public List<EGamePieces> PiecesCapd { get; }

        /* Create instance of the cpm.
        TODO: Add Singleton static check. */
        public ChessPositionModel() : base()
        {
            this.Size = 8;
            this.Board = new Tile[Size, Size];
            this.piecesCapd = new List<EGamePieces>();
            this.castle = new Dictionary<char, bool>();
            this.halfmoveClock = 0;
        }


        /* Perform the initial setup of the pieces on the board according to normal chess positioning.
        Then fire the boardchanged event with all of the positions. So after this model setup, the 
        View will do a big update to show the setup board in the display. */
        public void Setup()
        {
            // rank 0 (black)
            this.Board[0, 0] = new Tile(new Piece(EGamePieces.BlackRook));
            this.Board[0, 1] = new Tile(new Piece(EGamePieces.BlackKnight));
            this.Board[0, 2] = new Tile(new Piece(EGamePieces.BlackBishop));
            this.Board[0, 3] = new Tile(new Piece(EGamePieces.BlackQueen));
            this.Board[0, 4] = new Tile(new Piece(EGamePieces.BlackKing));
            this.Board[0, 5] = new Tile(new Piece(EGamePieces.BlackBishop));
            this.Board[0, 6] = new Tile(new Piece(EGamePieces.BlackKnight));
            this.Board[0, 7] = new Tile(new Piece(EGamePieces.BlackRook));


            // rank 1 (black)
            for (int col = 0; col < Size; col++)
                this.Board[1, col] = new Tile(new Piece(EGamePieces.BlackPawn));

            // empty ranks
            for (int row = 2; row < 6; row++)
                for (int col = 0; col < Size; col++)
                    this.Board[row, col] = new Tile();

            // rank 6 (white)
            for (int col = 0; col < Size; col++)
                this.Board[6, col] = new Tile(new Piece(EGamePieces.WhitePawn));

            // rank 7 (white)
            this.Board[7, 0] = new Tile(new Piece(EGamePieces.WhiteRook));
            this.Board[7, 1] = new Tile(new Piece(EGamePieces.WhiteKnight));
            this.Board[7, 2] = new Tile(new Piece(EGamePieces.WhiteBishop));
            this.Board[7, 3] = new Tile(new Piece(EGamePieces.WhiteQueen));
            this.Board[7, 4] = new Tile(new Piece(EGamePieces.WhiteKing));
            this.Board[7, 5] = new Tile(new Piece(EGamePieces.WhiteBishop));
            this.Board[7, 6] = new Tile(new Piece(EGamePieces.WhiteKnight));
            this.Board[7, 7] = new Tile(new Piece(EGamePieces.WhiteRook));

            // fire the board changed event with all locations as argument
            BoardChangedEventArgs e = new BoardChangedEventArgs();
            for (int row = 0; row < this.Size; row++)
                for (int col = 0; col < this.Size; col++)
                    e.Add(Tuple.Create(row, col));
            OnBoardChanged(e);
        }
        

       /* Same as the ChessPosition implementation but this also fires the 
        BoardChanged event since this is the singleton ChessPositionModel which is bound to the View. */
        protected override void updatePosWithPiece(Tuple<int,int> location, Piece newPiece)
        {
            int row = location.Item1;
            int col = location.Item2;
            this.Board[row, col].piece = newPiece;
            // finally fire the BoardChanged event!
            // EventArgs could be the tile coordinates which have changed
            BoardChangedEventArgs e = new BoardChangedEventArgs();
            e.Add(location);
            OnBoardChanged(e);
        }


        /* Same as the ChessPosition implementation but this also fires the 
        CapturedChanged event so the View can update accordingly */
        protected override void addToCaptured(EGamePieces piece)
        {
            this.piecesCapd.Add(piece);
            OnCapturedChanged(EventArgs.Empty);
        }


        /* ChessPosition represents a snapshot of a chess position so player will not need to change
        in each of those objects. In the CPM the player will change throughout the course of the 
        game and needs to have an event registered to let the View know. */
        public void ChangePlayer()
        {
            this.Player.change();
            OnPlayerChanged(EventArgs.Empty);
        }


        protected virtual void OnBoardChanged(BoardChangedEventArgs e)
        {
            if (BoardChanged != null)
            {
                BoardChanged(this, e);
            }
        }


        protected virtual void OnCapturedChanged(EventArgs e)
        {
            if (CapturedChanged != null)
            {
                CapturedChanged(this, e);
            }
        }


        protected virtual void OnPlayerChanged(EventArgs e)
        {
            if (PlayerChanged != null)
            {
                PlayerChanged(this, e);
            }
        }


    }
}
