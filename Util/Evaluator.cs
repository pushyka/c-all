using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using chess.Model;

namespace chess.Util
{
    

    public class Evaluator : IEvaluator
    {

        const int UNIQUE_PIECE_NUM = 12;
        

        //List<List<Tuple<int, int>>>[][,] rayArray;
        public List<List<Tuple<int, int>>>[][,] rayArray;

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
        public bool validateMove(FormedMove move, ChessPosition cpm, ref string moveType, ref List<Tuple<int, int>> kingCheckedBy)
        {
            // 1 a formed move is already guaranteed to be within the board bounds (from formatMove)
            // 2 check to and from pos are not the same
            // 3 check from pos contains piece of current player



            bool outcome = false;
            Tile pieceOnPosA = new Tile();
            Tile pieceOnPosB = new Tile();

            if (toAndFromPositionsDistinct(move))
            {
                if (isCurTurnPieceOnPosA(move, cpm, ref pieceOnPosA))
                {
                    // pieceOnPosA now contains the piece on posA (which is of cur turn player)
                    // good, if it was anything else (oponent or empty its immediatly invalid)




                    if (isCurTurnPieceOnPosB(move, cpm, ref pieceOnPosB))
                    {
                        // now both posA and posB contains cur player pieces

                        // possibly a castling .. to do later


                        //outcome = bool;
                        // IF OUTCOME:
                        outcome = false;
                        moveType = "castle";
                    }
                    else if (isOponentTurnPieceOnPosB(move, cpm, ref pieceOnPosB))
                    {
                        // posA is cur player, posB is opponent

                        // possibly a capture


                        //outcome = bool;
                        // IF OUTCOME :
                        outcome = canPieceALegallyCapturePieceOnPosB(move, cpm);
                        moveType = "capture";
                    }
                    else if (isEmptyPieceOnPosB(move, cpm, ref pieceOnPosB))
                    {

                        // possibly a movement move or a en passant move

                        if (outcome = canPieceALegallyMoveToPosB(move, cpm))
                        {
                            moveType = "movement";
                        }

                        else if (outcome = canPieceALegallyMoveEnPassantToPosB(move, cpm))
                        {
                            moveType = "enpassantcapture";
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



                
            }
            // if outcomevalid is true so far, finally check the move wont leave a check
            if (outcome)
            {
                ChessPosition cpmCopy = cpm.getEvaluateableChessPosition();
                cpmCopy.applyMove(move, moveType); // this cpm is not used by the view
                outcome = isKingInCheck(cpmCopy, ref kingCheckedBy);
                // if this is true the outcome should really be FALSE (for valid)
            }


            return outcome;
        }

        private bool toAndFromPositionsDistinct(FormedMove move)
        {
            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;
            return (posA.Item1 == posB.Item1 && posA.Item2 == posB.Item2) ? false : true;
        }

        private bool isCurTurnPieceOnPosA(FormedMove move, ChessPosition cpm, ref Tile posA)
        {

            bool result = false;
            posA = cpm.Board[move.PosA.Item1, move.PosA.Item2];
            ;
            if (posA.pID != 'e')
            {
                if ((cpm.Player == 'b' && Char.IsUpper(posA.pID)) || (cpm.Player == 'w' && Char.IsLower(posA.pID)))
                {
                    result = true;
                }
            }
            System.Console.WriteLine("JAJAJA" + cpm.Player + posA.pID + result);
            return result;
        }

        private bool isCurTurnPieceOnPosB(FormedMove move, ChessPosition cpm, ref Tile posB)
        {
            bool result = false;
            posB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            ;
            if (posB.pID != 'e')
            {
                if ((cpm.Player == 'b' && Char.IsUpper(posB.pID)) || (cpm.Player == 'w' && Char.IsLower(posB.pID)))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool isOponentTurnPieceOnPosB(FormedMove move, ChessPosition cpm, ref Tile posB)
        {
            bool result = false;
            posB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            ;
            if (posB.pID != 'e')
            {
                // player b uses the upper case chars, so this means piece is owned by the opposite to player b and vice versa
                if ((cpm.Player == 'b' && Char.IsLower(posB.pID)) || (cpm.Player == 'w' && Char.IsUpper(posB.pID)))
                {
                    result = true;
                }
            }

            return result;
        }


        private bool isEmptyPieceOnPosB(FormedMove move, ChessPosition cpm, ref Tile posB)
        {
            bool result;
            posB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            ;
            result = (posB.pID == 'e') ? true : false;
            return result;
        }

        private bool canPieceALegallyMoveToPosB(FormedMove move, ChessPosition cpm)
        {
           
            Tuple<int, int> posA = move.PosA;
            // contains infor relevant for pawns
            Tile pieceOnPosA = cpm.Board[posA.Item1, posA.Item2];
            // generate the coords which piece could move to given its starting position and rule, and board context


            // get a movement style for a given piece
            MovementStyle style = MovementStyles.getMovementStyle(pieceOnPosA);

            // apply the movement style to posA to generate valid positions pieceA can go to
            // do this until maximum iterations reached or another piece in the way

            // this is going to change up t the preload array lookup
            List<Tuple<int, int>> coordsPieceACanMoveTo = getCoordsPieceACanMoveTo(posA, style, cpm);
            ;

            // if move.posB exists in the resulting list of coords, then piece on posA can move to it
            return coordsPieceACanMoveTo.Contains(move.PosB);

        }

        private List<Tuple<int, int>> getCoordsPieceACanMoveTo(Tuple<int, int> posA, MovementStyle style, ChessPosition cpm)
        {
            List<Tuple<int, int>> coordsPieceACanMoveTo = new List<Tuple<int, int>>();
            
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
                    if (cpm.Board[newPos.Item1, newPos.Item2].pID != 'e')
                        break;
                    //otherwise its empty so add it to the coords and continue along the path
                    // no index error since already checked its not off the board
                    if (cpm.Board[newPos.Item1, newPos.Item2].pID == 'e')
                        coordsPieceACanMoveTo.Add(newPos);


                    coordsNum ++;
                }
            }

            return coordsPieceACanMoveTo;
        }

        private bool canPieceALegallyMoveEnPassantToPosB(FormedMove move, ChessPosition cpm)
        {

            List<Tuple<int, int>> coordsPieceACanMoveEnPassantTo = getCoordsPieceALegallyMoveEnPassantToPosB(move, cpm);
            return coordsPieceACanMoveEnPassantTo.Contains(move.PosB);

        }

        private List<Tuple<int, int>> getCoordsPieceALegallyMoveEnPassantToPosB(FormedMove move, ChessPosition cpm)
        {
            
            // THE EN PASSANT CAPTURE IS A ->MOVE<- WITH A SECONDARY CAPTURE SIDE EFFECT
            // check the positions adjacent to the posA:
            // IF they -contain an enemy pawn
            //         -the enemy pawn is.canBeCapturedEnPassant (this also implies the capturing pawn is in correct position)
            // THEN add the diagonal coord to coordspieceacancapture
            List<Tuple<int, int>> coordsPieceACanMoveEnPassantTo = new List<Tuple<int, int>>();
            Tuple<int, int> posA = move.PosA;
            Tile pieceOnPosA = cpm.Board[posA.Item1, posA.Item2];
            char posAPlayer = (char.IsUpper(pieceOnPosA.pID)) ? 'b' : 'w';
            if (pieceOnPosA.pID == 'p' || pieceOnPosA.pID == 'P')
            {
                ;
                CaptureStyle style = MovementStyles.getCaptureStyle(pieceOnPosA);
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
                    Tile pieceOnPassantPos = cpm.Board[passantPos.Item1, passantPos.Item2];
                    // if passant piece is a pawn
                    ;
                    if (pieceOnPassantPos.pID == 'p' || pieceOnPassantPos.pID == 'P')
                    {
                        // and its the other players piece
                        char pieceOnPassantPosOwner = (char.IsUpper(pieceOnPassantPos.pID)) ? 'b' : 'w';
                        if (pieceOnPassantPosOwner != posAPlayer)
                        {
                            // and it can be captured enpassant

                            if (passantPos.Equals(cpm.EnPassantSq))
                            {
                                // then add the attackPos to the list of valid capture to positions
                                coordsPieceACanMoveEnPassantTo.Add(attackPos);
                            }

                        }
                    }
                }
            }
            // in the apply move function as a capture, and the piece is  pawn and there IS NOTHING in the posB, then this means it was
            // an en passant capture.
            return coordsPieceACanMoveEnPassantTo;
        }

        private bool canPieceALegallyCapturePieceOnPosB(FormedMove move, ChessPosition cpm)
        {
            Tuple<int, int> posA = move.PosA;
            Tile pieceOnPosA = cpm.Board[posA.Item1, posA.Item2];
            CaptureStyle style = MovementStyles.getCaptureStyle(pieceOnPosA);
            // todo change to pre lookup method
            List<Tuple<int, int>> coordsWithPiecesPieceACanCapture = getCoordsWithPiecesPieceACanCapture(posA, style, cpm.Board);

            return coordsWithPiecesPieceACanCapture.Contains(move.PosB);
        }


        private List<Tuple<int, int>> getCoordsWithPiecesPieceACanCapture(Tuple<int, int> posA, CaptureStyle style, Tile[,] board)
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



        /// <summary>
        /// This function is passed the reference to the chessPosModel. It uses the information contained in this object
        /// to determine whether or not the current player to move's king is being threatened by check from the other player
        /// returning true if so. If it is being checked by the opposing player a list of the one / two coords containing the 
        /// attacking piece's is built in 'kingCheckedBy'
        /// </summary>
        /// <param name="cpm"></param>
        /// <param name="kingCheckedBy"></param>
        /// <returns></returns>
        public bool isKingInCheck(ChessPosition cpmCopy, ref List<Tuple<int, int>> kingCheckedBy)
        {
            // find the king
            char cur_player = cpmCopy.Player;
            Tuple<int, int> cur_playerKingPos;
            for (int row = 0; row < cpmCopy.Dim; row ++)
            {
                for (int col = 0; col < cpmCopy.Dim; col ++)
                {
                    char piece = cpmCopy.Board[row, col].pID;
                    if ((cur_player == 'w' && piece == 'k') ||
                        (cur_player == 'b' && piece == 'K'))
                    {
                        cur_playerKingPos = Tuple.Create(row, col);
                        break;
                    }
                }
            }

            // look along all possible attack vectors for isUpper if 'b' or usLower if 'w' using kingpos as origin
            // bishop vectors : break when find one of queen, bishop, pawn(?)
            // rook vectors : queen, rook
            // knight vectors : knight
            // king vectors : king

           // this will match the pre compute ray array forall computation
            // want to get a series of rays for each of the above pieces on the king pos
            // eg bishop (4 rays), then foreach ray: starting from the ray origin: if find queen, bishop, pawn(?) of opponent piece
            //    in this, the bishop ray, then break as this piece threatens the king on origin
            return true;
            
        }


        public void preloadRayArray()
        {
            
            //rayArray = new List<List<Tuple<int, int>>>[12][,];
            // init 12 element array
            rayArray = new List<List<Tuple<int,int>>>[UNIQUE_PIECE_NUM][,];
            // 12 pieces
            //      8 rows per piece
            //              8 columns per row       //
            //                      1 List
            //                              n Lists
            //                                  n Tuple<int, int>

            for (int i = 0; i < rayArray.Length; i++)
            {
                // foreach of the i piece types, create 64 tiles each containing a number of rays (n directions)
                MovementStyle style = MovementStyles.getMovementStyle((Pieces)i);
                rayArray[i] = new List<List<Tuple<int,int>>>[8, 8];
                for (int j = 0; j < 8; j++) // row
                {
                    for (int k = 0; k < 8; k++) // col
                    {
                        // now create the list and add the results of piece i at pos j,k
                        List<List<Tuple<int, int>>> rays = new List<List<Tuple<int, int>>>();
                        // get the set of rays of the given piece and location

                        foreach (Tuple<int, int> dir in style.dirs)
                        {
                            List<Tuple<int, int>> ray = new List<Tuple<int, int>>();
                            int coordsNum = 1;

                            while (coordsNum <= style.maxIterations)
                            {
                                //generate the new coordinate by adding 1 unit of direction to the initial posA
                                Tuple<int, int> newPos;
                                int newPosRank = j + (coordsNum * dir.Item1);
                                int newPosFile = k + (coordsNum * dir.Item2);
                                newPos = Tuple.Create(newPosRank, newPosFile);
                                // if the newPos is not on the board, dont add it to the list
                                // and break since movement along this direction cannot continue
                                if ((newPosRank < 0 || newPosRank > 7) ||
                                    (newPosFile < 0 || newPosFile > 7))
                                    break;
                                ;
                                ray.Add(newPos);

                                coordsNum++;
                            }
                            rays.Add(ray);
                        }
                        rayArray[i][j, k] = rays;

                    }

                }

            }
        
        }

        /// <summary>
        /// Takes a piece and a y,x board location and returns one List containing
        /// a List for each valid direction of movement containing a sequence of board locations that
        /// the piece could potentially move to.
        /// 
        /// e.g. piece=r, location=(3,6)
        /// </summary>
        /// <param name="piece"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<List<Tuple<int, int>>> rayArrayGet(Pieces piece, Tuple<int, int> location)
        {
            if (rayArray == null)
                throw new Exception("ray array not instantiated");
            ;
            System.Console.WriteLine($"> There are {rayArray[(int)piece][location.Item1, location.Item2].Count} direction rays");
            System.Console.WriteLine($"> for piece {piece} at location {location}");
            foreach (List<Tuple<int, int>> ray in rayArray[(int)piece][location.Item1, location.Item2])
            {
                System.Console.WriteLine($"> counts: {ray.Count}");
            }
            
            return rayArray[(int)piece][location.Item1, location.Item2];


        }
    }
}
