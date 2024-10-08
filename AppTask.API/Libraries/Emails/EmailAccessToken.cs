﻿using AppTask.Models;
using System.Net.Mail;

namespace AppTask.API.Libraries.Emails;

public class EmailAccessToken
{
    private SmtpClient _smtp;

    public EmailAccessToken(SmtpClient smtp)
    {
        _smtp = smtp;
    }

    public void Send(UserModel userModel)
    {
        var mailMessage = new MailMessage()
        {
            From = new MailAddress("michaelrhcp@gmail.com"),
            Subject = "AppTask - Token de acesso",
            Body = $"Esse é o seu token de acesso: {userModel.AccessToken}",
            IsBodyHtml = true
        };
        mailMessage.To.Add(userModel.Email);
        _smtp.Send(mailMessage);
    }
}
