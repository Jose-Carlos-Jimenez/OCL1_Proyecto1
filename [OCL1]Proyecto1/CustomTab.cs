using System.Windows.Forms;
using System.Drawing;
using FastColoredTextBoxNS;
using System;

namespace _OCL1_Proyecto1
{
    public class CustomTab : TabPage
    {
        public CustomTab(String title)
        {
            FastColoredTextBox cajadetexto = new FastColoredTextBox();
            this.Text = title;
            cajadetexto.BackColor = Color.FromArgb(29, 29, 29 );
            cajadetexto.ForeColor = Color.White;
            cajadetexto.Language = Language.PHP;
            this.Controls.Add(cajadetexto);
            cajadetexto.Dock = DockStyle.Fill;
        }
    }
}
