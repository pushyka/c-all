using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess.View
{
    /// <summary>
    /// This is a subclass of the label which overrides the OnPaint
    /// to add a custom border to the text which is painted. This allows
    /// us to create a white font with a black outline so that it stands
    /// out against its background.
    /// </summary>
    class CustomeOutlineLabel : Label
    {

        public Color OutlineColour { get; set; }
        public float OutlineColourWidth { get; set; }

        public CustomeOutlineLabel()
        {
            OutlineColour = Color.Black;
            OutlineColourWidth = 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // backcolor (transparent)
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

            using (GraphicsPath gp = new GraphicsPath())
            using (Pen outline = new Pen(OutlineColour, OutlineColourWidth)
            { LineJoin = LineJoin.Round })
            using (StringFormat sf = new StringFormat())
            using (Brush foreBrush = new SolidBrush(ForeColor))
            {
                gp.AddString(Text, Font.FontFamily, (int)Font.Style,
                    Font.Size, ClientRectangle, sf);
                e.Graphics.ScaleTransform(1.3f, 1.35f);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawPath(outline, gp);
                e.Graphics.FillPath(foreBrush, gp);
            }
        }


    }
}