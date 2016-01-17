namespace chess.View
{
    partial class PromotionSelection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBishop = new System.Windows.Forms.Panel();
            this.panelRook = new System.Windows.Forms.Panel();
            this.panelKnight = new System.Windows.Forms.Panel();
            this.panelQueen = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Queen";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Knight";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Rook";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(303, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Bishop";
            // 
            // panelBishop
            // 
            this.panelBishop.Location = new System.Drawing.Point(306, 88);
            this.panelBishop.Name = "panelBishop";
            this.panelBishop.Size = new System.Drawing.Size(55, 49);
            this.panelBishop.TabIndex = 11;
            this.panelBishop.Tag = "";
            // 
            // panelRook
            // 
            this.panelRook.Location = new System.Drawing.Point(217, 88);
            this.panelRook.Name = "panelRook";
            this.panelRook.Size = new System.Drawing.Size(48, 49);
            this.panelRook.TabIndex = 10;
            this.panelRook.Tag = "";
            // 
            // panelKnight
            // 
            this.panelKnight.Location = new System.Drawing.Point(124, 88);
            this.panelKnight.Name = "panelKnight";
            this.panelKnight.Size = new System.Drawing.Size(51, 49);
            this.panelKnight.TabIndex = 9;
            this.panelKnight.Tag = "";
            // 
            // panelQueen
            // 
            this.panelQueen.Location = new System.Drawing.Point(27, 88);
            this.panelQueen.Name = "panelQueen";
            this.panelQueen.Size = new System.Drawing.Size(51, 49);
            this.panelQueen.TabIndex = 8;
            this.panelQueen.Tag = "";
            // 
            // PromotionSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 207);
            this.Controls.Add(this.panelBishop);
            this.Controls.Add(this.panelRook);
            this.Controls.Add(this.panelKnight);
            this.Controls.Add(this.panelQueen);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PromotionSelection";
            this.Text = "Promotion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelBishop;
        private System.Windows.Forms.Panel panelRook;
        private System.Windows.Forms.Panel panelKnight;
        private System.Windows.Forms.Panel panelQueen;
    }
}