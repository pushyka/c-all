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
        public List<List<Tuple<int, int>>>[][,] rayArray;
        public List<List<Tuple<int, int>>>[][,] rayArrayPawnCapture;

        /* Takes an input string from a view / source and returns true if it is
        in the correct chess board move format e.g. 'A5 A6'. The corresponding
        move object is also created and passed to the non-local move variable. */
        public bool ValidateInput(string input, ref FormedMove move)
        {
            move = new FormedMove();

            string[] tileSpecifiers = input.Split();

            List<char> validFiles = "ABCDEFGH".ToList();
            List<int> validRanks = new List<int>(new int[] { 8, 7, 6, 5, 4, 3, 2, 1});

            if (tileSpecifiers.Length == 2)
            {
                foreach (string tileSpecifier in tileSpecifiers)
                {
                    if (tileSpecifier.Length == 2)
                    {
                        char file = Char.ToUpper(tileSpecifier[0]);
                        int rank = (int)Char.GetNumericValue(tileSpecifier[1]);
                        
                        if (validFiles.Contains(file) && validRanks.Contains(rank))
                        {
                            int fileToCol = validFiles.IndexOf(file);
                            int rankToRow = validRanks.IndexOf(rank);

                            Tuple<int, int> formedPosition = Tuple.Create<int, int>(rankToRow, fileToCol);
                            move.Add(formedPosition);
                        }
                    }
                }
            }
            return move.IsValid;
        }


        /* Take a valid-format move object and check if it may be legally 
        applied to the current chess game. It must pass the following checks:
        -the locations are distinct and
        -the A tile contains a piece of the current player
        Also one of the following:
            -the B tile contains a piece of the current player (further castle checks) or
            -the B tile contains a piece of the opponent player (further capture checks) or
            -the B tile is empty:
                -(further en passant checks)
                -(further movement checks)
        Finally it must check, if the move were to be applied, that it does not leave the 
        current player's king in check.*/
        public bool ValidateMove(FormedMove move, ChessPosition cpm, ref EChessMoveTypes moveType, ref List<Tuple<int, int>> kingCheckedBy)
        {
            bool outcome = false;
            TileStruct tileA = new TileStruct();
            TileStruct tileB = new TileStruct();

            if (IsMovePositionsDistinct(move))
            {
                if (IsMoveAPieceCurPlayer(move, cpm, ref tileA))
                {
                    if (IsMoveBPieceCurPlayer(move, cpm, ref tileB))
                    {
                        outcome = false;
                        moveType = EChessMoveTypes.Castle;
                    }
                    else if (IsMoveBPieceOtherPlayer(move, cpm, ref tileB))
                    {
                        outcome = IsCaptureLegal(move, cpm);
                        moveType = EChessMoveTypes.Capture;
                    }
                    else if (IsMoveBEmpty(move, cpm, ref tileB))
                    {
                        if (outcome = IsPieceMovementLegal(move, cpm))
                        {
                            moveType = EChessMoveTypes.Movement;
                        }
                        else if (outcome = IsEnPassantCaptureLegal(move, cpm))
                        {
                            moveType = EChessMoveTypes.EpMovement;
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Tile A did not contain a player piece");
                    }
                }
            }
            else
            {
                System.Console.WriteLine("Tile A and B were not distinct");
            }
            if (outcome)
            {
                // a copy of the chess position is made so that the move may
                // be applied in order for the king-check to be checked without
                // causing the chess position to update the display
                ChessPosition cpmCopy = cpm.getEvaluateableChessPosition();
                cpmCopy.applyMove(move, moveType);
                outcome = IsKingInCheck(cpmCopy, ref kingCheckedBy);
            }
            return outcome;
        }


        /* Take a move object and return true if the A and B
        positions are distinct from each other. */
        private bool IsMovePositionsDistinct(FormedMove move)
        {
            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;
            return !posB.Equals(posA);
        }


        /* Takes a move object and a chess board object and returns true if
        the piece on the A position of the move belongs to the current player. */
        private bool IsMoveAPieceCurPlayer(FormedMove move, ChessPosition cpm, ref TileStruct tileA)
        {
            bool result = false;
            tileA = cpm.Board[move.PosA.Item1, move.PosA.Item2];
            if (!tileA.IsEmpty())
                if (cpm.Player.Owns(tileA.piece))
                    result = true;
            return result;
        }


        /* Takes a move object and a chess board object and returns true if
        the piece on the B position of the move belongs to the current player. 

        TODO: merge into previous*/
        private bool IsMoveBPieceCurPlayer(FormedMove move, ChessPosition cpm, ref TileStruct tileB)
        {
            bool result = false;
            tileB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            if (!tileB.IsEmpty())
                if (cpm.Player.Owns(tileB.piece))
                    result = true;
            return result;
        }


        /* Takes a move object and a chess board object and returns true if
        the piece on the B position of the move belongs to the other player.
        
        TODO: replace with alternate call to previous functions*/
        private bool IsMoveBPieceOtherPlayer(FormedMove move, ChessPosition cpm, ref TileStruct tileB)
        {
            bool result = false;
            tileB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            if (!tileB.IsEmpty())
                if (!cpm.Player.Owns(tileB.piece))
                    result = true;
            return result;
        }


        /* Takes a move object and a chess board object and returns true if
        the tile on the B position of the move is empty. */
        private bool IsMoveBEmpty(FormedMove move, ChessPosition cpm, ref TileStruct tileB)
        {
            tileB = cpm.Board[move.PosB.Item1, move.PosB.Item2];
            return tileB.IsEmpty();
        }


        /* Takes a move which so far meets the requirements for a Movement type
        move on the chess board. This function checks that one of the moving piece's 
        rays covers this move object (The piece itself can move in this direction/way)
        and that the way is not blocked by another piece. */
        private bool IsPieceMovementLegal(FormedMove move, ChessPosition cpm)
        {
            bool isLegalMovement = true;
            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;
            TileStruct tileA = cpm.Board[posA.Item1, posA.Item2];
            List<List<Tuple<int, int>>> movementRays;
            // get the rays for a piece of type=piece.Val and location=posA
            movementRays = GetPieceRay(tileA.piece.Val, posA);
            List<Tuple<int, int>> moveRayUsed = null;
            foreach (List<Tuple<int, int>> ray in movementRays)
            {
                if (ray.Contains(posB))
                {
                    moveRayUsed = ray;
                    break;
                } 
            }
            if (moveRayUsed != null)
                // check there are no intermediate Pieces blocking the movement
                foreach (Tuple<int, int> tilePos in moveRayUsed)
                {
                    TileStruct tileAtPosition = cpm.Board[tilePos.Item1, tilePos.Item2];
                    if (!tileAtPosition.IsEmpty())
                    {
                        isLegalMovement = false;
                        break;
                    }
                    if (tilePos.Equals(posB))
                        break;
                }
            else
                isLegalMovement = false;
        
            return isLegalMovement;
        }


        /* Takes a move which so far meets the requirements for an EnPassant type
        move on the chess board. This function checks that one of the moving pawn's 
        diagonal rays covers this move object (The piece itself can move in this direction/way)
        and that the corresponding enpassant capture tile for this movement contains
        an enemy pawn piece to capture.*/
        private bool IsEnPassantCaptureLegal(FormedMove move, ChessPosition cpm)
        {
            bool isLegalEnPassantCapture = true;
            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;
            TileStruct tileA = cpm.Board[posA.Item1, posA.Item2];
            List<List<Tuple<int, int>>> epMovementRays;
            // get attack ray for the pawn on tileA
            // these are the movement rays in the case of EP
            epMovementRays = GetPieceRayPawnCapture(tileA.piece.Val, posA);
            List<Tuple<int, int>> moveRayUsed = null;
            foreach(List<Tuple<int, int>> ray in epMovementRays)
            {
                if (ray.Contains(posB))
                {
                    moveRayUsed = ray;
                    break;
                }
            }
            // so at this point:
            // have posA and posB and it is a valid enpassant-type
            // movement (diagonal to empty square)

            if (moveRayUsed != null)
            {
                // continue checking ep position
                int epX = posB.Item2;
                int epY = posA.Item1;
                Tuple<int, int> posEP = Tuple.Create(epY, epX);
                // if this computed ep square between posA and posB does not 
                // match the currently valid EnPassantSq, then this is not
                // a valid EP capture
                if (!posEP.Equals(cpm.EnPassantSq))
                    isLegalEnPassantCapture = false;

            }
            else
                isLegalEnPassantCapture = false;

            return isLegalEnPassantCapture;
        }


        /* Takes a move which so far meets the requirements for a Capture type
        move on the chess board. This function checks that the one of the capturer
        piece's rays covers this move object (The piece itself can move in this way)
        and that the way is not blocked by another piece en route.
        
        Note: This is very similar to the IsPieceMovementLegal function, only 
        difference is that the check for pieces on a tile is moved after the 
        check for the final tile (since the final tile will have a piece on it:
        the piece being captured, and dont want the code to consider that as an
        intermediate piece which would block the capture. 
        
        Other difference is that if the capturer piece is of type pawn, a unique
        set of pawn capture rays are gotten (Pawn only piece to use different rays
        for capture and movement). */
        private bool IsCaptureLegal(FormedMove move, ChessPosition cpm)
        {
            bool isLegalCapture = true;

            Tuple<int, int> posA = move.PosA;
            Tuple<int, int> posB = move.PosB;

            TileStruct tileA = cpm.Board[posA.Item1, posA.Item2];
            List<List<Tuple<int, int>>> captureRays;

            if (tileA.piece.Val == EGamePieces.BlackPawn || tileA.piece.Val == EGamePieces.WhitePawn)
                captureRays = GetPieceRayPawnCapture(tileA.piece.Val, posA);
            else
                captureRays = GetPieceRay(tileA.piece.Val, posA);

            List<Tuple<int, int>> captureRayUsed = null;
            foreach (List<Tuple<int, int>> ray in captureRays)
            {
                if (ray.Contains(posB))
                {
                    captureRayUsed = ray;
                    break;
                }
            }
            if (captureRayUsed != null)
                // check there are no intermediate Pieces blocking the capture movement
                foreach (Tuple<int, int> position in captureRayUsed)
                {
                    if (position.Equals(posB))
                        break;
                    TileStruct tileAtPosition = cpm.Board[position.Item1, position.Item2];
                    if (!tileAtPosition.IsEmpty())
                    {
                        isLegalCapture = false;
                        break;
                    } 
                }
            else
                isLegalCapture = false;

            return isLegalCapture;
        }


        /* This function is passed a copy of the chess game. It uses the information contained in this object
        to determine whether or not the Current player's king is being threatened by check from the other player
        returning true if so. If it is being checked by the opposing player a list of the one / two coords containing the 
        attacking piece's is stored in 'kingCheckedBy */
        public bool IsKingInCheck(ChessPosition cpmCopy, ref List<Tuple<int, int>> kingCheckedBy)
        {
            // find the king
            Player curPlayer = cpmCopy.Player;
            Tuple<int, int> curPlayersKingPos;
            for (int row = 0; row < cpmCopy.Dim; row ++)
            {
                for (int col = 0; col < cpmCopy.Dim; col ++)
                {
                    TileStruct tile = cpmCopy.Board[row, col];
                    if (!tile.IsEmpty())
                    {
                        if ((curPlayer.Owns(tile.piece)) && 
                            (tile.piece.Val == EGamePieces.WhiteKing || tile.piece.Val == EGamePieces.BlackKing))
                        {
                            curPlayersKingPos = Tuple.Create(row, col);
                            break;
                        }
                    }
                }
            }

            // look along all possible attack vectors for isUpper if 'b' or usLower if 'w' using kingpos as origin
            // bishop vectors : break when find one of queen, bishop
            // rook vectors : queen, rook
            // knight vectors : knight
            // king vectors : king
            // pawn vectors : pawn, pawn enpassant

           // this will match the pre compute ray array forall computation
            // want to get a series of rays for each of the above pieces on the king pos
            // eg bishop (4 rays), then foreach ray: starting from the ray origin: if find queen, bishop, pawn(?) of opponent piece
            //    in this, the bishop ray, then break as this piece threatens the king on origin
            return true;
            
        }


        /* Takes a piece and a y,x board location and returns one List containing a List for each valid 
        direction of movement. Each of these lists contains a sequence of board locations that the piece 
        could potentially move to (from that starting location) */
        public List<List<Tuple<int, int>>> GetPieceRay(EGamePieces piece, Tuple<int, int> location)
        {
            if (rayArray == null)
                throw new Exception("ray array not instantiated");
            return rayArray[(int)piece][location.Item1, location.Item2];
        }


        /* Same as GetPieceRay but takes a pawn piece and returns the diagonal attack rays rather than the vertical
        pawn movement ray.*/
        public List<List<Tuple<int,int>>> GetPieceRayPawnCapture(EGamePieces piece, Tuple<int, int> location)
        {
            if (rayArrayPawnCapture == null)
                throw new Exception("ray array (pawns) noy instantiated");
            int pieceIndex = (piece == EGamePieces.WhitePawn) ? 0 : 1;
            return rayArrayPawnCapture[pieceIndex][location.Item1, location.Item2];
        }



        /* Generates the rays for use by the GetPieceRay procedure. This method is called at the start of
        the program to eliminate the time cost of computing these rays on a as-i-need-it while the game
        is running.*/
        public void GenerateRays()
        {
            rayArray = new List<List<Tuple<int,int>>>[UNIQUE_PIECE_NUM][,];
            // 12 pieces
            //      8 rows per piece
            //              8 columns per row
            //                      1 List of
            //                              n Lists
            //                                  n Tuple<int, int>

            for (int i = 0; i < rayArray.Length; i++)
            {
                // foreach of the i piece types, create 64 tiles each containing a number of rays (n directions)
                MovementStyle style = MovementStyles.getMovementStyle((EGamePieces)i);
                rayArray[i] = new List<List<Tuple<int,int>>>[8, 8];
                for (int j = 0; j < 8; j++) // row
                {
                    for (int k = 0; k < 8; k++) // col
                    {
                        List<List<Tuple<int, int>>> rays = new List<List<Tuple<int, int>>>();
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


        /* Generates the rays for use by the GetPieceRayPawnCapture procedure. This method is called at the start of
        the program to eliminate the time cost of computing these rays on a as-i-need-it while the game
        is running.*/
        public void GeneratePawnRays()
        {
            // 2 unique pawn pieces
            rayArrayPawnCapture = new List<List<Tuple<int, int>>>[2][,];
            CaptureStyle WhitePawnCaptureStyle = MovementStyles.getCaptureStyle(EGamePieces.WhitePawn);
            CaptureStyle BlackPawnCaptureStyle = MovementStyles.getCaptureStyle(EGamePieces.BlackPawn);

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
    }
}
