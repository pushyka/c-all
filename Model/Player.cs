using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.Model
{
    public class Player
    {
        private string curPlayer;
        
        public Player(string player)
        {
            this.curPlayer = player;
        } 

        public void change()
        {
            this.curPlayer = (curPlayer == "white") ? "black" : "white";
        }

        public bool has(Pieces piece)
        {
            bool result;
            result = (this.curPlayer == "white" && piece < 0) ||
                     (this.curPlayer == "black" && piece > 0);
            return result;
        }

        public string CurPlayer
        {
            get
            {
                return this.curPlayer;
            }
        }
    }
}
