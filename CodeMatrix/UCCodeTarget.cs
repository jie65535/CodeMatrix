using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeMatrix
{
    public class UCCodeTarget : Control
    {
        public Size CellSize { get; private set; }
        public Padding CellMargin { get; set; }
        private byte _HoverCode;
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
        public event EventHandler<byte> HoverCodeEvent;
        public int BufferSize { get; set; }
        public int TargetLength { get; set; }
        public int OffsetIndex { get; private set; }
        public int CurrIndex { get; private set; }
        public byte[] TargetCodes { get; } = new byte[32];

        public enum State
        {
            Input,
            Pass,
            Reject,
        }
        public State CurrState { get; private set; }

        public UCCodeTarget()
        {
            InitData();
            InitComponent();
        }


        private void InitData()
        {
            BufferSize = 4;
            TargetLength = Common.Random.Next(2) + 2;
            OffsetIndex = 0;
            CurrIndex = 0;
            HoverCode = 0;
            CurrState = State.Input;
            for (int i = 0; i < TargetLength; i++)
                TargetCodes[i] = Common.Codes[Common.Random.Next(Common.Codes.Length)];
        }

        private void InitComponent()
        {
            CellSize = Styles.Default.CellSizeB;
            Margin = Padding.Empty;
            Padding = new Padding(10, 5, 10, 5);
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
            if (CurrState == State.Pass)
            {
                e.Graphics.FillRectangle(Styles.Default.PassBrush, ClientRectangle);
            }
            else if (CurrState == State.Reject)
            {
                e.Graphics.FillRectangle(Styles.Default.RejectBrush, ClientRectangle);
            }
            else
            {
                var cellOffset = new Rectangle(Padding.Left + CellMargin.Left,
                                           Padding.Top + CellMargin.Top,
                                           CellSize.Width-1,
                                           CellSize.Height-1);
                var cellOffsetWidth = CellSize.Width + CellMargin.Horizontal;
                cellOffset.X += cellOffsetWidth * OffsetIndex;
                int i = 0;
                for (; i < CurrIndex && i < TargetLength; i++)
                {
                    DrawCode(e.Graphics, TargetCodes[i], Font, Styles.Default.CodeBrush, cellOffset, Styles.Default.SelectedCellBorderPen);
                    cellOffset.X += cellOffsetWidth;
                }

                e.Graphics.FillRectangle(Styles.Default.CurrIndexBrush, cellOffset.X, 0, cellOffset.Width, this.Height);

                if (i == CurrIndex && HoverCode == TargetCodes[i])
                {
                    DrawCode(e.Graphics, TargetCodes[CurrIndex], Font, Styles.Default.SelectBrush, cellOffset, Styles.Default.SelectCellBorderPen);
                    cellOffset.X += cellOffsetWidth;
                    i++;
                }

                for (; i < TargetLength; i++)
                {
                    DrawCode(e.Graphics, TargetCodes[i], Font, Styles.Default.ToBeSelectBrush, cellOffset);
                    cellOffset.X += cellOffsetWidth;
                }
            }
            base.OnPaint(e);
        }

        private void DrawCode(Graphics g, byte code, Font font, Brush brush, Rectangle cellRect, Pen borderPen = null)
        {
            if (borderPen != null)
                g.DrawRectangle(borderPen, cellRect);
            var codeStr = code.ToString("X2");
            var codeSize = g.MeasureString(codeStr, font);
            var codeOffset = new PointF((CellSize.Width-codeSize.Width)/2, (CellSize.Height-codeSize.Height)/2);
            var codePoint = new PointF(codeOffset.X+cellRect.X, codeOffset.Y+cellRect.Y);
            g.DrawString(codeStr, font, brush, codePoint);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            
            //HoverCodeEvent?.Invoke(this, _HoverCode);


            base.OnMouseMove(e);
        }

        public void InputCode(byte code)
        {
            if (CurrState == State.Input)
            {
                if (code == TargetCodes[CurrIndex])
                    CurrIndex++;
                else
                    OffsetIndex++;

                if (OffsetIndex + TargetLength > BufferSize)
                    CurrState = State.Reject;
                else if (CurrIndex == TargetLength)
                    CurrState = State.Pass;
            }
        }

        public void ClearBuffer()
        {
            CurrIndex = 0;
            HoverCode = 0;
        }
    }
}
