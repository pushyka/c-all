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
        private EGameModels selectedGameModel;

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

        private static Dictionary<EGamePieces, Image> gamePieces = new Dictionary<EGamePieces, Image>
        {
            
            {EGamePieces.BlackKing, chess.Properties.Resources.Chess_kdt60},
            {EGamePieces.BlackQueen, chess.Properties.Resources.Chess_qdt60},
            {EGamePieces.BlackBishop, chess.Properties.Resources.Chess_bdt60},
            {EGamePieces.BlackKnight, chess.Properties.Resources.Chess_ndt60},
            {EGamePieces.BlackRook, chess.Properties.Resources.Chess_rdt60},
            {EGamePieces.BlackPawn, chess.Properties.Resources.Chess_pdt60},
            {EGamePieces.WhiteKing, chess.Properties.Resources.Chess_klt60},
            {EGamePieces.WhiteQueen, chess.Properties.Resources.Chess_qlt60},
            {EGamePieces.WhiteBishop, chess.Properties.Resources.Chess_blt60},
            {EGamePieces.WhiteKnight, chess.Properties.Resources.Chess_nlt60},
            {EGamePieces.WhiteRook, chess.Properties.Resources.Chess_rlt60},
            {EGamePieces.WhitePawn, chess.Properties.Resources.Chess_plt60},
            {EGamePieces.O, chess.Properties.Resources.game_piece_oh},
            {EGamePieces.X, chess.Properties.Resources.game_piece_ex}
        };

        public Display(GameController gc)
        {
            this.gameController = gc;
            // tripple buffer>

            InitializeComponent();
            InitializeOtherComponents();

            tintRef = new List<Control>(); //reset?
        }

        /// <summary>
        /// Creates and hides a generic c type board
        /// </summary>
        private void AssembleChessBoard()
        {
            this.genericBoardBase = new TableLayoutPanel();

            // now the main container panel

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
            this.genericBoardBase.Visible = true;
            this.genericBoardBase.Enabled = true;
        }

        private void AssembleTTTBoard()
        {
            this.genericBoardBase = new TableLayoutPanel();

            // now the main container panel

            this.genericBoardBase.RowCount = 3;
            this.genericBoardBase.ColumnCount = 3;

            this.genericBoardBase.Location = new System.Drawing.Point(126, 163);
            this.genericBoardBase.Name = "genericBoardBase";
            this.genericBoardBase.BackColor = Color.Black;
            this.genericBoardBase.Size = new System.Drawing.Size(245, 245); // 6x3 pixels pixels for the spacing, 3x50 for each square
            this.genericBoardBase.TabIndex = 4;
            //this.genericBoardBase.BorderStyle = BorderStyle.FixedSingle; // solution


            //this.genericBoardBase.BackColor = Color.Black;
            // do tiles
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Panel tile = new Panel();
                    tile.Size = new System.Drawing.Size(75, 75);
                    // remove the margin spacing from the outer edges
                    int top = 5;
                    int bot = 5;
                    int rgt = 5;
                    int lft = 5;
                    if (row == 0)
                        top = 0;
                    if (row == 2)
                        bot = 0;
                    if (col == 0)
                        lft = 0;
                    if (col == 2)
                        rgt = 0;
                    tile.Margin = new Padding(lft, top, rgt, bot);
                    tile.BackColor = Color.OldLace;
                    tile.Click += OnTileClick;

                    //tile.MouseEnter += Tile_MouseEnter; // maybe too messy for now
                    this.genericBoardBase.Controls.Add(tile);
                }
            }


            // add it to the main display form
            this.Controls.Add(this.genericBoardBase);
            // hide ot fpr npw
            this.genericBoardBase.Visible = true;
            this.genericBoardBase.Enabled = true;
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
                    //tile.MouseEnter += Tile_MouseEnter; // maybe too messy for now
                    this.genericBoardBase.Controls.Add(tile);
                }
                


            }

        }

        private void InitializeOtherComponents()
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
        private void menuItemChess_Click(object sender, EventArgs e)
        {
            // if a game was already in progress, clear it first
            if (this.gameController.State == EGameControlState.GameInProgress)
                abandonGameToolStripMenuItem_Click(null, null);

            this.selectedGameModel = EGameModels.Chess;

            // Model
            this.gameController.InitialiseModel(this.selectedGameModel);

            // View
            AssembleChessBoard();
            
            // register event handlers to the chess model
            this.gameController.PropertyChanged += message_PropertyChanged;
            // chess specific handlers
            this.gameController.Model.CapturedChanged += ChessBoardCaptureChanged;
            this.gameController.Model.PlayerChanged += ChessBoardPlayerChanged;
            this.gameController.Model.BoardChanged += ChessBoardChanged;

            // This comes after the handlers are registered so that
            // the boardchanged handler may update the view.
            this.gameController.PrepareModel(this.selectedGameModel);


            this.infoPanel.Visible = false;
            this.menuItemNewGame.Enabled = true;
            this.loadGameToolStripMenuItem.Enabled = false;
            this.abandonGameToolStripMenuItem.Enabled = true;
            this.message_box.Enabled = true;
            this.message_box.Visible = true;
            this.concedeButton.Visible = true;

            this.gameController.StartGameLoop(this.selectedGameModel);

        }


        private void menuItemTTT_Click(object sender, EventArgs e)
        {
            // if a game was already in progress, clear it first
            if (this.gameController.State == EGameControlState.GameInProgress)
                abandonGameToolStripMenuItem_Click(null, null);

            this.selectedGameModel = EGameModels.TicTacToe;

            // Model
            this.gameController.InitialiseModel(this.selectedGameModel);

            // View
            AssembleTTTBoard();

            // register event handlers to the ttt model
            // WONT NECESARILY USE ALL OF THESE / MABE SPECIFIC ONES FOR TTT
            this.gameController.PropertyChanged += message_PropertyChanged; // can keep this one
            this.gameController.Model.PlayerChanged += TTTBoardPlayerChanged; // could make this more generic
            this.gameController.Model.BoardChanged += TTTBoardChanged; // needs its own, since wont be using chesspieces

            // This comes after the handlers are registered so that
            // the boardchanged handler may update the view.
            this.gameController.PrepareModel(this.selectedGameModel);

            this.infoPanel.Visible = false;
            this.menuItemNewGame.Enabled = true;
            this.loadGameToolStripMenuItem.Enabled = false;
            this.abandonGameToolStripMenuItem.Enabled = true;
            this.message_box.Enabled = true;
            this.message_box.Visible = true;
            //this.concedeButton.Visible = true;

            this.gameController.StartGameLoop(this.selectedGameModel);
        }


        private void abandonGameToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            // ADD A SWITCH STATEMENT IN HERE FOR DIFFERENT MODELS -> HANDLERS, VIEW ELEMENTS ARE DIFFERENT ETC

            // stop the game loop thread
            this.gameController.Terminate(this.selectedGameModel);
            // deregister handlers from the model
            deregisterHandlers(this.selectedGameModel);

            // uninitialise the models
            this.gameController.UnInitialiseModel(this.selectedGameModel);
            // clear display
            resetView(this.selectedGameModel);
           
            // if sender is null this has been called between a game switch eg (chess-> ttt)
            // so save time by not doing these steps (prevents flicker of infoPanel too)
            if (sender != null)
            {
                
                this.message_box.Visible = false;
                this.menuItemNewGame.Enabled = true;
                this.loadGameToolStripMenuItem.Enabled = true;
                this.abandonGameToolStripMenuItem.Enabled = false;
                this.infoPanel.Visible = true;
            }
            this.concedeButton.Visible = false;
        }


        private void deregisterHandlers(EGameModels model)
        {
            switch(model)
            {
                case EGameModels.Chess:
                    this.gameController.Model.BoardChanged -= ChessBoardChanged;
                    this.gameController.PropertyChanged -= message_PropertyChanged;
                    this.gameController.Model.CapturedChanged -= ChessBoardCaptureChanged;
                    this.gameController.Model.PlayerChanged -= ChessBoardPlayerChanged;
                    break;
                case EGameModels.TicTacToe:
                    this.gameController.PropertyChanged -= message_PropertyChanged;
                    this.gameController.Model.PlayerChanged -= TTTBoardPlayerChanged;
                    this.gameController.Model.BoardChanged -= TTTBoardChanged;
                    break;
            }

        }

        private void resetView(EGameModels model)
        {
            // clear the view tiles and remove it from the view
            this.genericBoardBase.Controls.Clear();
            this.Controls.Remove(genericBoardBase);
            this.genericBoardBase = null;

            switch(model)
            {
                case EGameModels.Chess:
                    this.blackPiecesCaptured.Controls.Clear();
                    this.whitePiecesCaptured.Controls.Clear();
                    this.white_turn_panel.Visible = false;
                    this.black_turn_panel.Visible = false;
                    break;
                case EGameModels.TicTacToe:
                    break;
            }


            // clear message box
            this.message_box.Items.Clear();


        }




        private PictureBox getGuiPiece(EGamePieces mPiece)
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


        /* This function used also for creating the promotion selection dialogue */
        public static PictureBox getGuiPiece2(EGamePieces mPiece)
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

            switch(selectedGameModel)
            {
                case EGameModels.Chess:
                    processChessClicks(sender, e);
                    break;
                case EGameModels.TicTacToe:
                    processTTTClicks(sender, e);
                    break;
            }
        }

        private void processChessClicks(object sender, EventArgs e)
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

        private void processTTTClicks(object sender, EventArgs e)
        {
            System.Console.WriteLine("a ttt click");
            string tileClicked = "";

            if (sender is Panel)
            {
                Panel tile = (Panel)sender;
                TableLayoutPanelCellPosition pos = this.genericBoardBase.GetPositionFromControl(tile);

                System.Console.WriteLine("I am the view and I have registered a click: {0}", pos);
                tileClicked += pos.Column.ToString() +pos.Row.ToString();
            }
            if (USRMOVE == "")
            {
                USRMOVE += tileClicked;
                this.gameController.Input = USRMOVE;
                USRMOVE = "";
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



        delegate void ChessBoardChangedCallback(object sender, BoardChangedEventArgs e);

        private void ChessBoardChanged(object sender, BoardChangedEventArgs e)
        {
            // code calling this method from non Form disp thread, so need to invoke into
            // the thread which is responsible for running the disp (eg the genericboardbase's thread)
            if (this.genericBoardBase.InvokeRequired)
            {
                ChessBoardChangedCallback d = new ChessBoardChangedCallback(ChessBoardChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                
                // only update the pos which have changed
                List<Tuple<int, int>> positionsChanged = e.PositionsChanged;
                System.Console.WriteLine($"BoardChanged raised with {e.PositionsChanged.Count} locations");
                foreach (Tuple<int, int> pos in positionsChanged)
                {

                    // corresponding gui position
                    Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(pos.Item2, pos.Item1);
                    EGamePieces mPiece;
                    if (!this.gameController.Model.Board[pos.Item1, pos.Item2].IsEmpty())
                    {
                        mPiece = this.gameController.Model.Board[pos.Item1, pos.Item2].piece.Val;
                    }
                    else
                    {
                        // give it the empty value
                        mPiece = EGamePieces.empty;
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
                    else if (mPiece == EGamePieces.empty)
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
            //else we are in the thread which created the control
            //else
            //{
            //    this.message_box.BeginUpdate();
            //    this.message_box.Items.Add(this.gameController.Message);
            //    // set most recently added as the last one
            //    this.message_box.TopIndex = this.message_box.Items.Count - 1;
            //    this.message_box.EndUpdate();
            //}

            // this version only has one item in the message box at a time
            // maybe make it to display 2 items
            else
            {
                this.message_box.BeginUpdate();
                this.message_box.Items.Clear();
                this.message_box.Items.Add(this.gameController.Message);
                // set most recently added as the last one
                this.message_box.TopIndex = this.message_box.Items.Count - 1;
                this.message_box.EndUpdate();
            }

        }


        delegate void ChessBoardCaptureChangedCallback(object sender, EventArgs e);

        private void ChessBoardCaptureChanged(object sender, EventArgs e)
        {
            //System.Console.WriteLine("I have registered a capture");
            // todo cross thread etc
            if (this.genericBoardBase.InvokeRequired)
            {
                ChessBoardCaptureChangedCallback d = new ChessBoardCaptureChangedCallback(ChessBoardCaptureChanged);
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
                    EGamePieces capturedPiece = gameController.Model.PiecesCapd[gameController.Model.PiecesCapd.Count - 1];
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


        delegate void ChessBoardPlayerChangedCallback(object sender, EventArgs e);

        private void ChessBoardPlayerChanged(object sender, EventArgs e)
        {
            if (this.genericBoardBase.InvokeRequired)
            {
                ChessBoardPlayerChangedCallback d = new ChessBoardPlayerChangedCallback(ChessBoardPlayerChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                // update captured display current player turn indicator squares to the model.player value
                // which has recently been changed
                string curPlayer = this.gameController.Model.Player.PlayerValue;
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
            this.menuItemNewGame.Enabled = false;
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
            gameController.StopGameLoop();
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;
            // else user is closing so stop the game loop if it is running
            gameController.StopGameLoop();
        }



        delegate void TTTBoardChangedCallback(object sender, BoardChangedEventArgs e);

        private void TTTBoardChanged(object sender, BoardChangedEventArgs e)
        {
            // code calling this method from non Form disp thread, so need to invoke into
            // the thread which is responsible for running the disp (eg the genericboardbase's thread)
            if (this.genericBoardBase.InvokeRequired)
            {
                TTTBoardChangedCallback d = new TTTBoardChangedCallback(TTTBoardChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                System.Console.WriteLine("NOW UPDATE THE VIEW ACCORDING TO THE BOARD WHICH HAS JUST BEEN CHANGED");

                // for ttt there is only ever one position changed at a time
                List<Tuple<int, int>> positionsChanged = e.PositionsChanged;
                foreach (Tuple<int, int> pos in positionsChanged)
                {
                    EGamePieces mPiece;
                    // corresponding gui position
                    // all my code treats coords in the row,col order but this function uses col,row order
                    Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(pos.Item2, pos.Item1);
                    if (!this.gameController.Model.Board[pos.Item1, pos.Item2].IsEmpty())
                        mPiece = this.gameController.Model.Board[pos.Item1, pos.Item2].piece.Val;
                    else
                        mPiece = EGamePieces.empty;
                    if (gamePieces.ContainsKey(mPiece))
                    {
                        // getguipiece2 is the version which doesnt add a click handler on to the piece
                        // since in ttt we dont need to worry about player clicking the piece
                        PictureBox gPiece = getGuiPiece2(mPiece);
                        gTile.Controls.Add(gPiece);
                    }
                }
                //{

                //    // corresponding gui position
                //    Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(pos.Item2, pos.Item1);
                //    GamePieces mPiece;
                //    if (!this.gameController.Model.Board[pos.Item1, pos.Item2].IsEmpty())
                //    {
                //        mPiece = this.gameController.Model.Board[pos.Item1, pos.Item2].piece.Val;
                //    }
                //    else
                //    {
                //        // give it the empty value
                //        mPiece = GamePieces.empty;
                //    }

                //    // remove all existing items on the tile (picture boxes if any)
                //    foreach (Control pb in gTile.Controls.OfType<PictureBox>())
                //    {
                //        gTile.Controls.Remove(pb);
                //    }

                //    // if mPiece was changed to e, then this is satisfactorily cleared the tile

                //    // else mPiece is a piece which needs to be added to the now empty tile
                //    if (gamePieces.ContainsKey(mPiece))
                //    {
                //        PictureBox gPiece = getGuiPiece(mPiece);
                //        gTile.Controls.Add(gPiece);
                //    }
                //    else if (mPiece == GamePieces.empty)
                //    {
                //        // then the clearing is already done
                //    }
                //    else
                //    {
                //        //System.Console.WriteLine("model piece is some unexpected value");
                //    }


                //}
            }


        }


        delegate void TTTBoardPlayerChangedCallback(object sender, EventArgs e);

        private void TTTBoardPlayerChanged(object sender, EventArgs e)
        {
            if (this.genericBoardBase.InvokeRequired)
            {
                TTTBoardPlayerChangedCallback d = new TTTBoardPlayerChangedCallback(TTTBoardPlayerChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                // update captured display current player turn indicator squares to the model.player value
                // which has recently been changed
                string curPlayer = this.gameController.Model.Player.PlayerValue;
            }
        }



    }
}
