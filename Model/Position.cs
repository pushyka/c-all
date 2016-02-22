using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{

    /* This class represents a board position. X and Y denote column and row. */
    class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Position(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
    }
}
