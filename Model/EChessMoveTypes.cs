using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    /* Enum representing the various move types in the
    chess game. */
    public enum EChessMoveTypes
    {
        Movement,
        EpMovement,
        Capture,
        Castle,
        None
    }
}
