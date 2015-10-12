using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using chess.Model;
using chess.View;
using chess.Util;

namespace chess.Controller
{
    public class GameController
    {
        public string INPUT { get; set; } = null;
        // need a lock type value (prevent INPUT being changed again, during the validation phase)
        // eg input is only modified when a second value holds a certain value

        Chess c;
        Evaluator e;

        public GameController()
        {
            this.c = new Chess(); // model
            this.e = new Evaluator(); // utility


        }

        // following are test methodss
        public void setUp()
        {
            c.populate();
            c.Player = 'b';
            c.IsGame = true;
            
        }


        public void startGameLoop()
        {
            Thread t = new Thread(gameLoop);
            t.Start();
        }

        private void gameLoop()
        {
            string moveType;
            FormedMove move;

            // this bool will be set false when the game ends
            while (c.IsGame)
            {
                move = null;
                moveType = null;

                // check if there is a potential move, else IsGame = false

                // check if display has provided a move
                if (INPUT != null)
                {
                    if (e.validateInput(INPUT, ref move))
                    {
                        // then move is non null
                        System.Console.WriteLine("The input created a move: {0}", move.ToString());
                        if (e.validateMove(move, c.Board, c.Player, ref moveType))
                        {
                            c.applyMove(move, moveType);
                            System.Console.WriteLine("have applied move of type {0}", moveType);
                            // change the player
                            c.Player = (c.Player == 'b') ? 'w' : 'b';
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

                    INPUT = null;
                }

            }

            System.Console.WriteLine("The game has ended");
        }

























        public void recvInstructTEST()
        {
            c.applyMoveTEST();
        }

        public void recvInstruct(string move)
        {
            System.Console.WriteLine("Controller has recvd the move: {0}", move);
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
