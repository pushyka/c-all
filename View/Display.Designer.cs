namespace chess.View
{
    partial class Display
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Display()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Display));
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.chessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tTTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abandonGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.white_turn_panel = new System.Windows.Forms.Panel();
            this.black_turn_panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.message_box = new System.Windows.Forms.ListBox();
            this.concedeButton = new System.Windows.Forms.Button();
            this.blackPiecesCaptured = new System.Windows.Forms.TableLayoutPanel();
            this.whitePiecesCaptured = new System.Windows.Forms.TableLayoutPanel();
            this.placeholder = new System.Windows.Forms.Panel();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.customeOutlineLabel2 = new chess.View.CustomeOutlineLabel();
            this.customeOutlineLabel1 = new chess.View.CustomeOutlineLabel();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.infoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNewGame,
            this.loadGameToolStripMenuItem,
            this.abandonGameToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // menuItemNewGame
            // 
            this.menuItemNewGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chessToolStripMenuItem,
            this.tTTToolStripMenuItem});
            this.menuItemNewGame.Name = "menuItemNewGame";
            this.menuItemNewGame.Size = new System.Drawing.Size(156, 22);
            this.menuItemNewGame.Text = "New game";
            // 
            // chessToolStripMenuItem
            // 
            this.chessToolStripMenuItem.Name = "chessToolStripMenuItem";
            this.chessToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.chessToolStripMenuItem.Text = "Chess";
            this.chessToolStripMenuItem.Click += new System.EventHandler(this.menuItemChess_Click);
            // 
            // tTTToolStripMenuItem
            // 
            this.tTTToolStripMenuItem.Name = "tTTToolStripMenuItem";
            this.tTTToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.tTTToolStripMenuItem.Text = "TTT";
            this.tTTToolStripMenuItem.Click += new System.EventHandler(this.menuItemTTT_Click);
            // 
            // loadGameToolStripMenuItem
            // 
            this.loadGameToolStripMenuItem.Name = "loadGameToolStripMenuItem";
            this.loadGameToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.loadGameToolStripMenuItem.Text = "Load game";
            this.loadGameToolStripMenuItem.Click += new System.EventHandler(this.menuItem_loadGame_Click);
            // 
            // abandonGameToolStripMenuItem
            // 
            this.abandonGameToolStripMenuItem.Enabled = false;
            this.abandonGameToolStripMenuItem.Name = "abandonGameToolStripMenuItem";
            this.abandonGameToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.abandonGameToolStripMenuItem.Text = "Abandon game";
            this.abandonGameToolStripMenuItem.Click += new System.EventHandler(this.abandonGameToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.menuItem_close_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(901, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // white_turn_panel
            // 
            this.white_turn_panel.BackColor = System.Drawing.Color.White;
            this.white_turn_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.white_turn_panel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.white_turn_panel.Location = new System.Drawing.Point(432, 489);
            this.white_turn_panel.Name = "white_turn_panel";
            this.white_turn_panel.Size = new System.Drawing.Size(14, 14);
            this.white_turn_panel.TabIndex = 9;
            this.white_turn_panel.Visible = false;
            // 
            // black_turn_panel
            // 
            this.black_turn_panel.BackColor = System.Drawing.Color.Black;
            this.black_turn_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.black_turn_panel.Location = new System.Drawing.Point(432, 63);
            this.black_turn_panel.Name = "black_turn_panel";
            this.black_turn_panel.Size = new System.Drawing.Size(14, 14);
            this.black_turn_panel.TabIndex = 8;
            this.black_turn_panel.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.OldLace;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.message_box, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.concedeButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.blackPiecesCaptured, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.whitePiecesCaptured, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(452, 83);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.31543F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34229F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34229F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(399, 397);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // message_box
            // 
            this.message_box.BackColor = System.Drawing.Color.OldLace;
            this.message_box.Enabled = false;
            this.message_box.FormattingEnabled = true;
            this.message_box.Location = new System.Drawing.Point(202, 3);
            this.message_box.Name = "message_box";
            this.message_box.Size = new System.Drawing.Size(194, 69);
            this.message_box.TabIndex = 5;
            this.message_box.Visible = false;
            // 
            // concedeButton
            // 
            this.concedeButton.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.concedeButton.Location = new System.Drawing.Point(202, 135);
            this.concedeButton.Name = "concedeButton";
            this.concedeButton.Size = new System.Drawing.Size(75, 23);
            this.concedeButton.TabIndex = 4;
            this.concedeButton.Text = "Concede";
            this.concedeButton.UseVisualStyleBackColor = false;
            this.concedeButton.Visible = false;
            this.concedeButton.Click += new System.EventHandler(this.concedeButton_Click);
            // 
            // blackPiecesCaptured
            // 
            this.blackPiecesCaptured.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blackPiecesCaptured.ColumnCount = 8;
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.Location = new System.Drawing.Point(3, 334);
            this.blackPiecesCaptured.Name = "blackPiecesCaptured";
            this.blackPiecesCaptured.RowCount = 2;
            this.blackPiecesCaptured.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.blackPiecesCaptured.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.blackPiecesCaptured.Size = new System.Drawing.Size(193, 60);
            this.blackPiecesCaptured.TabIndex = 7;
            // 
            // whitePiecesCaptured
            // 
            this.whitePiecesCaptured.ColumnCount = 8;
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.whitePiecesCaptured.Location = new System.Drawing.Point(3, 3);
            this.whitePiecesCaptured.Name = "whitePiecesCaptured";
            this.whitePiecesCaptured.RowCount = 2;
            this.whitePiecesCaptured.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.whitePiecesCaptured.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.whitePiecesCaptured.Size = new System.Drawing.Size(193, 60);
            this.whitePiecesCaptured.TabIndex = 8;
            // 
            // placeholder
            // 
            this.placeholder.Enabled = false;
            this.placeholder.Location = new System.Drawing.Point(46, 83);
            this.placeholder.Name = "placeholder";
            this.placeholder.Size = new System.Drawing.Size(400, 400);
            this.placeholder.TabIndex = 7;
            this.placeholder.Visible = false;
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.Color.White;
            this.infoPanel.BackgroundImage = global::chess.Properties.Resources.wood_chess_pieces_board_box_combo_2;
            this.infoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.Controls.Add(this.customeOutlineLabel2);
            this.infoPanel.Controls.Add(this.customeOutlineLabel1);
            this.infoPanel.Location = new System.Drawing.Point(12, 36);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(877, 513);
            this.infoPanel.TabIndex = 10;
            // 
            // customeOutlineLabel2
            // 
            this.customeOutlineLabel2.BackColor = System.Drawing.Color.Transparent;
            this.customeOutlineLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customeOutlineLabel2.ForeColor = System.Drawing.Color.White;
            this.customeOutlineLabel2.Location = new System.Drawing.Point(18, 161);
            this.customeOutlineLabel2.Name = "customeOutlineLabel2";
            this.customeOutlineLabel2.OutlineColour = System.Drawing.Color.Black;
            this.customeOutlineLabel2.OutlineColourWidth = 2F;
            this.customeOutlineLabel2.Size = new System.Drawing.Size(840, 224);
            this.customeOutlineLabel2.TabIndex = 1;
            this.customeOutlineLabel2.Text = resources.GetString("customeOutlineLabel2.Text");
            // 
            // customeOutlineLabel1
            // 
            this.customeOutlineLabel1.AutoSize = true;
            this.customeOutlineLabel1.BackColor = System.Drawing.Color.Transparent;
            this.customeOutlineLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customeOutlineLabel1.ForeColor = System.Drawing.Color.White;
            this.customeOutlineLabel1.Location = new System.Drawing.Point(17, 14);
            this.customeOutlineLabel1.Name = "customeOutlineLabel1";
            this.customeOutlineLabel1.OutlineColour = System.Drawing.Color.Black;
            this.customeOutlineLabel1.OutlineColourWidth = 2F;
            this.customeOutlineLabel1.Size = new System.Drawing.Size(289, 26);
            this.customeOutlineLabel1.TabIndex = 0;
            this.customeOutlineLabel1.Text = "Welcome to Gareth\'s Chess!";
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(901, 561);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.white_turn_panel);
            this.Controls.Add(this.black_turn_panel);
            this.Controls.Add(this.placeholder);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(917, 600);
            this.Name = "Display";
            this.Text = "Chess";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button concedeButton;
        private System.Windows.Forms.ListBox message_box;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel placeholder;
        private System.Windows.Forms.TableLayoutPanel blackPiecesCaptured;
        private System.Windows.Forms.TableLayoutPanel whitePiecesCaptured;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewGame;
        private System.Windows.Forms.ToolStripMenuItem loadGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abandonGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel black_turn_panel;
        private System.Windows.Forms.Panel white_turn_panel;
        private System.Windows.Forms.ToolStripMenuItem chessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tTTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.Panel infoPanel;
        private CustomeOutlineLabel customeOutlineLabel2;
        private CustomeOutlineLabel customeOutlineLabel1;
    }
}