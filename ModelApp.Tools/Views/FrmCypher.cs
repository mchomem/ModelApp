using ModelApp.Utils;
using System;
using System.Windows.Forms;

namespace ModelApp.Tools.Views
{
    public partial class FrmCypher : Form
    {
        #region Methods

        public FrmCypher()
        {
            InitializeComponent();
        }

        private void Encrypt()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtValue.Text))
                {
                    MessageBox.Show(this, "Informe um valor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cypher cypher = new Cypher();
                this.txtValue.Text = cypher.Encrypt(this.txtValue.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    this
                    , string.Format("Falha ao encriptar o valor.\n\nMensagem: {0}\n{1}", e.Message, e.StackTrace)
                    , "Falha"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
        }

        private void Decrypt()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtValue.Text))
                {
                    MessageBox.Show(this, "Informe um valor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cypher cypher = new Cypher();
                this.txtValue.Text = cypher.Decrypt(this.txtValue.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    this
                    , string.Format("Falha ao encriptar o valor.\n\nMensagem: {0}\n{1}", e.Message, e.StackTrace)
                    , "Falha"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Events

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            this.Encrypt();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            this.Decrypt();
        }

        #endregion
    }
}
