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

    public class GameController : INotifyPropertyChanged, IGameController // remove the propertychanged event
    {
        
        private string input;
        private string message;
        private ChessModel chessModel;
        private Evaluator evaluator;
        private GameControlState state;
        private Thread t;

        public GameController()
        {
            input = null;
            message = null;
            chessModel = null;
            evaluator = null;
            t = null;
            state = GameControlState.Initial;
        }


        public void initModelandEval()
        {
            chessModel = new ChessModel(); // model
            evaluator = new Evaluator(); // utility
        }

        public void uninitModeandEval()
        {
            chessModel = null;
            evaluator = null;
        }

        // following are test methodss
        public void setUp()
        {

            chessModel.populate();
            chessModel.Player = 'w';
            state = GameControlState.Game;
            this.Message = "Game is setup";
            
        }

        public void tearDown()
        {
            //terminate t, if its running
            stopGameLoop();

            input = null;
            message = null;
            chessModel.Player = '\0';

            
            state = GameControlState.Initial;
            this.Message = "Game is teared down";
        }

        public void stopGameLoop()
        {
            if (t != null)
            {
                t.Abort();
                t = null;
            }
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
            List<Tuple<int, int>> attackerPositions = new List<Tuple<int, int>>();

            // this bool will be set false when the game ends
            while (state == GameControlState.Game)
            {
                move = null;
                moveType = null;
                attackerPositions.Clear();

                this.Message = "Player " + chessModel.Player + "'s turn";
                // check if there is a potential move before evaluating the input FOR CURRENT PLAYER
                // , else IsGame = false
                // if not in check and no legal move : stalemate
                // if in check and no legal move to remove attack : checkmate
                if (evaluator.isKingInCheck(chessModel.Board, chessModel.Player, ref attackerPositions))
                    this.Message = $"Player {chessModel.Player}'s king is in check";

                // break statemetns to exit the loop
                // validateMove will also ensure the move does not leave the king in check


                // check if display has provided a move
                if (input != null)
                {
                    if (input == "concede")
                    {
                        // c.Player has conceded
                        conceded();
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
                            // change display message here rather than whos turn
                            System.Console.WriteLine("have applied move of type {0}", moveType);
                            // change the player
                            chessModel.Player = (chessModel.Player == 'b') ? 'w' : 'b';
                        }
                        else
                            System.Console.WriteLine("The move was not valid");
                    }
                    else
                        System.Console.WriteLine("The input was not valid");

                    input = null;
                }
                //Thread.Sleep(1000);
                //System.Console.WriteLine("I am still alive");
            }

            this.Message = "The game has ended, loop thread detached";
        }

        
        private void conceded()
        {
            this.Message = "Player " + chessModel.Player + " has conceded!";
            chessModel.Player = '\0';
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
