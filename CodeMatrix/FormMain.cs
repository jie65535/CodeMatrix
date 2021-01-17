using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeMatrix
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            BackColor = Styles.Default.BackColor;

            var codeMatrix = new UCCodeMatrix();
            var codeQueue = new UCCodeQueue();

            codeMatrix.HoverValueChangedEvent += (_, value) => codeQueue.HoverCode = value;
            codeMatrix.CodeSelectedEvent += (_, value) => codeQueue.InputCode(value);

            codeQueue.Location = new Point(codeMatrix.Size.Width, 0);
            Controls.Add(codeMatrix);
            Controls.Add(codeQueue);

            var size = codeMatrix.Size;
            size.Width += codeQueue.Width;
            ClientSize = size;
        }

    }
}
