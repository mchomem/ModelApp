using System;
using System.Windows.Forms;

namespace ModelApp.Tools.Views
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
