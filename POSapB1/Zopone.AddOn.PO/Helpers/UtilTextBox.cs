using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zopone.AddOn.PO.View.Obra.Helpers
{
    public class UtilTextBox 
    {
        public static Color defaultColor = Color.White;
        public static Color focusColor = Color.FromArgb(254, 240, 158);


        public static void ExtOnGotFocus(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            textBox.BackColor = focusColor;
        }

        public static void MskOnGotFocus(object sender, EventArgs e)
        {
            MaskedTextBox maskedEdit = sender as MaskedTextBox;

            maskedEdit.BackColor = focusColor;
        }

        public static void ExtOnLostFocus(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            textBox.BackColor = defaultColor;
        }

        public static void MskOnLostFocus(object sender, EventArgs e)
        {
            MaskedTextBox maskedEdit = sender as MaskedTextBox;

            maskedEdit.BackColor = defaultColor;
        }
    }
}
