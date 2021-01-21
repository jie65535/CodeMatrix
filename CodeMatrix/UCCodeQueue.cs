using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeMatrix
{
    /// <summary>
    /// 表示代码队列控件  游戏中的输入缓冲区
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    internal class UCCodeQueue : Control
    {
        /// <summary>
        /// The maximum buffer size
        /// </summary>
        private const int MaximumBufferSize = 10;

        /// <summary>
        /// The minimum buffer size
        /// </summary>
        private const int MinimumBufferSize = 4;

        /// <summary>
        /// The buffer
        /// </summary>
        private byte[] _Buffer = new byte[MaximumBufferSize];

        /// <summary>
        /// The buffer size
        /// </summary>
        private int _BufferSize = 4;

        /// <summary>
        /// The cell margin
        /// </summary>
        private Padding _CellMargin;

        /// <summary>
        /// The cell size
        /// </summary>
        private Size _CellSize;

        /// <summary>
        /// The curr index
        /// </summary>
        private int _CurrIndex;

        /// <summary>
        /// The hover code
        /// </summary>
        private byte _HoverCode;

        /// <summary>
        /// The is loaded/
        /// </summary>
        private bool _IsLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="UCCodeQueue"/> class.
        /// </summary>
        public UCCodeQueue()
        {
            InitComponent();
        }

        /// <summary>
        /// Gets or sets the size of the buffer.
        /// </summary>
        /// <value>
        /// The size of the buffer.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">BufferSize - 设置缓冲区大小数值不能小于 {MinimumBufferSize} 或者大于 {MaximumBufferSize}</exception>
        public int BufferSize
        {
            get => _BufferSize;
            set
            {
                if (_BufferSize != value)
                {
                    if (value < MinimumBufferSize || value > MaximumBufferSize)
                        throw new ArgumentOutOfRangeException("BufferSize", value, $"设置缓冲区大小数值不能小于 {MinimumBufferSize} 或者大于 {MaximumBufferSize}");
                    _BufferSize = value;
                    OnBufferSizeChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the hover code.
        /// </summary>
        /// <value>
        /// The hover code.
        /// </value>
        public byte HoverCode
        {
            get => _HoverCode;
            set
            {
                if (_HoverCode != value)
                {
                    _HoverCode = value;
                    OnHoverCodeChanged();
                }
            }
        }

        /// <summary>
        /// Inputs the code.
        /// </summary>
        /// <param name="code">The code.</param>
        public void InputCode(byte code)
        {
            if (_CurrIndex < BufferSize)
                _Buffer[_CurrIndex++] = code;
        }

        /// <summary>
        /// Reloads the code queue.
        /// </summary>
        public void ReloadCodeQueue()
        {
            _CurrIndex = 0;
            HoverCode = 0;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var cellOffset = new Rectangle(Padding.Left + _CellMargin.Left,
                                           Padding.Top + _CellMargin.Top,
                                           _CellSize.Width-1,
                                           _CellSize.Height-1);
            var cellOffsetWidth = _CellSize.Width + _CellMargin.Horizontal;
            for (int i = 0; i < _CurrIndex && i < BufferSize; i++)
            {
                e.Graphics.DrawRectangle(Styles.Default.SelectedCellBorderPen, cellOffset);
                var code = _Buffer[i].ToString("X2");
                var codeSize = e.Graphics.MeasureString(code, Font);
                var codeOffset = new PointF((_CellSize.Width-codeSize.Width)/2, (_CellSize.Height-codeSize.Height)/2);
                var codePoint = new PointF(codeOffset.X+cellOffset.X, codeOffset.Y+cellOffset.Y);
                e.Graphics.DrawString(code, Font, Styles.Default.CodeBrush, codePoint);
                cellOffset.X += cellOffsetWidth;
            }

            if (_CurrIndex < BufferSize)
            {
                int i = _CurrIndex;
                if (HoverCode > 0)
                {
                    i++;
                    e.Graphics.DrawRectangle(Styles.Default.SelectCellBorderPen, cellOffset);
                    var code = HoverCode.ToString("X2");
                    var codeSize = e.Graphics.MeasureString(code, Font);
                    var codeOffset = new PointF((_CellSize.Width-codeSize.Width)/2, (_CellSize.Height-codeSize.Height)/2);
                    var codePoint = new PointF(codeOffset.X+cellOffset.X, codeOffset.Y+cellOffset.Y);
                    e.Graphics.DrawString(code, Font, Styles.Default.SelectBrush, codePoint);
                    cellOffset.X += cellOffsetWidth;
                }

                for (; i < BufferSize; i++)
                {
                    e.Graphics.DrawRectangle(Styles.Default.EmptyCellBorderPen, cellOffset);
                    cellOffset.X += cellOffsetWidth;
                }
            }

            e.Graphics.DrawRectangle(Styles.Default.DefaultBorderPen, 0, 0, Width - 1, Height - 1);

            base.OnPaint(e);
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
        /// Initializes the component.
        /// </summary>
        private void InitComponent()
        {
            _CellSize = Styles.Default.CellSizeB;
            Margin = Padding.Empty;
            Padding = new Padding(10);
            _CellMargin = new Padding(3);
            MinimumSize = new Size((_CellSize.Width + _CellMargin.Horizontal) * BufferSize + Padding.Horizontal,
                                   _CellSize.Height + _CellMargin.Vertical + Padding.Vertical);
            DoubleBuffered = true;

            Font = Styles.Default.CodeFontB;
            BackColor = Styles.Default.BackColor;
            ForeColor = Styles.Default.CodeColor;
        }

        /// <summary>
        /// Called when [buffer size changed].
        /// </summary>
        private void OnBufferSizeChanged()
        {
            MinimumSize = new Size((_CellSize.Width + _CellMargin.Horizontal) * BufferSize + Padding.Horizontal,
                                      _CellSize.Height + _CellMargin.Vertical + Padding.Vertical);
            ReloadCodeQueue();
        }

        /// <summary>
        /// Called when [hover code changed].
        /// </summary>
        private void OnHoverCodeChanged()
        {
            Invalidate();
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        private void OnLoad()
        {
            ReloadCodeQueue();
        }
    }
}