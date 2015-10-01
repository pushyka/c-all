using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace chess
{
    class Evaluator
    {

        /// <summary>
        /// Ensures the user input matches the expected format (B2 etc), then convert the input to the 
        /// primitive move Tuple structure. (could just as easily use a move class but I like these types)
        /// Function returns the formatted move ready to be evaluated and used, or a null-filled 
        /// formattedMove object if runs in to errors
        /// </summary>
        /// <param name="input"> "B2 B4" </param>
        /// <returns></returns>
        public FormedMove formatMove(string input)
        {

            FormedMove formedMove = new FormedMove();
            
            #region try to make a move
            try
            {
                // try to break the string into two components (there is a space)
                string[] locs = input.Split(); // no arg defaults to whitespace seperators

                List<char> validFiles = "ABCDEFGH".ToList();
                List<int> validRanks = new List<int>(new int[] { 8, 7, 6, 5, 4, 3, 2, 1});

                if (locs.Length == 2) // ensure two locations are given (a -> b)
                {
                    foreach (string loc in locs)
                    {
                        if (loc.Length == 2) // ensure the location specifier has two values
                        {
                            char file = Char.ToUpper(loc[0]);
                            int rank = (int)Char.GetNumericValue(loc[1]);
                            ;

                            // check if the given file and rank are within valid ranges
                            if (validFiles.Contains(file) && validRanks.Contains(rank))
                            {
                                ;
                                // convert alpha to numeric (a,b -> 0,1) use the validterm0 list again
                                int fileToCol = validFiles.IndexOf(file);
                                ;
                                // convert (1,2 .. 8 -> 7,6 .. 0) use validterm1 (this is why its reversed)
                                // validterm1 = [8,7,6,5,4,3,2,1]
                                // for term if user put in eg B3, (3rd rank on the board) the (3-1)th element of validterm1 
                                // would be obtained (which is 5) (the 5th row of the internal board)
                                int rankToRow = validRanks.IndexOf(rank);
                                ;
                                Tuple<int, int> formedPosition = Tuple.Create<int, int>(rankToRow, fileToCol);
                                formedMove.Add(formedPosition);
                            }
                        }


                    }
                }
                
            }
            #endregion

            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }

            return formedMove;

        }

        /// <summary>
        /// In-depth move checker function determines whether a FormedMove move
        /// is valid in the context of the rules/requirements. board is a cloned copy so that additions may be 
        /// made to it during the process of checking if the move is valid (after move is made does it result in check etc)
        /// At the end of the function, this board is discarded and a boolean result is returned 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool evaluateMove(FormedMove move, Square[,] board, char cur_turn)
        {
            // 1 a formed move is already guaranteed to be within the board bounds (from formatMove)
            // 2 check to and from pos are not the same
            // 3 check from pos contains piece of current player

            bool moveIsValidOnBoard, isDistinct, isPlayerPosA, isPlayerPosB, isOponentPosB, isEmptyPosB;
            Square piecePosA, piecePosB;

            isDistinct = toAndFromPositionsDistinct(move);

            isPlayerPosA = isPlayerPieceOnPosA(move, board, cur_turn);
            if (isPlayerPosA)
            {
                piecePosA = getPieceOnPosA(move, board);

                // can assume only one of these will be true
                isPlayerPosB = isPlayerPieceOnPosB(move, board, cur_turn);
                isOponentPosB = isOponentPieceOnPosB(move, board, cur_turn);
                isEmptyPosB = isEmptyPieceOnPosB(move, board);


                // each of these branches then check the typeof(the piece of posA) can infact reach posB
                // based on its move definitions, and there is a clear path to it and the move doesnt result in a check (maybe use of copy board)
                // isPlayerPosB (castling) slightly more indepth check checking
                if (isPlayerPosB)
                {
                    //piecePosB = getPieceOnPosB(move, board);
                    // possibly a castling .. to do later, success if piecePosA is king, piecePosB is castle etc
                    // and the checking
                    //outcome = bool;
                }
                else if (isOponentPosB)
                {
                    //piecePosB = getPieceOnPosB(move, board);
                    // possibly a capture
                    //outcome = bool;
                }
                else if (isEmptyPosB)
                {
                    //dont require getting the piece
                    // possibly a normal move
                    //outcome = bool;
                }
            }
            else
            {
                // invalid
            }

            


            
            //combine all the bools for answer
            moveIsValidOnBoard = true;
            return moveIsValidOnBoard;
        }

        private bool toAndFromPositionsDistinct(FormedMove move)
        {
            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;
            return (posA.Item1 == posB.Item1 && posA.Item2 == posB.Item2) ? false : true;
        }

        private bool isPlayerPieceOnPosA(FormedMove move, Square[,] board, char cur_turn)
        {
            bool result = false;
            char piece = board[move.PosA.Item1, move.PosA.Item2].piece;
            ;
            if (piece != 'e')
            {
                if ((cur_turn == 'b' && Char.IsUpper(piece)) || (cur_turn == 'w' && Char.IsLower(piece)))
                {
                    result = true;
                }
            }

            return result;
        }

        private Square getPieceOnPosA(FormedMove move, Square[,] board)
        {
            Square piece;
            piece = board[move.PosA.Item1, move.PosA.Item2];
            return piece;
        }

        private bool isPlayerPieceOnPosB(FormedMove move, Square[,] board, char cur_turn)
        {
            bool result = false;
            char piece = board[move.PosB.Item1, move.PosB.Item2].piece;
            ;
            if (piece != 'e')
            {
                if ((cur_turn == 'b' && Char.IsUpper(piece)) || (cur_turn == 'w' && Char.IsLower(piece)))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool isOponentPieceOnPosB(FormedMove move, Square[,] board, char cur_turn)
        {
            bool result = false;
            char piece = board[move.PosB.Item1, move.PosB.Item2].piece;
            ;
            if (piece != 'e')
            {
                // player b uses the upper case chars, so this means piece is owned by the opposite to player b and vice versa
                if ((cur_turn == 'b' && Char.IsLower(piece)) || (cur_turn == 'w' && Char.IsUpper(piece)))
                {
                    result = true;
                }
            }

            return result;
        }

        private Square getPieceOnPosB(FormedMove move, Square[,] board)
        {
            Square piece;
            piece = board[move.PosB.Item1, move.PosB.Item2];
            return piece;
        }

        private bool isEmptyPieceOnPosB(FormedMove move, Square[,] board)
        {
            bool result;
            char piece = board[move.PosB.Item1, move.PosB.Item2].piece;
            ;
            result = (piece == 'e') ? true : false;
            return result;
        }





        /// <summary>
        /// Takes a board, and a tuple specifying the location of one of the kings' squares.
        /// The function returns a boolean indicating whether or not that king currently 
        /// finds itself in check. ~this function is called frequently.
        /// i think it will call explorePath with a number of different configurations until it finds a threat
        /// </summary>
        /// <param name="a"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool isInCheck(Square[,] board, Tuple<int,int> loc)
        {
            bool check = false;

            int row = loc.Item1;
            int col = loc.Item2;
            Square kingSq = board[row, col];
            char player = Char.IsUpper(kingSq.piece) ? 'b' : 'w'; // only squares not empty, not of this player, and have a path to this king can attack it

            System.Diagnostics.Debug.Assert(Char.ToLower(board[row, col].piece) == 'k'); // already determined the player, just assert its really a king 

          



            return false;
        }


        /// <summary>
        /// 'path' is a vector to explore from the 'location' origin
        /// </summary>
        /// <param name="location"></param>
        /// <param name="path"></param>
        /// <param name="board"></param>
        private bool explorePathForThreats(Tuple<int,int> location, Tuple<int,int> vector, Square[,] board, char player)
        {
            // a threat is an encountered piece along the vector whose player value
            // is not player, and holds an inverse vector (can reach it the other way)
            // add the 12*12 back with X
            // search along a path until reach a non e square (including X)
        }

    }
}
