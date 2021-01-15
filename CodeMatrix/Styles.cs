using System.Drawing;

namespace CodeMatrix
{
    internal class Styles
    {
        public static Styles Default { get; } = new Styles();

        public Styles()
        {
            CodeFontA = new Font("微软雅黑", 14);
            CodeFontB = new Font("微软雅黑", 12);
            BackColor = Color.FromArgb(18, 13, 25);
            CodeColor = Color.FromArgb(208, 236, 92);
            SelectColor = Color.FromArgb(109, 232, 228);
            CodeBrush = new SolidBrush(CodeColor);
            SelectBrush = new SolidBrush(SelectColor);
            EmptyCellBrush = new SolidBrush(Color.FromArgb(65, 52, 76));
            DefaultLineBackColor = new SolidBrush(Color.FromArgb(42, 43, 60));
            SelectLineBackColor = new SolidBrush(Color.FromArgb(32, 31, 28));
            SelectCellBorderPen = new Pen(SelectColor, 1);
            EmptyCellBorderPen = new Pen(Color.FromArgb(116, 144, 0), 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Custom,
                DashPattern = new float[2] { 4, 4 }
            };
            SelectedCellBorderPen = new Pen(CodeColor, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Custom,
                DashPattern = new float[2] { 3, 3 }
            };
            DefaultBorderPen = new Pen(CodeColor, 1);
        }

        public Font CodeFontA { get; set; }
        public Font CodeFontB { get; set; }
        public Color BackColor { get; set; }
        public Color CodeColor { get; set; }
        public Color SelectColor { get; set; }
        public Brush CodeBrush { get; set; }
        public Brush SelectBrush { get; set; }
        public Brush EmptyCellBrush { get; set; }
        public Brush DefaultLineBackColor { get; set; }
        public Brush SelectLineBackColor { get; set; }
        public Pen SelectCellBorderPen { get; set; }
        public Pen EmptyCellBorderPen { get; set; }
        public Pen SelectedCellBorderPen { get; set; }
        public Pen DefaultBorderPen { get; set; }
    }
}