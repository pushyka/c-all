using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    interface IGameModel
    {

        void applyMove(FormedMove move, string moveType);

        Player Player { get; set; }
    }
}
