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

        public string type { get; set; }


        public MovementStyle() { }
        /// <summary>
        /// Create movement style object which contains two public fields.
        /// dirs : the directions of propagation on the board, eg 1,-1 (knight eg 1,-2
        /// maxIterations : the number of tiles to be added in that direction
        ///     (normally 7 maximum on the board, 1 for knight and pawn and king etc (CHANGE TO SIZE - 1)
        /// On construction the fields are filled according to the piece type passed to the constructor.
        /// </summary>
        /// <param name="piece"></param>


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
                    Console.WriteLine("MovementStyle switch error");
                    break;
            }
        }

        // TUPLES SHOULD BE Y,X format since ROW,COL
        private void createKingMovement()
        {
            // king moves all y, x directions
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

            this.type = "king";

        }

        // TUPLES SHOULD BE Y,X format since ROW,COL
        private void createQueenMovement()
        {
            this.dirs.Add(Tuple.Create(+1, 0));
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(0, +1));
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, 0));
            this.dirs.Add(Tuple.Create(-1, -1));
            this.dirs.Add(Tuple.Create(0, -1));
            this.dirs.Add(Tuple.Create(+1, -1));

            this.maxIterations = 7;

            this.type = "queen";
        }

        // TUPLES SHOULD BE Y,X format since ROW,COL
        private void createBishopMovement()
        {
            // diagonals only
            this.dirs.Add(Tuple.Create(+1, +1));
            this.dirs.Add(Tuple.Create(-1, +1));
            this.dirs.Add(Tuple.Create(-1, -1));
            this.dirs.Add(Tuple.Create(+1, -1));

            this.maxIterations = 7;

            this.type = "bishop";
        }

        // TUPLES SHOULD BE Y,X format since ROW,COL
        private void createKnightMovement()
        {
            this.dirs.Add(Tuple.Create(+2, +1));
            this.dirs.Add(Tuple.Create(+1, +2));
            this.dirs.Add(Tuple.Create(-1, +2));
            this.dirs.Add(Tuple.Create(-2, +1));
            this.dirs.Add(Tuple.Create(-2, -1));
            this.dirs.Add(Tuple.Create(-1, -2));
            this.dirs.Add(Tuple.Create(+1, -2));
            this.dirs.Add(Tuple.Create(+2, -1));

            this.maxIterations = 1;

            this.type = "knight";
        }

        // TUPLES SHOULD BE Y,X format since ROW,COL
        private void createRookMovement()
        {
            this.dirs.Add(Tuple.Create(+1,  0));
            this.dirs.Add(Tuple.Create( 0, +1));
            this.dirs.Add(Tuple.Create(-1,  0));
            this.dirs.Add(Tuple.Create( 0, -1));

            this.maxIterations = 7;

            this.type = "rook";
        }

        // TUPLES SHOULD BE Y,X format since ROW,COL
        public virtual void createBlackPawnMovement(Piece piece)
        {
            this.dirs.Add(Tuple.Create(+1, 0));
            this.maxIterations = 2;

            this.type = "pawn";
        }

        // TUPLES SHOULD BE Y,X format since ROW,COL
        public virtual void createWhitePawnMovement(Piece piece)
        {
            // y / row direction for advancement is negative
            this.dirs.Add(Tuple.Create(-1, 0));
            this.maxIterations = 2;

            this.type = "pawn";
        }





    }
}
