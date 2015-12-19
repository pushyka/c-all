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

        public bool Owns(Piece piece)
        {
            bool result;
            result = (this.playerValue == "white" && (int)piece.Val < 6) ||
                     (this.playerValue == "black" && (int)piece.Val >= 6 && (int)piece.Val < 12);
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
