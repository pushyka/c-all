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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_add_Board = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.concedeButton = new System.Windows.Forms.Button();
            this.message_box = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.placeholder = new System.Windows.Forms.Panel();
            this.whitePiecesCaptured = new System.Windows.Forms.TableLayoutPanel();
            this.blackPiecesCaptured = new System.Windows.Forms.TableLayoutPanel();
            this.abandonGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(901, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_add_Board,
            this.abandonGameToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // menuItem_add_Board
            // 
            this.menuItem_add_Board.Name = "menuItem_add_Board";
            this.menuItem_add_Board.Size = new System.Drawing.Size(156, 22);
            this.menuItem_add_Board.Text = "New game";
            this.menuItem_add_Board.Click += new System.EventHandler(this.menuItem_addBoard_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // concedeButton
            // 
            this.concedeButton.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.concedeButton.Location = new System.Drawing.Point(214, 135);
            this.concedeButton.Name = "concedeButton";
            this.concedeButton.Size = new System.Drawing.Size(75, 23);
            this.concedeButton.TabIndex = 4;
            this.concedeButton.Text = "Concede";
            this.concedeButton.UseVisualStyleBackColor = false;
            this.concedeButton.Visible = false;
            this.concedeButton.Click += new System.EventHandler(this.concedeButton_Click);
            // 
            // message_box
            // 
            this.message_box.Enabled = false;
            this.message_box.FormattingEnabled = true;
            this.message_box.Location = new System.Drawing.Point(214, 3);
            this.message_box.Name = "message_box";
            this.message_box.Size = new System.Drawing.Size(205, 121);
            this.message_box.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.message_box, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.concedeButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.whitePiecesCaptured, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.blackPiecesCaptured, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(467, 83);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.31543F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34229F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34229F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(422, 397);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // placeholder
            // 
            this.placeholder.Enabled = false;
            this.placeholder.Location = new System.Drawing.Point(60, 80);
            this.placeholder.Name = "placeholder";
            this.placeholder.Size = new System.Drawing.Size(400, 400);
            this.placeholder.TabIndex = 7;
            this.placeholder.Visible = false;
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
            this.whitePiecesCaptured.Size = new System.Drawing.Size(195, 126);
            this.whitePiecesCaptured.TabIndex = 6;
            // 
            // blackPiecesCaptured
            // 
            this.blackPiecesCaptured.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.blackPiecesCaptured.ColumnCount = 8;
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.blackPiecesCaptured.Location = new System.Drawing.Point(3, 267);
            this.blackPiecesCaptured.Name = "blackPiecesCaptured";
            this.blackPiecesCaptured.RowCount = 2;
            this.blackPiecesCaptured.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.blackPiecesCaptured.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.blackPiecesCaptured.Size = new System.Drawing.Size(205, 126);
            this.blackPiecesCaptured.TabIndex = 7;
            // 
            // abandonGameToolStripMenuItem
            // 
            this.abandonGameToolStripMenuItem.Enabled = false;
            this.abandonGameToolStripMenuItem.Name = "abandonGameToolStripMenuItem";
            this.abandonGameToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.abandonGameToolStripMenuItem.Text = "Abandon game";
            this.abandonGameToolStripMenuItem.Click += new System.EventHandler(this.abandonGameToolStripMenuItem_Click);
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(901, 561);
            this.Controls.Add(this.placeholder);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Display";
            this.Text = "Chess";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuItem_add_Board;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button concedeButton;
        private System.Windows.Forms.ListBox message_box;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel placeholder;
        private System.Windows.Forms.TableLayoutPanel whitePiecesCaptured;
        private System.Windows.Forms.TableLayoutPanel blackPiecesCaptured;
        private System.Windows.Forms.ToolStripMenuItem abandonGameToolStripMenuItem;
    }
}