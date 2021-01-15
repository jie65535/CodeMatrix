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
        public Point CursorPoint { get; private set; }
        public Point HoverPoint { get; private set; }
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
            Rows = 7;
            Columns = 7;
            CellSize = new SizeF(40, 40);
            _currDir = Directions.Horizontal;
            HoverPoint = new Point(-1, -1);
        }

        private void InitComponent()
        {
            MinimumSize = new Size((int)(CellSize.Width * Columns + 100), (int)(CellSize.Height * Rows + 10));
            Margin = Padding.Empty;
            DoubleBuffered = true;

            Font = Styles.Default.CodeFontA;
            BackColor = Styles.Default.BackColor;
            ForeColor = Styles.Default.CodeColor;
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

        //int _paintCount;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_IsLoaded) return;
            //_paintCount++;

            var offset = new PointF(SelectPoint.X*CellSize.Width, SelectPoint.Y*CellSize.Height);
            if (_currDir == Directions.Horizontal)
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBackColor, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
            else if (_currDir == Directions.Vertical)
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBackColor, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);

            if (HoverPoint.X >= 0)
            {
                Cursor = Cursors.Hand;
                if (_currDir == Directions.Horizontal)
                {
                    offset.X = HoverPoint.X * CellSize.Width;
                    e.Graphics.FillRectangle(Styles.Default.SelectLineBackColor, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);
                }
                else if (_currDir == Directions.Vertical)
                {
                    offset.Y = HoverPoint.Y * CellSize.Height;
                    e.Graphics.FillRectangle(Styles.Default.SelectLineBackColor, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
                }

                offset.X += CodeMatrixRect.X;
                offset.Y += CodeMatrixRect.Y;
                e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X, offset.Y, CellSize.Width - 1, CellSize.Height - 1);
                e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X + 4, offset.Y + 4, CellSize.Width - 8 - 1, CellSize.Height - 8 - 1);
            }
            else
            {
                Cursor = Cursors.Default;
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
                        if (HoverPoint.X == col && HoverPoint.Y == row)
                            brush = Styles.Default.SelectBrush;
                        else
                            brush = Styles.Default.CodeBrush;
                        code = Matrix[col, row].ToString("X2");
                    }
                    var codeSize = e.Graphics.MeasureString(code, Font);
                    var codeOffset = new PointF((CellSize.Width-codeSize.Width)/2, (CellSize.Height-codeSize.Height)/2);
                    var codePoint = new PointF(codeOffset.X+cellOffset.X+CodeMatrixRect.X, codeOffset.Y+cellOffset.Y+CodeMatrixRect.Y);
                    e.Graphics.DrawString(code, Font, brush, codePoint);
                }
            }

            e.Graphics.DrawRectangle(Styles.Default.DefaultBorderPen, 0, 0, Width - 1, Height - 1);
            //e.Graphics.DrawString(_paintCount.ToString(), Font, Styles.Default.CodeBrush, 0, 0);
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
                if (CursorPoint != current)
                {
                    CursorPoint = current;
                    var hoverPoint = new Point(-1, -1);
                    if (Matrix[CursorPoint.X, CursorPoint.Y] != 0)
                    {
                        if (_currDir == Directions.Horizontal && CursorPoint.Y == SelectPoint.Y)
                            hoverPoint = CursorPoint;
                        else if (_currDir == Directions.Vertical && CursorPoint.X == SelectPoint.X)
                            hoverPoint = CursorPoint;
                    }
                    if (HoverPoint != hoverPoint)
                    {
                        HoverPoint = hoverPoint;
                        Invalidate();
                    }
                    
                }
            }
            else
            {
                if (HoverPoint.X >= 0)
                {
                    HoverPoint = CursorPoint = new Point(-1, -1);
                    Invalidate();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (HoverPoint.X >= 0)
            {
                SelectPoint = HoverPoint;
                HoverPoint = new Point(-1, -1);
                Matrix[SelectPoint.X, SelectPoint.Y] = 0;
                _currDir = _currDir == Directions.Horizontal ? Directions.Vertical : Directions.Horizontal;
                Invalidate();
            }

            base.OnMouseClick(e);
        }
    }
}