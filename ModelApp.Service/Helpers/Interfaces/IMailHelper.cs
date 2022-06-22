namespace ModelApp.Service.Helpers.Interfaces
{
    public interface IMailHelper
    {
        public string FormatBreakLineEmail(string body);
        public void Send(string subject, string bodyMessage, IEnumerable<string> emails);
    }
}
