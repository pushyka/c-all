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
        private EGameControlState state;

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
            state = EGameControlState.PreInitial;
           
        }


        public void InitialiseModel(EGameModels model)
        {
            switch(model)
            {
                case EGameModels.Chess:
                    gameModel = new ChessPositionModel();
                    evaluator = new Evaluator();
                    evaluator.GenerateRays();
                    evaluator.GeneratePawnRays();
                    break;
                case EGameModels.TicTacToe:
                    gameModel = new TTTPositionModel();
                    break;
            }
            state = EGameControlState.Initial;


        }

        public void testStuff()
        {
            // preloaded array
            evaluator.GetPieceRay(EGamePieces.BlackRook, Tuple.Create(3, 6));
        }

        public void UnInitialiseModel(EGameModels model)
        {
            gameModel = null;
            evaluator = null;
            state = EGameControlState.PreInitial;
        }
        
       
        public void PrepareModel()
        {
            gameModel.Setup();
            gameModel.SetPlayer();
            state = EGameControlState.Ready;
            //this.Message = "Game is setup";
            
        }

        public void Terminate()
        {
            //terminate t, if its running
            StopGameLoop();

            input = null;
            message = null;
            gameModel.Player = null;
            state = EGameControlState.Initial;
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


        public void StartGameLoop(EGameModels model)
        {
            switch(model)
            {
                case EGameModels.Chess:
                    t = new Thread(ChessGameLoop);
                    break;
                case EGameModels.TicTacToe:
                    t = new Thread(TTTGameLoop);
                    break;
            }
            if (state == EGameControlState.Ready)
                t.Start();
                state = EGameControlState.GameInProgress;
                //this.Message = "Game has started";
        }

        private void ChessGameLoop()
        {
            // get the explicit type of model since evaluator only works with
            // specifically the chess position model types
            ChessPositionModel cpm = (ChessPositionModel)this.gameModel;
            EChessMoveTypes moveType;
            FormedMove move;
            string winner = null;
            List<Tuple<int, int>> kingCheckedBy = new List<Tuple<int, int>>();

            // this bool will be set false when the game ends
            while (true)
            {
                
                // start of new turn, clear variables
                move = null;
                moveType = EChessMoveTypes.None;
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
                        this.Message = "Player " + cpm.Player.CurPlayer + " has conceded!";
                        cpm.Player.change();
                        winner = cpm.Player.CurPlayer;
                        cpm.Player = null;
                        input = null;
                        break;
                    }

                    else if (evaluator.ValidateInput(input, ref move))
                    {
                        // then move is non null

                        if (evaluator.ValidateMove(move, cpm, ref moveType, ref kingCheckedBy))
                        {
                            //this.Message = "move passed validation";
                            cpm.applyMove(move, moveType);


                            // change display message here rather than whos turn
                            //System.Console.WriteLine("have applied move of type {0}", moveType);

                            // change the player
                            cpm.ChangePlayer();
                            // its the start of the player's turn so if he had any pawns that could have been captured
                            // en passant during hte oponents turn, they will now be unable to be captured en passant
                            //System.Console.WriteLine(" CLEARING PASSANTS");
                            cpm.clearEnPassantPawns(cpm.Player);
                            //this.Message = $"Player {cpm.Player.CurPlayer}'s turn";
                            //this.Message = $"passant sq if any is{cpm.EnPassantSq}";


                        }
                        else
                            this.Message = "The move was not valid";
                    }
                    else
                        this.Message = "The input was not valid";

                    input = null;
                }
                //Thread.Sleep(1000);
            }
            if (winner != null)
                this.Message = $"Player {winner} has won the game!";
            else
                this.Message = "The game has ended, loop thread detached";
        }


        private void TTTGameLoop()
        {
            
            TTTPositionModel tttpm = (TTTPositionModel)this.gameModel;
            FormedMove move;
            string winner = null;
            bool isMaxTurns = false;

            while (true)
            {
                // check if a winner has been found or reached max turns
                if (tttpm.IsWinningPosition(ref winner) || tttpm.IsMaxTurns(ref isMaxTurns))
                {
                    break;
                }

                this.Message = $"Player {tttpm.Player.CurPlayer}, make your move";
                move = null;
                // check if display has provided a move
                if (input != null)
                {
                    // Do Stuff..
                    move = new FormedMove(input);
                    if (tttpm.validateMove(move))
                    {
                        tttpm.applyMove(move, null);
                        tttpm.ChangePlayer();
                    }
                    else
                        this.Message = "The move was not valid";
                    input = null;


                }
                //Thread.Sleep(1000);

            }
            // at this point game has ended
            // why..

            if (winner != null)
                this.Message = $"Player {winner} has won the game!"; // make bold ?
            else if (isMaxTurns)
                this.Message = "Draw, no more moves can be made";

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

        public EGameControlState State
        {
            get
            {
                return this.state;
            }
        }

    }
}
