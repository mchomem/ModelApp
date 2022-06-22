using ModelApp.Domain.Shareds;
using ModelApp.Service.Helpers.Interfaces;
using System.Net;
using System.Net.Mail;

namespace ModelApp.Service.Helpers
{
    public class MailHelper : IMailHelper
    {
        public string FormatBreakLineEmail(string body) =>
            body.Replace("\r\n", "<br>");

        public void Send(string subject, string bodyMessage, IEnumerable<string> emails)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(AppSettings.SmtpEmail.From);

                foreach (string email in emails)
                    if (!string.IsNullOrEmpty(email))
                        mail.To.Add(email);

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = this.FormatBreakLineEmail(bodyMessage);

                SmtpClient smtpClient = new SmtpClient(AppSettings.SmtpEmail.Host, Convert.ToInt32(AppSettings.SmtpEmail.Port));
                smtpClient.EnableSsl = AppSettings.SmtpEmail.EnableSSL;

                if (!AppSettings.SmtpEmail.UseRelay)
                {
                    smtpClient.Credentials =
                        new NetworkCredential(AppSettings.SmtpEmail.User, AppSettings.SmtpEmail.Password);
                }

                smtpClient.Send(mail);
            }
        }
    }
}
