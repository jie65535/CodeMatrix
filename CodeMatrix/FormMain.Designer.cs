
namespace CodeMatrix
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.FLPCodeTargets = new System.Windows.Forms.FlowLayoutPanel();
            this.ucCodeTarget1 = new CodeMatrix.UCCodeTarget();
            this.ucCodeTarget2 = new CodeMatrix.UCCodeTarget();
            this.ucCodeTarget3 = new CodeMatrix.UCCodeTarget();
            this.CodeQueue = new CodeMatrix.UCCodeQueue();
            this.CodeMatrix = new CodeMatrix.UCCodeMatrix();
            this.BtnMin = new CodeMatrix.BaseControl.MyButton();
            this.BtnClose = new CodeMatrix.BaseControl.MyButton();
            this.FLPCodeTargets.SuspendLayout();
            this.SuspendLayout();
            // 
            // FLPCodeTargets
            // 
            this.FLPCodeTargets.Controls.Add(this.ucCodeTarget1);
            this.FLPCodeTargets.Controls.Add(this.ucCodeTarget2);
            this.FLPCodeTargets.Controls.Add(this.ucCodeTarget3);
            this.FLPCodeTargets.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FLPCodeTargets.Location = new System.Drawing.Point(321, 121);
            this.FLPCodeTargets.Name = "FLPCodeTargets";
            this.FLPCodeTargets.Padding = new System.Windows.Forms.Padding(1);
            this.FLPCodeTargets.Size = new System.Drawing.Size(362, 210);
            this.FLPCodeTargets.TabIndex = 4;
            this.FLPCodeTargets.Paint += new System.Windows.Forms.PaintEventHandler(this.FLPCodeTargets_Paint);
            // 
            // ucCodeTarget1
            // 
            this.ucCodeTarget1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(13)))), ((int)(((byte)(25)))));
            this.ucCodeTarget1.BufferSize = 4;
            this.ucCodeTarget1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ucCodeTarget1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(236)))), ((int)(((byte)(92)))));
            this.ucCodeTarget1.HoverCode = ((byte)(0));
            this.ucCodeTarget1.Location = new System.Drawing.Point(1, 1);
            this.ucCodeTarget1.Margin = new System.Windows.Forms.Padding(0);
            this.ucCodeTarget1.MinimumSize = new System.Drawing.Size(144, 41);
            this.ucCodeTarget1.Name = "ucCodeTarget1";
            this.ucCodeTarget1.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.ucCodeTarget1.Size = new System.Drawing.Size(144, 41);
            this.ucCodeTarget1.TabIndex = 0;
            this.ucCodeTarget1.TargetLength = 3;
            this.ucCodeTarget1.Text = "ucCodeTarget1";
            // 
            // ucCodeTarget2
            // 
            this.ucCodeTarget2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(13)))), ((int)(((byte)(25)))));
            this.ucCodeTarget2.BufferSize = 4;
            this.ucCodeTarget2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ucCodeTarget2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(236)))), ((int)(((byte)(92)))));
            this.ucCodeTarget2.HoverCode = ((byte)(0));
            this.ucCodeTarget2.Location = new System.Drawing.Point(1, 42);
            this.ucCodeTarget2.Margin = new System.Windows.Forms.Padding(0);
            this.ucCodeTarget2.MinimumSize = new System.Drawing.Size(144, 41);
            this.ucCodeTarget2.Name = "ucCodeTarget2";
            this.ucCodeTarget2.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.ucCodeTarget2.Size = new System.Drawing.Size(144, 41);
            this.ucCodeTarget2.TabIndex = 1;
            this.ucCodeTarget2.TargetLength = 3;
            this.ucCodeTarget2.Text = "ucCodeTarget2";
            // 
            // ucCodeTarget3
            // 
            this.ucCodeTarget3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(13)))), ((int)(((byte)(25)))));
            this.ucCodeTarget3.BufferSize = 4;
            this.ucCodeTarget3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ucCodeTarget3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(236)))), ((int)(((byte)(92)))));
            this.ucCodeTarget3.HoverCode = ((byte)(0));
            this.ucCodeTarget3.Location = new System.Drawing.Point(1, 83);
            this.ucCodeTarget3.Margin = new System.Windows.Forms.Padding(0);
            this.ucCodeTarget3.MinimumSize = new System.Drawing.Size(144, 41);
            this.ucCodeTarget3.Name = "ucCodeTarget3";
            this.ucCodeTarget3.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.ucCodeTarget3.Size = new System.Drawing.Size(144, 41);
            this.ucCodeTarget3.TabIndex = 2;
            this.ucCodeTarget3.TargetLength = 3;
            this.ucCodeTarget3.Text = "ucCodeTarget3";
            // 
            // CodeQueue
            // 
            this.CodeQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(13)))), ((int)(((byte)(25)))));
            this.CodeQueue.BufferSize = 4;
            this.CodeQueue.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.CodeQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(236)))), ((int)(((byte)(92)))));
            this.CodeQueue.HoverCode = ((byte)(0));
            this.CodeQueue.Location = new System.Drawing.Point(323, 67);
            this.CodeQueue.Margin = new System.Windows.Forms.Padding(0);
            this.CodeQueue.MinimumSize = new System.Drawing.Size(144, 51);
            this.CodeQueue.Name = "CodeQueue";
            this.CodeQueue.Padding = new System.Windows.Forms.Padding(10);
            this.CodeQueue.Size = new System.Drawing.Size(144, 51);
            this.CodeQueue.TabIndex = 3;
            this.CodeQueue.Text = "ucCodeQueue1";
            // 
            // CodeMatrix
            // 
            this.CodeMatrix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(13)))), ((int)(((byte)(25)))));
            this.CodeMatrix.CodeMatrixSize = new System.Drawing.Size(5, 5);
            this.CodeMatrix.Cursor = System.Windows.Forms.Cursors.Default;
            this.CodeMatrix.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.CodeMatrix.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(236)))), ((int)(((byte)(92)))));
            this.CodeMatrix.HighlightCode = ((byte)(0));
            this.CodeMatrix.Location = new System.Drawing.Point(18, 121);
            this.CodeMatrix.Margin = new System.Windows.Forms.Padding(0);
            this.CodeMatrix.MinimumSize = new System.Drawing.Size(200, 200);
            this.CodeMatrix.Name = "CodeMatrix";
            this.CodeMatrix.Size = new System.Drawing.Size(300, 210);
            this.CodeMatrix.TabIndex = 2;
            this.CodeMatrix.Text = "ucCodeMatrix1";
            this.CodeMatrix.CodeSelectedEvent += new System.EventHandler<byte>(this.CodeMatrix_CodeSelectedEvent);
            this.CodeMatrix.HoverValueChangedEvent += new System.EventHandler<byte>(this.CodeMatrix_HoverValueChangedEvent);
            // 
            // BtnMin
            // 
            this.BtnMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMin.BackColor = System.Drawing.Color.Transparent;
            this.BtnMin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(232)))), ((int)(((byte)(228)))));
            this.BtnMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(109)))), ((int)(((byte)(232)))), ((int)(((byte)(228)))));
            this.BtnMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(109)))), ((int)(((byte)(232)))), ((int)(((byte)(228)))));
            this.BtnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMin.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.BtnMin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(232)))), ((int)(((byte)(228)))));
            this.BtnMin.Location = new System.Drawing.Point(637, 12);
            this.BtnMin.Name = "BtnMin";
            this.BtnMin.Size = new System.Drawing.Size(30, 30);
            this.BtnMin.TabIndex = 1;
            this.BtnMin.TabStop = false;
            this.BtnMin.Text = "-";
            this.BtnMin.UseVisualStyleBackColor = false;
            this.BtnMin.Click += new System.EventHandler(this.BtnMin_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.BackColor = System.Drawing.Color.Transparent;
            this.BtnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(232)))), ((int)(((byte)(228)))));
            this.BtnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(109)))), ((int)(((byte)(232)))), ((int)(((byte)(228)))));
            this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClose.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.BtnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(232)))), ((int)(((byte)(228)))));
            this.BtnClose.Location = new System.Drawing.Point(673, 12);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(30, 30);
            this.BtnClose.TabIndex = 1;
            this.BtnClose.TabStop = false;
            this.BtnClose.Text = "×";
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(13)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(715, 380);
            this.Controls.Add(this.FLPCodeTargets);
            this.Controls.Add(this.CodeQueue);
            this.Controls.Add(this.CodeMatrix);
            this.Controls.Add(this.BtnMin);
            this.Controls.Add(this.BtnClose);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Matrix";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            this.FLPCodeTargets.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseControl.MyButton BtnClose;
        private BaseControl.MyButton BtnMin;
        private UCCodeMatrix CodeMatrix;
        private UCCodeQueue CodeQueue;
        private System.Windows.Forms.FlowLayoutPanel FLPCodeTargets;
        private UCCodeTarget ucCodeTarget1;
        private UCCodeTarget ucCodeTarget2;
        private UCCodeTarget ucCodeTarget3;
    }
}

