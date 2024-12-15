// TransparentTableLayoutPanel.cs
using System;
using System.Windows.Forms;
using System.Drawing;

namespace FP
{
    public class TransparentTableLayoutPanel : TableLayoutPanel
    {
        public TransparentTableLayoutPanel()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background to allow transparency
            // base.OnPaintBackground(e);
        }
    }
}
