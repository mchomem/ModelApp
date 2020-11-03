using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MCHomem.Poc.CR.Utils
{
    public class MailHelper
    {
        /// <summary>
        /// Sends an email to one or more informed email addresses, containing no or multiple attachments.
        /// </summary>
        /// <param name="smtpHost">E-mail service server</param>
        /// <param name="smtpUser">SMTP user</param>
        /// <param name="smtpPass">SMTP user password</param>
        /// <param name="from">Address of</param>
        /// <param name="to">List of addresses for</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        /// <param name="enablesSSL">Optional, enables SSL</param>
        /// <param name="attachments">Optional, List of attachments</param>
        public void Send(String smtpHost, Int32 smtpPort, String smtpUser, String smtpPass, String from, List<String> to, String subject, String body, Boolean enablesSSL = false, List<Attachment> attachments = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);

                foreach (String address in to)
                {
                    if (!String.IsNullOrEmpty(address))
                    {
                        mail.To.Add(address);
                    }
                }

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = this.FormatBreakLineEmail(body);
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (attachments != null && attachments.Count > 0)
                {
                    foreach (Attachment attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }
                }

                SmtpClient client = new SmtpClient(smtpHost, smtpPort);
                client.EnableSsl = enablesSSL;
                client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                client.Send(mail);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private String FormatBreakLineEmail(String body)
        {
            body = body.Replace("\r\n", "<br>");
            return body;
        }
    }
}
