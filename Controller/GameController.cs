using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


using chess.Model;
using chess.View;
using chess.Util;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace chess.Controller
{
    public enum GameControlState { Initial = 1, Game = 2, Load = 3, Settings = 4}

    public class GameController : INotifyPropertyChanged
    {
        
        private string input = null;
        private string message = null;
        private ChessModel chessModel = null;
        private Evaluator evaluator = null;
        private GameControlState state;
        private Thread t;

        public GameController()
        {

            this.state = GameControlState.Initial;


        }

        // following are test methodss
        public void setUp()
        {
            chessModel = new ChessModel(); // model
            evaluator = new Evaluator(); // utility
            chessModel.populate();
            chessModel.Player = 'b';
            state = GameControlState.Game;
            this.Message = "Game is setup";
            
        }

        public void tearDown()
        {
            chessModel = null;
            evaluator = null;
            state = GameControlState.Initial;
            //terminate t, if its running
            t.Abort();
            this.Message = "Initial state";
        }


        public void startGameLoop()
        {
            t = new Thread(gameLoop);
            t.Start();
            this.Message = "Game has started";
        }

        private void gameLoop()
        {
            string moveType;
            FormedMove move;

            // this bool will be set false when the game ends
            while (state == GameControlState.Game)
            {
                move = null;
                moveType = null;
                this.Message = "Player " + chessModel.Player + "'s turn";
                // check if there is a potential move, else IsGame = false

                // check if display has provided a move
                if (input != null)
                {
                    if (input == "concede")
                    {
                        // c.Player has conceded
                        conceded(chessModel.Player);
                        input = null;
                        break;
                    }

                    else if (evaluator.validateInput(input, ref move))
                    {
                        // then move is non null
                        System.Console.WriteLine("The input created a move: {0}", move.ToString());
                        if (evaluator.validateMove(move, chessModel.Board, chessModel.Player, ref moveType))
                        {
                            chessModel.applyMove(move, moveType);
                            System.Console.WriteLine("have applied move of type {0}", moveType);
                            // change the player
                            chessModel.Player = (chessModel.Player == 'b') ? 'w' : 'b';
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

                    input = null;
                }
                Thread.Sleep(1000);
                System.Console.WriteLine("I am still alive");
            }

            this.Message = "The game has ended, loop thread detached";
        }













        private void conceded(char p)
        {
            this.Message = "Player " + p + " has conceded!";
        }











        public void recvInstructTEST()
        {
            //c.applyMoveTEST();
        }

        public void recvInstruct(string move)
        {
            System.Console.WriteLine("Controller has recvd the move: {0}", move);
        }


        public void test()
        {
            

            string moveType;
            FormedMove move;
           
            
            chessModel.populate();
            chessModel.Player = 'b';
            while (true)
            {
                //c.display();
                System.Console.Write("Player {0}, enter a move: ", chessModel.Player);
                string input = Console.ReadLine();
                move = null;
                moveType = null;
                
                
                if (evaluator.validateInput(input, ref move))
                {
                    // then move is non null
                    System.Console.WriteLine("The input created a move: {0}", move.ToString());
                    if(evaluator.validateMove(move, chessModel.Board, chessModel.Player, ref moveType))
                    {
                        chessModel.applyMove(move, moveType);
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

        public ChessModel ChessModel
        {
            get
            {
                return this.chessModel;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        // The CallerMemberName attribute makes propertyName of the caller to be the argument.
        // so Message is the arg for propertyName
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string Message
        {
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    NotifyPropertyChanged();
                }

            }
            get
            {
                return this.message;
            }
        }

        public string Input
        {
            get
            {
                return this.input;
            }
            set
            {
                if (this.input == null)
                    this.input = value;
                else
                    System.Console.WriteLine("A previous input hsant been cleared yet (currently being processed) so this Set has failed");
            }
        }

    }
}
