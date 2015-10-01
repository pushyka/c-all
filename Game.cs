using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    class Game
    {
        public Game() { }

        public void start()
        {
            Chess c = new Chess();
            c.populate();

            c.Player = 'w';
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
            Evaluator e;
            c = new Chess();
            e = new Evaluator();
            c.populate();
            c.Player = 'b';
            while (true)
            {
                c.display();
                string input = Console.ReadLine();
                FormedMove move = e.formatMove(input);
                System.Console.WriteLine("The input created a move: {0}, with coords: {1}", move.IsCompletelyFormed, move.ToString());
                if (move.IsCompletelyFormed)
                {
                    bool result = e.evaluateMove(move, c.Board, c.Player);
                    System.Console.WriteLine("So far move is valid {0}", result);
                }
                // eg if completely formed, evaluate it
                // if isValidNow, apply it
            }

        }

    }
}
