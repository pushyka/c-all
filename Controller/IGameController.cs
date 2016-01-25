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

        /* Receives a value indicating a game model and sets up the model as
        'displayableModel' (to which the view is bound) and also
        '*Model' (which is used in the game loop etc). This method also
        sets up any dependent utilities. In the case of chess a reference to the display
        form is also passed along, only needed for promotionselection focussing
        */
        void InitialiseModel(EGameModels model, System.Windows.Forms.Form viewRef);


        /* This takes a value indicating a gameModel and returns the values of the displayable
        and game models and utilities to null. */
        void UnInitialiseModel(EGameModels model);


        /* This takes a value indicating a gameModel and performs the initial setup of the
        model. Mainly populating with initial starting pieces / setting the starting player. */
        void PrepareModel(EGameModels model);


        /* This takes a value indicating a gameModel and starts a new thread
        running the appropriate gameLoop for the gameModel. */
        void StartGameLoop(EGameModels model);


        /* This stops and nulls the thread which is running a gameLoop. */
        void StopGameLoop();

        /* This takes a value indicating a gameModel and terminates any running gameLoop,
        sets some values to null and sets the models player value to null. */
        void Terminate(EGameModels model);


        /* This is the displayable version of the current game model. It is exposed via this controller to the View. */
        IDisplayableModel Model { get; }

        string Message { get; set; }

        string Input { get; set; }
    }
}
