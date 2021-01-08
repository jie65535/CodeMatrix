using System.Drawing;

namespace CodeMatrix
{
    internal class Styles
    {
        public static Styles Default { get; } = new Styles();

        public Styles()
        {
            Font = new Font("微软雅黑", 14);
            BackColor = Color.FromArgb(18, 13, 25);
            ForeColor = Color.FromArgb(208, 236, 92);
            CodeBrush = new SolidBrush(Color.FromArgb(208, 236, 92));
            EmptyCellBrush = new SolidBrush(Color.FromArgb(65, 52, 76));
            DefaultLineBackColor = new SolidBrush(Color.FromArgb(42, 43, 60));
            SelectLineBackColor = new SolidBrush(Color.FromArgb(32, 31, 28));
            SelectCellBorderPen = new Pen(Color.FromArgb(109, 232, 228), 1);
        }

        public Font Font { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public Brush CodeBrush { get; set; }
        public Brush EmptyCellBrush { get; set; }
        public Brush DefaultLineBackColor { get; set; }
        public Brush SelectLineBackColor { get; set; }
        public Pen SelectCellBorderPen { get; set; }
    }
}