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
            CellSizeA = new Size(40, 40);
            CellSizeB = new Size(25, 25);
            BackColor = Color.FromArgb(18, 13, 25);
            CodeColor = Color.FromArgb(208, 236, 92);
            SelectColor = Color.FromArgb(109, 232, 228);
            CodeBrush = new SolidBrush(CodeColor);
            SelectBrush = new SolidBrush(SelectColor);
            ToBeSelectBrush = Brushes.White;
            EmptyCellBrush = new SolidBrush(Color.FromArgb(65, 52, 76));
            DefaultLineBrush = new SolidBrush(Color.FromArgb(42, 43, 60));
            SelectLineBrush = new SolidBrush(Color.FromArgb(32, 31, 28));
            PassBrush = new SolidBrush(Color.FromArgb(31, 248, 136));
            RejectBrush = new SolidBrush(Color.FromArgb(255, 100, 92));
            CurrIndexBrush = new SolidBrush(Color.FromArgb(26, 27, 46));
            SelectCellBorderPen = new Pen(SelectColor, 1);
            EmptyCellBorderPen = new Pen(Color.FromArgb(116, 144, 0), 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Custom,
                DashPattern = new float[2] { 6, 6 },
                DashOffset = 3F
            };
            SelectedCellBorderPen = new Pen(CodeColor, 1);
            DefaultBorderPen = new Pen(CodeColor, 1);
        }

        public Font CodeFontA { get; set; }
        public Font CodeFontB { get; set; }
        public Size CellSizeA { get; set; }
        public Size CellSizeB { get; set; }
        public Color BackColor { get; set; }
        public Color CodeColor { get; set; }
        public Color SelectColor { get; set; }
        public Brush CodeBrush { get; set; }
        public Brush SelectBrush { get; set; }
        public Brush ToBeSelectBrush { get; set; }
        public Brush EmptyCellBrush { get; set; }
        public Brush DefaultLineBrush { get; set; }
        public Brush SelectLineBrush { get; set; }
        public Brush PassBrush { get; set; }
        public Brush RejectBrush { get; set; }
        public Brush CurrIndexBrush { get; set; }
        public Pen SelectCellBorderPen { get; set; }
        public Pen EmptyCellBorderPen { get; set; }
        public Pen SelectedCellBorderPen { get; set; }
        public Pen DefaultBorderPen { get; set; }
    }
}