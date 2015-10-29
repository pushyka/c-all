using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess.Model
{
    // define a struct to be used for the positions in the chessboard, a VALUE TYPE, also containing all VALUE TYPES
    public struct Tile // this struct also used publicly eg for evaluator
    {
        public char pID;
        public bool movedOnce; // relevant only for pawn, king, rook

        public Tile(char piece)
        {
            this.pID = piece;
            this.movedOnce = false;
        }
    }
}
