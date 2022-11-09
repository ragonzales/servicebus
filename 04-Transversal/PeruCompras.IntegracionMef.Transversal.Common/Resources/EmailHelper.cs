using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using PeruCompras.IntegracionMef.Transversal.Common.Models;

namespace PeruCompras.IntegracionMef.Transversal.Common.Resources
{
    public static class EmailHelper
    {
        public static void SendEmail(Email dataEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(dataEmail.From));

            foreach (string para in dataEmail.To)
            {
                email.To.Add(MailboxAddress.Parse(para));
            }

            email.Subject = dataEmail.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = dataEmail.Message };

            using var smtp = new SmtpClient();
            smtp.Connect(dataEmail.Host, dataEmail.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(dataEmail.User, dataEmail.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
