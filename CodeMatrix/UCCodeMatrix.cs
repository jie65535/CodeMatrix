using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeMatrix
{
    internal class UCCodeMatrix : Control
    {
        public Size CellSize { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public byte[,] Matrix { get; set; }
        public Point SelectPoint { get; private set; }
        public Point CursorPoint { get; private set; }
        private Point _HoverPoint;
        public Point HoverPoint
        {
            get => _HoverPoint;
            set
            {
                _HoverPoint = value;
                if (HoverPoint.X >= 0)
                    HoverValueChangedEvent?.Invoke(this, Matrix[HoverPoint.X, HoverPoint.Y]);
                else
                    HoverValueChangedEvent?.Invoke(this, 0);
            }
        }
        public RectangleF CodeMatrixRect { get; private set; }
        public event EventHandler<byte> HoverValueChangedEvent;
        public event EventHandler<byte> CodeSelectedEvent;

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
            _currDir = Directions.Horizontal;
            HoverPoint = new Point(-1, -1);
        }

        private void InitComponent()
        {
            CellSize = Styles.Default.CellSizeA;
            MinimumSize = new Size(CellSize.Width * Columns + 100, CellSize.Height * Rows + 10);
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
                    Matrix[col, row] = Common.Codes[Common.Random.Next(Common.Codes.Length)];

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
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBrush, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
            else if (_currDir == Directions.Vertical)
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBrush, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);

            if (HoverPoint.X >= 0)
            {
                Cursor = Cursors.Hand;
                if (_currDir == Directions.Horizontal)
                {
                    offset.X = HoverPoint.X * CellSize.Width;
                    e.Graphics.FillRectangle(Styles.Default.SelectLineBrush, offset.X + CodeMatrixRect.X, 0, CellSize.Width, Height);
                }
                else if (_currDir == Directions.Vertical)
                {
                    offset.Y = HoverPoint.Y * CellSize.Height;
                    e.Graphics.FillRectangle(Styles.Default.SelectLineBrush, 0, offset.Y + CodeMatrixRect.Y, Width, CellSize.Height);
                }

                offset.X += CodeMatrixRect.X;
                offset.Y += CodeMatrixRect.Y;
                e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X, offset.Y, CellSize.Width - 1, CellSize.Height - 1);
                e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X + 3, offset.Y + 3, CellSize.Width - 6 - 1, CellSize.Height - 6 - 1);
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
                    Point hoverPoint;
                    if (_currDir == Directions.Vertical)
                        hoverPoint = new Point(SelectPoint.X, CursorPoint.Y);
                    else
                        hoverPoint = new Point(CursorPoint.X, SelectPoint.Y);
                    
                    if (Matrix[hoverPoint.X, hoverPoint.Y] == 0)
                        hoverPoint = new Point(-1, -1);

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
                _currDir = _currDir == Directions.Horizontal ? Directions.Vertical : Directions.Horizontal;
                CodeSelectedEvent?.Invoke(this, Matrix[SelectPoint.X, SelectPoint.Y]);
                HoverPoint = CursorPoint = new Point(-1, -1);
                Matrix[SelectPoint.X, SelectPoint.Y] = 0;
                Invalidate();
            }
            base.OnMouseClick(e);
        }
    }
}