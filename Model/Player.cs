using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class Player
    {

        public EGamePlayers PlayerValue { get; set; }


        /* Create a Player object with a string indicating the player.
        TODO: change to enum*/
        public Player(EGamePlayers player)
        {
            this.PlayerValue = player;
        }


        /* If playerValue is not null, changes to the next player in the game. */
        public void change()
        {
            switch (PlayerValue)
            {
                case EGamePlayers.White:
                    PlayerValue = EGamePlayers.Black;
                    break;
                case EGamePlayers.Black:
                    PlayerValue = EGamePlayers.White;
                    break;
                case EGamePlayers.X:
                    PlayerValue = EGamePlayers.O;
                    break;
                case EGamePlayers.O:
                    PlayerValue = EGamePlayers.X;
                    break;
            }
        }


        /* For chess the white player owns all of the GamePieces 0-5,
        The black player owns all of the GamePieces 6-11. */
        public bool Owns(Piece piece)
        {
            bool result = false;
            if (this.PlayerValue == EGamePlayers.White)
                result = (int)piece.Val < 6;
            if (this.PlayerValue == EGamePlayers.Black)
                result = (int)piece.Val >= 6 && (int)piece.Val < 12;
            return result;
        }
    }
}
