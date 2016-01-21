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
        
        public FormedMove()
        {
            this.posA = null;
            this.posB = null;
            this.isValid = false;
            this.PromotionSelection = EGamePieces.empty;
        }

        
        /* Constructor for creating a FormedMove which has only one position.
        This is the partial version used by the TicTacToe game. */
        public FormedMove(string mvSpecifier)
        {
            if (mvSpecifier.Length == 2)
            {
                int col = (int)Char.GetNumericValue(mvSpecifier[0]);
                int row = (int)Char.GetNumericValue(mvSpecifier[1]);
                posA = Tuple.Create<int, int>(row, col);
                isValid = true;
            }
        }


        /* Constructor for creating a FormedMove when the location specifiers are
        provided. Currently the evaluator 'builds' the move using the .Add method. */
        public FormedMove(string mvSpecifier, string mvSpecifier2)
        {
            // Todo
            PromotionSelection = EGamePieces.empty;
            isValid = true;
        }


        /* The currently used method for building a chess move. The evaluator computes the 
        positions for posA and posB and adds them sequentially to the empty FormedMove object.
        Once the posB position is assigned the move is marked as Valid. */
        public void Add(Tuple<int, int> t)
        {
            if (posA == null)
            {
                this.posA = t;
            }
            else
            {
                this.posB = t;
                this.isValid = true;
            }
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
