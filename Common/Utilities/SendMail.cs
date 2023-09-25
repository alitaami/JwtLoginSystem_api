using System.Net;
using System.Net.Mail;

namespace Common.Utilities
{
    using Microsoft.Extensions.Options;
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class SendMail
    {
        private readonly EmailSettings _smtpSettings;

        public SendMail(IOptions<EmailSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendAsync(string to, string subject, string body)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(_smtpSettings.SmtpUsername, "سامانه اطلاع رسانی تاو");
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(_smtpSettings.SmtpServer))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Port = _smtpSettings.SmtpPort;
                        smtpClient.Credentials = new NetworkCredential(_smtpSettings.SmtpUsername, _smtpSettings.SmtpPassword);
                        smtpClient.EnableSsl = true;

                        await smtpClient.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception here
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }
    }
}
