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
            // a formed move is garunteed to be within the board bounds

            bool moveIsValidOnBoard = false;
            if (toAndFromPositionsDistinct(move) &&
                posAContainsCurPlayerPiece(move, board, cur_turn)

                )
            // && ..
            // && ..
            {
                moveIsValidOnBoard = true;
            }
            // TODO
            return moveIsValidOnBoard;
        }

        private bool toAndFromPositionsDistinct(FormedMove move)
        {
            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;
            return (posA.Item1 == posB.Item1 && posA.Item2 == posB.Item2) ? false : true;
        }

        private bool posAContainsCurPlayerPiece(FormedMove move, Square[,] board, char cur_turn)
        {
            bool containsCurPlayerPiece = false;
            char piece = board[move.PosA.Item1, move.PosA.Item2].piece;
            ;
            if (piece != 'e')
            {
                if ((cur_turn == 'b' && Char.IsUpper(piece)) || (cur_turn == 'w' && Char.IsLower(piece)))
                {
                    containsCurPlayerPiece = true;
                }
            }

            return containsCurPlayerPiece;
        }

        /// <summary>
        /// Takes a position a, upon which a king is expected to be in the board object.
        /// Determines if the king at a is currently in check. This method called frequently.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool determineIfCheck(Tuple<int,int> a, Chess board)
        {
            // TODO, optimise
            return false;
        }

    }
}
