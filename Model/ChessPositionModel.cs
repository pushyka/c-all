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

    public class ChessPositionModel : ChessPosition, IChessModel
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
            this.board = new Tile[dim, dim]; // 8x8 surrounded by 2-width of invalid assigned squares (prevents outofbounds issues)
             // hence 2,2 becomes the top left origin

        }



        public void populate()
        {


            // initial=te the capture list
            this.piecesCapd = new List<char>();


            // black row 2
            this.board[0, 0] = new Tile('R'); // top left origin
            this.board[0, 1] = new Tile('N');
            this.board[0, 2] = new Tile('B');
            this.board[0, 3] = new Tile('Q');
            this.board[0, 4] = new Tile('K');
            this.board[0, 5] = new Tile('B');
            this.board[0, 6] = new Tile('N');
            this.board[0, 7] = new Tile('R');


            // black row 3 (pawns)
            for (int col = 0; col < dim; col++)
            {
                this.board[1, col] = new Tile('P'); ; // pawns have 2 additional properties {movedOnce=false : used to allow +1/+2 advance for FIRST move}
                                                        //                                    {canBeCapEnPassant=false : set=true if pawn advances +2, function 
                                                        //to set=false for every pawn of oposite player at the end of current players turn.
                                                        //seems naive, implement last anyway
            }

            // empty rows 
            for (int row = 2; row < 6; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    this.board[row, col] = new Tile('e'); // empty 
                }
            }

            // white row 8 (pawns)
            for (int col = 0; col < dim; col++)
            {
                this.board[6, col] = new Tile('p'); ; // as above
            }

            // white row 9
            this.board[7, 0] = new Tile('r');
            this.board[7, 1] = new Tile('n');
            this.board[7, 2] = new Tile('b');
            this.board[7, 3] = new Tile('q');
            this.board[7, 4] = new Tile('k');
            this.board[7, 5] = new Tile('b');
            this.board[7, 6] = new Tile('n');
            this.board[7, 7] = new Tile('r'); // 9,9 the bottomright (rather than 7,7) due to the 2x off board buffer




        }

        public void display()
        {
            for (int row = 0; row < dim; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    System.Console.Write("{0,-2} ", this.board[row, col].pID); // each entry allotted 2 chars, with 1 char space
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
        protected override void changeTile(Tuple<int,int> location, Tile newValues)
        {
            int row = location.Item1;
            int col = location.Item2;

            this.board[row, col] = newValues;

            // finally fire the BoardChanged event!
            // EventArgs could be the tile coordinates which have changed
            BoardChangedEventArgs e = new BoardChangedEventArgs();
            e.Add(location);
            OnBoardChanged(e);
        }





        


        protected override void addToCaptured(char piece)
        {
            this.piecesCapd.Add(piece);
            OnCapturedChanged(EventArgs.Empty);
        }

        public List<char> PiecesCapd
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


        public char Player
        {
            get
            {
                return this.player;
            }
            set
            {
                this.player = value;
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


        /// <summary>
        /// This method of the chess position object returns a clone of the chess position without the listeners and
        /// captured list etc, a version which can be modified during evaluation of positions (used in search & checkcheck after move) 
        /// independantly of the chess position model object used for final moves and updating display
        /// </summary>
        /// <returns></returns>
        public ChessPositionModel getEvaluateableChessPosition()
        {

            // copy piece placement
            // side to move from player
            // castling status
            // cur en passant sq
            // halfmove clock

            //ChessPosition cpos = new ChessPosition();
            


            return null;
        }

    }

}
