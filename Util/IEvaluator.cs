using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess.Model;

namespace chess.Util
{
    interface IEvaluator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        bool ValidateInput(string input, ref FormedMove move);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="move"></param>
        /// <param name="board"></param>
        /// <param name="cur_turn"></param>
        /// <param name="moveType"></param>
        /// <returns></returns>
        bool IsValidMove(FormedMove move, ChessPositionModel cpm, ref EChessMoveTypes moveType, ref List<Tuple<int, int>> kingCheckedBy);

        /// <summary>
        /// given a board context and a current player, return true / false if the players king is
        /// currently in check. If the king is in check, add the attacker's coords to the attackerPositions
        /// list. These positions are used in the legalmoveavailable to determine if the attack can be countered.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <param name="attackerPositions"></param>
        /// <returns></returns>
        bool IsKingInCheck(ChessPosition cpm, ref List<Tuple<int, int>> attackerPositions);

        void GenerateRays();
    }
}
