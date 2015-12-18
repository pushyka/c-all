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
        public List<List<Tuple<int, int>>>[][,] rayArrayPawnCapture;

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
            // move is loaded into these A -> B tiles
            TileStruct tileA = new TileStruct();
            TileStruct tileB = new TileStruct();

            if (toAndFromPositionsDistinct(move))
            {
                System.Console.WriteLine("is distinct");

                if (isTileACurrentPlayer(move, cpm, ref tileA))
                {
                    System.Console.WriteLine("is current player");

                    if (isTileBCurrentPlayer(move, cpm, ref tileB))
                    {
                        System.Console.WriteLine("1");
                        //outcome = bool;
                        // IF OUTCOME:
                        outcome = false;
                        moveType = "castle";
                        System.Console.WriteLine($"Movetype should be a {moveType} and it is?:{outcome}");
                    }

                    else if (isTileBOtherPlayer(move, cpm, ref tileB))
                    {
                        System.Console.WriteLine("2");
                        //outcome = bool;
                        // IF OUTCOME :
                        outcome = isMoveLegalCapture(move, cpm);                            //        <--  done (todo: pawns, horses)
                        moveType = "capture";
                        System.Console.WriteLine($"Movetype should be a {moveType} and it is?:{outcome}");
                    }

                    else if (isTileBEmpty(move, cpm, ref tileB))
                    {
                        if (outcome = isMoveLegalMovement(move, cpm))                      //          <-- done
                        {
                            System.Console.WriteLine("3");
                            moveType = "movement";
                        }

                        else if (outcome = isMoveLegalEP(move, cpm))                       //          <-- todo
                        {
                            moveType = "enpassantcapture";
                            System.Console.WriteLine("4");
                        }
                        System.Console.WriteLine($"Movetype should be a {moveType} and it is?:{outcome}");
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

                ;

                
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
            // The .Equals method of the Tuple does this
            return (posA.Item1 == posB.Item1 && posA.Item2 == posB.Item2) ? false : true;
        }

        private bool isTileACurrentPlayer(FormedMove move, ChessPosition cpm, ref TileStruct posA)
        {

            bool result = false;
            posA = cpm.Board[move.PosA.Item1, move.PosA.Item2];
            ;
            if (!posA.IsEmpty())
            {
                if (cpm.Player.has(posA.piece.Val))
                {
                    result = true;
                }
            }
            //System.Console.WriteLine("JAJAJA" + cpm.Player + posA.piece + result);
            return result;
        }

        private bool isTileBCurrentPlayer(FormedMove move, ChessPosition cpm, ref TileStruct posB)
        {
            bool result = false;
            posB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            ;
            if (!posB.IsEmpty())
            {
                if ((cpm.Player.has(posB.piece.Val)))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool isTileBOtherPlayer(FormedMove move, ChessPosition cpm, ref TileStruct tileB)
        {
            bool result = false;
            tileB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            ;
            if (!tileB.IsEmpty())
            {
                // player b uses the upper case chars, so this means piece is owned by the opposite to player b and vice versa
                if (!cpm.Player.has(tileB.piece.Val))
                    result = true;
            }

            return result;
        }


        private bool isTileBEmpty(FormedMove move, ChessPosition cpm, ref TileStruct posB)
        {
            bool result;
            posB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            ;
            result = (posB.IsEmpty()) ? true : false;
            return result;
        }

        /// <summary>
        /// Check if a given movement FormedMove is valid on the current board.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="cpm"></param>
        /// <returns></returns>
        private bool isMoveLegalMovement(FormedMove move, ChessPosition cpm)
        {

            bool isMoveLegalMovement = true;

            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;

            TileStruct tileA = cpm.Board[posA.Item1, posA.Item2];
            List<List<Tuple<int, int>>> rays;
                // get a list of movement rays for the Apiece on the board
            rays = getRays(tileA.piece.Val, posA);

            // get the ray which contains the capture (it will have posB)
            List<Tuple<int, int>> moveRay = null;
            foreach (List<Tuple<int, int>> ray in rays)
            {
                if (ray.Contains(posB))
                {
                    moveRay = ray;
                    break;
                }
                    
            }
                ;
            // check all intermediate tiles before posB are empty
            if (moveRay != null)
                foreach (Tuple<int, int> position in moveRay)
                {

                    // if an intermediate tile not empty, stop checking and set valid move to false
                    TileStruct tileAtPosition = cpm.Board[position.Item1, position.Item2];
                    if (!tileAtPosition.IsEmpty())
                    {
                        isMoveLegalMovement = false;
                        break;
                    }
                        
                    // if reached the tileB, (and so far so good) break on still true
                    if (position.Equals(posB))
                    {
                        ;
                        break;
                    }
                }
            //else posB not in any of the rays (ie not even on the naive rays
            else
                isMoveLegalMovement = false;

            //List<Tuple<int, int>> coordsWithPiecesPieceACanCapture = getCoordsWithPiecesPieceACanCapture(posA, style, cpm.Board);

            //return coordsWithPiecesPieceACanCapture.Contains(move.PosB);
            return isMoveLegalMovement;

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
                    if (!cpm.Board[newPos.Item1, newPos.Item2].IsEmpty())
                        break;
                    //otherwise its empty so add it to the coords and continue along the path
                    // no index error since already checked its not off the board
                    if (cpm.Board[newPos.Item1, newPos.Item2].IsEmpty())
                        coordsPieceACanMoveTo.Add(newPos);


                    coordsNum ++;
                }
            }

            return coordsPieceACanMoveTo;
        }

        private bool isMoveLegalEP(FormedMove move, ChessPosition cpm)
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
            TileStruct pieceOnPosA = cpm.Board[posA.Item1, posA.Item2];
            char posAPlayer = ((int)pieceOnPosA.piece.Val < 6) ? 'w' : 'b';
            if (pieceOnPosA.piece.Val == Pieces.pawnW || pieceOnPosA.piece.Val == Pieces.pawnB)
            {
                ;
                CaptureStyle style = MovementStyles.getCaptureStyle(pieceOnPosA.piece);
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
                    TileStruct pieceOnPassantPos = cpm.Board[passantPos.Item1, passantPos.Item2];
                    // if passant piece is a pawn
                    ;
                    if (pieceOnPassantPos.piece.Val == Pieces.pawnW || pieceOnPassantPos.piece.Val == Pieces.pawnB)
                    {
                        // and its the other players piece
                        char pieceOnPassantPosOwner = ((int)pieceOnPassantPos.piece.Val < 6) ? 'w' : 'b';
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



        /* Takes a move object which contains a piece of current player on position A
           and a piece of opposite player on position B (garunteed). Uses the cpm board to determine
           if the capture can be made. I.e it is not blocked by any pieces en route.
        */
        private bool isMoveLegalCapture(FormedMove move, ChessPosition cpm)
        {
            bool isMoveLegalCapture = true;

            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;

            TileStruct tileA = cpm.Board[posA.Item1, posA.Item2];
            List<List<Tuple<int, int>>> rays;
            // if the moving piece IS a pawn, then the capture rays are different than the normal movement ray
            if (tileA.piece.Val == Pieces.pawnB || tileA.piece.Val == Pieces.pawnW)
            {
                rays = getRaysPawnCapture(tileA.piece.Val, posA);
            }

            // if the moving piece is not a pawn
            else
            {
                // get a list of movement rays for the Apiece on the board
                rays = getRays(tileA.piece.Val, posA);
            }

            // get the ray which contains the capture (it will have posB)
            List<Tuple<int, int>> capRay = null;
            foreach (List<Tuple<int, int>> ray in rays)
            {
                if (ray.Contains(posB))
                {
                    capRay = ray;
                    break;
                }
                    
            }
                ;
            // check all intermediate tiles before posB are empty
            if (capRay != null)
                foreach (Tuple<int, int> position in capRay)
                {
                    // if reached the target, stop checking the intermediate tiles
                    if (position.Equals(posB))
                        break;
                    // if an intermediate tile not empty, stop checking and set valid move to false
                    TileStruct tileAtPosition = cpm.Board[position.Item1, position.Item2];
                    if (!tileAtPosition.IsEmpty())
                    {
                        isMoveLegalCapture = false;
                        break;
                    }
                        
                }
            //else posB not in any of the rays (ie not even on the naive rays
            else
                isMoveLegalCapture = false;

            //List<Tuple<int, int>> coordsWithPiecesPieceACanCapture = getCoordsWithPiecesPieceACanCapture(posA, style, cpm.Board);

            //return coordsWithPiecesPieceACanCapture.Contains(move.PosB);
            return isMoveLegalCapture;
        }



        private List<Tuple<int, int>> getCoordsWithPiecesPieceACanCapture(Tuple<int, int> posA, CaptureStyle style, TileStruct[,] board)
        {
            List<Tuple<int, int>> coordsPieceACanCapture = new List<Tuple<int, int>>();
            char posAPlayer = ((int)board[posA.Item1, posA.Item2].piece.Val < 6) ? 'w' : 'b';
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
                    if (!board[newPos.Item1, newPos.Item2].IsEmpty())
                    {
                        //and the piece is of opponent
                        char owner = ((int)board[newPos.Item1, newPos.Item2].piece.Val < 6) ? 'w' : 'b';
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
            Player cur_player = cpmCopy.Player;
            Tuple<int, int> cur_playerKingPos;
            for (int row = 0; row < cpmCopy.Dim; row ++)
            {
                for (int col = 0; col < cpmCopy.Dim; col ++)
                {
                    TileStruct tile = cpmCopy.Board[row, col];
                    if (!tile.IsEmpty())
                    {
                        if ((tile.piece.Val == Pieces.k || tile.piece.Val == Pieces.K) &&
                            (cur_player.has(tile.piece.Val)))
                        {
                            cur_playerKingPos = Tuple.Create(row, col);
                            break;
                        }
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

        public void preloadRayArrayPawnCapture()
        {
            // 2 unique pawn pieces
            rayArrayPawnCapture = new List<List<Tuple<int, int>>>[2][,];
            CaptureStyle WhitePawnCaptureStyle = MovementStyles.getCaptureStyle(Pieces.pawnW);
            CaptureStyle BlackPawnCaptureStyle = MovementStyles.getCaptureStyle(Pieces.pawnB);

            rayArrayPawnCapture[0] = new List<List<Tuple<int, int>>>[8, 8]; // white
            rayArrayPawnCapture[1] = new List<List<Tuple<int, int>>>[8, 8]; // black

            for (int j = 0; j < 8; j ++)
            {
                for (int k = 0; k < 8; k ++)
                {
                    List<List<Tuple<int, int>>> whiteRays = new List<List<Tuple<int, int>>>();
                    List<List<Tuple<int, int>>> blackRays = new List<List<Tuple<int, int>>>();

                    foreach (Tuple<int, int> dir in WhitePawnCaptureStyle.dirs)
                    {
                        List<Tuple<int, int>> whiteRay = new List<Tuple<int, int>>();
                        Tuple<int, int> newPos;
                        int newPosRank = j + dir.Item1;
                        int newPosFile = k + dir.Item2;
                        newPos = Tuple.Create(newPosRank, newPosFile);
                        if ((newPosRank < 0 || newPosRank > 7) ||
                            (newPosFile < 0 || newPosFile > 7))
                            break;
                        whiteRay.Add(newPos);
                        whiteRays.Add(whiteRay);
                    }

                    foreach (Tuple<int, int> dir in BlackPawnCaptureStyle.dirs)
                    {
                        List<Tuple<int, int>> blackRay = new List<Tuple<int, int>>();
                        Tuple<int, int> newPos;
                        int newPosRank = j + dir.Item1;
                        int newPosFile = k + dir.Item2;
                        newPos = Tuple.Create(newPosRank, newPosFile);
                        if ((newPosRank < 0 || newPosRank > 7) ||
                            (newPosFile < 0 || newPosFile > 7))
                            break;
                        blackRay.Add(newPos);
                        blackRays.Add(blackRay);
                    }

                    rayArrayPawnCapture[0][j, k] = whiteRays;
                    rayArrayPawnCapture[1][j, k] = blackRays;

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
        public List<List<Tuple<int, int>>> getRays(Pieces piece, Tuple<int, int> location)
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

        public List<List<Tuple<int,int>>> getRaysPawnCapture(Pieces piece, Tuple<int, int> location)
        {
            if (rayArrayPawnCapture == null)
                throw new Exception("ray array (pawns) noy instantiated");

            int pieceIndex = (piece == Pieces.pawnW) ? 0 : 1;
            System.Console.WriteLine($"> There are {rayArray[pieceIndex][location.Item1, location.Item2].Count} direction rays");
            System.Console.WriteLine($"> for piece {piece} at location {location}");
            foreach (List<Tuple<int, int>> ray in rayArrayPawnCapture[pieceIndex][location.Item1, location.Item2])
            {
                System.Console.WriteLine($"> counts: {ray.Count}");
            }
            return rayArrayPawnCapture[pieceIndex][location.Item1, location.Item2];
        }
    }
}
