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
        private bool isValid;
        
        public FormedMove()
        {
            this.posA = null;
            this.posB = null;
            this.isValid = false;
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
            if (posA == null)
                this.posA = t;
            else
            {
                this.posB = t;
                this.isValid = true;
            }
        }


        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
        }


        public override string ToString()
        {
            if (this.isValid)
            {
                return this.posA.ToString() + this.posB.ToString();
            }
            else
            {
                return "EMPTYMOVE";
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
