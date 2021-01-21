using System.Drawing;
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
            var codeQueue = new UCCodeQueue
            {
                Location = new Point(codeMatrix.Size.Width, 0)
            };
            var codeTargets = new UCCodeTarget[3];
            for (int i = 0, y = codeQueue.Size.Height; i < codeTargets.Length; y += codeTargets[i++].Size.Height)
            {
                codeTargets[i] = new UCCodeTarget
                {
                    Location = new Point(codeQueue.Location.X, y)
                };
                codeTargets[i].HoverCodeEvent += (_, value) =>
                {
                    codeMatrix.HighlightCode = value;
                };
            }

            codeMatrix.HoverValueChangedEvent += (_, value) =>
            {
                codeQueue.HoverCode = value;
                foreach (var codeTarget in codeTargets)
                    codeTarget.HoverCode = value;
            };
            codeMatrix.CodeSelectedEvent += (_, value) =>
            {
                codeQueue.InputCode(value);
                foreach (var codeTarget in codeTargets)
                    codeTarget.InputCode(value);
            };

            Controls.Add(codeMatrix);
            Controls.Add(codeQueue);
            Controls.AddRange(codeTargets);

            var size = codeMatrix.Size;
            size.Width += codeQueue.Width;
            ClientSize = size;
        }
    }
}