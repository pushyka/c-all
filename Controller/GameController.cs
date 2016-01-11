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
using System.Diagnostics;

namespace chess.Controller
{
    

    public class GameController : INotifyPropertyChanged, IGameController // remove the propertychanged event
    {
        
        private string input;
        private string message;
        private GameControlState state;

        private IDisplayableModel gameModel;
        private Evaluator evaluator;
        private Thread t;

        public GameController()
        {
            input = null;
            message = null;
            gameModel = null;
            evaluator = null;
            t = null;
            state = GameControlState.PreInitial;
           
        }


        public void InitialiseModel(GameModels model)
        {
            switch(model)
            {
                case GameModels.Chess:
                    gameModel = new ChessPositionModel();
                    evaluator = new Evaluator();
                    evaluator.GenerateRays();
                    evaluator.GeneratePawnRays();
                    break;
                case GameModels.TicTacToe:
                    gameModel = new TTTPositionModel();
                    break;
            }
            state = GameControlState.Initial;


        }

        public void testStuff()
        {
            // preloaded array
            evaluator.GetPieceRay(GamePieces.BlackRook, Tuple.Create(3, 6));
        }

        public void UnInitialiseModel(GameModels model)
        {
            gameModel = null;
            evaluator = null;
            state = GameControlState.PreInitial;
        }
        
       
        public void PrepareModel()
        {
            gameModel.Setup();
            gameModel.SetPlayer();
            state = GameControlState.Ready;
            this.Message = "Game is setup";
            
        }

        public void Terminate()
        {
            //terminate t, if its running
            StopGameLoop();

            input = null;
            message = null;
            gameModel.Player = null;
            state = GameControlState.Initial;
            this.Message = "Game is terminated";
        }


        public void StopGameLoop()
        {
            if (t != null)
            {
                t.Abort();
                t = null;
            }
        }


        public void StartGameLoop(GameModels model)
        {
            switch(model)
            {
                case GameModels.Chess:
                    t = new Thread(ChessGameLoop);
                    break;
                case GameModels.TicTacToe:
                    t = new Thread(TTTGameLoop);
                    break;
            }
            if (state == GameControlState.Ready)
                t.Start();
                state = GameControlState.GameInProgress;
                this.Message = "Game has started";
        }

        private void ChessGameLoop()
        {
            // get the explicit type of model since evaluator only works with
            // specifically the chess position model types
            ChessPositionModel cpm = (ChessPositionModel)this.gameModel;
            string moveType;
            FormedMove move;
            List<Tuple<int, int>> kingCheckedBy = new List<Tuple<int, int>>();

            // this bool will be set false when the game ends
            while (state == GameControlState.GameInProgress)
            {
                // start of new turn, clear variables
                move = null;
                moveType = null;
                kingCheckedBy.Clear();


                // TODO
                // check if there is a potential move before evaluating the input FOR CURRENT PLAYER
                // , else IsGame = false
                // if not in check and no legal move : stalemate
                // if in check and no legal move to remove attack : checkmate

                if (evaluator.IsKingInCheck(cpm, ref kingCheckedBy))
                    this.Message = $"Player {cpm.Player.CurPlayer}'s king is in check";

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

                    else if (evaluator.ValidateInput(input, ref move))
                    {
                        // then move is non null

                        if (evaluator.ValidateMove(move, cpm, ref moveType, ref kingCheckedBy))
                        {
                            //this.Message = "move passed validation";
                            ;
                            cpm.applyMove(move, moveType);


                            // change display message here rather than whos turn
                            //System.Console.WriteLine("have applied move of type {0}", moveType);

                            // change the player
                            cpm.ChangePlayer();
                            // its the start of the player's turn so if he had any pawns that could have been captured
                            // en passant during hte oponents turn, they will now be unable to be captured en passant
                            //System.Console.WriteLine(" CLEARING PASSANTS");
                            cpm.clearEnPassantPawns(cpm.Player);
                            this.Message = "Player " + cpm.Player.CurPlayer + "'s turn";
                            this.Message = "passant sq if any is " + cpm.EnPassantSq;


                        }
                        else
                            System.Console.WriteLine("The move was not valid");
                        // anything in kingcheckedby?
                    }
                    else
                        System.Console.WriteLine("The input was not valid");

                    input = null;
                }
                //Thread.Sleep(1000);
            }

            this.Message = "The game has ended, loop thread detached";
        }


        private void TTTGameLoop()
        {
            
            TTTPositionModel tttpm = (TTTPositionModel)this.gameModel;
            FormedMove move;
            string winner = null;

            while (state == GameControlState.GameInProgress)
            {
                // check if a winner has been found 
                // isWinner = tttpm.IsWinningPosition(ref winner); // checks it for both players
                // if (isWinner)
                //      break;

                move = null;
                // check if display has provided a move
                if (input != null)
                {
                    // Do Stuff..
                    System.Console.WriteLine("process the input");
                    move = new FormedMove(input);
                    tttpm.applyMove(move, null);
                    tttpm.ChangePlayer();
                    this.Message = "Player " + tttpm.Player.CurPlayer + "'s turasdsadasdsdan";
                    input = null;
                }
                Thread.Sleep(1000);

            }

            this.Message = "The game has ended, loop thread detached";
            // if (winner != null)
            //      etc
        }



        private void conceded()
        {
            this.Message = "Player " + this.gameModel.Player.CurPlayer + " has conceded!";
            this.gameModel.Player = null;
        }

        
        /// <summary>
        /// Returns a reference to the current game model.
        /// </summary>
        /// <returns></returns>
        public IDisplayableModel Model
        {
            get
            {
                return this.gameModel;
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
