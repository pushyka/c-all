using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

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
        private GameController gameController = null;
        private TableLayoutPanel genericBoardBase = null;
        private LoadForm loadform;
        private string USRMOVE = "";
        private List<Control> tintRef = null;

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

        private Dictionary<Pieces, Image> gamePieces = new Dictionary<Pieces, Image>
        {
            
            {Pieces.K, chess.Properties.Resources.Chess_kdt60},
            {Pieces.queenB, chess.Properties.Resources.Chess_qdt60},
            {Pieces.B, chess.Properties.Resources.Chess_bdt60},
            {Pieces.N, chess.Properties.Resources.Chess_ndt60},
            {Pieces.R, chess.Properties.Resources.Chess_rdt60},
            {Pieces.pawnB, chess.Properties.Resources.Chess_pdt60},
            {Pieces.k, chess.Properties.Resources.Chess_klt60},
            {Pieces.queenW, chess.Properties.Resources.Chess_qlt60},
            {Pieces.b, chess.Properties.Resources.Chess_blt60},
            {Pieces.n, chess.Properties.Resources.Chess_nlt60},
            {Pieces.r, chess.Properties.Resources.Chess_rlt60},
            {Pieces.pawnW, chess.Properties.Resources.Chess_plt60},
        };

        public Display(GameController gc)
        {
            this.gameController = gc;
            // tripple buffer>
            InitializeComponent();
            InitializeComponent2();
            InitGenericBoard();
            
            

            tintRef = new List<Control>(); //reset?


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

            this.genericBoardBase.Location = new System.Drawing.Point(46, 83);
            this.genericBoardBase.Name = "genericBoardBase";
            //this.genericBoardBase.BackColor = Color.Transparent;
            this.genericBoardBase.Size = new System.Drawing.Size(400, 400);
            this.genericBoardBase.TabIndex = 4;
            this.genericBoardBase.BorderStyle = BorderStyle.FixedSingle; // solution



            generateTilesForBase();


            // add it to the main display form
            this.Controls.Add(this.genericBoardBase);
            // hide ot fpr npw
            //this.genericBoardBase.Visible = false;
            this.genericBoardBase.Enabled = false;
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
                    tile.MouseEnter += Tile_MouseEnter; // maybe too messy for now
                    this.genericBoardBase.Controls.Add(tile);
                }
                


            }

        }

        private void InitializeComponent2()
        {
            this.concedeButton.FlatStyle = FlatStyle.Flat;
            loadform = new LoadForm();
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




            this.gameController.initModelandEval();
            // register event handlers
            this.gameController.PropertyChanged += message_PropertyChanged;
            this.gameController.ChessModel.CapturedChanged += model_CapturedChanged;
            this.gameController.ChessModel.PlayerChanged += model_PlayerChanged;
            // setup the game (populate)
            this.gameController.setUp();
            

            // update the view to match the model 
            //todoREMOVE THIS AND MOVE BOardChanged Handler above REGISTERED BEFORE THE MODEL CODE
            this.updateView(this.gameController.ChessModel.Board);

            //finally register the view to the model.BoardChanged event so it will update itself on future changes
            this.gameController.ChessModel.BoardChanged += model_BoardChanged;

            this.menuItem_add_Board.Enabled = false;
            this.loadGameToolStripMenuItem.Enabled = false;
            this.abandonGameToolStripMenuItem.Enabled = true;
            this.genericBoardBase.Enabled = true;
            this.message_box.Enabled = true;
            this.concedeButton.Visible = true;



            this.gameController.startGameLoop();

        }


        private void abandonGameToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            // stop the game loop thread
            this.gameController.tearDown();
            // deregister handlers from the model
            this.gameController.ChessModel.BoardChanged -= model_BoardChanged;
            this.gameController.PropertyChanged -= message_PropertyChanged;
            this.gameController.ChessModel.CapturedChanged -= model_CapturedChanged;
            this.gameController.ChessModel.PlayerChanged -= model_PlayerChanged;
            // clear the model and evaluator
            this.gameController.uninitModeandEval();
            // clear display
            resetView();
            this.genericBoardBase.Enabled = false;
            this.concedeButton.Visible = false;
            this.menuItem_add_Board.Enabled = true;
            this.loadGameToolStripMenuItem.Enabled = true;
            this.abandonGameToolStripMenuItem.Enabled = false;
        }

        private void resetView()
        {
            // clear the view tiles
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(col, row);
                    gTile.Controls.Clear();
                }
            }


            this.blackPiecesCaptured.Controls.Clear();
            this.whitePiecesCaptured.Controls.Clear();

            // clear message box
            this.message_box.Items.Clear();
        }


        /// <summary>
        /// Called once to initially draw the state of the model onto the
        /// display. Successive updates will be BoardChanged event driven
        /// and will only seek to update the few tiles which have changed.
        /// </summary>
        /// <param name="board"></param>
        private void updateView(TileStruct[, ] board)
        {
            // go through the model tiles, when find a piece on a tile,
            for (int row = 0; row < 8; row ++)
            {
                for (int col = 0; col < 8; col ++)
                {

                    TileStruct tile = board[row, col];
                    // if the tile isnt empty
                    if (!tile.IsEmpty())
                    {
                        // get the piece graphic (dict?)
                        PictureBox gPiece = getGuiPiece(tile.piece.Val);

                        // get the associated display tile
                        Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(col, row);
                        //dispTile.BackColor = Color.Yellow;
                        // draw the graphic onto the display tile (graphic must inherit Click ability)
                        gTile.Controls.Add(gPiece);
                        

                    }
                }
            }
            
        }

        private PictureBox getGuiPiece(Pieces mPiece)
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

        private PictureBox getGuiPiece2(Pieces mPiece)
        {
            PictureBox pb = new PictureBox();
            pb.Name = "pb";
            pb.Size = new Size(50, 50);
            pb.Image = gamePieces[mPiece];
            pb.SizeMode = PictureBoxSizeMode.Zoom;
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
                TableLayoutPanelCellPosition pos;
                if (tile.Name == "tint")
                {
                    // then the user has clicked already tinted panel (same panel of this turn so they want to unselect it)
                    // send the duplicate tiles as normal, the evaluator will detect the error
                    // and will untint it after sending like normal,
                    // the purpose of these lines is to simply switch from the tinted tile back to the parent
                    // guiTile, (this may be the parent if empty, or the parents parent if a piece square)
                    // since only the guiTile can provide the pos info required for the evaluator
                    if (tile.Parent is Panel)
                    {
                        pos = this.genericBoardBase.GetPositionFromControl(tile.Parent);
                    }
                    else if (tile.Parent is PictureBox)
                    {
                        pos = this.genericBoardBase.GetPositionFromControl(tile.Parent.Parent);
                    }
                    else
                    {
                        throw new Exception();
                    }
                    

                }
                else
                {
                    addTint(tile);
                    pos = this.genericBoardBase.GetPositionFromControl(tile);
                }

                //System.Console.WriteLine("I am the view and I have registered a click: {0}", pos);
                tileClicked += colToFile[pos.Column].ToString() + rowToRank[pos.Row].ToString();
            }

            else if (sender is PictureBox)
            {
                PictureBox picture = (PictureBox)sender;
                // add the tint
                addTint(picture);
                // get the parent Panel container of the picturebox
                Panel tile = (Panel)picture.Parent;

                TableLayoutPanelCellPosition pos = this.genericBoardBase.GetPositionFromControl(tile);
                //System.Console.WriteLine("I am the view and I have registered a click: {0}", pos);
                tileClicked += colToFile[pos.Column].ToString() + rowToRank[pos.Row].ToString();
            }
            

            

            

            
            

            if (USRMOVE == "")
            {
                USRMOVE += tileClicked;
            }
            else // else the first click has already been added to usermove, so add the second
            {
                USRMOVE += ' ' + tileClicked;
                this.gameController.Input = USRMOVE;
                // clear USRMOVE for next turn
                USRMOVE = "";
                // clear those highlights now
                Thread.Sleep(100);
                removeTint();
            }
        }


        /// <summary>
        /// Creates a tinted panel to be layered on to the top of the topmost control on the tile.
        /// This could be the tile panel if empty or a picturebox if its occupied.
        /// </summary>
        /// <param name="tileref"></param>
        private void addTint(Control tileref)
        {

            this.tintRef.Add(tileref);
            
            Panel tintPane = new Panel();
            tintPane.Name = "tint";
            tintPane.Size = tileref.Size;

            tintPane.BackColor = Color.FromArgb(70, Color.Yellow);
            tintPane.Click += OnTileClick;
            tileref.Controls.Add(tintPane);
        }


        private void removeTint()
        {
            // foreach board square control (2 of them)
            foreach (Control c in this.tintRef)
            {
                // the control is either a panel or a picturebox
                // with only 1 control within in, = the tint panel
                foreach (Control tp in c.Controls.OfType<Panel>())
                {
                    c.Controls.Remove(tp);
                }
            }
            this.tintRef.Clear();
        }


        private void Tile_MouseEnter(object sender, EventArgs e)
        {
            //Panel tile = (Panel)sender;
            //tile.BackColor = Color.FromArgb(25, tile.BackColor);
          // todo
            //    modify it slightly and restore on leave

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
                    
                    // corresponding gui position
                    Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(pos.Item2, pos.Item1);
                    Pieces mPiece;
                    if (!this.gameController.ChessModel.Board[pos.Item1, pos.Item2].IsEmpty())
                    {
                        mPiece = this.gameController.ChessModel.Board[pos.Item1, pos.Item2].piece.Val;
                    }
                    else
                    {
                        // give it the empty value
                        mPiece = Pieces.empty;
                    }

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
                    else if (mPiece == Pieces.empty)
                    {
                        // then the clearing is already done
                    }
                    else
                    {
                        //System.Console.WriteLine("model piece is some unexpected value");
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
                this.message_box.Items.Add(this.gameController.Message);
                // set most recently added as the last one
                this.message_box.TopIndex = this.message_box.Items.Count - 1;
                this.message_box.EndUpdate();
            }

        }


        delegate void model_CapturedChangedCallback(object sender, EventArgs e);

        private void model_CapturedChanged(object sender, EventArgs e)
        {
            //System.Console.WriteLine("I have registered a capture");
            // todo cross thread etc
            if (this.genericBoardBase.InvokeRequired)
            {
                model_CapturedChangedCallback d = new model_CapturedChangedCallback(model_CapturedChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                // update captured display to match cpatpieces list
                // only update the most recent item captured
                ;
                try
                {
                    // get the last item
                    Pieces capturedPiece = gameController.ChessModel.PiecesCapd[gameController.ChessModel.PiecesCapd.Count - 1];
                    ;
                    // uses version of the function which doesnt add click handler
                    PictureBox gCapturedPiece = getGuiPiece2(capturedPiece);
                    if ((int)(capturedPiece) >= 6 && (int)capturedPiece < 12)
                    {
                        // add to the black display
                        this.blackPiecesCaptured.Controls.Add(gCapturedPiece);
                    }
                    else if ((int)capturedPiece < 6)
                    {
                        this.whitePiecesCaptured.Controls.Add(gCapturedPiece);
                    }

                }
                catch (Exception)
                {

                }

                    

            }
        }


        delegate void model_PlayerChangedCallback(object sender, EventArgs e);

        private void model_PlayerChanged(object sender, EventArgs e)
        {
            if (this.genericBoardBase.InvokeRequired)
            {
                model_PlayerChangedCallback d = new model_PlayerChangedCallback(model_PlayerChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                // update captured display current player turn indicator squares to the model.player value
                // which has recently been changed
                string curPlayer = this.gameController.ChessModel.Player.CurPlayer;
                this.white_turn_panel.Visible = false;
                this.black_turn_panel.Visible = false;

                if (curPlayer == "white")
                    this.white_turn_panel.Visible = true;

                else if (curPlayer == "black")
                    this.black_turn_panel.Visible = true;
                
            }
        }



        private void concedeButton_Click(object sender, EventArgs e)
        {
            // remove any lingering tints for aesthetics
            removeTint();
            this.genericBoardBase.Enabled = false;
            this.concedeButton.Visible = false;
            this.gameController.Input = "concede";
        }

        private void menuItem_loadGame_Click(object sender, EventArgs e)
        {
            this.menuItem_add_Board.Enabled = false;
            this.abandonGameToolStripMenuItem.Enabled = false;

            DialogResult result = this.loadform.ShowDialog();
            //System.Console.WriteLine(result.ToString());
            // worker thread to select the file and load method
            // load game by applying moves etc
            // when this thread completed :
            // start the game thread
            // visible abandon again
            // if game is not ended transition into playing it
            // when game thread ends / ends immediately, visible new game again etc 
        }

        private void menuItem_close_Click(object sender, EventArgs e)
        {
            // make sure to terminate the gameloop thread if its running
            gameController.stopGameLoop();
            // now end the main thread
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // else user is closing also need to stop this loop if its running
            gameController.stopGameLoop();
        }

        private void menuItem_test_Click(object sender, EventArgs e)
        {
            this.gameController.initModelandEval();
            this.gameController.testStuff();
        }
    }
}
