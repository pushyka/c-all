using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using chess.Controller;
using chess.Model;

namespace chess.View
{
    /// <summary>
    /// This is the manual partial of the displayform class
    /// This version contains the arg-constructor . the only 
    /// one which is actually used.
    /// </summary>
    public partial class Display : Form
    {
        GameController gc;

        public Display(GameController gc)
        {
            this.gc = gc;
            InitializeComponent();
            InitMoreComponents();
        }

        /// <summary>
        /// Reserved for manually adding components without the designer
        /// eg an array of gui Tiles
        /// </summary>
        private void InitMoreComponents()
        {
            // TODO
        }


        // This space reserved for tests
        private void menuItem_Test_Click(object sender, EventArgs e)
        {
            //this.gc.setUp();
            

            // so a property changed / object changed handler will be attached to this ref
           
        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            this.gc.recvInstructTEST();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.gc.setUp();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Square[,] refToB = this.gc.lookAtChessModel();
            System.Console.WriteLine("Testing for access to c.Board...");
            System.Console.WriteLine("I can see... ({0}) ?? hopefully capital r", refToB[0, 0].piece);
        }
    }
}
