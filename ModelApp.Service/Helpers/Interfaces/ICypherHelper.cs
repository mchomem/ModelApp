namespace ModelApp.Service.Helpers.Interfaces
{
    public interface ICypherHelper
    {
        public string Encrypt(string plainText);
        public string Decrypt(string value);
    }
}
