using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeMatrix
{
    class UCCodeMatrix : Control
    {
        static readonly byte[] Codes = new byte[] { 0x55, 0x1C, 0xBD, 0xE9, 0x7A };

        static readonly Random Random = new Random();
        public SizeF CellSize { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public byte[,] Matrix { get; set; }
        public Point SelectPoint { get; private set; }
        public Point CursorPosition { get; private set; }
        public RectangleF CodeMatrixRect { get; private set; }

        /// <summary>
        /// 方向
        /// </summary>
        enum Directions
        {
            /// <summary>
            /// 垂直方向
            /// </summary>
            Vertical,

            /// <summary>
            /// 水平方向
            /// </summary>
            Horizontal,
        }

        private Directions _currDir;

        public UCCodeMatrix()
        {
            Font = new Font("微软雅黑", 14);
            BackColor = Color.FromArgb(18, 13, 25);
            ForeColor = Color.FromArgb(208, 236, 92);
            CodeBrush = new SolidBrush(ForeColor);
            EmptyCellBrush = new SolidBrush(Color.FromArgb(65, 52, 76));
            DefaultLineBackColor = new SolidBrush(Color.FromArgb(42, 43, 60));
            SelectLineBackColor = new SolidBrush(Color.FromArgb(32, 31, 28));
            SelectCellBorderPen = new Pen(Color.FromArgb(109, 232, 228), 1);
            Rows = 5;
            Columns = 5;
            CellSize = new SizeF(40, 40);
            Width = (int)(CellSize.Width * Columns + 100);
            Height = (int)(CellSize.Height * Rows + 10);
            Margin = Padding.Empty;
            _currDir = Directions.Horizontal;
            DoubleBuffered = true;
        }

        bool _IsLoaded;

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible && !_IsLoaded)
            {
                Load();
                _IsLoaded = true;
            }
            base.OnVisibleChanged(e);
        }

        void Load()
        {
            Matrix = new byte[Columns, Rows];
            for (int col = 0; col < Columns; col++)
                for (int row = 0; row < Rows; row++)
                    Matrix[col, row] = Codes[Random.Next(Codes.Length)];

            var blockSize = new SizeF(Columns*CellSize.Width, Rows*CellSize.Height);
            var blockOffset = new PointF((Width-blockSize.Width)/2, (Height-blockSize.Height)/2);
            CodeMatrixRect = new RectangleF(blockOffset, blockSize);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            CodeBrush = new SolidBrush(ForeColor);
            base.OnForeColorChanged(e);
        }

        Brush CodeBrush;
        Brush EmptyCellBrush;
        Brush DefaultLineBackColor;
        Brush SelectLineBackColor;
        Pen   SelectCellBorderPen;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_IsLoaded) return;

            var offset = new PointF(SelectPoint.X*CellSize.Width, SelectPoint.Y*CellSize.Height);
            if (_currDir == Directions.Horizontal)
                e.Graphics.FillRectangle(DefaultLineBackColor, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
            else if (_currDir == Directions.Vertical)
                e.Graphics.FillRectangle(DefaultLineBackColor, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);


            Cursor = Cursors.Default;
            if (CursorPosition.X >= 0 && Matrix[CursorPosition.X, CursorPosition.Y] != 0)
            {
                if (_currDir == Directions.Horizontal)
                {
                    if (CursorPosition.Y == SelectPoint.Y)
                    {
                        offset.X = CursorPosition.X*CellSize.Width;
                        e.Graphics.FillRectangle(SelectLineBackColor, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);
                        Cursor = Cursors.Hand;
                    }
                }
                else if (_currDir == Directions.Vertical)
                {
                    if (CursorPosition.X == SelectPoint.X)
                    {
                        offset.Y = CursorPosition.Y*CellSize.Height;
                        e.Graphics.FillRectangle(SelectLineBackColor, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
                        Cursor = Cursors.Hand;
                    }
                }

                if (Cursor == Cursors.Hand)
                {
                    offset.X += CodeMatrixRect.X;
                    offset.Y += CodeMatrixRect.Y;
                    e.Graphics.DrawRectangle(SelectCellBorderPen, offset.X, offset.Y, CellSize.Width-1, CellSize.Height - 1);
                    e.Graphics.DrawRectangle(SelectCellBorderPen, offset.X+4, offset.Y+4, CellSize.Width-8 - 1, CellSize.Height-8 - 1);
                }
            }


            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    var cellOffset = new PointF(col*CellSize.Width, row*CellSize.Height);
                    //var cellRect = new RectangleF(cellOffset, CellSize);
                    Brush brush = CodeBrush;
                    string code = Matrix[col, row].ToString("X2");
                    if (Matrix[col, row] == 0)
                    {
                        code = "[  ]";
                        brush = EmptyCellBrush;
                    }
                    var codeSize = e.Graphics.MeasureString(code, Font);
                    var codeOffset = new PointF((CellSize.Width-codeSize.Width)/2, (CellSize.Height-codeSize.Height)/2);
                    var codePoint = new PointF(codeOffset.X+cellOffset.X+CodeMatrixRect.X, codeOffset.Y+cellOffset.Y+CodeMatrixRect.Y);
                    e.Graphics.DrawString(code, Font, brush, codePoint);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (CodeMatrixRect.Left <= e.X
                && CodeMatrixRect.Top <= e.Y
                && CodeMatrixRect.Right > e.X
                && CodeMatrixRect.Bottom > e.Y)
            {
                var offset = new PointF(e.X-CodeMatrixRect.X, e.Y-CodeMatrixRect.Y);
                var current = new Point((int)(offset.X / CellSize.Width), (int)(offset.Y / CellSize.Height));
                if (CursorPosition != current)
                {
                    CursorPosition = current;
                    Invalidate();
                }
            }
            else
            {
                if (CursorPosition.X >= 0)
                {
                    CursorPosition = new Point(-1, -1);
                    Invalidate();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (CursorPosition.X >= 0 && Matrix[CursorPosition.X, CursorPosition.Y] != 0)
            {
                if (_currDir == Directions.Horizontal)
                {
                    if (CursorPosition.Y == SelectPoint.Y)
                    {
                        Matrix[CursorPosition.X, CursorPosition.Y] = 0;
                        _currDir = Directions.Vertical;
                        SelectPoint = CursorPosition;
                        Invalidate();
                    }
                }
                else if (_currDir == Directions.Vertical)
                {
                    if (CursorPosition.X == SelectPoint.X)
                    {
                        Matrix[CursorPosition.X, CursorPosition.Y] = 0;
                        _currDir = Directions.Horizontal;
                        SelectPoint = CursorPosition;
                        Invalidate();
                    }
                }
            }


            base.OnMouseClick(e);
        }
    }
}
