using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{

    public class Piece
    {
        public EGamePieces Val;
        public bool MovedOnce;


        /* Represents a Game piece object. All that a piece consists of is
        its value and a MovedOnce property which is false until the piece
        is moved for the first time. This property is only relevant for
        the Pawn pieces and is only updated by the model when the piece moves. */
        public Piece(EGamePieces val)
        {
            this.Val = val;
            this.MovedOnce = false;
        }
    }
}
