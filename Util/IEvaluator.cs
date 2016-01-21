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
        /* Takes an input string from a view / source and returns true if it is
        in the correct chess board move format e.g. 'A5 A6'. The corresponding
        move object is also created and passed to the non-local move variable. */
        bool ValidateInput(string input, ref FormedMove move);


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
        bool IsValidMove(FormedMove move, ChessPositionModel cpm, ref EChessMoveTypes moveType, ref List<Tuple<int, int>> kingCheckedBy);


        /* This function is passed a copy of the chess game. It uses the information contained in this object
        to determine whether or not the Current player's king is being threatened by check from the other player
        returning true if so. If it is being checked by the opposing player a list of the one / two coords containing the 
        attacking piece's is stored in 'kingCheckedBy */
        bool IsKingInCheck(ChessPosition cpm, ref List<Tuple<int, int>> attackerPositions);


        /* Generates the rays for use by the GetPieceRay procedure. This method is called at the start of
        the program to eliminate the time cost of computing these rays on a as-i-need-it while the game
        is running.*/
        void GenerateRays();


        /* Generates the rays for use by the GetPieceRayPawnCapture procedure. This method is called at 
        the start of the program to eliminate the time cost of computing these rays on a as-i-need-it 
        while the game is running.*/
        void GeneratePawnRays();
    }
}
