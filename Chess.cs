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

        public char Player { get; set; }
        public bool IsGame { get; set; }




        public Chess()
        {
            this.board = new Square[8, 8]; // 8x8 surrounded by 2-width of invalid assigned squares (prevents outofbounds issues)
             // hence 2,2 becomes the top left origin

        }

        public void populate()
        {





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
            for (int col = 0; col < 8; col ++)
            {
                this.board[1, col] = new Square('P'); ; // pawns have 2 additional properties {movedOnce=false : used to allow +1/+2 advance for FIRST move}
                                            //                                    {canBeCapEnPassant=false : set=true if pawn advances +2, function 
                                            //to set=false for every pawn of oposite player at the end of current players turn.
                                            //seems naive, implement last anyway
            }

            // empty rows 
            for (int row = 2; row < 6; row ++)
            {
                for (int col = 0; col < 8; col ++)
                {
                    this.board[row, col] = new Square('e'); // empty 
                }
            }

            // white row 8 (pawns)
            for (int col = 0; col < 8; col ++)
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
            for (int row = 0; row < 8; row ++)
            {
                for (int col = 0; col < 8; col ++)
                {
                    System.Console.Write("{0,-2} ", this.board[row, col].piece); // each entry allotted 2 chars, with 1 char space
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
        public Square[ , ] Board
        {
            get
            {
                return this.board;
            }
        }
    }

}
