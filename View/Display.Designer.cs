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
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_add_Board = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.concedeButton = new System.Windows.Forms.Button();
            this.message_box = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.menuItem_add_Board,
            this.closeToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem2.Text = "Test";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.menuItem_Test_Click);
            // 
            // menuItem_add_Board
            // 
            this.menuItem_add_Board.Name = "menuItem_add_Board";
            this.menuItem_add_Board.Size = new System.Drawing.Size(136, 22);
            this.menuItem_add_Board.Text = "show board";
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
            this.concedeButton.Location = new System.Drawing.Point(517, 305);
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
            this.message_box.Location = new System.Drawing.Point(500, 80);
            this.message_box.Name = "message_box";
            this.message_box.Size = new System.Drawing.Size(242, 95);
            this.message_box.TabIndex = 5;
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.message_box);
            this.Controls.Add(this.concedeButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Display";
            this.Text = "Chess";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuItem_add_Board;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button concedeButton;
        private System.Windows.Forms.ListBox message_box;
    }
}