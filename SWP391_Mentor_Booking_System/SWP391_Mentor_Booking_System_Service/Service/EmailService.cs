using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SWP391_Mentor_Booking_System_Data.DTO.Email;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_emailSettings.SMTPServer)
            {
                Port = _emailSettings.SMTPPort,
                Credentials = new NetworkCredential(_emailSettings.SMTPUsername, _emailSettings.SMTPPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(recipientEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendOtpEmailAsync(string recipientEmail, string otp)
        {
            string subject = "Your Password Reset OTP";
            string body = $@"
            <h1>Password Reset OTP</h1>
            <p>Hi there,</p>
            <p>Your OTP for password reset is: <strong>{otp}</strong></p>
            <p>This OTP will expire in 10 minutes.</p>
            <p>If you did not request this, please ignore this email.</p>
            <p>Thank you!</p>";

            await SendEmailAsync(recipientEmail, subject, body);
        }
    }
}

