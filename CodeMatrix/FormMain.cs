using System.Windows.Forms;

using CodeMatrix.Utils;

namespace CodeMatrix
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            FormHelper.EnableFromDropShadow(Handle);

            BackColor = Styles.Default.BackColor;

            foreach (UCCodeTarget codeTarget in FLPCodeTargets.Controls)
                codeTarget.HoverCodeEvent += CodeTarget_HoverCodeEvent;

            //var codeMatrix = new UCCodeMatrix();
            //var codeQueue = new UCCodeQueue
            //{
            //    Location = new Point(codeMatrix.Size.Width, 0)
            //};
            //var codeTargets = new UCCodeTarget[3];
            //for (int i = 0, y = codeQueue.Size.Height; i < codeTargets.Length; y += codeTargets[i++].Size.Height)
            //{
            //    codeTargets[i] = new UCCodeTarget
            //    {
            //        Location = new Point(codeQueue.Location.X, y)
            //    };
            //    codeTargets[i].HoverCodeEvent += (_, value) =>
            //    {
            //        codeMatrix.HighlightCode = value;
            //    };
            //}

                    //codeMatrix.HoverValueChangedEvent += (_, value) =>
                    //{
                    //    codeQueue.HoverCode = value;
                    //    foreach (var codeTarget in codeTargets)
                    //        codeTarget.HoverCode = value;
                    //};
                    //codeMatrix.CodeSelectedEvent += (_, value) =>
                    //{
                    //    codeQueue.InputCode(value);
                    //    foreach (var codeTarget in codeTargets)
                    //        codeTarget.InputCode(value);
                    //};

                    //Controls.Add(codeMatrix);
                    //Controls.Add(codeQueue);
                    //Controls.AddRange(codeTargets);

                    //var size = codeMatrix.Size;
                    //size.Width += codeQueue.Width;
                    //ClientSize = size;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Styles.Default.DefaultBorderPen, 0, 0, Width - 1, Height - 1);
        }

        private void BtnMin_Click(object sender, System.EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void BtnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            FormHelper.OnMouseDown(Handle);
        }

        private void FLPCodeTargets_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Styles.Default.DefaultBorderPen, 0, 0, FLPCodeTargets.Width - 1, FLPCodeTargets.Height - 1);
        }

        private void CodeMatrix_CodeSelectedEvent(object sender, byte e)
        {
            CodeQueue.InputCode(e);
            foreach (UCCodeTarget codeTarget in FLPCodeTargets.Controls)
                codeTarget.InputCode(e);
        }

        private void CodeMatrix_HoverValueChangedEvent(object sender, byte e)
        {
            CodeQueue.HoverCode = e;
            foreach (UCCodeTarget codeTarget in FLPCodeTargets.Controls)
                codeTarget.HoverCode = e;
        }

        private void CodeTarget_HoverCodeEvent(object sender, byte e)
        {
            CodeMatrix.HighlightCode = e;
            foreach (UCCodeTarget codeTarget in FLPCodeTargets.Controls)
                codeTarget.HoverCode = e;
        }

        //private void CodeTarget_CodeTargetStateChangedEvent(object sender, System.EventArgs e)
        //{
        //    // TODO
        //}
    }
}