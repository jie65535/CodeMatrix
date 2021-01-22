using System.Drawing;
using System.Windows.Forms;

namespace CodeMatrix.BaseControl
{
    internal class MyButton : Button
    {
        public MyButton()
        {
            BackColor = Color.Transparent;
            FlatAppearance.BorderColor = Styles.Default.SelectColor;
            FlatAppearance.MouseDownBackColor = Color.FromArgb(100, Styles.Default.SelectColor);
            FlatAppearance.MouseOverBackColor = Color.FromArgb(50, Styles.Default.SelectColor);
            FlatStyle = FlatStyle.Flat;
            ForeColor = Styles.Default.SelectColor;
            Size = new Size(90, 30);
        }
    }
}