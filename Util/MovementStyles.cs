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
        public static MovementStyle getMovementStyle(Tile pieceOnPosA)
        {
           
            return new MovementStyle(pieceOnPosA);
        }

        public static MovementStyle getMovementStyle(Pieces piece)
        {
            return new MovementStyle(piece);
        }

        public static CaptureStyle getCaptureStyle(Tile pieceOnPosA)
        {
            // subclass of movementstyle, pawn definitions are overridden 
            return new CaptureStyle(pieceOnPosA);
        }
        
    }
}
