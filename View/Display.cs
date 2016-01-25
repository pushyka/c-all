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

    public partial class Display : Form
    {
        private GameController gameController = null;
        private TableLayoutPanel genericBoardBase = null;
        private LoadForm loadform;
        private string USRMOVE = "";
        private List<Control> tintRef = null;
        private EGameModels selectedGameModel;

        // provides conversion of column to chess file
        private Dictionary<int, char> colToFile = new Dictionary<int, char>
        {
            {0, 'a'}, {1, 'b'}, {2, 'c'}, {3, 'd'}, {4, 'e'}, {5, 'f'}, {6, 'g'}, {7, 'h'}
        };

        // provides conversion of row to chess rank
        private Dictionary<int, int> rowToRank = new Dictionary<int, int>
        {
            {0, 8}, {1, 7}, {2, 6}, {3, 5}, {4, 4}, {5, 3}, {6, 2}, {7, 1}
        };

        // provides association of game pieces with their image
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


        /* Manual partial of the Main Display form.
        This form contains code for creating and updating the
        display and handling inputs from users and sending them
        to the gameController for processing. This constructor 
        takes the game controller gc, assigns it to a local reference
        and performs the initialisation of Display elements. */
        public Display(GameController gc)
        {
            this.gameController = gc;
            // triple buffer
            InitializeComponent();
            InitializeOtherComponents();

            tintRef = new List<Control>(); //reset?
            //testc();
        }

        private void testc()
        {
            PromotionSelection p = new PromotionSelection(new Piece(EGamePieces.BlackPawn));
            //p.ControlBox = false;
            p.ShowDialog(this);
        }
        
        /* Generates a chess board and adds it to the parent
        Display (main) form's Control. Makes it visible and 
        responsive to user interaction. The tiles have a click
        handler registered to them. */
        private void AssembleChessBoard()
        {
            this.genericBoardBase = new TableLayoutPanel();
            this.genericBoardBase.RowCount = 8;
            this.genericBoardBase.ColumnCount = 8;
            this.genericBoardBase.Location = new System.Drawing.Point(46, 83);
            this.genericBoardBase.Name = "genericBoardBase";
            this.genericBoardBase.Size = new System.Drawing.Size(400, 400);
            this.genericBoardBase.TabIndex = 4;
            this.genericBoardBase.BorderStyle = BorderStyle.FixedSingle;
            
            generateTilesForBase();

            // add it to the main display form
            this.Controls.Add(this.genericBoardBase);

            this.genericBoardBase.Visible = true;
            this.genericBoardBase.Enabled = true;
        }


        /* Generates a generic TicTacToe board and adds it to 
        the parent Display (main) form's Control. Makes it visible and 
        responsive to user interaction. The tiles have a click
        handler registered to them. */
        private void AssembleTTTBoard()
        {
            this.genericBoardBase = new TableLayoutPanel();
            this.genericBoardBase.RowCount = 3;
            this.genericBoardBase.ColumnCount = 3;
            this.genericBoardBase.Location = new System.Drawing.Point(126, 163);
            this.genericBoardBase.Name = "genericBoardBase";
            this.genericBoardBase.BackColor = Color.Black;
            this.genericBoardBase.Size = new System.Drawing.Size(245, 245); // pixel spacing and squares incl
            this.genericBoardBase.TabIndex = 4;
            
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
                    
                    this.genericBoardBase.Controls.Add(tile);
                }
            }


            // add it to the main display form
            this.Controls.Add(this.genericBoardBase);
            this.genericBoardBase.Visible = true;
            this.genericBoardBase.Enabled = true;
        }


        /* Procedure for generating and adding the Chess board tiles to the chess board base.
        The click handler is registered to each of the tiles. */
        private void generateTilesForBase()
        {
            for (int row = 0; row < 8; row ++)
            {
                for (int col = 0; col < 8; col ++)
                {
                    Panel tile = new Panel();
                    tile.Size = new System.Drawing.Size(50, 50);
                    // remove the space between tiles 
                    tile.Margin = new Padding(0);

                    if ((row % 2 == 0 && col % 2 == 0) ||
                        (row % 2 == 1 && col % 2 == 1))
                        tile.BackColor = Color.BlanchedAlmond;
                    else
                        tile.BackColor = Color.Peru;

                    tile.Click += OnTileClick;
                    this.genericBoardBase.Controls.Add(tile);
                }
            }
        }


        /* Additional initialisation code. */
        private void InitializeOtherComponents()
        {
            this.concedeButton.FlatStyle = FlatStyle.Flat;
            loadform = new LoadForm();
        }
        

        /* Handler registered to the new chess game menu item.
        This code clears any games if they are already in progress,
        tells the controller to prepare a game of type chess,
        creates the View components for the game,
        registers event handlers to the gameModels displayable events,
        prepares the game model (model.setup) (the view will now update accordingly),
        adjusts final view components,
        finally tells the controller to begin the chess game loop. */
        private void menuItemChess_Click(object sender, EventArgs e)
        {
            // if a game was already in progress, clear it first
            if (this.gameController.State == EGameControlState.GameInProgress)
                abandonGameToolStripMenuItem_Click(null, null);

            this.selectedGameModel = EGameModels.Chess;

            // Model
            this.gameController.InitialiseModel(this.selectedGameModel, this); 

            // View
            AssembleChessBoard();
            
            // register event handlers to the chess model
            this.gameController.PropertyChanged += message_PropertyChanged;
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
            // start game loop
            this.gameController.StartGameLoop(this.selectedGameModel);
        }


        /* Handler registered to the new TicTacToe game menu item.
        This code clears any games if they are already in progress,
        tells the controller to prepare a game of type TicTacToe,
        creates the View components for the game,
        registers event handlers to the gameModels displayable events,
        prepares the game model (model.setup),
        adjusts final view components,
        finally tells the controller to begin the TicTacToe game loop. */
        private void menuItemTTT_Click(object sender, EventArgs e)
        {
            // if a game was already in progress, clear it first
            if (this.gameController.State == EGameControlState.GameInProgress)
                abandonGameToolStripMenuItem_Click(null, null);

            this.selectedGameModel = EGameModels.TicTacToe;

            // Model
            this.gameController.InitialiseModel(this.selectedGameModel, this);

            // View
            AssembleTTTBoard();

            // register event handlers to the ttt model
            this.gameController.PropertyChanged += message_PropertyChanged;
            this.gameController.Model.BoardChanged += TTTBoardChanged;

            // This comes after the handlers are registered so that
            // the boardchanged handler may update the view.
            this.gameController.PrepareModel(this.selectedGameModel);

            this.infoPanel.Visible = false;
            this.menuItemNewGame.Enabled = true;
            this.loadGameToolStripMenuItem.Enabled = false;
            this.abandonGameToolStripMenuItem.Enabled = true;
            this.message_box.Enabled = true;
            this.message_box.Visible = true;

            this.gameController.StartGameLoop(this.selectedGameModel);
        }


        /* Abandon menu item click handler. 
        If this menu item is clicked this code will stop the game loop if one is
        running, deregister the handlers from the currently selected game model,
        uninitialise the models, reset the view. If during a game switch, doesn't 
        need to reset entirety of elements. Else, reset entirety of view elements.
        (message box, menu items etc). */
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

        
        /* As part of the abandon procedure of a game, remove the handlers registered to
        the current game model. */
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
                    this.gameController.Model.BoardChanged -= TTTBoardChanged;
                    break;
            }

        }


        /* Part of the reset view procedure. Removes the 
        board from the display. */
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


        /* Takes a piece identifier and returns the image/icon for that
        piece in a picturebox component. Ready for placement on the 
        View board component. The click handler is registered to the 
        piece also. */
        private PictureBox getClickableGuiPiecee(EGamePieces mPiece)
        {
            PictureBox pb = new PictureBox();
            pb.Name = "pb";
            pb.Size = new Size(50, 50);
            pb.Image = gamePieces[mPiece];
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Click += OnTileClick;
            return pb;
        }


        /* Takes a piece identifier and returns the image/icon for that
        piece in a picturebox component. Ready for placement on the 
        View board component. There is no click handler registered 
        so this may be used for the promotionSelection dialog (gets a 
        different click handler) or the captured display. */
        public static PictureBox getGuiPiece(EGamePieces mPiece)
        {
            PictureBox pb = new PictureBox();
            pb.Name = "pb";
            pb.Size = new Size(50, 50);
            pb.Image = gamePieces[mPiece];
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            return pb;
        }
        

        /* First click handler, registers a click and sends the information off to the clickhandler
        designated for the current selected game model. */
        private void OnTileClick(object sender, EventArgs e)
        {
            switch(selectedGameModel)
            {
                case EGameModels.Chess:
                    processChessClicks(sender, e);
                    break;
                case EGameModels.TicTacToe:
                    processTicTacToeClicks(sender, e);
                    break;
            }
        }
        
        
        /* This function when called, generates a tile location eg("B3") based on what the user
        has clicked.It will store the first tile location in a persistent field variable. 'USRMOVE'
        When USRMOVE has received two locations it will send them to the evaluator.The evaluator will do any
        checking, including for correct player pieces clicked. So this event handler is ready to be
        used at any stage of the programs running. 
        The tile itself and the Piece on the tile: their click events are both directed here.
        For the piece, the function must get its container control (the empty Panel). */
        private void processChessClicks(object sender, EventArgs e)
        {
            string tileClicked = "";

            // if clicked on a tile
            if (sender is Panel)
            {
                Panel tile = (Panel)sender;
                TableLayoutPanelCellPosition pos;
                if (tile.Name == "tint")
                {
                    /* then the user has clicked already tinted panel (same panel of this turn so they want to unselect it)
                    send the duplicate tiles as normal, the evaluator will detect the error
                    and will remove the tint after sending it like normal.
                    The purpose of these lines is to simply switch from the tinted tile back to the parent
                    guiTile, (this may be the parent if empty, or the parents parent if a piece square)
                    since only the guiTile can provide the position info required for the evaluator */
                    if (tile.Parent is Panel)
                        pos = this.genericBoardBase.GetPositionFromControl(tile.Parent);
                    else if (tile.Parent is PictureBox)
                        pos = this.genericBoardBase.GetPositionFromControl(tile.Parent.Parent);
                    else
                        throw new ArgumentException($"The value for tile.Parent is unexpected: {tile.Parent}");
                }
                else
                {
                    addTint(tile);
                    pos = this.genericBoardBase.GetPositionFromControl(tile);
                }
                tileClicked += colToFile[pos.Column].ToString() + rowToRank[pos.Row].ToString();
            }
            // else clicked on a piece
            else if (sender is PictureBox)
            {
                PictureBox picture = (PictureBox)sender;
                addTint(picture);

                // get the parent Panel container of the picturebox
                Panel tile = (Panel)picture.Parent;
                TableLayoutPanelCellPosition pos = this.genericBoardBase.GetPositionFromControl(tile);
                tileClicked += colToFile[pos.Column].ToString() + rowToRank[pos.Row].ToString();
            }
            



            if (USRMOVE == "")
                USRMOVE += tileClicked;
            // else the first click has already been added to usermove, so add the second
            else 
            {
                USRMOVE += ' ' + tileClicked;
                // send the USRMOVE to the controller
                this.gameController.Input = USRMOVE;
                // clear USRMOVE for next turn
                USRMOVE = "";
                // this delay before clearing the tints allows it to appear briefly on the second clicked tile
                Thread.Sleep(100);
                removeTint();
            }
        }


        /* This handler function generates a position which has been clicked.
        Adds the position to USRMOVE, since its TicTacToe it sends that first
        click as the Full USRMOVE to the controller. Then resets, awaiting the
        next click. */
        private void processTicTacToeClicks(object sender, EventArgs e)
        {
            string tileClicked = "";

            if (sender is Panel)
            {
                Panel tile = (Panel)sender;
                TableLayoutPanelCellPosition pos = this.genericBoardBase.GetPositionFromControl(tile);
                tileClicked += pos.Column.ToString() +pos.Row.ToString();
            }
            if (USRMOVE == "")
            {
                USRMOVE += tileClicked;
                this.gameController.Input = USRMOVE;
                USRMOVE = "";
            }
        }


        /* Creates a yellow-tinted panel and adds it on top of the 
        tile indicated by the tileref argument. */
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

        
        /* Remove the tints from the 1 or 2 tiles referenced in the this.tintref List. */
        private void removeTint()
        {
            foreach (Control c in this.tintRef)
            {
                // the control is either a panel or a PictureBox
                // with only 1 control within it, the tint panel
                foreach (Control tp in c.Controls.OfType<Panel>())
                    c.Controls.Remove(tp);
            }
            this.tintRef.Clear();
        }


        /* MouseEnter tile tint. */
        private void Tile_MouseEnter(object sender, EventArgs e)
        {
            //Panel tile = (Panel)sender;
            //tile.BackColor = Color.FromArgb(25, tile.BackColor);
          // todo
            //    modify it slightly and restore on leave
        }

        
        /* == cross thread calls ==
        *  Most of the changes to the message variable are occuring inside the gameLoop child thread.
        *  So this thread is raising the PropertyChanged event and the handler and this is all within
        *  that child thread. When it tries to update the form control message_box, there is a problem
        *  since the message_box was created in the main gui thread, and this is detected as an unsafe
        *  cross thread operation. 
        *  Need to call InvokeRequired which returns true if that message_box was created in a different
        *  thread than the one currently executing. Then calling invoke and passing it a callback to this
        *  same function, allows it to do the execution on that parent thread asynchronously.
        */

        delegate void ChessBoardChangedCallback(object sender, BoardChangedEventArgs e);
        /* This is the handler registered to the ChessBoardChanged event. Code calling this method resides in a separate thread
        (a non display/form thread) so the code must invoke back into the display/form thread. 
        
        This function received from the BoardChangedEventArgs a list of model positions which have been
        changed, causing this event. It looks at each changed position, computes the corresponding View
        position, and updates it according to what is in the model position.*/
        private void ChessBoardChanged(object sender, BoardChangedEventArgs e)
        {
            if (this.genericBoardBase.InvokeRequired)
            {
                ChessBoardChangedCallback d = new ChessBoardChangedCallback(ChessBoardChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                List<Tuple<int, int>> positionsChanged = e.PositionsChanged;
                foreach (Tuple<int, int> pos in positionsChanged)
                {
                    // get corresponding gui position
                    Panel gTile = (Panel)this.genericBoardBase.GetControlFromPosition(pos.Item2, pos.Item1);
                    EGamePieces mPiece;
                    if (!this.gameController.Model.Board[pos.Item1, pos.Item2].IsEmpty())
                        mPiece = this.gameController.Model.Board[pos.Item1, pos.Item2].piece.Val;
                    else
                        mPiece = EGamePieces.empty;

                    // remove all existing items on the tile (picture boxes if any)
                    foreach (Control pb in gTile.Controls.OfType<PictureBox>())
                        gTile.Controls.Remove(pb);

                    // if mPiece was changed to empty, then this has satisfactorily cleared the tile
                    // otherwise mPiece is a piece which needs to be added to the now empty tile
                    if (gamePieces.ContainsKey(mPiece))
                    {
                        PictureBox gPiece = getClickableGuiPiecee(mPiece);
                        gTile.Controls.Add(gPiece);
                    }
                }
            }
        }


        delegate void message_PropertyChangedCallback(object sender, PropertyChangedEventArgs e);
        /* Handler for the PropertyChanged event raised when gameController.Message is changed.
        When this event occurs, the function updates the message ListBox with 
        the new value. */
        private void message_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.genericBoardBase.InvokeRequired)
            {
                // delegate d a pointer to the function
                message_PropertyChangedCallback d = new message_PropertyChangedCallback(message_PropertyChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            // this version appends messages
            // maybe make it to display 2 items
            else
            {
                this.message_box.BeginUpdate();
                //this.message_box.Items.Clear();
                this.message_box.Items.Add(this.gameController.Message);
                // set most recently added as the last one
                this.message_box.TopIndex = this.message_box.Items.Count - 1;
                this.message_box.EndUpdate();
            }
        }


        delegate void ChessBoardCaptureChangedCallback(object sender, EventArgs e);
        /* Handler for the ChessBoardCapture Event.
        When this event is raised the captured model piece is added to the .PiecesCapd property.
        This code gets the most recently added piece from .PiecesCapd and adds the view piece corresponding 
        to it to the appropriate side of the board. So the White player will have a collection of Black pieces
        near to his end of the board of the pieces he has captured over the course of the game. */
        private void ChessBoardCaptureChanged(object sender, EventArgs e)
        {
            if (this.genericBoardBase.InvokeRequired)
            {
                ChessBoardCaptureChangedCallback d = new ChessBoardCaptureChangedCallback(ChessBoardCaptureChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                try
                {
                    // get the most recent captured piece (appended to the list)
                    EGamePieces capturedPiece = gameController.Model.PiecesCapd[gameController.Model.PiecesCapd.Count - 1];
                    // uses version of the function which doesn't add click handler
                    PictureBox gCapturedPiece = getGuiPiece(capturedPiece);
                    // determine which side to add the guiPiece to for display
                    if ((int)(capturedPiece) >= 6 && (int)capturedPiece < 12)
                        // add to the black display
                        this.blackPiecesCaptured.Controls.Add(gCapturedPiece);
                    else if ((int)capturedPiece < 6)
                        this.whitePiecesCaptured.Controls.Add(gCapturedPiece);
                }
                catch (Exception)
                {
                    // ?
                }
            }
        }

        
        delegate void ChessBoardPlayerChangedCallback(object sender, EventArgs e);
        /* Handler for the ChessBoardPlayerChanged event.
        When this code is called, update the View current turn panel indicator squares
        to reflect the current player (the value of model.player.playervalue). This
        function displays a coloured square to indicate whos turn it is. */
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

        
        delegate void TTTBoardChangedCallback(object sender, BoardChangedEventArgs e);
        /* This is the handler registered to the TTTBoardChanged event. Code calling this method resides in a separate thread
        (a non display/form thread) so the code must invoke back into the display/form thread. 

        This function received from the BoardChangedEventArgs a list of model positions which have been
        changed, causing this event. It looks at each changed position, computes the corresponding View
        position, and updates it according to what is in the model position.*/
        private void TTTBoardChanged(object sender, BoardChangedEventArgs e)
        {
            if (this.genericBoardBase.InvokeRequired)
            {
                TTTBoardChangedCallback d = new TTTBoardChangedCallback(TTTBoardChanged);
                this.genericBoardBase.Invoke(d, new object[] { sender, e });
            }
            else
            {
                // for tictactoe there is only ever one position changed at a time
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
                        PictureBox gPiece = getGuiPiece(mPiece);
                        gTile.Controls.Add(gPiece);
                    }
                }
            }
        }
        

        /* Click handler registered to the concede button which is displayed during
        forfeit-able games. It removes any lingering tint, disables the board and sends
        the 'concede' keyword to the controller. The controller then handles this accordingly. */
        private void concedeButton_Click(object sender, EventArgs e)
        {
            removeTint();
            this.genericBoardBase.Enabled = false;
            this.concedeButton.Visible = false;
            this.gameController.Input = "concede";
        }


        /* Click handler for the load game menu item.
        Opens a dialog where the user can browse to open and 
        load a saved game. */
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


        /* Handler registered to the close menu item.
        This simply stops the game loop thread before 
        ending the main thread and exiting the program. */
        private void menuItem_close_Click(object sender, EventArgs e)
        {
            gameController.StopGameLoop();
            this.Close();
        }


        /* This overrides the program closing if the 'X' button is pressed
        to stop the game loop thread first before stopping the main thread and exiting. */
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;
            // else user is closing so stop the game loop if it is running
            gameController.StopGameLoop();
        }
    }
}
