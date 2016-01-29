using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess.Model
{
    /* Represents a generic chess move. Currently the TicTacToe game
    also uses this object for its moves in a partial way. */
    public class FormedMove
    {
        private Tuple<int, int> posA;
        private Tuple<int, int> posB;
        public bool isValid;
        public EGamePieces PromotionSelection { get; set; }
        public EChessMoveTypes MoveType { get; set; }
        
        public FormedMove()
        {
            this.posA = null;
            this.posB = null;
            this.PromotionSelection = EGamePieces.empty;
        }

        
        /* Constructor for creating a FormedMove which has only one position.
        This is the partial version used by the TicTacToe game. Needs to be refactored to
        next non string type. */
        public FormedMove(string mvSpecifier)
        {
            if (mvSpecifier.Length == 2)
            {
                int col = (int)Char.GetNumericValue(mvSpecifier[0]);
                int row = (int)Char.GetNumericValue(mvSpecifier[1]);
                posA = Tuple.Create<int, int>(row, col);
            }
        }


        /* Constructor for building a chess move. The evaluator computes the 
        positions for posA and posB and adds them to the empty FormedMove object. */
        public FormedMove(Tuple<int, int> posA, Tuple<int, int> posB)
        {
            PromotionSelection = EGamePieces.empty;
            MoveType = EChessMoveTypes.None;
            this.posA = posA;
            this.posB = posB;
        }


        /* The position A (fromPos) */
        public Tuple<int,int> PosA
        { 
            get
            {
                return this.posA;
            }
        }


        /* The position B (toPos) */
        public Tuple<int,int> PosB
        {
            get
            {
                return this.posB;
            }
        }
    }
}
