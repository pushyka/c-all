using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    /* This interface describes a displayable model. Any model implementing this interface
    can be displayed / updated by the View. Some of the functionality will not
    be used by all types of game models. E.g. tic tac toe does not raise the Captured event.
    These properties are required for display. */
    public interface IDisplayableModel
    {
        event EventHandler<BoardChangedEventArgs> BoardChanged;
        event EventHandler CapturedChanged;
        event EventHandler PlayerChanged;

        /* The board representation as array of Tiles */
        Tile[,] Board { get; }
        /* The pieces currently captured */
        List<EGamePieces> PiecesCapd { get; }
        /* The current player */
        Player Player { get; }
    }
}
