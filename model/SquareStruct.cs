using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess.Model
{
    // define a struct to be used for the positions in the chessboard, a VALUE TYPE, also containing all VALUE TYPES
    public struct Square // this struct also used publicly eg for evaluator
    {
        public char pID;
        public bool movedOnce; // relevant only for pawn, king, rook
        public bool canBeCapturedEnPassant; // relevant only for pawn

        public Square(char piece)
        {
            this.pID = piece;
            this.movedOnce = false;
            this.canBeCapturedEnPassant = false;
        }
    }
}
