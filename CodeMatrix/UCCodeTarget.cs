using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeMatrix
{
    /// <summary>
    /// 表示代码目标序列控件  游戏中的目标序列
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    public class UCCodeTarget : Control
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
        /// The minimum target length
        /// </summary>
        private const int MinimumTargetLength = 2;

        /// <summary>
        /// The KMP next Array
        /// </summary>
        private readonly int[] _Next = new int[MaximumBufferSize];

        /// <summary>
        /// The target codes
        /// </summary>
        private readonly byte[] _TargetCodes = new byte[MaximumBufferSize];

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
        /// The curr state
        /// </summary>
        private State _CurrState;

        /// <summary>
        /// The hover code
        /// </summary>
        private byte _HoverCode;

        /// <summary>
        /// The is loaded
        /// </summary>
        private bool _IsLoaded;

        /// <summary>
        /// The offset index
        /// </summary>
        private int _OffsetIndex;

        /// <summary>
        /// The target length
        /// </summary>
        private int _TargetLength = 3;

        /// <summary>
        /// The hover index
        /// </summary>
        private int HoverIndex = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="UCCodeTarget"/> class.
        /// </summary>
        public UCCodeTarget()
        {
            InitComponent();
        }

        /// <summary>
        /// Occurs when [code target state changed event].
        /// </summary>
        public event EventHandler CodeTargetStateChangedEvent;

        /// <summary>
        /// Occurs when [hover code event].
        /// </summary>
        public event EventHandler<byte> HoverCodeEvent;

        /// <summary>
        /// 状态枚举
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 输入中
            /// </summary>
            Input,

            /// <summary>
            /// 已通过/已安装
            /// </summary>
            Pass,

            /// <summary>
            /// 已拒绝
            /// </summary>
            Reject,
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
        /// 获取当前正在输入的下标位置
        /// </summary>
        /// <value>
        /// 当前正在输入的下标位置
        /// </value>
        public int CurrIndex { get; private set; }

        /// <summary>
        /// 获取当前序列的输入状态
        /// </summary>
        /// <value>
        /// 当前序列的输入状态
        /// </value>
        public State CurrState
        {
            get => _CurrState;
            private set
            {
                if (_CurrState != value)
                {
                    _CurrState = value;
                    OnTargetStateChanged();
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
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the length of the target.
        /// </summary>
        /// <value>
        /// The length of the target.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">TargetLength - 设置目标序列长度不能小于 {MinimumTargetLength} 或者大于缓冲区大小</exception>
        public int TargetLength
        {
            get => _TargetLength;
            set
            {
                if (_TargetLength != value)
                {
                    if (value < MinimumBufferSize || value > BufferSize)
                        throw new ArgumentOutOfRangeException("TargetLength", value, $"设置目标序列长度不能小于 {MinimumTargetLength} 或者大于缓冲区大小");
                    _TargetLength = value;
                    OnTargetLengthChanged();
                }
            }
        }

        /// <summary>
        /// Inputs the code.
        /// </summary>
        /// <param name="code">The code.</param>
        public void InputCode(byte code)
        {
            if (CurrState == State.Input)
            {
                while (true)
                {
                    if (CurrIndex == -1)
                    {
                        CurrIndex = 0;
                        break;
                    }
                    if (code == _TargetCodes[CurrIndex])
                    {
                        CurrIndex++;
                        break;
                    }
                    else
                    {
                        var next = _Next[CurrIndex];
                        _OffsetIndex += CurrIndex - next;
                        CurrIndex = next;
                    }
                }

                if (_OffsetIndex + TargetLength > BufferSize)
                    CurrState = State.Reject;
                else if (CurrIndex == TargetLength)
                    CurrState = State.Pass;
            }
        }

        /// <summary>
        /// Reloads the target sequence.
        /// </summary>
        public void ReloadTargetSequence()
        {
            _OffsetIndex = 0;
            CurrIndex = 0;
            HoverCode = 0;
            CurrState = State.Input;
            GenerateTargetSequence();
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (CurrState == State.Input)
            {
                int hoverIndex = -1;
                Point currPoint = new Point(e.X - Padding.Left, e.Y - Padding.Top);
                if (currPoint.X >= 0 && currPoint.Y >= 0 && currPoint.Y < _CellSize.Height)
                {
                    int cellfullwidth = _CellSize.Width + _CellMargin.Horizontal;
                    int index = currPoint.X / cellfullwidth - _OffsetIndex;
                    if (index >= 0 && index < TargetLength)
                    {
                        hoverIndex = index;
                    }
                }

                if (HoverIndex != hoverIndex)
                {
                    HoverIndex = hoverIndex;
                    if (HoverIndex >= 0)
                        HoverCodeEvent?.Invoke(this, _TargetCodes[HoverIndex]);
                    else
                        HoverCodeEvent?.Invoke(this, 0);
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (CurrState == State.Pass)
            {
                e.Graphics.FillRectangle(Styles.Default.PassBrush, ClientRectangle);
                e.Graphics.DrawString("已安装", Font, Brushes.Black, 20, 8);
            }
            else if (CurrState == State.Reject)
            {
                e.Graphics.FillRectangle(Styles.Default.RejectBrush, ClientRectangle);
                e.Graphics.DrawString("失败", Font, Brushes.Black, 20, 8);
            }
            else
            {
                var cellOffset = new Rectangle(Padding.Left + _CellMargin.Left,
                                           Padding.Top + _CellMargin.Top,
                                           _CellSize.Width-1,
                                           _CellSize.Height-1);
                var cellOffsetWidth = _CellSize.Width + _CellMargin.Horizontal;
                cellOffset.X += cellOffsetWidth * _OffsetIndex;
                int i = 0;
                for (; i < CurrIndex && i < TargetLength; i++)
                {
                    DrawCode(e.Graphics, _TargetCodes[i], Font, Styles.Default.CodeBrush, cellOffset, Styles.Default.SelectedCellBorderPen);
                    cellOffset.X += cellOffsetWidth;
                }

                e.Graphics.FillRectangle(Styles.Default.CurrIndexBrush, cellOffset.X, 0, cellOffset.Width, this.Height);

                if (i == CurrIndex && HoverCode == _TargetCodes[i])
                {
                    DrawCode(e.Graphics, _TargetCodes[CurrIndex], Font, Styles.Default.SelectBrush, cellOffset, Styles.Default.SelectCellBorderPen);
                    cellOffset.X += cellOffsetWidth;
                    i++;
                }

                for (; i < TargetLength; i++)
                {
                    DrawCode(e.Graphics, _TargetCodes[i], Font, Styles.Default.ToBeSelectBrush, cellOffset);
                    cellOffset.X += cellOffsetWidth;
                }
            }
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
        /// Draws the code.
        /// </summary>
        /// <param name="g">The graphics.</param>
        /// <param name="code">The code.</param>
        /// <param name="font">The font.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="cellRect">The cell rect.</param>
        /// <param name="borderPen">The border pen.</param>
        private void DrawCode(Graphics g, byte code, Font font, Brush brush, Rectangle cellRect, Pen borderPen = null)
        {
            if (borderPen != null)
                g.DrawRectangle(borderPen, cellRect);
            var codeStr = code.ToString("X2");
            var codeSize = g.MeasureString(codeStr, font);
            var codeOffset = new PointF((_CellSize.Width-codeSize.Width)/2, (_CellSize.Height-codeSize.Height)/2);
            var codePoint = new PointF(codeOffset.X+cellRect.X, codeOffset.Y+cellRect.Y);
            g.DrawString(codeStr, font, brush, codePoint);
        }

        /// <summary>
        /// Generates the target sequence.
        /// </summary>
        private void GenerateTargetSequence()
        {
            for (int i = 0; i < TargetLength; i++)
                _TargetCodes[i] = Common.Codes[Common.Random.Next(Common.Codes.Length)];
            _Next[0] = -1;
            _Next[1] = 0;
            for (int i = 1, j = 0; i < TargetLength;)
            {
                if (j == -1 || _TargetCodes[i] == _TargetCodes[j])
                {
                    i++;
                    j++;
                    if (_TargetCodes[i] != _TargetCodes[j])
                        _Next[i] = j;
                    else
                        _Next[i] = _Next[j];
                }
                else
                {
                    j = _Next[j];
                }
            }
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitComponent()
        {
            _CellSize = Styles.Default.CellSizeB;
            Margin = Padding.Empty;
            Padding = new Padding(10, 5, 10, 5);
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
            if (TargetLength > BufferSize)
                TargetLength = BufferSize;
            else
                ReloadTargetSequence();
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        private void OnLoad()
        {
            ReloadTargetSequence();
        }

        /// <summary>
        /// Called when [target length changed].
        /// </summary>
        private void OnTargetLengthChanged()
        {
            ReloadTargetSequence();
        }

        /// <summary>
        /// Called when [target state changed].
        /// </summary>
        private void OnTargetStateChanged()
        {
            CodeTargetStateChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}