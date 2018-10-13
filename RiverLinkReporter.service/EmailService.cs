﻿using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RiverLinkReporter.models;

namespace RiverLinkReporter.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly RiverLinkReporterSettings _Settings;

        public EmailService(IOptions<RiverLinkReporterSettings> Settings)
        {
            _Settings = Settings.Value;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtp = new System.Net.Mail.SmtpClient(_Settings.SmtpServer);
            var mail = new System.Net.Mail.MailMessage();

            mail.IsBodyHtml = true;
            mail.From = new System.Net.Mail.MailAddress(_Settings.FromAddress, _Settings.FromName);
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;

            smtp.Timeout = 1000;

            var t = Task.Run(() => smtp.SendAsync(mail, null));

            return t;

            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
        }
    }

}