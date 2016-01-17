using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace chess.View
{
    public partial class PromotionSelection : Form
    {

        private Model.EGamePieces selectedPiece;

        public PromotionSelection(chess.Model.Piece mvPawn)
        {
            InitializeComponent();
            // add the images to the picture boxes based on the colour of the mving pawn
            PictureBox queen, knight, rook, bishop;
            if (mvPawn.Val == Model.EGamePieces.WhitePawn)
            {
                queen = View.Display.getGuiPiece2(Model.EGamePieces.WhiteQueen);
                knight = View.Display.getGuiPiece2(Model.EGamePieces.WhiteKnight);
                rook = View.Display.getGuiPiece2(Model.EGamePieces.WhiteRook);
                bishop = View.Display.getGuiPiece2(Model.EGamePieces.WhiteBishop);
                queen.Tag = Model.EGamePieces.WhiteQueen;
                knight.Tag = Model.EGamePieces.WhiteKnight;
                rook.Tag = Model.EGamePieces.WhiteRook;
                bishop.Tag = Model.EGamePieces.WhiteBishop;
            }
            else
            { 
                queen = View.Display.getGuiPiece2(Model.EGamePieces.BlackQueen);
                knight = View.Display.getGuiPiece2(Model.EGamePieces.BlackKnight);
                rook = View.Display.getGuiPiece2(Model.EGamePieces.BlackRook);
                bishop = View.Display.getGuiPiece2(Model.EGamePieces.BlackBishop);
                queen.Tag = Model.EGamePieces.BlackQueen;
                knight.Tag = Model.EGamePieces.BlackKnight;
                rook.Tag = Model.EGamePieces.BlackRook;
                bishop.Tag = Model.EGamePieces.BlackBishop;
            }



            queen.Click += promotionPieceSelect_Click;
            knight.Click += promotionPieceSelect_Click;
            rook.Click += promotionPieceSelect_Click;
            bishop.Click += promotionPieceSelect_Click;

            this.panelQueen.Controls.Add(queen);
            this.panelKnight.Controls.Add(knight);
            this.panelRook.Controls.Add(rook);
            this.panelBishop.Controls.Add(bishop);

            System.Console.WriteLine("IVE BEEN CREATED");


        }

        private void promotionPieceSelect_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            this.selectedPiece = (Model.EGamePieces)p.Tag;
            this.DialogResult = DialogResult.OK;

        }

        public Model.EGamePieces SelectedPiece
        {
            get
            {
                return this.selectedPiece;
            }
        }
    }
}
