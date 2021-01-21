using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeMatrix
{
    /// <summary>
    /// 表示代码矩阵控件  游戏中的代码矩阵
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    internal class UCCodeMatrix : Control
    {
        private static readonly Size MaximumMatrixSize = new Size(10, 10);
        private static readonly Size MinimumMatrixSize = new Size(5, 5);

        /// <summary>
        /// The cell size
        /// </summary>
        private Size _CellSize;

        /// <summary>
        /// The code matrix rect
        /// </summary>
        private RectangleF _CodeMatrixRect;

        /// <summary>
        /// The columns
        /// </summary>
        private int _Columns = 5;

        /// <summary>
        /// The curr dir
        /// </summary>
        private Directions _currDir;

        /// <summary>
        /// The cursor point
        /// </summary>
        private Point _CursorPoint;

        /// <summary>
        /// The highlight code
        /// </summary>
        private byte _HighlightCode;

        /// <summary>
        /// The hover point
        /// </summary>
        private Point _HoverPoint;

        /// <summary>
        /// The is loaded
        /// </summary>
        private bool _IsLoaded;

        /// <summary>
        /// The matrix
        /// </summary>
        private byte[,] _Matrix;

        /// <summary>
        /// The rows
        /// </summary>
        private int _Rows = 5;

        /// <summary>
        /// The select point
        /// </summary>
        private Point _SelectPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="UCCodeMatrix"/> class.
        /// </summary>
        public UCCodeMatrix()
        {
            InitComponent();
        }

        /// <summary>
        /// Occurs when [code selected event].
        /// </summary>
        public event EventHandler<byte> CodeSelectedEvent;

        /// <summary>
        /// Occurs when [hover value changed event].
        /// </summary>
        public event EventHandler<byte> HoverValueChangedEvent;

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

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">Columns - 设置矩阵大小数值不能小于 {MinimumMatrixSize.Width} 或者大于 {MaximumMatrixSize.Width}</exception>
        public int Columns
        {
            get => _Columns;
            set
            {
                if (_Columns != value)
                {
                    if (value < MinimumMatrixSize.Width || value > MaximumMatrixSize.Width)
                        throw new ArgumentOutOfRangeException("Columns", value, $"设置矩阵大小数值不能小于 {MinimumMatrixSize.Width} 或者大于 {MaximumMatrixSize.Width}");
                    _Columns = value;
                    OnCodeMatrixSizeChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the highlight code.
        /// </summary>
        /// <value>
        /// The highlight code.
        /// </value>
        public byte HighlightCode
        {
            get => _HighlightCode;
            set
            {
                if (_HighlightCode != value)
                {
                    _HighlightCode = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public int Rows
        {
            get => _Rows;
            set
            {
                if (_Rows != value)
                {
                    if (value < MinimumMatrixSize.Height || value > MaximumMatrixSize.Height)
                        throw new ArgumentOutOfRangeException("value", value, $"设置矩阵大小数值不能小于 {MinimumMatrixSize.Height} 或者大于 {MaximumMatrixSize.Height}");
                    _Rows = value;
                    OnCodeMatrixSizeChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the hover point.
        /// </summary>
        /// <value>
        /// The hover point.
        /// </value>
        private Point HoverPoint
        {
            get => _HoverPoint;
            set
            {
                _HoverPoint = value;
                if (HoverPoint.X >= 0)
                    HoverValueChangedEvent?.Invoke(this, _Matrix[HoverPoint.X, HoverPoint.Y]);
                else
                    HoverValueChangedEvent?.Invoke(this, 0);
            }
        }

        /// <summary>
        /// Reloads the code matrix.
        /// </summary>
        public void ReloadCodeMatrix()
        {
            _currDir = Directions.Horizontal;
            HoverPoint = new Point(-1, -1);
            GenerateCodeMatrix();
            CalcCodeMatrixRect();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (HoverPoint.X >= 0)
            {
                _SelectPoint = HoverPoint;
                _currDir = _currDir == Directions.Horizontal ? Directions.Vertical : Directions.Horizontal;
                CodeSelectedEvent?.Invoke(this, _Matrix[_SelectPoint.X, _SelectPoint.Y]);
                HoverPoint = _CursorPoint = new Point(-1, -1);
                _Matrix[_SelectPoint.X, _SelectPoint.Y] = 0;
                Invalidate();
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_CodeMatrixRect.Left <= e.X
                && _CodeMatrixRect.Top <= e.Y
                && _CodeMatrixRect.Right > e.X
                && _CodeMatrixRect.Bottom > e.Y)
            {
                var offset = new PointF(e.X-_CodeMatrixRect.X, e.Y-_CodeMatrixRect.Y);
                var current = new Point((int)(offset.X / _CellSize.Width), (int)(offset.Y / _CellSize.Height));
                if (_CursorPoint != current)
                {
                    _CursorPoint = current;
                    Point hoverPoint;
                    if (_currDir == Directions.Vertical)
                        hoverPoint = new Point(_SelectPoint.X, _CursorPoint.Y);
                    else
                        hoverPoint = new Point(_CursorPoint.X, _SelectPoint.Y);

                    if (_Matrix[hoverPoint.X, hoverPoint.Y] == 0)
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
                    HoverPoint = _CursorPoint = new Point(-1, -1);
                    Invalidate();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_IsLoaded) return;

            var offset = new PointF(_SelectPoint.X*_CellSize.Width, _SelectPoint.Y*_CellSize.Height);
            if (_currDir == Directions.Horizontal)
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBrush, 0, offset.Y + _CodeMatrixRect.Y, Width, _CellSize.Height);
            else if (_currDir == Directions.Vertical)
                e.Graphics.FillRectangle(Styles.Default.DefaultLineBrush, offset.X + _CodeMatrixRect.X, 0, _CellSize.Width, Height);

            if (HoverPoint.X >= 0)
            {
                Cursor = Cursors.Hand;
                if (_currDir == Directions.Horizontal)
                {
                    offset.X = HoverPoint.X * _CellSize.Width;
                    e.Graphics.FillRectangle(Styles.Default.SelectLineBrush, offset.X + _CodeMatrixRect.X, 0, _CellSize.Width, Height);
                }
                else if (_currDir == Directions.Vertical)
                {
                    offset.Y = HoverPoint.Y * _CellSize.Height;
                    e.Graphics.FillRectangle(Styles.Default.SelectLineBrush, 0, offset.Y + _CodeMatrixRect.Y, Width, _CellSize.Height);
                }

                offset.X += _CodeMatrixRect.X;
                offset.Y += _CodeMatrixRect.Y;
                e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X, offset.Y, _CellSize.Width - 1, _CellSize.Height - 1);
                e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, offset.X + 3, offset.Y + 3, _CellSize.Width - 6 - 1, _CellSize.Height - 6 - 1);
            }
            else
            {
                Cursor = Cursors.Default;
            }

            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    var cellOffset = new PointF(col*_CellSize.Width, row*_CellSize.Height);
                    //var cellRect = new RectangleF(cellOffset, CellSize);
                    Brush brush;
                    string code;
                    if (_Matrix[col, row] == 0)
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
                        code = _Matrix[col, row].ToString("X2");
                    }
                    if (HighlightCode != 0 && HighlightCode == _Matrix[col, row])
                    {
                        e.Graphics.DrawRectangle(Styles.Default.SelectedCellBorderPen,
                            _CodeMatrixRect.X + cellOffset.X + 1,
                            _CodeMatrixRect.Y + cellOffset.Y + 1,
                            _CellSize.Width - 3,
                            _CellSize.Height - 3);
                    }
                    var codeSize = e.Graphics.MeasureString(code, Font);
                    var codeOffset = new PointF((_CellSize.Width-codeSize.Width)/2, (_CellSize.Height-codeSize.Height)/2);
                    var codePoint = new PointF(codeOffset.X+cellOffset.X+_CodeMatrixRect.X, codeOffset.Y+cellOffset.Y+_CodeMatrixRect.Y);
                    e.Graphics.DrawString(code, Font, brush, codePoint);
                }
            }

            e.Graphics.DrawRectangle(Styles.Default.DefaultBorderPen, 0, 0, Width - 1, Height - 1);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible && !_IsLoaded)
            {
                OnLoad();
                _IsLoaded = true;
            }
            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// Calculates the code matrix rect.
        /// </summary>
        private void CalcCodeMatrixRect()
        {
            var blockSize = new SizeF(Columns*_CellSize.Width, Rows*_CellSize.Height);
            var blockOffset = new PointF((Width-blockSize.Width)/2, (Height-blockSize.Height)/2);
            _CodeMatrixRect = new RectangleF(blockOffset, blockSize);
        }

        /// <summary>
        /// Generates the code matrix.
        /// </summary>
        private void GenerateCodeMatrix()
        {
            _Matrix = new byte[Columns, Rows];
            for (int col = 0; col < Columns; col++)
                for (int row = 0; row < Rows; row++)
                    _Matrix[col, row] = Common.Codes[Common.Random.Next(Common.Codes.Length)];
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitComponent()
        {
            _CellSize = Styles.Default.CellSizeA;
            MinimumSize = new Size(_CellSize.Width * Columns, _CellSize.Height * Rows);
            Size = MinimumSize + new Size(100, 10);
            Margin = Padding.Empty;
            DoubleBuffered = true;

            Font = Styles.Default.CodeFontA;
            BackColor = Styles.Default.BackColor;
            ForeColor = Styles.Default.CodeColor;
        }

        /// <summary>
        /// Called when [code matrix size changed].
        /// </summary>
        private void OnCodeMatrixSizeChanged()
        {
            MinimumSize = new Size(_CellSize.Width * Columns, _CellSize.Height * Rows);
            if (_IsLoaded)
                ReloadCodeMatrix();
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        private void OnLoad()
        {
            ReloadCodeMatrix();
        }
    }
}