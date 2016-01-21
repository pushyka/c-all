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
    
    /* The GameController class is responsible for receiving gameInput from the display / players,
    setting up models and utilities used by the game and running the games' main loops. */
    public class GameController : INotifyPropertyChanged, IGameController
    {
        // PropertyChanged event is for the .Message
        private string infoMessage;
        private string gameInput;
        private EGameControlState gameState;

        // display is bound to this object reference
        private IDisplayableModel displayableGameModel;

        // game loops use these objects for specialised functionality
        // like passing cpm to eval (which a tttpm cant be passed to)
        private ChessPositionModel chessModel; 
        private TicTacToePositionModel tictactoeModel;

        private Evaluator evaluator;
        private Thread gLoopThread;


        public GameController()
        {
            gameInput = null;
            infoMessage = null;
            displayableGameModel = null;
            evaluator = null;
            gLoopThread = null;
            gameState = EGameControlState.PreInitial;
        }


        /* Receives a value indicating a game model and sets up the model as
        'displayableModel' (to which the view is bound) and also
        '*Model' (which is used in the game loop etc). This method also
        sets up any dependent utilities. */
        public void InitialiseModel(EGameModels model)
        {
            switch(model)
            {
                case EGameModels.Chess:
                    displayableGameModel = new ChessPositionModel();
                    chessModel = (ChessPositionModel)displayableGameModel;
                    evaluator = new Evaluator();
                    evaluator.GenerateRays();
                    evaluator.GeneratePawnRays();
                    break;
                case EGameModels.TicTacToe:
                    displayableGameModel = new TicTacToePositionModel();
                    tictactoeModel = (TicTacToePositionModel)displayableGameModel;
                    break;
            }
            gameState = EGameControlState.Initial;
        }


        /* This takes a value indicating a gameModel and returns the values of the displayable
        and game models and utilities to null. */
        public void UnInitialiseModel(EGameModels model)
        {
            switch(model)
            {
                case EGameModels.Chess:
                    chessModel = null;
                    evaluator = null;
                    break;
                case EGameModels.TicTacToe:
                    tictactoeModel = null;
                    break;
            }
            displayableGameModel = null;
            gameState = EGameControlState.PreInitial;
        }
        
        
        /* This takes a value indicating a gameModel and performs the initial setup of the
        model. Mainly populating with initial starting pieces / setting the starting player. */
        public void PrepareModel(EGameModels model)
        {
            switch(model)
            {
                case EGameModels.Chess:
                    chessModel.Setup();
                    chessModel.Player = new Player("white");
                    break;
                case EGameModels.TicTacToe:
                    tictactoeModel.Setup();
                    tictactoeModel.Player = new Player("X");
                    break;
            }
            gameState = EGameControlState.Ready;
        }


        /* This takes a value indicating a gameModel and terminates any running gameLoop,
        sets some values to null and sets the models player value to null. */
        public void Terminate(EGameModels model)
        {
            //terminate t, if its running
            StopGameLoop();
            gameInput = null;
            infoMessage = null;
            switch(model)
            {
                case EGameModels.Chess:
                    chessModel.Player = null;
                    break;
                case EGameModels.TicTacToe:
                    tictactoeModel.Player = null;
                    break;
            }
            gameState = EGameControlState.Initial;
            this.Message = "Game is terminated";
        }


        /* This stops and nulls the thread which is running a gameLoop. */
        public void StopGameLoop()
        {
            if (gLoopThread != null)
            {
                gLoopThread.Abort();
                gLoopThread = null;
            }
        }


        /* This takes a value indicating a gameModel and starts a new thread
        running the appropriate gameLoop for the gameModel. */
        public void StartGameLoop(EGameModels model)
        {
            switch(model)
            {
                case EGameModels.Chess:
                    gLoopThread = new Thread(ChessGameLoop);
                    break;
                case EGameModels.TicTacToe:
                    gLoopThread = new Thread(TicTacToeGameLoop);
                    break;
            }
            if (gameState == EGameControlState.Ready)
                gLoopThread.Start();
                gameState = EGameControlState.GameInProgress;
        }


        /* This is the Game Loop for a chess Game. This is run while a 
        game is in progress in a seperate thread. When the game meets an ending criteria
        (winner, concede) this thread will end naturally. This thread may also be 
        terminated externally. */
        private void ChessGameLoop()
        {
            EChessMoveTypes moveType;
            FormedMove move;
            Player winner = null;
            List<Tuple<int, int>> kingCheckedBy = new List<Tuple<int, int>>();

            while (true)
            {
                // start of new turn, refresh variables
                move = null;
                moveType = EChessMoveTypes.None;
                kingCheckedBy.Clear();

                // check there exists a legal move for the current player
                // if not in check and no legal move : stalemate
                // if in check and no legal move to remove attack : checkmate
                // if legal move proceed
                if (evaluator.IsKingInCheck(chessModel, ref kingCheckedBy))
                    this.Message = $"Player {chessModel.Player.PlayerValue}'s king is in check";

                // check if display has provided a move
                if (gameInput != null)
                {
                    if (gameInput == "concede")
                    {
                        this.Message = $"Player {chessModel.Player.PlayerValue} has conceded!";
                        chessModel.Player.change();
                        winner = chessModel.Player;
                        chessModel.Player = null;
                        gameInput = null;
                        break;
                    }
                    // else normal move input
                    else if (evaluator.ValidateInput(gameInput, ref move))
                    {
                        if (evaluator.IsValidMove(move, chessModel, ref moveType, ref kingCheckedBy))
                        {
                            chessModel.applyMove(move, moveType);
                            chessModel.ChangePlayer();
                            chessModel.clearEnPassantPawns(chessModel.Player);
                        }
                        else
                            this.Message = "The move was not valid";
                    }
                    else
                        this.Message = "The input was not valid";

                    gameInput = null;
                }
            }
            if (winner != null)
                this.Message = $"Player {winner.PlayerValue} has won the game!";
            else
                this.Message = "The game has ended, loop thread detached";
        }


        /* This is the Game Loop for a Tic Tac Toe Game. This is run while a 
        game is in progress in a seperate thread. When the game meets an ending criteria
        (winner) this thread will end naturally. This thread may also be 
        terminated externally. */
        private void TicTacToeGameLoop()
        {
            FormedMove move;
            string winner = null; // change to player
            bool isMaxTurns = false;

            while (true)
            {
                // check if a winner has been found or reached max turns
                if (tictactoeModel.IsWinningPosition(ref winner) || tictactoeModel.IsMaxTurns(ref isMaxTurns))
                    break;

                this.Message = $"Player {tictactoeModel.Player.PlayerValue}, make your move";
                move = null;
                // check if display has provided a move
                if (gameInput != null)
                {
                    move = new FormedMove(gameInput);
                    if (tictactoeModel.IsValidMove(move))
                    {
                        tictactoeModel.applyMove(move);
                        tictactoeModel.ChangePlayer();
                    }
                    else
                        this.Message = "The move was not valid";
                    gameInput = null;
                }
            }
            // at this point game has ended
            if (winner != null)
                this.Message = $"Player {winner} has won the game!";
            else if (isMaxTurns)
                this.Message = "Draw, no more moves can be made";
        }


        /* This is the displayable version of the current game model, exposed via this controller to the View. */
        public IDisplayableModel Model
        {
            get
            {
                return this.displayableGameModel;
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        /* This is the MessageChanged Event, It is declared here in the controller since
        mostly it is the controller which changes the message to be displayed on the View. */
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /* This is the Message setter and getter, when a message is set the notifypropertychanged
        (messagechanged event) is fired to let the View know. */
        public string Message
        {
            set
            {
                if (this.infoMessage != value)
                {
                    this.infoMessage = value;
                    NotifyPropertyChanged();
                }
            }
            get
            {
                return this.infoMessage;
            }
        }


        /* Input setter. This is mainly used by the View / gui using the game controller.
        If a previous input has not yet been cleared by the controller, it means the controller is
        not ready to process another one. */
        public string Input
        {
            get // is this used?
            {
                return this.gameInput;
            }
            set
            {
                if (this.gameInput == null)
                {
                    this.gameInput = value;
                }
                    
                else
                    System.Console.WriteLine("A previous input hsant been cleared yet (currently being processed) so this Set has failed");
            }
        }


        /* Return the state of the game from the controller. */
        public EGameControlState State
        {
            get
            {
                return this.gameState;
            }
        }
    }
}
