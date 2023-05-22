﻿namespace Shared.Infrastructure.EmailSender.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
