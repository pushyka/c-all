using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess.Model;

namespace chess.Util
{

    
    /// <summary>
    /// provides static methods which return MovementStyle objects for piece move generation
    /// </summary>
    class MovementStyles
    {

        public static MovementStyle getMovementStyle(Piece piece)
        {
            return new MovementStyle(piece);
        }


        public static CaptureStyle getCaptureStyle(Piece piece)
        {
            // subclass of movementstyle, pawn definitions are overridden 
            return new CaptureStyle(piece);
        }

        // the preloadray array doesnt have piece objects
        public static MovementStyle getMovementStyle(EGamePieces piece)
        {
            return new MovementStyle(new Piece(piece));
        }

        // the preloadray array doesnt have piece objects
        public static CaptureStyle getCaptureStyle(EGamePieces piece)
        {
            return new CaptureStyle(new Piece(piece));
        }
        
    }
}
