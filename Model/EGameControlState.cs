using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
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
