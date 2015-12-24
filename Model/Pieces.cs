using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public enum GamePieces
    {
        WhitePawn,
        WhiteRook,
        WhiteKnight,
        WhiteBishop,
        WhiteQueen,
        WhiteKing,
        BlackPawn,
        BlackRook,
        BlackKnight,
        BlackBishop,
        BlackQueen,
        BlackKing,
        empty,
        X,
        O
    }

    public enum GameControlState { Initial = 1, Game = 2, Load = 3, Settings = 4 }

    public enum GameModels { Chess, TicTacToe}
}
