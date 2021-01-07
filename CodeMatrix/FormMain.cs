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
           
            var matrix = new UCCodeMatrix();
            Controls.Add(matrix);
            ClientSize = matrix.Size;
        }

    }
}
