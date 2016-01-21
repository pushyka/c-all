using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class Player
    {

        public string PlayerValue { get; set; }


        /* Create a Player object with a string indicating the player.
        TODO: change to enum*/
        public Player(string player)
        {
            this.PlayerValue = player;
        }


        /* If playerValue is not null, changes to the next player in the game. */
        public void change()
        {
            if (PlayerValue != null)
            {
                switch (PlayerValue)
                {
                    case "white":
                        PlayerValue = "black";
                        break;
                    case "black":
                        PlayerValue = "white";
                        break;
                    case "X":
                        PlayerValue = "O";
                        break;
                    case "O":
                        PlayerValue = "X";
                        break;
                }
            }
        }


        /* For chess the white player owns all of the GamePieces 0-5,
        The black player owns all of the GamePieces 6-11. */
        public bool Owns(Piece piece)
        {
            bool result = false;
            if (this.PlayerValue == "white")
                result = (int)piece.Val < 6;
            if (this.PlayerValue == "black")
                result = (int)piece.Val >= 6 && (int)piece.Val < 12;
            return result;
        }
    }
}
