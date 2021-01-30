using DPUruNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color, Font font)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            if (font != null)
            {
                box.SelectionFont = font;
            }

            if (color != null)
            {
                box.SelectionColor = color;
            }            
            
            box.AppendText(text);

            box.SelectionFont = box.Font;
            box.SelectionColor = box.ForeColor;
        }
    }
}
