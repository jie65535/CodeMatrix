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
        public SizeF CellSize { get; private set; }
        public Padding CellMargin { get; set; }
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
            CellSize = new SizeF(40, 40);
            BufferSize = 7;
            CurrIndex = 0;
        }

        private void InitComponent()
        {
            Margin = Padding.Empty;
            CellMargin = new Padding(3);
            MinimumSize = new Size((int)((CellSize.Width + CellMargin.Horizontal) * BufferSize), (int)(CellSize.Height + CellMargin.Vertical));
            DoubleBuffered = true;

            Font = Styles.Default.Font;
            BackColor = Styles.Default.BackColor;
            ForeColor = Styles.Default.ForeColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
