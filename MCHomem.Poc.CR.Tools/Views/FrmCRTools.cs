using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCHomem.Poc.CR.Tools.Views
{
    public partial class FrmCRTools : Form
    {
        public FrmCRTools()
        {
            InitializeComponent();
            this.Text += " v. " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void cypherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCypher frmCypher = new FrmCypher();
            frmCypher.MdiParent = this;
            frmCypher.Show();
        }

        private void dBICToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDBIC frmDBIC = new FrmDBIC();
            frmDBIC.MdiParent = this;
            frmDBIC.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
