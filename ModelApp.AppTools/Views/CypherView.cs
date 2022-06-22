using ModelApp.AppTools.Shareds;
using ModelApp.Service.Helpers.Interfaces;

namespace ModelApp.AppTools.Views
{
    public partial class CypherView : MdiChieldFormBase
    {
        #region Fields

        private readonly ICypherHelper _cypherHelper;

        #endregion

        #region Constructors

        public CypherView(ICypherHelper cypherHelper)
        {
            _cypherHelper = cypherHelper;
            InitializeComponent();
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

        #region Methods

        private void Encrypt()
        {
            try
            {
                if (string.IsNullOrEmpty(this.textBoxValue.Text))
                {
                    MessageBox.Show(this, "Type a value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.textBoxValue.Text = _cypherHelper.Encrypt(this.textBoxValue.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    this
                    , $"Fail to encrypt value.\n\nMessage: {e.Message}\n{e.StackTrace}"
                    , "Fail"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
        }

        private void Decrypt()
        {
            try
            {
                if (string.IsNullOrEmpty(this.textBoxValue.Text))
                {
                    MessageBox.Show(this, "Type a value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.textBoxValue.Text = _cypherHelper.Decrypt(this.textBoxValue.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    this
                    , $"Fail to decrypt value.\n\nMessage: {e.Message}\n{e.StackTrace}"
                    , "Fail"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
