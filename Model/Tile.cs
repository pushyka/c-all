using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess.Model
{
    public class Tile
    {

        public Piece piece;


        /* This constructor usually used when a tile is a made with a piece ready to be placed on it.
        This is the constructor used during the ChessPosition.Setup method where the board is populated. */
        public Tile(Piece piece)
        {
            this.piece = piece;
        }


        /* This constructor is used when a tile is made without a piece ready to be placed on it.
        If the piece's value hasn't yet been determined or the tile is intended to be empty. */
        public Tile()
        {
            this.piece = null;
        }


        /* Returns true if the tile does not contain a piece. */
        public bool IsEmpty()
        {
            return this.piece == null;
        }
    }
}
