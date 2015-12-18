using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class Piece
    {
        private Pieces val;
        private bool movedOnce;

        public Piece(Pieces val)
        {
            this.val = val;
            this.movedOnce = false;
        }

        public Pieces Val
        {
            get
            {
                return this.val;
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
