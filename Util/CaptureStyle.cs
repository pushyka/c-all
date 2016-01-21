using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess.Model;

namespace chess.Util
{
    class CaptureStyle
    {

        public List<Tuple<int, int>> dirs { get; }

        public int maxIterations { get; set; }


        /* Create a CaptureStyle object which takes as argument a piece and generates
        The valid directions that piece can capture in and the valid lengths of the 
        capture rays in each of those directions*/
        public CaptureStyle(Piece piece)
        {
            this.dirs = new List<Tuple<int, int>>();

            switch (piece.Val)
            {
                case EGamePieces.WhitePawn:
                    createWhitePawnCapture();
                    break;
                case EGamePieces.BlackPawn:
                    createBlackPawnCapture();
                    break;
                case EGamePieces.WhiteRook:
                case EGamePieces.BlackRook:
                    createRookCapture();
                    break;
                case EGamePieces.WhiteKnight:
                case EGamePieces.BlackKnight:
                    createKnightCapture();
                    break;
                case EGamePieces.WhiteBishop:
                case EGamePieces.BlackBishop:
                    createBishopCapture();
                    break;
                case EGamePieces.WhiteQueen:
                case EGamePieces.BlackQueen:
                    createQueenCapture();
                    break;
                case EGamePieces.WhiteKing:
                case EGamePieces.BlackKing:
                    createKingCapture();
                    break;
                default:
                    throw new ArgumentException($"Piece value provided to CaptureStyle invalid {piece.Val}");
            }
        }
        

        private void createKingCapture()
        {
            // king captures all directions
            this.dirs.Add(Tuple.Create(+1, 0));
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(0, +1));
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, 0));
            this.dirs.Add(Tuple.Create(-1, -1));
            this.dirs.Add(Tuple.Create(0, -1));
            this.dirs.Add(Tuple.Create(+1, -1));

            // only moves 1 tile
            this.maxIterations = 1;
        }


        private void createQueenCapture()
        {
            // queen captures all directions
            this.dirs.Add(Tuple.Create(+1, 0));
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(0, +1));
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, 0));
            this.dirs.Add(Tuple.Create(-1, -1));
            this.dirs.Add(Tuple.Create(0, -1));
            this.dirs.Add(Tuple.Create(+1, -1));

            // moves maximum number of tiles on the board (size-1)
            this.maxIterations = 7;
        }


        private void createBishopCapture()
        {
            // diagonals only
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, -1));
            this.dirs.Add(Tuple.Create(+1, -1));

            // moves maximum numer of tiles
            this.maxIterations = 7;
        }


        private void createKnightCapture()
        {
            // knights directions are only ones not adjacent to starting position (jump)
            this.dirs.Add(Tuple.Create(+2, +1));
            this.dirs.Add(Tuple.Create(+1, +2));
            this.dirs.Add(Tuple.Create(-1, +2));
            this.dirs.Add(Tuple.Create(-2, +1));
            this.dirs.Add(Tuple.Create(-2, -1));
            this.dirs.Add(Tuple.Create(-1, -2));
            this.dirs.Add(Tuple.Create(+1, -2));
            this.dirs.Add(Tuple.Create(+2, -1));

            // moves only once in its non adjacent direction (jump)
            this.maxIterations = 1;
        }


        private void createRookCapture()
        {
            // only moves horizontal / vertical
            this.dirs.Add(Tuple.Create(+1, 0));
            this.dirs.Add(Tuple.Create(0, +1));
            this.dirs.Add(Tuple.Create(-1, 0));
            this.dirs.Add(Tuple.Create(0, -1));

            // moves max number of tiles
            this.maxIterations = 7;
        }


        private void createBlackPawnCapture()
        {
            // pawn captures only on 'advancing' diagonal
            // this includes the en passant capture
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(+1, -1));

            this.maxIterations = 1;
        }

        private void createWhitePawnCapture()
        {
            // pawn captures only on 'advancing' diagonal
            // this includes the en passant capture
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, -1));

            this.maxIterations = 1;
        }

    }
}
