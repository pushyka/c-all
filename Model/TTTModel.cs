using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class TTTModel : IGameModel
    {
        private int dim;
        private TileStruct[,] board;
        private Player player;
        private int rowToWin;

        public TTTModel()
        {
            this.dim = 3;
            this.board = new TileStruct[dim, dim];
            this.player = new Player("white"); //fix player class
            this.rowToWin = dim;
        
        }


        /* Takes a move (a board coordinate) which has been checked for emptiness.
           Add a piece of the current player to the location in the move.*/
        public void applyMove(FormedMove move, string moveType)
        {
            var location = move.PosA;
            var pieceOfPlayer = new Piece(Pieces.empty); // fix
            updateTileWithPiece(location, pieceOfPlayer);
        }



        private void updateTileWithPiece(Tuple<int, int> location, Piece newPiece)
        {
            int row = location.Item1;
            int col = location.Item2;
            board[row, col].piece = newPiece;
            // add the event 
        }

        public Player Player
        {
            get
            {
                return this.player;
            }
            set
            {
                this.player = value;
                // add the event
            }
        }
    }
}
