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

    public class ChessPositionModel : ChessPosition, IDisplayableModel
    {

        // using multi-dimensional array since rows of equal lengths
        // string or char
        // 
        //private Square[,] board; // the core element, this is the cloned piece
        
        //private int dim;
        public event EventHandler<BoardChangedEventArgs> BoardChanged;
        public event EventHandler CapturedChanged;
        public event EventHandler PlayerChanged;

        //private char player;
        public bool IsGame { get; set; }



        // model constructor
        public ChessPositionModel()
        {
            
            this.dim = 8;
            this.board = new TileStruct[dim, dim]; // 8x8 surrounded by 2-width of invalid assigned squares (prevents outofbounds issues)
                                             // hence 2,2 becomes the top left origin
                                             // initial=te the capture list
            this.piecesCapd = new List<GamePieces>();
            this.castle = new Dictionary<char, bool>();
            this.halfmoveClock = 0;
            this.player = new Player("white");

        }



        public void Setup()
        {





            // black row 2
            this.board[0, 0] = new TileStruct(new Piece(GamePieces.BlackRook)); // top left origin
            this.board[0, 1] = new TileStruct(new Piece(GamePieces.BlackKnight));
            this.board[0, 2] = new TileStruct(new Piece(GamePieces.BlackBishop));
            this.board[0, 3] = new TileStruct(new Piece(GamePieces.BlackQueen));
            this.board[0, 4] = new TileStruct(new Piece(GamePieces.BlackKing));
            this.board[0, 5] = new TileStruct(new Piece(GamePieces.BlackBishop));
            this.board[0, 6] = new TileStruct(new Piece(GamePieces.BlackKnight));
            this.board[0, 7] = new TileStruct(new Piece(GamePieces.BlackRook));


            // black row 3 (pawns)
            for (int col = 0; col < dim; col++)
            {
                this.board[1, col] = new TileStruct(new Piece(GamePieces.BlackPawn)); ; // pawns have 2 additional properties {movedOnce=false : used to allow +1/+2 advance for FIRST move}
                                                        //                                    {canBeCapEnPassant=false : set=true if pawn advances +2, function 
                                                        //to set=false for every pawn of oposite player at the end of current players turn.
                                                        //seems naive, implement last anyway
            }

            // empty rows 
            for (int row = 2; row < 6; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    this.board[row, col] = new TileStruct();
                }
            }

            // white row 8 (pawns)
            for (int col = 0; col < dim; col++)
            {
                this.board[6, col] = new TileStruct(new Piece(GamePieces.WhitePawn)); ; // as above
            }

            // white row 9
            this.board[7, 0] = new TileStruct(new Piece(GamePieces.WhiteRook));
            this.board[7, 1] = new TileStruct(new Piece(GamePieces.WhiteKnight));
            this.board[7, 2] = new TileStruct(new Piece(GamePieces.WhiteBishop));
            this.board[7, 3] = new TileStruct(new Piece(GamePieces.WhiteQueen));
            this.board[7, 4] = new TileStruct(new Piece(GamePieces.WhiteKing));
            this.board[7, 5] = new TileStruct(new Piece(GamePieces.WhiteBishop));
            this.board[7, 6] = new TileStruct(new Piece(GamePieces.WhiteKnight));
            this.board[7, 7] = new TileStruct(new Piece(GamePieces.WhiteRook)); // 9,9 the bottomright (rather than 7,7) due to the 2x off board buffer

            // fire the board changed event marked as ALL locations
            BoardChangedEventArgs e = new BoardChangedEventArgs();
            // add all of the positions of the board to the changedevent
            for (int row = 0; row < this.dim; row++)
            {
                for (int col = 0; col < this.dim; col++)
                {
                    e.Add(Tuple.Create(row, col));
                }
            }
            OnBoardChanged(e);

        }

        public void display()
        {
            for (int row = 0; row < dim; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    System.Console.Write("{0,-2} ", this.board[row, col].piece); // each entry allotted 2 chars, with 1 char space
                }
                System.Console.WriteLine(); // each row on a new line
            }
        }


        /// <summary>
        /// The base update operation on the model data. A given movetype will result in a call to this method for
        /// each tile which is being changed for that move. The boardchanged event can be fired here.
        /// this overrides the version which doesnt call update events
        /// cant be private since overriden and dont wish to be public so virtual
        /// </summary>
        /// <param name="toTile"></param>
        /// <param name="fromTile"></param>
        protected override void updateTileWithPiece(Tuple<int,int> location, Piece newPiece)
        {
            int row = location.Item1;
            int col = location.Item2;

            this.board[row, col].piece = newPiece;

            // finally fire the BoardChanged event!
            // EventArgs could be the tile coordinates which have changed
            BoardChangedEventArgs e = new BoardChangedEventArgs();
            e.Add(location);
            OnBoardChanged(e);
        }





        


        protected override void addToCaptured(GamePieces piece)
        {
            this.piecesCapd.Add(piece);
            OnCapturedChanged(EventArgs.Empty);
        }

        public List<GamePieces> PiecesCapd
        {
            get
            {
                return this.piecesCapd;
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

        protected virtual void OnCapturedChanged(EventArgs e)
        {
            if (CapturedChanged != null)
            {
                CapturedChanged(this, e);
            }
        }


        public override Player Player
        {
            get
            {
                return this.player;
            }
            set
            {
                // this is the override
                this.player.change();
                OnPlayerChanged(EventArgs.Empty);
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
