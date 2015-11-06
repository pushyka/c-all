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
        protected Tile[, ] board;
        protected char player;
        protected Dictionary<char, bool> castle;
        protected Tuple<int, int> enPassantSq;
        protected int halfmoveClock;
        protected List<char> piecesCapd;//x2 

        // used when creating a chess position model
        protected ChessPosition() { } 

        // used when creating a chess position
        public ChessPosition(int dim,
                             Tile[,] board,
                             char player,
                             Dictionary<char, bool> castle,
                             Tuple<int, int> enPassantSq,
                             int halfmoveClock)
        {
            this.dim = dim;
            this.board = board;
            this.player = player;
            this.castle = castle;
            this.enPassantSq = enPassantSq;
            this.halfmoveClock = halfmoveClock;
        }



        /// <summary>
        /// The Entry point for move application, this method
        /// selects the appropriate type of move to make based on moveType
        /// </summary>
        /// <param name="move"></param>
        /// <param name="moveType"></param>
        public void applyMove(FormedMove move, string moveType)
        {
            switch (moveType)
            {
                case "castle":
                    applyCastle(move);
                    break;
                case "capture":
                    applyCapture(move);
                    break;
                case "movement":
                    applyMovement(move);
                    break;
                case "enpassantcapture":
                    applyEnPassantCapture(move);
                    break;
                default:
                    System.Console.WriteLine("ERROR: No moveType set"); // should never reach 
                    break;
            }

            // fire the event (board changed)
        }

        /// <summary>
        /// Applies a formed move to the current state of the board.
        /// The formed move has already been validated so this function 
        /// simply performs the MOVE operation on the game board. 
        /// </summary>
        /// <param name="input"></param>
        private void applyMovement(FormedMove move)
        {
            Tuple<int, int> frSqPos = move.PosA;
            Tuple<int, int> toSqPos = move.PosB;

            Tile frSqTl = getTile(frSqPos);
            Tile toSqTl = getTile(toSqPos);

            //  if moving piece is a pawn and its its first move
            if ((frSqTl.pID == 'p' || frSqTl.pID == 'P') &&
                (!frSqTl.movedOnce))
            {
                //if it moved 2 tiles (abs difference between fromsqloc.y and tsqloc.y)
                if (Math.Abs((frSqPos.Item1 - toSqPos.Item1)) == 2)
                {
                    this.enPassantSq = toSqPos;
                }
                // the pawn has been moved atleast once so
                frSqTl.movedOnce = true;
            }
            // if moving piece is a pawn check if its promoted
            if (frSqTl.pID == 'p' || frSqTl.pID == 'P')
                if (checkPromotion(frSqTl, toSqPos))
                {
                    // then prompt player for piece to change pawn to
                    // atm just do queen
                    // update it to the new selection
                    char q = (char.IsUpper(frSqTl.pID)) ? 'Q' : 'q';
                    frSqTl = new Tile(q);
                }


            // move the piece to the to square (copy)
            changeTile(toSqPos, frSqTl);
            // empty the from square
            Tile emSqTl = new Tile('e');
            changeTile(frSqPos, emSqTl);
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
            Tile toSqTl = getTile(toSqPos);
            Tile frSqTl = getTile(frSqPos);

            // if moving piece is a pawn check if its promoted
            if (frSqTl.pID == 'p' || frSqTl.pID == 'P')
                if (checkPromotion(frSqTl, toSqPos))
                {
                    // then prompt player for piece to change pawn to
                    // atm just do queen
                    // update it to the new selection
                    char q = (char.IsUpper(frSqTl.pID)) ? 'Q' : 'q';
                    frSqTl = new Tile(q);
                }
            // move the piece to the to square (copy)
            changeTile(toSqPos, frSqTl);
            // empty the from square
            Tile emptySquareVal = new Tile('e');
            changeTile(frSqPos, emptySquareVal);

            // add the captured to the list
            addToCaptured(toSqTl.pID);
        }

        private void applyEnPassantCapture(FormedMove move)
        {

            // then this is an en passant capture
            // add the passant square to the captured list
            Tuple<int, int> fromSquareLoc = move.PosA;
            Tuple<int, int> toSquareLoc = move.PosB;

            Tile fromSquareVal = getTile(fromSquareLoc);
            Tile toSquareVal = getTile(toSquareLoc);

            // move the pawn to the to square (copy)
            changeTile(toSquareLoc, fromSquareVal);
            // empty the from square
            Tile emptySquareVal = new Tile('e');
            changeTile(fromSquareLoc, emptySquareVal);
            // clear the passant square -> empty
            Tuple<int, int> passantSquareLoc;
            int passantSquareLocRank = fromSquareLoc.Item1;
            int passantSquareLocFile = toSquareLoc.Item2;
            passantSquareLoc = Tuple.Create(passantSquareLocRank, passantSquareLocFile);
            Tile passantSquareVal = getTile(passantSquareLoc);
            changeTile(passantSquareLoc, emptySquareVal);
            // add the passant square to the captured list
            addToCaptured(passantSquareVal.pID);
            System.Console.WriteLine("en passant move registered");

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

        private bool checkPromotion(Tile frSqTl, Tuple<int, int> toSqPos)
        {
            bool isP = false;
            if ((frSqTl.pID == 'p' && toSqPos.Item1 == 0) ||
                (frSqTl.pID == 'P' && toSqPos.Item1 == dim - 1))
            {
                isP = true;
                System.Console.WriteLine("PROMOTION TOOK PLACE");
            }
                


            return isP;
        }


        public void clearEnPassantPawns(char player)
        {
            //System.Console.WriteLine(" www {0}", board.);
            
            if (enPassantSq != null)
            {
                if ((char.IsUpper(getTile(enPassantSq).pID) && player == 'b') ||
                    (char.IsLower(getTile(enPassantSq).pID) && player == 'w'))
                    enPassantSq = null;
            }

        }






        /// <summary>
        /// Given a Tuple(int,int) position, return a reference
        /// to the Tile object located at that at that position on the board.
        /// </summary>
        private Tile getTile(Tuple<int, int> position)
        {
           return this.board[position.Item1, position.Item2];
        }





        /// <summary>
        /// This is overridden by the CPM since the CPM also needs to raise events
        /// to be handled by the display.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="newValues"></param>
        protected virtual void changeTile(Tuple<int, int> location, Tile newValues)
        {
            int row = location.Item1;
            int col = location.Item2;
            board[row, col] = newValues;
        }
        /// <summary>
        /// same reasoning as above function
        /// </summary>
        /// <param name="piece"></param>
        protected virtual void addToCaptured(char piece)
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
            int dimCopy = dim;
            // copy piece placement
            Tile[,] boardCopy = (Tile[,]) board.Clone();
            // side to move from player
            char playerCopy = player;
            // castling status
            Dictionary<char, bool> castleCopy = new Dictionary<char, bool>(castle);
            // cur en passant sq
            Tuple<int, int> enPassantSqCopy = (enPassantSq == null) ? null : Tuple.Create(enPassantSq.Item1, enPassantSq.Item2);
            // halfmove clock
            int halfmoveClockCopy = halfmoveClock;
            ChessPosition cpos = new ChessPosition(dimCopy,
                                              boardCopy,
                                              playerCopy,
                                              castleCopy,
                                              enPassantSqCopy,
                                              halfmoveClockCopy);
            return cpos;
        }


        public virtual char Player
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
        public Tile[,] Board
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
