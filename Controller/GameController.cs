using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using chess.Model;
using chess.View;
using chess.Util;

namespace chess.Controller
{
    public class GameController
    {
        Chess c;
        Evaluator e;

        public GameController()
        {
            this.c = new Chess(); // model
            this.e = new Evaluator(); // utility


        }

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

                //c.doMove(input); // either applies the move to the game and returns true, or fails and returns false (with no game change)
                //c.togglePlayer();

            }
        }

        // following are test methodss
        public void setUp()
        {
            c.populate();
            c.Player = 'b';
        }

        public void recvInstructTEST()
        {
            c.applyMoveTEST();
        }


        public void test()
        {
            

            string moveType;
            FormedMove move;
           
            
            c.populate();
            c.Player = 'b';
            while (true)
            {
                //c.display();
                System.Console.Write("Player {0}, enter a move: ", c.Player);
                string input = Console.ReadLine();
                move = null;
                moveType = null;
                
                
                if (e.validateInput(input, ref move))
                {
                    // then move is non null
                    System.Console.WriteLine("The input created a move: {0}", move.ToString());
                    if(e.validateMove(move, c.Board, c.Player, ref moveType))
                    {
                        c.applyMove(move, moveType);
                        System.Console.WriteLine("have applied move of type {0}", moveType);
                        // apply the move
                    }
                    else
                    {
                        // move failed to validate
                        System.Console.WriteLine("The move was not valid");
                    }
                }
                else
                {
                    // input failed to validate
                    System.Console.WriteLine("The input was not valid");
                }

                // change player

            }

        }

        public Chess lookAtChessModel()
        {
            return c;
        }



    }
}
