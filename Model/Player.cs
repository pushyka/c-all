using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class Player
    {
        private string playerValue;
        
        public Player(string player)
        {
            this.playerValue = player;
        } 

        public void change()
        {
            this.playerValue = (playerValue == "white") ? "black" : "white";
        }


        /* For chess the white player owns all of the GamePieces 0-5,
           The black player owns all of the GamePieces 6-11
           For TTT the white player owns 13(X)
           The Black player owns 14(O) 
           -probably better way to do this than enums for many games (a class for each piece would be better
           but so much refactoring
           -todo: tidy up a little with -/+ for w/b */
        public bool Owns(Piece piece)
        {
            bool result;
            result = (this.playerValue == "white" && 
                     ((int)piece.Val < 6 || (int)piece.Val == 13))
                    ||
                     (this.playerValue == "black" &&
                     ((int)piece.Val >= 6 && (int)piece.Val < 12) || (int)piece.Val == 14);



            return result;
        }

        public void set(string player)
        {
            this.playerValue = player;
        }

        public string CurPlayer
        {
            get
            {
                return this.playerValue;
            }
        }
    }
}
