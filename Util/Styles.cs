using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess.Model;

namespace chess.Util
{

    /* Utility providing static methods for obtaining a movement or capture style. 
    The code calling these methods is responsible for handling the results correctly.
    In most cases a MovementStyle and a CaptureStyle for a piece are the same (pawns..),
    but should be applied differently depending on the Type (capture / movement) since
    subtle difference between where to stop on a ray on a capture / movement. */
    class Styles
    {
        /* Takes a piece value, generates a pseudo piece and uses it
        to get the corresponding style of movement. The style contains 
        the directions the piece can go (dirs) and how many times (maxIters). */
        public static MovementStyle getMovementStyle(EGamePieces piece)
        {
            return new MovementStyle(new Piece(piece));
        }

        /* Takes a piece value, generates a pseudo piece and uses it
        to get the corresponding style of capture movement. The style
        contains the directions the piece can go (dirs) and how many 
        times (maxIters). */
        public static CaptureStyle getCaptureStyle(EGamePieces piece)
        {
            return new CaptureStyle(new Piece(piece));
        }
    }
}
