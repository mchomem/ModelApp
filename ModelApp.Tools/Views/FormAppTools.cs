using System;
using System.Windows.Forms;

namespace ModelApp.Tools.Views
{
    public partial class FormAppTools : Form
    {
        public FormAppTools()
        {
            InitializeComponent();
            this.Text += " v. " + System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetName().Version
                .ToString();
        }

        private void cypherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCypher frmCypher = new FormCypher();
            frmCypher.MdiParent = this;
            frmCypher.Show();
        }

        private void dBICToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDBIC frmDBIC = new FormDBIC();
            frmDBIC.MdiParent = this;
            frmDBIC.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
