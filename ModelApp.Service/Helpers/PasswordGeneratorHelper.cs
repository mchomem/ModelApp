using ModelApp.Service.Helpers.Interfaces;

namespace ModelApp.Service.Helpers
{
    public class PasswordGeneratorHelper : IPasswordGeneratorHelper
    {
        #region Methods

        public string GetAlphaValue(string value) =>
            string.Format
                (
                    "{0}-{1}"
                    , value.Replace('a', '4').Replace('e', '3').Replace('i', '1').Replace('o', '0')
                    , DateTime.Now.ToString("dd.MM.yyyy")
                );

        public string GetDateTime() =>
            string.Format("system@{0}", DateTime.Now.ToString("dd.MM.yyyy"));

        #endregion
    }
}
