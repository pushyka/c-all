using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class ChessPosition
    {
        protected int dim;
        protected TileStruct[, ] board;
        protected Player player;
        protected Dictionary<char, bool> castle;
        protected Tuple<int, int> enPassantSq;
        protected int halfmoveClock;
        protected List<EGamePieces> piecesCapd;//x2 

        // used when creating a chess position model
        protected ChessPosition() { } 

        // used when creating a chess position
        public ChessPosition(int dim,
                             TileStruct[,] board,
                             Player player,
                             Dictionary<char, bool> castle,
                             Tuple<int, int> enPassantSq,
                             int halfmoveClock,
                             List<EGamePieces> piecesCapd)
        {
            this.dim = dim;
            this.board = board;
            this.player = player;
            this.castle = castle;
            this.enPassantSq = enPassantSq;
            this.halfmoveClock = halfmoveClock;
            this.piecesCapd = piecesCapd;
        }



        


        
        /* The Entry point for move application, this method
        selects the appropriate type of move to make based on moveType*/
        public void applyMove(FormedMove move, EChessMoveTypes moveType)
        {
            switch (moveType)
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
                    System.Console.WriteLine("ERROR: No moveType set"); // should never reach 
                    break;
            }

            // fire the event (board changed)
        }

        /* Apply a movement type move to the board. This move has already been validated by
        the evaluator and found to be legal. If the moving piece is a pawn, need to update the
        movedOnce value, determine if it can be captured enPassant next turn and determine if it
        requires promotion.*/
        private void applyMovement(FormedMove move)
        {
            Tuple<int, int> frSqPos = move.PosA;
            Tuple<int, int> toSqPos = move.PosB;

            TileStruct frSqTl = getTile(frSqPos);
            TileStruct toSqTl = getTile(toSqPos);

            Piece mvPiece = frSqTl.piece;

            #region MOVE PAWN STUFF TO END
            //  if moving piece is a pawn and its its first move
            if (mvPiece.Val == EGamePieces.WhitePawn ||
                mvPiece.Val == EGamePieces.BlackPawn)
            {
                // if it has moved two squares on its first move it can be captured en passant
                if (!mvPiece.MovedOnce && mvTwoTiles(frSqPos, toSqPos))
                {
                    this.enPassantSq = toSqPos;
                }
                mvPiece.MovedOnce = true;
                // if it has reached the opposite side rank it can be promoted
                if (toSqIsHighRank(toSqPos))
                {
                    Piece selectedUpgrPiece;
                    EGamePieces selectedUpgrVal;

                    View.PromotionSelection promotionSelection = new View.PromotionSelection(mvPiece);
                    if (promotionSelection.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        selectedUpgrVal = promotionSelection.SelectedPiece;
                        promotionSelection.Dispose();
                        selectedUpgrPiece = new Piece(selectedUpgrVal);
                        frSqTl = new TileStruct(selectedUpgrPiece);
                        
                    }

                }
            }

            #endregion

            // move the piece to the to square (copy)
            updateTileWithPiece(toSqPos, frSqTl.piece);
            // empty the from square
            updateTileWithPiece(frSqPos, null);
        }



        /// <summary>
        /// Performs the CAPTURE operation, (a move and
        /// replacement of a piece) also adds any captured 
        /// pieces to the capture list. Essentially same as movement.
        /// </summary>
        /// <param name="move"></param>
        private void applyCapture(FormedMove move)
        {
            Tuple<int, int> frSqPos = move.PosA;
            Tuple<int, int> toSqPos = move.PosB;
            // get the piece from the toSquare (which is being captured)
            TileStruct toSqTl = getTile(toSqPos);
            TileStruct frSqTl = getTile(frSqPos);

            // if moving piece is a pawn check if its promoted
            if (frSqTl.piece.Val == EGamePieces.WhitePawn || frSqTl.piece.Val == EGamePieces.BlackPawn)
                if (toSqIsHighRank(toSqPos))
                {
                    // then prompt player for piece to change pawn to
                    // atm just do queen
                    // update it to the new selection
                    EGamePieces q = ((int)frSqTl.piece.Val < 6) ? EGamePieces.WhiteQueen : EGamePieces.BlackQueen;
                    frSqTl = new TileStruct(new Piece(q));
                }
            // move the piece to the to square (copy)
            updateTileWithPiece(toSqPos, frSqTl.piece);
            // empty the from square
            updateTileWithPiece(frSqPos, null);

            // add the captured to the list
            addToCaptured(toSqTl.piece.Val);
        }

        private void applyEnPassantCapture(FormedMove move)
        {

            // then this is an en passant capture
            // add the passant square to the captured list
            Tuple<int, int> fromSquareLoc = move.PosA;
            Tuple<int, int> toSquareLoc = move.PosB;

            TileStruct fromSquareVal = getTile(fromSquareLoc);
            TileStruct toSquareVal = getTile(toSquareLoc);

            // move the pawn to the to square (copy)
            updateTileWithPiece(toSquareLoc, fromSquareVal.piece);
            // empty the from square
            updateTileWithPiece(fromSquareLoc, null);
            // clear the passant square -> empty
            Tuple<int, int> passantSquareLoc;
            int passantSquareLocRank = fromSquareLoc.Item1;
            int passantSquareLocFile = toSquareLoc.Item2;
            passantSquareLoc = Tuple.Create(passantSquareLocRank, passantSquareLocFile);
            TileStruct passantSquareVal = getTile(passantSquareLoc);

            updateTileWithPiece(passantSquareLoc, null);
            // add the passant square to the captured list
            addToCaptured(passantSquareVal.piece.Val); // this shouldnt be after the set of the val on loc to null...

        }


        /// <summary>
        /// Performs the CASTLE operation (a specific
        /// move type operation)
        /// As with the other applyMove functions, the move has
        /// already been validated as legal. For castling this
        /// includes check for check operation.
        /// </summary>
        /// <param name="move"></param>
        private void applyCastle(FormedMove move)
        {
            // todo
        }


        /* A pawn is moving to toSqPos. This has already been established
        Don't need to check the colour of the pawn since by the rules of the game
        a black pawn can only move to dim-1 and a white pawn can only move to 0 ranks.
        (only moves forwards) so IF a pawn is mvoing on to toSqPos, only then is the function called,
        and can assume the color of the pawn is correct. */
        private bool toSqIsHighRank(Tuple<int, int> toSqPos)
        {
            return ((toSqPos.Item1 == 0) || (toSqPos.Item1 == dim - 1));
        }

        private bool mvTwoTiles(Tuple<int, int> frSqPos, Tuple<int, int> toSqPos)
        {
            return Math.Abs(frSqPos.Item1 - toSqPos.Item1) == 2;
        }
            


        public void clearEnPassantPawns(Player player)
        {
            //System.Console.WriteLine(" www {0}", board.);
            
            if (enPassantSq != null)
            {
                if (player.Owns(getTile(enPassantSq).piece))
                    enPassantSq = null;
            }

        }






        /// <summary>
        /// Given a Tuple(int,int) position, return a reference
        /// to the Tile object located at that at that position on the board.
        /// </summary>
        private TileStruct getTile(Tuple<int, int> position)
        {
           return this.board[position.Item1, position.Item2];
        }





        /// <summary>
        /// This is overridden by the CPM since the CPM also needs to raise events
        /// to be handled by the display.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="newValues"></param>
        protected virtual void updateTileWithPiece(Tuple<int, int> location, Piece newPiece)
        {
            int row = location.Item1;
            int col = location.Item2;
            board[row, col].piece = newPiece;
        }
        /// <summary>
        /// same reasoning as above function
        /// </summary>
        /// <param name="piece"></param>
        protected virtual void addToCaptured(EGamePieces piece)
        {
            this.piecesCapd.Add(piece);
        }


        /// <summary>
        /// This method of the chess position object returns a clone of the chess position without the listeners and
        /// captured list etc, a version which can be modified during evaluation of positions (used in search & checkcheck after move) 
        /// independantly of the chess position model object used for final moves and updating display.
        /// This copy is required during evaluation (for check checking) and also during generation
        /// of positions as nodes in the search (parent node copied and modified by one move to create a child node)
        /// </summary>
        /// <returns></returns>
        public ChessPosition getEvaluateableChessPosition()
        {
            int dimCopy = dim; // fix

            // ============  Deep copy the board Manually =================
            TileStruct[,] boardCopy = new TileStruct[dimCopy, dimCopy];
            for (int j = 0; j < dimCopy; j ++)
            {
                for (int k = 0; k < dimCopy; k ++)
                {
                    // foreach tile create a new tile struct, copy the piece from the original, add it to the tilestrcut, add the tilestruct to the array
                    TileStruct tileCopy = new TileStruct();
                    // copy the piece into the tile if there is one
                    if (!board[j,k].IsEmpty())
                    {
                        Piece piece = board[j, k].piece;
                        EGamePieces val = piece.Val;
                        bool mv = piece.MovedOnce;
                        Piece pieceCopy = new Piece(val);
                        pieceCopy.MovedOnce = mv;
                        tileCopy.piece = pieceCopy;
                    }

                    boardCopy[j, k] = tileCopy;
                }
            }
            // done
            // =============================================================


            // side to move from player
            Player playerCopy = player;
            // castling status
            Dictionary<char, bool> castleCopy = new Dictionary<char, bool>(castle);
            // cur en passant sq
            Tuple<int, int> enPassantSqCopy = (enPassantSq == null) ? null : Tuple.Create(enPassantSq.Item1, enPassantSq.Item2);
            // halfmove clock
            int halfmoveClockCopy = halfmoveClock;
            //capdsofar
            List<EGamePieces> piecesCapdCopy = new List<EGamePieces>(piecesCapd);
            ChessPosition cpos = new ChessPosition(dimCopy,
                                              boardCopy,
                                              playerCopy,
                                              castleCopy,
                                              enPassantSqCopy,
                                              halfmoveClockCopy,
                                              piecesCapdCopy);
            return cpos;
        }


        public virtual Player Player
        {
            get
            {
                return this.player;
            }
            set
            {
                this.player = value;
            }
        }


        /// <summary>
        /// 
        /// Possibly don't need anymore since array of strings (base type) copied by value
        /// </summary>
        public TileStruct[,] Board
        {
            get
            {
                return this.board;
            }
        }

        public Tuple<int, int> EnPassantSq
        {
            get
            {
                return this.enPassantSq;
            }
            set
            {
                this.enPassantSq = value;
            }
        }

        public int Dim
        {
            get
            {
                return this.dim;
            }
            
        }

    }
}
