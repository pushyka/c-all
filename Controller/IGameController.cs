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
        /// Instantiates the game objects, models and utilities.
        /// </summary>
        /// <param name="model"></param>
        void InitialiseModel(EGameModels model);


        /// <summary>
        /// Removes the game objects, models and utilities.
        /// </summary>
        /// <param name="model"></param>
        void UnInitialiseModel(EGameModels model);


        /// <summary>
        /// Sets the initial parameters of the game model
        /// like the initial population of pieces and the 
        /// starting player.
        /// </summary>
        void PrepareModel(EGameModels model);


        /// <summary>
        /// Once everything is prepared, starts the loop thread for the game.
        /// </summary>
        void StartGameLoop(EGameModels model);


        /// <summary>
        /// Stops the runnig game loop thread.
        /// </summary>
        void StopGameLoop();
    
        /// <summary>
        /// Stops the running game loop thread and nullifies the
        /// initial parameters (unpreparemodel)
        /// </summary>
        void Terminate(EGameModels model);


        // The view binds to the {model exposed by the controller}
        IDisplayableModel Model { get; }

        string Message { get; set; }

        string Input { get; set; }
    }
}
