using ModelApp.Utils;
using System;
using System.Windows.Forms;

namespace ModelApp.Tools.Views
{
    public partial class FormCypher : Form
    {
        #region Methods

        public FormCypher()
        {
            InitializeComponent();
        }

        private void Encrypt()
        {
            try
            {
                if (string.IsNullOrEmpty(this.textValue.Text))
                {
                    MessageBox.Show(this, "Informe um valor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cypher cypher = new Cypher();
                this.textValue.Text = cypher.Encrypt(this.textValue.Text);
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
                if (string.IsNullOrEmpty(this.textValue.Text))
                {
                    MessageBox.Show(this, "Informe um valor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cypher cypher = new Cypher();
                this.textValue.Text = cypher.Decrypt(this.textValue.Text);
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

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            this.Encrypt();
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            this.Decrypt();
        }

        #endregion
    }
}
