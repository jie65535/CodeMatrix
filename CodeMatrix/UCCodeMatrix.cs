using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeMatrix
{
    internal class UCCodeMatrix : Control
    {
        private static readonly byte[] Codes = new byte[] { 0x55, 0x1C, 0xBD, 0xE9, 0x7A };

        private static readonly Random Random = new Random();
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
        private enum Directions
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
            InitData();
            InitComponent();
        }

        private void InitData()
        {
            Rows = 5;
            Columns = 5;
            CellSize = new SizeF(40, 40);
            _currDir = Directions.Horizontal;
        }

        private void InitComponent()
        {
            MinimumSize = new Size((int)(CellSize.Width * Columns + 100), (int)(CellSize.Height * Rows + 10));
            Margin = Padding.Empty;
            DoubleBuffered = true;

            Font = Styles.Default.Font;
            BackColor = Styles.Default.BackColor;
            ForeColor = Styles.Default.ForeColor;
        }

        private bool _IsLoaded;

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible && !_IsLoaded)
            {
                Load();
                _IsLoaded = true;
            }
            base.OnVisibleChanged(e);
        }

        private void Load()
        {
            Matrix = new byte[Columns, Rows];
            for (int col = 0; col < Columns; col++)
                for (int row = 0; row < Rows; row++)
                    Matrix[col, row] = Codes[Random.Next(Codes.Length)];

            var blockSize = new SizeF(Columns*CellSize.Width, Rows*CellSize.Height);
            var blockOffset = new PointF((Width-blockSize.Width)/2, (Height-blockSize.Height)/2);
            CodeMatrixRect = new RectangleF(blockOffset, blockSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_IsLoaded) return;

            var offset = new PointF(SelectPoint.X*CellSize.Width, SelectPoint.Y*CellSize.Height);
            if (_currDir == Directions.Horizontal)
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBackColor, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
            else if (_currDir == Directions.Vertical)
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBackColor, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);

            Cursor = Cursors.Default;
            if (CursorPosition.X >= 0 && Matrix[CursorPosition.X, CursorPosition.Y] != 0)
            {
                if (_currDir == Directions.Horizontal)
                {
                    if (CursorPosition.Y == SelectPoint.Y)
                    {
                        offset.X = CursorPosition.X * CellSize.Width;
                        e.Graphics.FillRectangle(Styles.Default.SelectLineBackColor, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);
                        Cursor = Cursors.Hand;
                    }
                }
                else if (_currDir == Directions.Vertical)
                {
                    if (CursorPosition.X == SelectPoint.X)
                    {
                        offset.Y = CursorPosition.Y * CellSize.Height;
                        e.Graphics.FillRectangle(Styles.Default.SelectLineBackColor, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
                        Cursor = Cursors.Hand;
                    }
                }

                if (Cursor == Cursors.Hand)
                {
                    offset.X += CodeMatrixRect.X;
                    offset.Y += CodeMatrixRect.Y;
                    e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X, offset.Y, CellSize.Width - 1, CellSize.Height - 1);
                    e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X + 4, offset.Y + 4, CellSize.Width - 8 - 1, CellSize.Height - 8 - 1);
                }
            }

            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    var cellOffset = new PointF(col*CellSize.Width, row*CellSize.Height);
                    //var cellRect = new RectangleF(cellOffset, CellSize);
                    Brush brush;
                    string code;
                    if (Matrix[col, row] == 0)
                    {
                        code = "[  ]";
                        brush = Styles.Default.EmptyCellBrush;
                    }
                    else
                    {
                        brush = Styles.Default.CodeBrush;
                        code = Matrix[col, row].ToString("X2");
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