using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    interface IChessModel
    {
        event EventHandler<BoardChangedEventArgs> BoardChanged;
        event EventHandler CapturedChanged;

        /// <summary>
        /// Populates the model with the initial board state of
        /// pieces
        /// </summary>
        void populate();

        /// <summary>
        /// Takes a validified and checked move and 
        /// applies it to the model
        /// </summary>
        /// <param name="move"></param>
        /// <param name="moveType"></param>
        void applyMove(FormedMove move, string moveType);

        /// <summary>
        /// Returns a list containing the pieces captured so far.
        /// </summary>
        List<Pieces> PiecesCapd { get; }

    }
}
