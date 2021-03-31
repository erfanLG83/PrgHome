using System;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace PrgHome.Web.Classes
{
    public class EmailSender : IEmailSender
    {
        [Obsolete]
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            MimeMessage mimeMessage = new MimeMessage
            {
                Subject = subject,
                Body = bodyBuilder.ToMessageBody()
            };
            mimeMessage.From.Add(new MailboxAddress("erfan2esh123erfan@gmail.com"));
            mimeMessage.To.Add(new MailboxAddress(email));
            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(new NetworkCredential("erfan2esh123erfan", "erfan123esh"));
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
            await Task.CompletedTask;
        }
    }
}
