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
        public static MovementStyle getMovementStyle(Square pieceOnPosA)
        {
            return new MovementStyle(pieceOnPosA);
        }

        public static CaptureStyle getCaptureStyle(Square pieceOnPosA)
        {
            // subclass of movementstyle, pawn definitions are overridden 
            return new CaptureStyle(pieceOnPosA);
        }
        
    }
}
