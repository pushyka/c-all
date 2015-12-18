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
            result = (this.curPlayer == "white" && (int)piece < 6) ||
                     (this.curPlayer == "black" && (int)piece >= 6 && (int)piece < 12);
            return result;
        }

        public void set(string player)
        {
            this.curPlayer = player;
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
