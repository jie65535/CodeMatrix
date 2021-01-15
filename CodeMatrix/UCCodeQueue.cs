using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeMatrix
{
    class UCCodeQueue : Control
    {
        public Size CellSize { get; private set; }
        public Padding CellMargin { get; set; }
        public byte HoverCode { get; set; }
        public int BufferSize { get; set; }
        public int CurrIndex { get; private set; }
        public byte[] Buffer { get; } = new byte[32];

        public UCCodeQueue()
        {
            InitData();
            InitComponent();
        }

        private void InitData()
        {
            CellSize = new Size(25, 25);
            BufferSize = 7;

            CurrIndex = 4;
            HoverCode = 0x55;
            Buffer[0] = 0xE9;
            Buffer[1] = 0x55;
            Buffer[2] = 0x1C;
            Buffer[3] = 0xBD;
        }

        private void InitComponent()
        {
            Margin = Padding.Empty;
            Padding = new Padding(10);
            CellMargin = new Padding(3);
            MinimumSize = new Size((CellSize.Width + CellMargin.Horizontal) * BufferSize + Padding.Horizontal,
                                   CellSize.Height + CellMargin.Vertical + Padding.Vertical);
            DoubleBuffered = true;

            Font = Styles.Default.CodeFontB;
            BackColor = Styles.Default.BackColor;
            ForeColor = Styles.Default.CodeColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var cellOffset = new Rectangle(Padding.Left + CellMargin.Left,
                                           Padding.Top + CellMargin.Top,
                                           CellSize.Width-1,
                                           CellSize.Height-1);
            var cellOffsetWidth = CellSize.Width + CellMargin.Horizontal;
            int index = 0;
            for (; index < CurrIndex; index++)
            {
                e.Graphics.DrawRectangle(Styles.Default.SelectedCellBorderPen, cellOffset);
                var code = Buffer[index].ToString("X2");
                var codeSize = e.Graphics.MeasureString(code, Font);
                var codeOffset = new PointF((CellSize.Width-codeSize.Width)/2, (CellSize.Height-codeSize.Height)/2);
                var codePoint = new PointF(codeOffset.X+cellOffset.X, codeOffset.Y+cellOffset.Y);
                e.Graphics.DrawString(code, Font, Styles.Default.CodeBrush, codePoint);
                cellOffset.X += cellOffsetWidth;
            }
            if (HoverCode > 0)
            {
                e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, cellOffset);
                var code = HoverCode.ToString("X2");
                var codeSize = e.Graphics.MeasureString(code, Font);
                var codeOffset = new PointF((CellSize.Width-codeSize.Width)/2, (CellSize.Height-codeSize.Height)/2);
                var codePoint = new PointF(codeOffset.X+cellOffset.X, codeOffset.Y+cellOffset.Y);
                e.Graphics.DrawString(code, Font, Styles.Default.SelectBrush, codePoint);


                cellOffset.X += cellOffsetWidth;
                index++;
            }
            for (; index < BufferSize; index++)
            {
                e.Graphics.DrawRectangle(Styles.Default.EmptyCellBorderPen, cellOffset);
                cellOffset.X += cellOffsetWidth;
            }


            e.Graphics.DrawRectangle(Styles.Default.DefaultBorderPen, 0, 0, Width - 1, Height - 1);

            base.OnPaint(e);
        }
    }
}
