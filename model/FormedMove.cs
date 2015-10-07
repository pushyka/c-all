using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess.Model
{
    /// <summary>
    /// A move object to hold the 2 coord specifications for a move.
    /// FormedMove moves are only created by the Evaluator.formatMove
    /// and are thus garunteed to be bound by the board 
    /// </summary>
    public class FormedMove
    {
        private Tuple<int, int> posA;
        private Tuple<int, int> posB;
        private bool isCompletelyFormed;
        private bool isValidNow;

        public FormedMove()
        {
            this.posA = null;
            this.posB = null;
            this.isCompletelyFormed = false;
            this.isValidNow = false;
        }

        /// <summary>
        /// Takes a tuple t (representing a position on the board)
        /// And adds it to this move object 
        /// If the move object does not yet contain any position, 
        /// then the position t is the first position a so add it to posA
        /// etc
        /// </summary>
        /// <param name="t"></param>
        public void Add(Tuple<int, int> t)
        {
            // if posA is null, then assign the first coord position to it
            if (posA == null)
            {
                this.posA = t;
            }
            else
            // posA assigned, so assign 2nd position to posB
            // Note: the Evaluator.formatMove method will only send TWO positions to this class
            {
                this.posB = t;
                // when the second is added, then this move is completly formed
                this.isCompletelyFormed = true;
            }
        }

        public bool IsCompletelyFormed
        {
            get
            {
                return this.isCompletelyFormed;
            }
        }

        public bool IsValidNow
        {
            get
            {
                return this.isValidNow;
            }
        }

        public override string ToString()
        {
            if (this.isCompletelyFormed)
            {
                return this.posA.ToString() + this.posB.ToString();
            }
            else
            {
                return "no move created";
            }
            
        }

        public Tuple<int,int> PosA
        {
            get
            {
                return this.posA;
            }
        }

        public Tuple<int,int> PosB
        {
            get
            {
                return this.posB;
            }
        }

    }
}
