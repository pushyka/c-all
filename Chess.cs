using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    class Chess // interface
    {

        // using multi-dimensional array since rows of equal lengths
        // string or char
        // 
        private Square[,] board; // the core element, this is the cloned piece
        private List<string> piecesCapd;//x2 

        public string Player { get; set; }
        public bool IsGame { get; set; }

        // define a struct to be used for the positions in the chessboard, a VALUE TYPE, also containing all VALUE TYPES
        public struct Square // this struct also used publicly eg for evaluator
        {
            public char piece;
            public bool movedOnce; // relevant only for pawn, king, rook
            public bool canBeCapturedEnPassant; // relevant only for pawn

            public Square(char piece)
            {
                this.piece = piece;
                this.movedOnce = false;
                this.canBeCapturedEnPassant = false;
            }
        }


        public Chess()
        {
            this.board = new Square[12, 12]; // 8x8 surounded by 2-width of invalid assigned squares (prevents outofbounds issues)
             // hence 2,2 becomes the topleft origin

        }

        public void populate()
        {

            // out of bounds squares (2x top and bottom rows)
            for (int row = 0; row < 2; row ++)
            {
                for (int col = 0; col < 12; col ++)
                {
                    this.board[row, col] = new Square('X');
                }
            }
            for (int row = 10; row < 12; row++)
            {
                for (int col = 0; col < 12; col++)
                {
                    this.board[row, col] = new Square('X');
                }
            }
            // and 2-col width either side
            for (int row = 2; row < 10; row++)
            {
                this.board[row, 0] = new Square('X');
                this.board[row, 1] = new Square('X');
                this.board[row, 10] = new Square('X');
                this.board[row, 11] = new Square('X');
            }



            // black row 2
            this.board[2, 2] = new Square('R'); // topleft origin
            this.board[2, 3] = new Square('N');
            this.board[2, 4] = new Square('B');
            this.board[2, 5] = new Square('Q');
            this.board[2, 6] = new Square('K');
            this.board[2, 7] = new Square('B');
            this.board[2, 8] = new Square('N');
            this.board[2, 9] = new Square('R');


            // black row 3 (pawns)
            for (int col = 2; col < 10; col ++)
            {
                this.board[3, col] = new Square('P'); ; // pawns have 2 additional properties {movedOnce=false : used to allow +1/+2 advance for FIRST move}
                                            //                                    {canBeCapEnPassant=false : set=true if pawn advances +2, function 
                                            //to set=false for every pawn of oposite player at the end of current players turn.
                                            //seems naive, implement last anyway
            }

            // empty rows 
            for (int row = 4; row < 8; row ++)
            {
                for (int col = 2; col < 10; col ++)
                {
                    this.board[row, col] = new Square('e'); // empty 
                }
            }

            // white row 8 (pawns)
            for (int col = 2; col < 10; col ++)
            {
                this.board[8, col] = new Square('p'); ; // as above
            }

            // white row 9
            this.board[9, 2] = new Square('r');
            this.board[9, 3] = new Square('n');
            this.board[9, 4] = new Square('b');
            this.board[9, 5] = new Square('q');
            this.board[9, 6] = new Square('k');
            this.board[9, 7] = new Square('b');
            this.board[9, 8] = new Square('n');
            this.board[9, 9] = new Square('r'); // 9,9 the bottomright (rather than 7,7) due to the 2x offboard buffer




        }

        public void display()
        {
            for (int row = 0; row < 12; row ++)
            {
                for (int col = 0; col < 12; col ++)
                {
                    System.Console.Write("{0,-2} ", (this.board[row, col].piece != 'X') ? this.board[row, col].piece : ' '); // each entry alotted 2 chars, with 1 char space
                }
                System.Console.WriteLine(); // each row on a new line
            }
        }

        public void doMove(string input)
        {
            // TODO
            // any captures are added to capture array
        }

        public void togglePlayer()
        {
            // TODO
        }

        /// <summary>
        /// Provides a deep copy of the board array to be used in the Evaluator functions.
        /// This means the Evaluator functions can manipulate the board freely without 
        /// the changes being saved. eg Evaluator might need to place a piece as a result of 
        /// a move first before then checking if it opens up an attack on a king.
        /// 
        /// Possibly don't need anymore since array of strings (base type) copied by value
        /// </summary>
        public Square[ , ] deepCopyBoard()
        {
            // create a deep copy and return it
            return null;
        }
    }

}
