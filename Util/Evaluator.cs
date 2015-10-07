using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using chess.Model;

namespace chess.Util
{
    class Evaluator
    {

        /// <summary>
        /// Ensures the user input matches the expected format (B2 etc), then convert the input to the 
        /// FormedMove format. (could just as easily use a move class but I like these types)
        /// Function returns whether or not the move was completely formed or not.
        /// If the function returns true, then this guarantees the move object has been completely 
        /// formed and is ready to be validated against the current board state. 
        /// The move object is in the calling scope, this function assembles it via 
        /// a reference.
        /// 
        /// TODO: remove forloop
        /// </summary>
        /// <param name="input"> "B2 B4" </param>
        /// <returns></returns>
        public bool validateInput(string input, ref FormedMove move)
        {

           move = new FormedMove();
            
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
                                move.Add(formedPosition);
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

            return move.IsCompletelyFormed;

        }

        /// <summary>
        /// In-depth move checker function determines whether a FormedMove move
        /// is valid in the context of the rules/requirements. board is a cloned copy so that additions may be 
        /// made to it during the process of checking if the move is valid (after move is made does it result in check etc)
        /// At the end of the function, this board is discarded and a boolean result is returned indicating 
        /// if the move is legal or not.
        /// If the move fails to validate, a report string is stored in 'invalidInfo'
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="board"></param>
        /// <param name="moveType"></param>
        /// <returns></returns>
        public bool validateMove(FormedMove move, Square[,] board, char cur_turn, ref string moveType)
        {
            // 1 a formed move is already guaranteed to be within the board bounds (from formatMove)
            // 2 check to and from pos are not the same
            // 3 check from pos contains piece of current player


            bool outcome = false; 
            Square pieceOnPosA = new Square();
            Square pieceOnPosB = new Square();

            if (toAndFromPositionsDistinct(move))
            {
                if (isCurTurnPieceOnPosA(move, board, cur_turn, ref pieceOnPosA))
                {
                    // pieceOnPosA now contains the piece on posA (which is of cur turn player)
                    // good, if it was anything else (oponent or empty its immediatly invalid)




                    if (isCurTurnPieceOnPosB(move, board, cur_turn, ref pieceOnPosB))
                    {
                        // now both posA and posB contains cur player pieces

                        // possibly a castling .. to do later


                        //outcome = bool;
                        // IF OUTCOME:
                             moveType = "castle";
                        return outcome;
                    }
                    else if (isOponentTurnPieceOnPosB(move, board, cur_turn, ref pieceOnPosB))
                    {
                        // posA is cur player, posB is opponent

                        // possibly a capture


                        //outcome = bool;
                        // IF OUTCOME :
                            moveType = "capture";
                        return outcome;
                    }
                    else if (isEmptyPieceOnPosB(move, board, ref pieceOnPosB))
                    {
                        // posA is cur player, posB is empty square

                        // possibly a normal move


                        //outcome = check path / check;
                        // IF OUTCOME: 
                        outcome = true;
                        moveType = "movement";
                        return outcome;
                    }
                }
                else
                {
                    // invalid (posA is either non player allegiance or empty)
                    System.Console.WriteLine("posA was nota player piece");
                }
            }

            else
            {
                System.Console.WriteLine("positions are not distinct");
            }








            //outcome of the final check (path check / check check)
            // if that check is not reached (fails on distinct check)
            // returns the default false value of outcome :)
            // if control reaches here, then it hasnt dentified a valid move
            return outcome;
        }

        private bool toAndFromPositionsDistinct(FormedMove move)
        {
            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;
            return (posA.Item1 == posB.Item1 && posA.Item2 == posB.Item2) ? false : true;
        }

        private bool isCurTurnPieceOnPosA(FormedMove move, Square[,] board, char cur_turn, ref Square posA)
        {
            bool result = false;
            posA = board[move.PosA.Item1, move.PosA.Item2];
            ;
            if (posA.piece != 'e')
            {
                if ((cur_turn == 'b' && Char.IsUpper(posA.piece)) || (cur_turn == 'w' && Char.IsLower(posA.piece)))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool isCurTurnPieceOnPosB(FormedMove move, Square[,] board, char cur_turn, ref Square posB)
        {
            bool result = false;
            posB = board[move.PosB.Item1, move.PosB.Item2];
            ;
            if (posB.piece != 'e')
            {
                if ((cur_turn == 'b' && Char.IsUpper(posB.piece)) || (cur_turn == 'w' && Char.IsLower(posB.piece)))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool isOponentTurnPieceOnPosB(FormedMove move, Square[,] board, char cur_turn, ref Square posB)
        {
            bool result = false;
            posB = board[move.PosB.Item1, move.PosB.Item2];
            ;
            if (posB.piece != 'e')
            {
                // player b uses the upper case chars, so this means piece is owned by the opposite to player b and vice versa
                if ((cur_turn == 'b' && Char.IsLower(posB.piece)) || (cur_turn == 'w' && Char.IsUpper(posB.piece)))
                {
                    result = true;
                }
            }

            return result;
        }


        private bool isEmptyPieceOnPosB(FormedMove move, Square[,] board, ref Square posB)
        {
            bool result;
            posB = board[move.PosB.Item1, move.PosB.Item2];
            ;
            result = (posB.piece == 'e') ? true : false;
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
            return false;
        }

    }
}
