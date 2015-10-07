using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using chess.Controller;
using chess.View;

namespace chess
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            GameController gc = new GameController(); // take care of setting up the model etc

            // The Display can can talk to the controller (send it move inputs
            // and when menu is clicked eg)
            // controller provides the display with a restricted view of the model
            // Display may only view Chess.Board
            Application.EnableVisualStyles();
            Application.Run(new Display(gc));
        }
    }
}
