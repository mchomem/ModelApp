using System;

namespace MCHomem.Poc.CR.Utils
{
    public class PasswordGenerator
    {
        public String WithAlphaValue(String value)
        {
            String newPassword =
                String.Format
                (
                    "{0}-{1}"
                    , value.Replace('a', '4').Replace('e', '3').Replace('i', '1').Replace('o', '0')
                    , DateTime.Now.ToString("dd.MM.yyyy")
                );

            return newPassword;
        }

        public String WithDateTime()
        {
            String newPassword = String.Format("system@{0}", DateTime.Now.ToString("dd.MM.yyyy"));

            return newPassword;
        }
    }
}
