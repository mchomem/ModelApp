using System;

namespace ModelApp.Utils
{
    public class PasswordGenerator
    {
        public string WithAlphaValue(string value)
        {
            string newPassword =
                string.Format
                (
                    "{0}-{1}"
                    , value.Replace('a', '4').Replace('e', '3').Replace('i', '1').Replace('o', '0')
                    , DateTime.Now.ToString("dd.MM.yyyy")
                );

            return newPassword;
        }

        public string WithDateTime()
        {
            string newPassword = string.Format("system@{0}", DateTime.Now.ToString("dd.MM.yyyy"));

            return newPassword;
        }
    }
}
