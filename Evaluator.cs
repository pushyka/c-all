using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_minimal
{
    class Evaluator
    {

        /// <summary>
        /// In-depth move checker function determines whether a given move (pos a to pos b)
        /// is valid in the context of the rules/requirements. board is a cloned copy so that additions may be 
        /// made to it during the process of checking if the move is valid (after move is made does it result in check etc)
        /// At the end of the function, this board is discarded and a boolean result is returned 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool evaluateMove(Tuple<int,int> a, Tuple<int,int> b, Chess board)
        {
            // TODO
            return false;
        }

        /// <summary>
        /// Takes a position a, upon which a king is expected to be in the board object.
        /// Determines if the king at a is currently in check. This method called frequently.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool determineIfCheck(Tuple<int,int> a, Chess board)
        {
            // TODO, optimise
            return false;
        }

    }
}
