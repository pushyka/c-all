using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_minimal
{
    class Game
    {
        public Game() { }

        public void start()
        {
            Chess c = new Chess();
            c.populate();

            c.Player = "w";
            c.IsGame = true;

            while (c.IsGame)
            {
                c.display();
                string input = Console.ReadLine(); // prompt input for player 
                // getdeepcopy of c.board
                // use the deep copy in the evaluator, then discard
                // c.doMove on the original

                c.doMove(input); // either applies the move to the game and returns true, or fails and returns false (with no game change)
                c.togglePlayer();

            }
        }

        public void test()
        {
            Chess c;
            c = new Chess();
            c.populate();
            while (true)
            {
                c.display();
                Console.ReadLine();
            }

        }

    }
}
