using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace MRaabta.Util
{
    public static class Email
    {
        public static void SendEmail(List<string> to, List<string> cc, string subject, string body, MemoryStream ms = null)
        {
            using (SmtpClient mail = new SmtpClient())
            {
                mail.Port = 587; //25
                mail.Host = "smtp.office365.com";
                mail.EnableSsl = true;
                mail.UseDefaultCredentials = true;
                mail.Credentials = new System.Net.NetworkCredential("no-reply@mulphilog.com", "Mpl@1234");

                MailMessage message = new MailMessage();
                message.From = new MailAddress("no-reply@mulphilog.com");
                message.Subject = subject;
                to.ForEach(x => message.To.Add(new MailAddress(x)));
                cc?.ForEach(x => message.CC.Add(new MailAddress(x)));
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                message.Body = body;

                if (ms != null && ms.Length > 0)
                    message.Attachments.Add(new Attachment(ms, "data.xlsx", "application/vnd.ms-excel"));


                //Add this line to bypass the certificate validation
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };


                mail.Send(message);
            }
        }
    }
}