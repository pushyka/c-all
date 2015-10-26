using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using chess.Model;

namespace chess.Util
{
    class Evaluator : IEvaluator
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
                        outcome = true;
                             moveType = "castle";
                        return outcome;
                    }
                    else if (isOponentTurnPieceOnPosB(move, board, cur_turn, ref pieceOnPosB))
                    {
                        // posA is cur player, posB is opponent

                        // possibly a capture


                        //outcome = bool;
                        // IF OUTCOME :
                        outcome = canPieceALegallyCapturePieceOnPosB(move, board);
                        moveType = "capture";
                        return outcome;
                    }
                    else if (isEmptyPieceOnPosB(move, board, ref pieceOnPosB))
                    {
                        // check if the piece on posA can legally move to the posB position
                        System.Console.WriteLine("4 NOWABOUTS PASSANT y:3 x:5 {0}", board[3, 5].canBeCapturedEnPassant);

                        //outcome = check path / check;
                        // IF OUTCOME: 
                        outcome = canPieceALegallyMoveToPosB(move, board);
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
            if (posA.pID != 'e')
            {
                if ((cur_turn == 'b' && Char.IsUpper(posA.pID)) || (cur_turn == 'w' && Char.IsLower(posA.pID)))
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
            if (posB.pID != 'e')
            {
                if ((cur_turn == 'b' && Char.IsUpper(posB.pID)) || (cur_turn == 'w' && Char.IsLower(posB.pID)))
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
            if (posB.pID != 'e')
            {
                // player b uses the upper case chars, so this means piece is owned by the opposite to player b and vice versa
                if ((cur_turn == 'b' && Char.IsLower(posB.pID)) || (cur_turn == 'w' && Char.IsUpper(posB.pID)))
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
            result = (posB.pID == 'e') ? true : false;
            return result;
        }

        private bool canPieceALegallyMoveToPosB(FormedMove move, Square[,] board)
        {
            System.Console.WriteLine("3 NOWABOUTS PASSANT y:3 x:5 {0}", board[3, 5].canBeCapturedEnPassant);
            Tuple<int, int> posA = move.PosA;
            // contains infor relevant for pawns
            Square pieceOnPosA = board[posA.Item1, posA.Item2];
            // generate the coords which piece could move to given its starting position and rule, and board context


            // get a movement style for a given piece
            MovementStyle style = MovementStyles.getMovementStyle(pieceOnPosA);

            // apply the movement style to posA to generate valid positions pieceA can go to
            // do this until maximum iterations reached or another piece in the way

            List<Tuple<int, int>> coordsPieceACanMoveTo = getCoordsPieceACanMoveTo(posA, style, board);
            ;

            // if move.posB exists in the resulting list of coords, then piece on posA can move to it
            return coordsPieceACanMoveTo.Contains(move.PosB);

        }

        private List<Tuple<int, int>> getCoordsPieceACanMoveTo(Tuple<int, int> posA, MovementStyle style, Square[,] board)
        {
            List<Tuple<int, int>> coordsPieceACanMoveTo = new List<Tuple<int, int>>();
            System.Console.WriteLine("2 NOWABOUTS PASSANT y:3 x:5 {0}", board[3, 5].canBeCapturedEnPassant);
            foreach (Tuple<int, int> direction in style.dirs)
            {
                int coordsNum = 1;
                
                while (coordsNum <= style.maxIterations)
                {
                    //generate the new coordinate by adding 1 unit of direction to the initial posA
                    Tuple<int, int> newPos;
                    int newPosRank = posA.Item1 + (coordsNum * direction.Item1);
                    int newPosFile = posA.Item2 + (coordsNum * direction.Item2);
                    newPos = Tuple.Create(newPosRank, newPosFile);
                    // if the newPos is not on the board, dont add it to the list
                    // and break since movement along this direction cannot continue
                    if ((newPosRank < 0 || newPosRank > 7) ||
                        (newPosFile < 0 || newPosFile > 7))
                        break;
                    // if the newPos is occupied by a non e, then movement along this path is blocked
                    // so dont add it to the list and also break since movement cannot continue
                    if (board[newPos.Item1, newPos.Item2].pID != 'e')
                        break;
                    //otherwise its empty so add it to the coords and continue along the path
                    // no index error since already checked its not off the board
                    if (board[newPos.Item1, newPos.Item2].pID == 'e')
                        coordsPieceACanMoveTo.Add(newPos);


                    coordsNum ++;
                }
            }

            // THE EN PASSANT CAPTURE IS A ->MOVE<- WITH A SECONDARY CAPTURE SIDE EFFECT
            // check the positions adjacent to the posA:
            // IF they -contain an enemy pawn
            //         -the enemy pawn is.canBeCapturedEnPassant (this also implies the capturing pawn is in correct position)
            // THEN add the diagonal coord to coordspieceacancapture
            System.Console.WriteLine("1 NOWABOUTS PASSANT y:3 x:5 {0}", board[3, 5].canBeCapturedEnPassant);
            System.Console.WriteLine("HERE 1");
            Square pieceOnPosA = board[posA.Item1, posA.Item2];
            char posAPlayer = (char.IsUpper(pieceOnPosA.pID)) ? 'b' : 'w';
            if (pieceOnPosA.pID == 'p' || pieceOnPosA.pID == 'P')
            {
                ;
                style = MovementStyles.getCaptureStyle(pieceOnPosA);
                foreach (Tuple<int, int> attackDirection in style.dirs)
                {
                    Tuple<int, int> attackPos;
                    int attackPosRank = posA.Item1 + attackDirection.Item1;
                    int attackPosFile = posA.Item2 + attackDirection.Item2;
                    attackPos = Tuple.Create(attackPosRank, attackPosFile);
                    
                    Tuple<int, int> passantPos;
                    int passantPosRank = posA.Item1; // adjacent
                    int passantPosFile = attackPosFile;
                    passantPos = Tuple.Create(passantPosRank, passantPosFile);
                    if ((passantPosRank < 0 || passantPosRank > 7) ||
                        (passantPosFile < 0 || passantPosFile > 7))
                        break;
                    ; // correct here
                    Square pieceOnPassantPos = board[passantPos.Item1, passantPos.Item2];
                    // if passant piece is a pawn
                    ;
                    if (pieceOnPassantPos.pID == 'p' || pieceOnPassantPos.pID == 'P')
                    {
                        // and its the other players piece
                        char pieceOnPassantPosOwner = (char.IsUpper(pieceOnPassantPos.pID)) ? 'b' : 'w';
                        if (pieceOnPassantPosOwner != posAPlayer)
                        {
                            // and it can be captured enpassant
                         
                            if (pieceOnPassantPos.canBeCapturedEnPassant)
                            {
                                // then add the attackPos to the list of valid capture to positions
                                coordsPieceACanMoveTo.Add(attackPos);
                            }

                        }
                    }
                }
            }
            // in the apply move function as a capture, and the piece is  pawn and there IS NOTHING in the posB, then this means it was
            // an en passant capture.


            return coordsPieceACanMoveTo;
        }

        private bool canPieceALegallyCapturePieceOnPosB(FormedMove move, Square[,] board)
        {
            Tuple<int, int> posA = move.PosA;
            Square pieceOnPosA = board[posA.Item1, posA.Item2];
            CaptureStyle style = MovementStyles.getCaptureStyle(pieceOnPosA);
            List<Tuple<int, int>> coordsWithPiecesPieceACanCapture = getCoordsWithPiecesPieceACanCapture(posA, style, board);

            return coordsWithPiecesPieceACanCapture.Contains(move.PosB);
        }


        private List<Tuple<int, int>> getCoordsWithPiecesPieceACanCapture(Tuple<int, int> posA, CaptureStyle style, Square[,] board)
        {
            List<Tuple<int, int>> coordsPieceACanCapture = new List<Tuple<int, int>>();
            char posAPlayer = (char.IsUpper(board[posA.Item1, posA.Item2].pID)) ? 'b' : 'w';
            // foreach move direction form a list of any coords which have tiles can be captured (one per direction at most)
            foreach (Tuple<int, int> attackDirection in style.dirs)
            {
                int coordsNum = 1;
                while (coordsNum <= style.maxIterations)
                {
                    //generate the new coordinate by adding 1 unit of direction to the initial posA
                    Tuple<int, int> newPos;
                    int newPosRank = posA.Item1 + (coordsNum * attackDirection.Item1);
                    int newPosFile = posA.Item2 + (coordsNum * attackDirection.Item2);
                    newPos = Tuple.Create(newPosRank, newPosFile);
                    // if the newPos is not on the board, dont add it to the list
                    // and break since movement along this direction cannot continue
                    if ((newPosRank < 0 || newPosRank > 7) ||
                        (newPosFile < 0 || newPosFile > 7))
                        break;
                    // if reach a piece and the piece is not empty
                    if (board[newPos.Item1, newPos.Item2].pID != 'e')
                    {
                        //and the piece is of opponent
                        char owner = (char.IsUpper(board[newPos.Item1, newPos.Item2].pID)) ? 'b' : 'w';
                        if (owner != posAPlayer)
                        {
                            coordsPieceACanCapture.Add(newPos);
                            // add it to the list
                            break;
                        }

                        // else its own piece
                        else
                            break;
                    }
                    // otherwise its empty so the attack direction is not blocked etc
                    coordsNum++;
                }
            }
            



            return coordsPieceACanCapture;
        }




        public bool isKingInCheck(Square[,] board, char player, ref List<Tuple<int, int>> attackerPositions)
        {
            // find the king
            Tuple<int, int> kingPos;
            for (int row = 0; row < 8; row ++)
            {
                for (int col = 0; col < 8; col ++)
                {
                    char piece = board[row, col].pID;
                    if ((player == 'w' && piece == 'k') ||
                        (player == 'b' && piece == 'K'))
                    {
                        kingPos = Tuple.Create(row, col);
                        break;
                    }
                }
            }

            // look along all possible attack vectors for isUpper if 'b' or usLower if 'w' using kingpos as origin

            return false;
            
        }
    }
}
