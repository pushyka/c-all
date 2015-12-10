using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public enum Pieces
    {
        p, r, n, b, q, k, P, R, N, B, Q, K
    }


    // if a function is given a Pieces enum and a player argument, quickly determine if
    // the piece is of player ...

    // player = -1 (white), 1 (black)
    // if palyer < 0 and piece < 0 OR player > 0 and piece > 0 : is of cur player
}
