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
        private GameController gc;
        private TableLayoutPanel genericBoardBase;
        private string USRMOVE = "";

        // dictionaries for quick conversion from coordinates to file/rank
        private Dictionary<int, char> colToFile = new Dictionary<int, char>
        {
            {0, 'a'},
            {1, 'b'},
            {2, 'c'},
            {3, 'd'},
            {4, 'e'},
            {5, 'f'},
            {6, 'g'},
            {7, 'h'}
        };
        private Dictionary<int, int> rowToRank = new Dictionary<int, int>
        {
            {0, 8},
            {1, 7},
            {2, 6},
            {3, 5},
            {4, 4},
            {5, 3},
            {6, 2},
            {7, 1}
        };

        private Dictionary<char, Image> gamePieces = new Dictionary<char, Image>
        {
            
            {'K', chess.Properties.Resources.Chess_kdt60},
            {'Q', chess.Properties.Resources.Chess_qdt60},
            {'B', chess.Properties.Resources.Chess_bdt60},
            {'N', chess.Properties.Resources.Chess_ndt60},
            {'R', chess.Properties.Resources.Chess_rdt60},
            {'P', chess.Properties.Resources.Chess_pdt60},
            {'k', chess.Properties.Resources.Chess_klt60},
            {'q', chess.Properties.Resources.Chess_qlt60},
            {'b', chess.Properties.Resources.Chess_blt60},
            {'n', chess.Properties.Resources.Chess_nlt60},
            {'r', chess.Properties.Resources.Chess_rlt60},
            {'p', chess.Properties.Resources.Chess_plt60},
        };

        public Display(GameController gc)
        {
            this.gc = gc;
            // tripple buffer>
            InitializeComponent();
            InitGenericBoard();

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

            this.genericBoardBase.Location = new System.Drawing.Point(60, 80);
            this.genericBoardBase.Name = "genericBoardBase";
            //this.genericBoardBase.BackColor = Color.Transparent;
            this.genericBoardBase.Size = new System.Drawing.Size(400, 400);
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
                    Panel tile = new Panel();
                    tile.Size = new System.Drawing.Size(50, 50);
                    tile.Margin = new Padding(0); // remove the space between tiles

                    if ((row % 2 == 0 && col % 2 == 0) ||
                        (row % 2 == 1 && col % 2 == 1))
                    {
                        tile.BackColor = Color.BlanchedAlmond;
                    }
                    else
                    {
                        tile.BackColor = Color.Peru;
                    }
                    tile.Click += OnTileClick;
                    this.genericBoardBase.Controls.Add(tile);
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

            // update the view to match the model 
            //todo
            this.updateView(this.gc.lookAtChessModel().Board);


            //finally register the view to the model.BoardChanged event so it will update itself on future changes

            // the view sees a change to the model and updates accordingly
            // an event hander in the view subscribes to an event in the model
            this.gc.lookAtChessModel().BoardChanged += model_BoardChanged;

            
            this.gc.startGameLoop();




        }

        /// <summary>
        /// Called once to initially draw the state of the model onto the
        /// display. Successive updates will be BoardChanged event driven
        /// and will only seek to update the few tiles which have changed.
        /// </summary>
        /// <param name="board"></param>
        private void updateView(Square[, ] board)
        {
            // go through the model tiles, when find a piece on a tile,
            for (int row = 0; row < 8; row ++)
            {
                for (int col = 0; col < 8; col ++)
                {
                    char mPiece = board[row, col].piece;
                    // if the tile isnt empty
                    if (mPiece != 'e')
                    {
                        // get the piece graphic (dict?)
                        PictureBox gPiece = getGuiPiece(mPiece);

                        // get the associated display tile
                        Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(col, row);
                        //dispTile.BackColor = Color.Yellow;
                        // draw the graphic onto the display tile (graphic must inherit Click ability)
                        System.Console.WriteLine("tile size is {0}", gTile.Size);
                        gTile.Controls.Add(gPiece);

                    }
                }
            }
            
        }

        private PictureBox getGuiPiece(char mPiece)
        {
            PictureBox pb = new PictureBox();
            pb.Size = new Size(50, 50);
            pb.Image = gamePieces[mPiece];
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            // inherit click ability
            pb.Click += OnTileClick;
            return pb;
        }




        /// <summary>
        /// This function when called, generates a tile location eg ("B3") based on what the user
        /// has clicked. It will store the first click in a persistent field variable. When it has
        /// received two clicks it will send them to the evaluator. The evaluator will do any 
        /// checking, including for correct player pieces clicked. So this event handler is ready to be
        /// used at any stage of the programs running. 
        /// The empty tile and the Piece on the tile's click events are both tied to this handler.
        /// For the piece, the function must get its container control (the empty Panel) .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTileClick(object sender, EventArgs e)
        {
            string tileClicked = "";

            if (sender is Panel)
            {
                Panel tile = (Panel)sender;
                TableLayoutPanelCellPosition pos = this.genericBoardBase.GetPositionFromControl(tile);
                System.Console.WriteLine("I am the view and I have registered a click: {0}", pos);
                tileClicked += colToFile[pos.Column].ToString() + rowToRank[pos.Row].ToString();
            }

            else if (sender is PictureBox)
            {
                PictureBox picture = (PictureBox)sender;
                // get the parent Panel container of the picturebox
                Panel tile = (Panel)picture.Parent;

                TableLayoutPanelCellPosition pos = this.genericBoardBase.GetPositionFromControl(tile);
                System.Console.WriteLine("I am the view and I have registered a click: {0}", pos);
                tileClicked += colToFile[pos.Column].ToString() + rowToRank[pos.Row].ToString();
            }
            

            

            

            
            

            if (USRMOVE == "")
            {
                USRMOVE += tileClicked;
            }
            else // else the first click has already been added to usermove, so add the second
            {
                USRMOVE += ' ' + tileClicked;
                this.gc.INPUT = USRMOVE;
                // clear USRMOVE for next turn
                USRMOVE = "";
            }



            // when have 2 ready, form the string to be sent to the evaluator
            
        }

        // private void GENERIC tileClicked handler which waits until TWO tiles eg "b4 c2" have been clicked then it sends it
        // to the controller
        // use STATIC or DISPLAY-CLASS FIELD VALUES

        private void model_BoardChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("I am the view and I have registered that the model has changed");
            // redraw the display using the model
        }
    }
}
