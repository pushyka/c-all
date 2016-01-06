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

        // all of the game models are stored in here, the View can access this
        private IDisplayableModel[] models = new IDisplayableModel[2] { null, null };

        // this way the game models can be used for display by the View and
        // the game loops are free to use the models like they want.
        // e.g. the chessposition contains more information than the tttposition

            // TODO maybe change to a dictionary

        private ChessPositionModel cpm;
        private TTTPositionModel tttpm;


        private Evaluator evaluator;
        private GameControlState state;
        private Thread t;

        public GameController()
        {
            input = null;
            message = null;
            models[(int)GameModels.Chess] = null;
            models[(int)GameModels.TicTacToe] = null;
            evaluator = null;
            t = null;
            state = GameControlState.Initial;
        }


        public void InitialiseAllObjects()
        {
            models[(int)GameModels.Chess] = new ChessPositionModel();
            cpm = (ChessPositionModel)models[(int)GameModels.Chess];
            models[(int)GameModels.TicTacToe] = new TTTPositionModel();
            tttpm = (TTTPositionModel)models[(int)GameModels.TicTacToe];

            evaluator = new Evaluator();
            evaluator.GenerateRays();
            evaluator.GeneratePawnRays();
            System.Console.WriteLine("preload complete");


        }

        public void testStuff()
        {
            // preloaded array
            evaluator.GetPieceRay(GamePieces.BlackRook, Tuple.Create(3, 6));
        }

        public void UninitialiseObjects()
        {
            models[(int)GameModels.Chess] = null;
            cpm = null;
            models[(int)GameModels.TicTacToe] = null;
            tttpm = null;
            evaluator = null;
        }
        

       
        public void PrepareChessModel()
        {

            cpm.Setup();
            state = GameControlState.Game;
            this.Message = "Game is setup";
            
        }

        public void PrepareTTTModel()
        {

            tttpm.Setup();
            state = GameControlState.Game;
            this.Message = "Game is setup";

        }

        public void Terminate()
        {
            //terminate t, if its running
            StopGameLoop();

            input = null;
            message = null;
            cpm.Player = null;
            tttpm.Player = null;
            state = GameControlState.Initial;
            this.Message = "Game is terminated";
        }

        public void InitialSetupTTT()
        {
            state = GameControlState.Game;
            this.Message = "Game is setup";
        }


        public void StopGameLoop()
        {
            if (t != null)
            {
                t.Abort();
                t = null;
            }
        }


        public void StartChessGameLoop()
        {
            t = new Thread(ChessGameLoop);
            t.Start();
            this.Message = "Game has started";
        }

        private void ChessGameLoop()
        {
            string moveType;
            FormedMove move;
            List<Tuple<int, int>> kingCheckedBy = new List<Tuple<int, int>>();

            // this bool will be set false when the game ends
            while (state == GameControlState.Game)
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
                            cpm.Player.change();
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


        public void StartTTTGameLoop()
        {
            t = new Thread(TTTGameLoop);
            t.Start();
            this.Message = "Game has started";
        }

        private void TTTGameLoop()
        {
            
            while (state == GameControlState.Game)
            {
                
                
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
                    else
                        System.Console.WriteLine("The input was not valid");

                    input = null;
                }
                else
                {
                    // Do Stuff..
                }
                Thread.Sleep(1000);
            }

            this.Message = "The game has ended, loop thread detached";
        }



        private void conceded()
        {
            this.Message = "Player " + cpm.Player.CurPlayer + " has conceded!";
            cpm.Player = null;
        }

        

        public IDisplayableModel Model(GameModels model)
        {
            return this.models[(int)model];
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
