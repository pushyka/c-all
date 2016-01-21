using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{

    /* Enum representing the various states the game controller can be in. */
    public enum EGameControlState
    {
        PreInitial = 1,
        Initial,
        Ready,
        GameInProgress,
        GameEnded,
        Load,
        Settings
    }
}
