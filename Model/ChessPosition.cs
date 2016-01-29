using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    /* Base chess position representing a snapshot of a turn. Such an object can be used
    for move search comparisons and position evaluation. Along with positions of pieces 
    it holds information about the current en passant square if there is one, halfmove
    counter, castling rights. */
    public class ChessPosition
    {
        public int Size { get; set; }
        public Tile[, ] Board { get; set; }
        public Player Player { get; set; }
        protected Dictionary<char, bool> castle;
        public Tuple<int, int> EnPassantSq { get; set; }
        protected int halfmoveClock;
        protected List<EGamePieces> piecesCapd;


        /* This is the default base constructor overridden by the ChessPositionModel 
        constructor ~ I think. */
        protected ChessPosition() { } 


        /* Used when a new chess position is created. The property copies are made
        and then this constructor is called with the copies. */
        public ChessPosition(int size,
                             Tile[,] board,
                             Player player,
                             Dictionary<char, bool> castle,
                             Tuple<int, int> enPassantSq,
                             int halfmoveClock,
                             List<EGamePieces> piecesCapd)
        {
            this.Size = size;
            this.Board = board;
            this.Player = player;
            this.castle = castle;
            this.EnPassantSq = enPassantSq;
            this.halfmoveClock = halfmoveClock;
            this.piecesCapd = piecesCapd;
        }

        
        /* The Entry point for a move application, this method selects the 
        appropriate type of move to make based on the moveType property. */
        public void applyMove(FormedMove move)
        {
            switch (move.MoveType)
            {
                case EChessMoveTypes.Castle:
                    applyCastle(move);
                    break;
                case EChessMoveTypes.Capture:
                    applyCapture(move);
                    break;
                case EChessMoveTypes.Movement:
                    applyMovement(move);
                    break;
                case EChessMoveTypes.EpMovement:
                    applyEnPassantCapture(move);
                    break;
                default:
                    throw new ArgumentException($"Unexpected value: {move.MoveType}, for moveType");
            }
        }


        /* Apply a movement type move to the board. This move has already been validated by
        the evaluator and found to be legal.
        There are additional actions to be made if the moving piece is a pawn and fits certain
        criteria. */
        private void applyMovement(FormedMove move)
        {
            Tuple<int, int> frSqPos = move.PosA;
            Tuple<int, int> toSqPos = move.PosB;

            Tile frSqTl = getTile(frSqPos);
            Tile toSqTl = getTile(toSqPos);

            Piece mvPiece = frSqTl.piece;
            
            // if moving piece is a pawn do additional actions //
            if (mvPiece.Val == EGamePieces.WhitePawn ||
                mvPiece.Val == EGamePieces.BlackPawn)
            {
                // if it has moved two squares on its first move it can be captured En Passant
                if (!mvPiece.MovedOnce && mvTwoTiles(frSqPos, toSqPos))
                    this.EnPassantSq = toSqPos;

                // if there exists a non-empty value in PromotionSelection
                // this means the moving piece is a pawn being promoted
                if (move.PromotionSelection != EGamePieces.empty)
                    mvPiece.Val = move.PromotionSelection;

                mvPiece.MovedOnce = true;
            }

            // move the piece
            updatePosWithPiece(toSqPos, mvPiece);
            updatePosWithPiece(frSqPos, null);
        }


        /* Apply a capture type move to the board. This capture has already been validated by
        the evaluator and found to be legal.
        There are additional actions to be made if the moving piece is a pawn and fits certain
        criteria. 
        The captured piece is also added to the piecesCapd List property of this ChessPosition.*/
        private void applyCapture(FormedMove move)
        {
            Tuple<int, int> frSqPos = move.PosA;
            Tuple<int, int> toSqPos = move.PosB;

            Tile toSqTl = getTile(toSqPos);
            Tile frSqTl = getTile(frSqPos);

            Piece mvPiece = frSqTl.piece;

            // if moving piece is a pawn do additional actions //
            if (mvPiece.Val == EGamePieces.WhitePawn ||
                mvPiece.Val == EGamePieces.BlackPawn)
            {
                // if there exists a non-empty value in PromotionSelection
                // this means the capturing piece is also pawn being promoted
                if (move.PromotionSelection != EGamePieces.empty)
                    mvPiece.Val = move.PromotionSelection;
                // in the cases where a pawns first move is a capture, need to
                // set this to prevent the 2 move option only applying to first 'movement'
                mvPiece.MovedOnce = true;
            }

            // add the captured to the list
            addToCaptured(toSqTl.piece.Val);

            // move the piece
            updatePosWithPiece(toSqPos, mvPiece);
            updatePosWithPiece(frSqPos, null);
        }


        /* Apply an En Passant type move to the board. This move has already been validated by
        the evaluator and found to be legal and there exists an opponent piece on the EnPassant
        Square which can be captured.
        As in a normal capture, the piece captured En Passant is also added to the piecesCapd 
        List property of this ChessPosition.*/
        private void applyEnPassantCapture(FormedMove move)
        {
            Tuple<int, int> frSqPos = move.PosA;
            Tuple<int, int> toSqPos = move.PosB;

            // Compute En Passant Square
            int epSqRank = frSqPos.Item1;
            int epSqFile = toSqPos.Item2;
            Tuple<int, int> epSqPos = Tuple.Create(epSqRank, epSqFile);

            Tile frSqTl = getTile(frSqPos);
            Tile toSqTl = getTile(toSqPos);
            Tile epSqTl = getTile(epSqPos);

            Piece mvPiece = frSqTl.piece;

            // move the pawn
            updatePosWithPiece(toSqPos, mvPiece);
            updatePosWithPiece(frSqPos, null);

            // add the captured to the list
            addToCaptured(epSqTl.piece.Val);
            updatePosWithPiece(epSqPos, null);
        }


        /* Apply a Castle type move to the board. This move has already been validated by
        the evaluator and found to be legal. */
        private void applyCastle(FormedMove move)
        {
            
        }


        /* Return true if distance between the two tiles at the given positions is 2.
        This is used in determining if a given pawn's position should be (added) marked
        as the EnPassant captureable position. */
        private bool mvTwoTiles(Tuple<int, int> frSqPos, Tuple<int, int> toSqPos)
        {
            /* A pawn is moving to toSqPos. This has already been established
            Don't need to check the colour of the pawn since by the rules of the game
            a black pawn can only move to dim-1 and a white pawn can only move to 0 ranks.
            (only moves forwards) so IF a pawn is mvoing on to toSqPos, only then is the function called,
            and can assume the color of the pawn is correct. */
            return Math.Abs(frSqPos.Item1 - toSqPos.Item1) == 2;
        }


        /* Clear the value from the EnPassant Square marker if it is owned by the current
        player. This method is called at the start of each turn. Therefore if a piece is 
        cleared by this method, it was a EnPassant captureable pawn which was not captured
        in the previous turn (eg by the other player). So the other player no longer has 
        this ability to capture it. */
        public void clearEnPassantPawns(Player player)
        {
            if (EnPassantSq != null)
            {
                Piece enPassantPiece = getTile(EnPassantSq).piece;
                if (enPassantPiece != null)
                {
                    if (player.Owns(getTile(EnPassantSq).piece))
                        EnPassantSq = null;
                }
                // else piece is null which must mean it has been captured
                else
                    EnPassantSq = null;
            }
        }

        
        /* Given a Tuple(int,int) position, return a reference
        to the Tile object located at that at that position on the board. */
        private Tile getTile(Tuple<int, int> position)
        {
           return this.Board[position.Item1, position.Item2];
        }


        /* Takes a location and a piece value and updates the tile at that location with
        this new piece value. 
        This method is overridden by the ChessPositionModel which adds a BoardChanged event. */
        protected virtual void updatePosWithPiece(Tuple<int, int> location, Piece newPiece)
        {
            int row = location.Item1;
            int col = location.Item2;
            Board[row, col].piece = newPiece;
        }


        /* Takes a piece value and adds it to the PiecesCapd List.
        This method is overridden by the ChessPositionModel which adds a CapturedChanged event.*/
        protected virtual void addToCaptured(EGamePieces piece)
        {
            this.piecesCapd.Add(piece);
        }


        /* Returns a ChessPosition object which is a deep copy of this one. Since this method is
        also inherited by the ChessPositionModel, if the CPM produces this object it will be a
        deep copy of the ChessPositionModel object as if it were a ChessPosition. (CPM minus handlers etc).
        This function will be used during evaluation in the king check phase, and will also be used during
        the move search of the ai function. */
        public ChessPosition getChessPositionCopy()
        {
            int dimCopy = Size;
            // deep copy the board manually //
            Tile[,] boardCopy = new Tile[dimCopy, dimCopy];
            for (int j = 0; j < dimCopy; j ++)
            {
                for (int k = 0; k < dimCopy; k ++)
                {
                    Tile tileCopy = new Tile();
                    if (!Board[j,k].IsEmpty())
                    {
                        Piece piece = Board[j, k].piece;
                        EGamePieces val = piece.Val;
                        bool mv = piece.MovedOnce;
                        Piece pieceCopy = new Piece(val);
                        pieceCopy.MovedOnce = mv;
                        tileCopy.piece = pieceCopy;
                    }
                    boardCopy[j, k] = tileCopy;
                }
            }
            // copy rest of ChessPosition properties
            Player playerCopy = this.Player;
            Dictionary<char, bool> castleCopy = new Dictionary<char, bool>(castle);
            Tuple<int, int> enPassantSqCopy = (EnPassantSq == null) ? null : Tuple.Create(EnPassantSq.Item1, EnPassantSq.Item2);
            int halfmoveClockCopy = halfmoveClock;
            List<EGamePieces> piecesCapdCopy = new List<EGamePieces>(piecesCapd);
            // create and return the copy
            ChessPosition cpCopy = new ChessPosition(dimCopy, boardCopy, playerCopy, castleCopy, enPassantSqCopy, halfmoveClockCopy, piecesCapdCopy);
            return cpCopy;
        }
    }
}
