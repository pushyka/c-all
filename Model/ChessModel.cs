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

    public class ChessModel : IChessModel
    {

        // using multi-dimensional array since rows of equal lengths
        // string or char
        // 
        private Square[,] board; // the core element, this is the cloned piece
        private List<char> piecesCapd;//x2 
        private int dim;
        public event EventHandler<BoardChangedEventArgs> BoardChanged;
        public event EventHandler CapturedChanged;
        public event EventHandler PlayerChanged;

        private char player;
        public bool IsGame { get; set; }




        public ChessModel()
        {
            this.dim = 8;
            this.board = new Square[dim, dim]; // 8x8 surrounded by 2-width of invalid assigned squares (prevents outofbounds issues)
             // hence 2,2 becomes the top left origin

        }

        public void populate()
        {


            // initial=te the capture list
            this.piecesCapd = new List<char>();


            // black row 2
            this.board[0, 0] = new Square('R'); // top left origin
            this.board[0, 1] = new Square('N');
            this.board[0, 2] = new Square('B');
            this.board[0, 3] = new Square('Q');
            this.board[0, 4] = new Square('K');
            this.board[0, 5] = new Square('B');
            this.board[0, 6] = new Square('N');
            this.board[0, 7] = new Square('R');


            // black row 3 (pawns)
            for (int col = 0; col < dim; col ++)
            {
                this.board[1, col] = new Square('P'); ; // pawns have 2 additional properties {movedOnce=false : used to allow +1/+2 advance for FIRST move}
                                            //                                    {canBeCapEnPassant=false : set=true if pawn advances +2, function 
                                            //to set=false for every pawn of oposite player at the end of current players turn.
                                            //seems naive, implement last anyway
            }

            // empty rows 
            for (int row = 2; row < 6; row ++)
            {
                for (int col = 0; col < dim; col ++)
                {
                    this.board[row, col] = new Square('e'); // empty 
                }
            }

            // white row 8 (pawns)
            for (int col = 0; col < dim; col ++)
            {
                this.board[6, col] = new Square('p'); ; // as above
            }

            // white row 9
            this.board[7, 0] = new Square('r');
            this.board[7, 1] = new Square('n');
            this.board[7, 2] = new Square('b');
            this.board[7, 3] = new Square('q');
            this.board[7, 4] = new Square('k');
            this.board[7, 5] = new Square('b');
            this.board[7, 6] = new Square('n');
            this.board[7, 7] = new Square('r'); // 9,9 the bottomright (rather than 7,7) due to the 2x off board buffer




        }

        public void display()
        {
            for (int row = 0; row < dim; row ++)
            {
                for (int col = 0; col < dim; col ++)
                {
                    System.Console.Write("{0,-2} ", this.board[row, col].pID); // each entry allotted 2 chars, with 1 char space
                }
                System.Console.WriteLine(); // each row on a new line
            }
        }

        /// <summary>
        /// The Entry point for move application, this method
        /// selects the appropriate type of move to make based on moveType
        /// </summary>
        /// <param name="move"></param>
        /// <param name="moveType"></param>
        public void applyMove(FormedMove move, string moveType)
        {
            switch (moveType)
            {
                case "castle":
                    applyCastling(move);
                    break;
                case "capture":
                    applyCapture(move);
                    break;
                case "movement":
                    applyMovement(move);
                    break;
                default:
                    System.Console.WriteLine("ERROR: No moveType set"); // should never reach 
                    break;
            }
            // fire the event (board changed)
        }

        /// <summary>
        /// Applies a formed move to the current state of the board.
        /// The formed move has already been validated so this function 
        /// simply performs the MOVE operation on the game board. 
        /// </summary>
        /// <param name="input"></param>
        private void applyMovement(FormedMove move)
        {
            Tuple<int, int> fromSquareLoc = move.PosA;
            Tuple<int, int> toSquareLoc = move.PosB;
            
            Square fromSquareVal = getSquare(fromSquareLoc);
            Square toSquareVal = getSquare(toSquareLoc);
            

         


            //  if moving piece is a pawn and its its first move
            if ((fromSquareVal.pID == 'p' || fromSquareVal.pID == 'P') &&
                (!fromSquareVal.movedOnce))
            {
                //if it moved 2 tiles (abs difference between fromsqloc.y and tsqloc.y)
                if (Math.Abs((fromSquareLoc.Item1 - toSquareLoc.Item1)) == 2)
                {
                    fromSquareVal.canBeCapturedEnPassant = true;
                }
                // the pawn has been moved atleast once so
                fromSquareVal.movedOnce = true;
            }

            
            // if moving piece is a pawn and the move is diagonal (non capture)
            if ((fromSquareVal.pID == 'p' || fromSquareVal.pID == 'P') &&
                (toSquareLoc.Item2 != fromSquareLoc.Item2) &&
                (toSquareVal.pID == 'e'))
            {
                // then this is an en passant capture
                // add the passant square to the captured list

                // move the pawn to the to square (copy)
                changeSquare(toSquareLoc, fromSquareVal);
                // empty the from square
                Square emptySquareVal2 = new Square('e');
                changeSquare(fromSquareLoc, emptySquareVal2);
                // clear the passant square -> empty
                Tuple<int, int> passantSquareLoc;
                int passantSquareLocRank = fromSquareLoc.Item1;
                int passantSquareLocFile = toSquareLoc.Item2;
                passantSquareLoc = Tuple.Create(passantSquareLocRank, passantSquareLocFile);
                Square passantSquareVal = getSquare(passantSquareLoc);
                Square emptySquareVal3 = new Square('e');
                changeSquare(passantSquareLoc, emptySquareVal2);
                // add the passant square to the captured list
                addToCaptured(passantSquareVal.pID);
                System.Console.WriteLine("en passant move registered");
            }
            // otherwise its not an en passant type capture
            else
            {
  
                // move the piece to the to square (copy)
                changeSquare(toSquareLoc, fromSquareVal);
                // empty the from square
                Square emptySquareVal = new Square('e');
                changeSquare(fromSquareLoc, emptySquareVal);

            }


            


        }

        /// <summary>
        /// Performs the CAPTURE operation, (a move and
        /// replacement of a piece) also adds any captured 
        /// pieces to the capture list. Essentially same as movement.
        /// </summary>
        /// <param name="move"></param>
        private void applyCapture(FormedMove move)
        {
            Tuple<int, int> fromSquareLoc = move.PosA;
            Tuple<int, int> toSquareLoc = move.PosB;
            // get the piece from the toSquare (which is being captured)
            Square toSquareVal = getSquare(toSquareLoc);
            Square fromSquareVal = getSquare(fromSquareLoc);

     
            // move the piece to the to square (copy)
            changeSquare(toSquareLoc, fromSquareVal);
            // empty the from square
            Square emptySquareVal = new Square('e');
            changeSquare(fromSquareLoc, emptySquareVal);

            // add the captured to the list
            addToCaptured(toSquareVal.pID);
        }




        /// <summary>
        /// Performs the CASTLE operation (a specific
        /// move type operation)
        /// As with the other applyMove functions, the move has
        /// already been validated as legal. For castling this
        /// includes check for check operation.
        /// </summary>
        /// <param name="move"></param>
        private void applyCastling(FormedMove move)
        {
            // todo
        }

        /// <summary>
        /// Performs the pawn PROMOTION operation. 
        /// Since this an optional side effect of either the MOVE or 
        /// CAPTURE operations, this is left for them to check.
        /// Rather than the evaluator.
        /// 
        /// TODO
        /// 
        /// ATM default to QUEEN
        /// </summary>
        /// <param name="move"></param>
        private void promotePawn(Square tile)
        {
            // todo - change piece to q of same allegiance (case)
            // isUpper = black
            char queen = (Char.IsUpper(tile.pID)) ? 'Q' : 'q';
            tile.pID = queen;
            System.Console.WriteLine("pawn promoted to queen");
        }

        public void clearEnPassantPawns(char player)
        {
            for (int row = 0; row < dim; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    if (board[row, col].pID == 'p' && player == 'w')
                        board[row, col].canBeCapturedEnPassant = false;
                    if (board[row, col].pID == 'P' && player == 'b')
                        board[row, col].canBeCapturedEnPassant = false;
                }
            }
        }




        /// <summary>
        /// Provides a deep copy of the board array to be used in the Evaluator functions.
        /// This means the Evaluator functions can manipulate the board freely without 
        /// the changes being saved. eg Evaluator might need to place a piece as a result of 
        /// a move first before then checking if it opens up an attack on a king.
        /// 
        /// Possibly don't need anymore since array of strings (base type) copied by value
        /// </summary>
        public Square[ , ] Board
        {
            get
            {
                return this.board;
            }
        }

        /// <summary>
        /// Given a Tuple(int,int) position, return a reference
       /// to the Square object located at that at that position on the board.
        /// </summary>
        private Square getSquare(Tuple<int, int> position)
        {
            int row = position.Item1;
            int col = position.Item2;

            return this.board[row, col];
        }

        /// <summary>
        /// The base update operation on the model data. A given movetype will result in a call to this method for
        /// each tile which is being changed for that move. The boardchanged event can be fired here.
        /// 
        /// </summary>
        /// <param name="toTile"></param>
        /// <param name="fromTile"></param>
        private void changeSquare(Tuple<int,int> location, Square newValues)
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





        


        private void addToCaptured(char piece)
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

    }

}
