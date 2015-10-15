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
            // if dont disable this menu option, it can be clicked multiple times causing
            // for example these handlers to be registered multiple times , so ensure 
            // this menu item is not clickeed multiple times
            this.menuItem_add_Board.Enabled = false;

            // make the board visible
            this.genericBoardBase.Visible = true;

            // setup the model (populate)
            this.gc.setUp();

            // update the view to match the model 
            //todo
            this.updateView(this.gc.lookAtChessModel().Board);

            this.concedeButton.Visible = true;


            //finally register the view to the model.BoardChanged event so it will update itself on future changes

            // the view sees a change to the model and updates accordingly
            // an event hander in the view subscribes to an event in the model
            this.gc.lookAtChessModel().BoardChanged += model_BoardChanged;



            // the display code to update the message box will subscribe to this Message.propertyChanged event
            this.gc.PropertyChanged += message_PropertyChanged;

            this.gc.lookAtChessModel().CapturedChanged += model_CapturedChanged;
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
                        gTile.Controls.Add(gPiece);

                    }
                }
            }
            
        }

        private PictureBox getGuiPiece(char mPiece)
        {
            PictureBox pb = new PictureBox();
            pb.Name = "pb";
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
                this.gc.Input = USRMOVE;
                // clear USRMOVE for next turn
                USRMOVE = "";
            }



            // when have 2 ready, form the string to be sent to the evaluator
            
        }

        delegate void model_BoardChangedCallback(object sender, BoardChangedEventArgs e);

        private void model_BoardChanged(object sender, BoardChangedEventArgs e)
        {
            // code calling this method from non Form disp thread, so need to invoke into
            // the thread which is responsible for running the disp (eg the genericboardbase's thread)
            if (this.genericBoardBase.InvokeRequired)
            {
                model_BoardChangedCallback d = new model_BoardChangedCallback(model_BoardChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                List<Tuple<int, int>> positionsChanged = e.PositionsChanged;
                foreach (Tuple<int, int> pos in positionsChanged)
                {
                    // update the display to match the model 

                    // corresponding gui position
                    Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(pos.Item2, pos.Item1);

                    char mPiece = this.gc.lookAtChessModel().Board[pos.Item1, pos.Item2].piece;

                    // remove all existing items on the tile (picture boxes if any)
                    foreach (Control pb in gTile.Controls.OfType<PictureBox>())
                    {
                        gTile.Controls.Remove(pb);
                    }

                    // if mPiece was changed to e, then this is satisfactorily cleared the tile

                    // else mPiece is a piece which needs to be added to the now empty tile
                    if (gamePieces.ContainsKey(mPiece))
                    {
                        PictureBox gPiece = getGuiPiece(mPiece);
                        gTile.Controls.Add(gPiece);
                    }
                    else if (mPiece == 'e')
                    {
                        // then the clearing is already done
                    }
                    else
                    {
                        System.Console.WriteLine("model piece is some unexpected value");
                    }

                }
            }
            
            
        }


        delegate void message_PropertyChangedCallback(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Handler for the PropertyChanged event raised when gc.Message is changed.
        /// When this event occurs, the function updates the message listbox with 
        /// the new value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void message_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // e.PropertyName is that which has changed, always Message in this case

            /* == cross thread call problem ==
            *  Most of the changes to the message variable are occuring inside the gameLoop child thread.
            *  So this thread is raising the PropertyChanged event and the handler and this is all within
            *  that child thread. When it tries to update the form control message_box, there is a problem
            *  since the message_box was created in the main gui thread, and this is detected as an unsafe
            *  cross thread operation. 
            *  Need to call InvokeRequired which returns true if that message_box was created in a different
            *  thread than the one currently executing. Then calling invoke and passing it a callback to this
            *  same function, allows it to do the execution on that parent thread asynchronously.
            */

            if (this.genericBoardBase.InvokeRequired)
            {
                // delegate d a pointer to the function
                message_PropertyChangedCallback d = new message_PropertyChangedCallback(message_PropertyChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            // else we are in the thread which created the control
            else
            {
                this.message_box.BeginUpdate();
                this.message_box.Items.Add(this.gc.Message);
                // set most recently added as the last one
                this.message_box.TopIndex = this.message_box.Items.Count - 1;
                this.message_box.EndUpdate();
            }

        }




        private void model_CapturedChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("I have registered a capture");
            // todo cross thread etc
        }



        private void concedeButton_Click(object sender, EventArgs e)
        {
            this.gc.Input = "concede";
        }
    }
}
