using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class Piece
    {
        private EGamePieces val;
        private bool movedOnce;

        public Piece(EGamePieces val)
        {
            this.val = val;
            this.movedOnce = false;
        }

        public EGamePieces Val
        {
            get
            {
                return this.val;
            }
            set
            {
                this.val = value;
            }
        }

        public bool MovedOnce
        {
            get
            {
                return this.movedOnce;
            }
            set
            {
                this.movedOnce = value;
            }
        }

    }
}
