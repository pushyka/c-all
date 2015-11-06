using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess.Model;

namespace chess.Util
{
    class CaptureStyle : MovementStyle
    {
        // all non pawn pieces create an object with same properties as movement style
        public CaptureStyle(Tile piece) : base(piece) { }

        public CaptureStyle() : base() { }

        // override the pawn methods of movement style
        public override void createBlackPawnMovement(Tile piece)
        {
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(+1, -1));

            this.maxIterations = 1;
            this.type = "pawn";
        }

        public override void createWhitePawnMovement(Tile piece)
        {
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, -1));

            this.maxIterations = 1;
            this.type = "pawn";
        }

    }
}
