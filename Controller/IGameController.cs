using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess.Model;

namespace chess.Controller
{
    interface IGameController
    {
        /// <summary>
        /// Creates the model and evaluator instances
        /// sets the model to the initial populated game state
        /// sets the initial player
        /// </summary>
        void InitialSetupChess();

        /// <summary>
        /// nulls the model and evaluator
        /// terminates the gameloop thread
        /// </summary>
        void TerminateChess();

        /// <summary>
        /// creates and starts the gameloop thread
        /// this is called following a setUp call
        /// </summary>
        void StartTTTGameLoop();

        /// <summary>
        /// stops the gameloop by terminating the gameloop thread
        /// if it is running. mainly called on an 'abandon' or 'close' app command
        /// eg to ensure the thread is stopped before closing the application
        /// </summary>
        void stopGameLoop();

        // The view etc can access the model via the controller
        ChessPositionModel ChessModel { get; }
        // message is mostly set by the controller but sometimes the
        // view might wish to update it with something
        string Message { get; set; }
        // input is mostly set by the view
        string Input { get; set; }

        
        
    }
}
