using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess.Model
{
    // define a struct to be used for the positions in the chessboard, a VALUE TYPE, also containing all VALUE TYPES
    public class TileStruct // this struct also used publicly eg for evaluator
    {
        public Piece piece;

        public TileStruct(Piece piece)
        {
            this.piece = piece;
        }

        public TileStruct()
        {
            this.piece = null;
        }

        public bool IsEmpty()
        {
            return this.piece == null;
        }
    }
}
