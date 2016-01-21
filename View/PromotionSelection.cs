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


        /* Creates a promotion selection form which lists 
        the pieces which the player may select for upgrading their high ranked pawn.
        The constructor of the form takes the moving pawn as mvPawn in order
        to determine the colour of the upgraded piece to display and use. */
        public PromotionSelection(chess.Model.Piece mvPawn)
        {
            InitializeComponent();

            PictureBox queen, knight, rook, bishop;
            if (mvPawn.Val == Model.EGamePieces.WhitePawn)
            {
                queen = View.Display.getGuiPiece(Model.EGamePieces.WhiteQueen);
                knight = View.Display.getGuiPiece(Model.EGamePieces.WhiteKnight);
                rook = View.Display.getGuiPiece(Model.EGamePieces.WhiteRook);
                bishop = View.Display.getGuiPiece(Model.EGamePieces.WhiteBishop);
                queen.Tag = Model.EGamePieces.WhiteQueen;
                knight.Tag = Model.EGamePieces.WhiteKnight;
                rook.Tag = Model.EGamePieces.WhiteRook;
                bishop.Tag = Model.EGamePieces.WhiteBishop;
            }
            else
            { 
                queen = View.Display.getGuiPiece(Model.EGamePieces.BlackQueen);
                knight = View.Display.getGuiPiece(Model.EGamePieces.BlackKnight);
                rook = View.Display.getGuiPiece(Model.EGamePieces.BlackRook);
                bishop = View.Display.getGuiPiece(Model.EGamePieces.BlackBishop);
                queen.Tag = Model.EGamePieces.BlackQueen;
                knight.Tag = Model.EGamePieces.BlackKnight;
                rook.Tag = Model.EGamePieces.BlackRook;
                bishop.Tag = Model.EGamePieces.BlackBishop;
            }


            // register the click handler to the selection pieces
            queen.Click += promotionPieceSelect_Click;
            knight.Click += promotionPieceSelect_Click;
            rook.Click += promotionPieceSelect_Click;
            bishop.Click += promotionPieceSelect_Click;

            this.panelQueen.Controls.Add(queen);
            this.panelKnight.Controls.Add(knight);
            this.panelRook.Controls.Add(rook);
            this.panelBishop.Controls.Add(bishop);
        }


        /* Click handler for the promotion selection dialog. This assigns the 
        users choice to the SelectedPiece property of the dialog object. This
        property may then be accessed by the evaluator. */
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

        // what if the dialogue is terminated forecfully without a selection made?
    }
}
