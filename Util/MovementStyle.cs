using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess.Model;

namespace chess.Util
{
    class MovementStyle
    {

        public List<Tuple<int, int>> dirs { get; }

        public int maxIterations { get; set; }


        /* Create a MovementStyle object which takes as argument a piece and generates
        The valid directions that piece can move in and the valid lengths of the 
        movement rays in each of those directions*/
        public MovementStyle(Piece piece)
        {
            this.dirs = new List<Tuple<int, int>>();

            switch (piece.Val)
            {
                case EGamePieces.WhitePawn:
                    createWhitePawnMovement(piece);
                    break;
                case EGamePieces.BlackPawn:
                    createBlackPawnMovement(piece);
                    break;
                case EGamePieces.WhiteRook:
                case EGamePieces.BlackRook:
                    createRookMovement();
                    break;
                case EGamePieces.WhiteKnight:
                case EGamePieces.BlackKnight:
                    createKnightMovement();
                    break;
                case EGamePieces.WhiteBishop:
                case EGamePieces.BlackBishop:
                    createBishopMovement();
                    break;
                case EGamePieces.WhiteQueen:
                case EGamePieces.BlackQueen:
                    createQueenMovement();
                    break;
                case EGamePieces.WhiteKing:
                case EGamePieces.BlackKing:
                    createKingMovement();
                    break;
                default:
                    throw new ArgumentException($"Piece value provided to CaptureStyle invalid {piece.Val}");
            }
        }

        
        private void createKingMovement()
        {
            // king moves all directions
            this.dirs.Add(Tuple.Create(+1,  0));
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create( 0, +1));
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1,  0));
            this.dirs.Add(Tuple.Create(-1, -1));
            this.dirs.Add(Tuple.Create( 0, -1));
            this.dirs.Add(Tuple.Create(+1, -1));

            // only moves one tile
            this.maxIterations = 1;
        }

        
        private void createQueenMovement()
        {
            // queen moves all directions
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


        private void createBishopMovement()
        {
            // moves diagonals only
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, -1));
            this.dirs.Add(Tuple.Create(+1, -1));

            // moves maximum number of tiles on the board
            this.maxIterations = 7;
        }


        private void createKnightMovement()
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


        private void createRookMovement()
        {
            // only moves horizontal / vertical
            this.dirs.Add(Tuple.Create(+1,  0));
            this.dirs.Add(Tuple.Create( 0, +1));
            this.dirs.Add(Tuple.Create(-1,  0));
            this.dirs.Add(Tuple.Create( 0, -1));

            // moves max number of tiles
            this.maxIterations = 7;
        }


        /* pawn moves 'advancing' forward only
        in all cases of pawn movement(when piece.MovedOnce = true), except for the first
        the calling code will only consider the first position in the
        movement ray generated from this style.
        So this movement style creates the Set of pawn moves, but pawn moves after its
        first only consider a subset of this style. */
        private void createBlackPawnMovement(Piece piece)
        {
             this.dirs.Add(Tuple.Create(+1, 0));
            
            this.maxIterations = 2;
        }


        /* pawn moves 'advancing' forward only
        in all cases of pawn movement(when piece.MovedOnce = true), except for the first
        the calling code will only consider the first position in the
        movement ray generated from this style.
        So this movement style creates the Set of pawn moves, but pawn moves after its
        first only consider a subset of this style. */
        private void createWhitePawnMovement(Piece piece)
        {
            this.dirs.Add(Tuple.Create(-1, 0));

            this.maxIterations = 2;
        }
    }
}
