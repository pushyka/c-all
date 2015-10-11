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
        TableLayoutPanel genericBoardBase;

        public Display(GameController gc)
        {
            this.gc = gc;
            // tripple buffer>
            InitializeComponent();
            InitMoreComponents();
            InitGenericBoard();

        }

        /// <summary>
        /// Reserved for manually adding components without the designer
        /// eg an array of gui Tiles
        /// </summary>
        private void InitMoreComponents()
        {
            // TODO
        }

        /// <summary>
        /// Creates and hides a generic c type board
        /// </summary>
        private void InitGenericBoard()
        {
            

            // now the main container panel
            this.genericBoardBase = new TableLayoutPanel();
            this.genericBoardBase.RowCount = 8;
            this.genericBoardBase.ColumnCount = 8;

            this.genericBoardBase.Location = new System.Drawing.Point(85, 234);
            this.genericBoardBase.Name = "genericBoardBase";
            this.genericBoardBase.Size = new System.Drawing.Size(240, 240);
            this.genericBoardBase.TabIndex = 4;
            this.genericBoardBase.BorderStyle = BorderStyle.FixedSingle; // solution

            generateTilesForBase();

            // add it to the main display form
            this.Controls.Add(this.genericBoardBase);
            // hide ot fpr npw
            this.genericBoardBase.Visible = false;
        }

        private void generateTilesForBase()
        {
            for (int row = 0; row < 8; row ++)
            {
                for (int col = 0; col < 8; col ++)
                {
                    Panel p = new Panel();
                    p.Size = new System.Drawing.Size(30, 30);
                    p.Margin = new Padding(0); // remove the space between tiles

                    if ((row % 2 == 0 && col % 2 == 0) ||
                        (row % 2 == 1 && col % 2 == 1))
                    {
                        p.BackColor = Color.White;
                    }
                    else
                    {
                        p.BackColor = Color.Black;
                    }
                    this.genericBoardBase.Controls.Add(p);
                }
                


            }

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
            Square[,] refToB = this.gc.lookAtChessModel().Board;
            System.Console.WriteLine("Testing for access to c.Board...");
            System.Console.WriteLine("I can see... ({0}) ?? hopefully capital r", refToB[0, 0].piece);
        }

        /// <summary>
        /// load one of a number of games to the display.
        /// For now, load a chess game display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_addBoard_Click(object sender, EventArgs e)
        {
            // make the board visible
            this.genericBoardBase.Visible = true;

            // setup the model (populate)
            this.gc.setUp();

            // the view sees a change to the model and updates accordingly
            // an event hander in the view subscribes to an event in the model
            this.gc.lookAtChessModel().BoardChanged += model_BoardChanged;


        }

        private void model_BoardChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("I am the view and I have registered that the model has changed");
        }
    }
}
