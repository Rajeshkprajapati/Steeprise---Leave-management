using System;
using System.Net;
using System.Net.Mail;
using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Shared;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Shared;
using Microsoft.Extensions.Configuration;

namespace LMS.Business.Handlers.Shared
{
    public class EMailHandler : IEMailHandler
    {
        private readonly IConfiguration configuration;
        private readonly IEmailRepository emailRepository;
        public EMailHandler(IConfiguration _configuration)
        {
            var factory = new ProcessorFactoryResolver<IEmailRepository>(_configuration);
            emailRepository = factory.CreateProcessor();
            configuration = _configuration;
        }


        public void SendMail(EmailViewModel email, int userId, bool isInsertInDB)
        {

            foreach (string to in email.To)
            {
                if (!string.IsNullOrWhiteSpace(to))
                {
                    var message = new MailMessage(email.From, to)
                    {
                        Subject = email.Subject,
                        Body = email.Body,
                        IsBodyHtml = email.IsHtml
                    };

                    var smtp = new SmtpClient
                    {
                        Host = configuration["SMTPClient:Host"].Trim(),
                        Port = Convert.ToInt32(configuration["SMTPClient:Port"]),
                        EnableSsl = Convert.ToBoolean(configuration["SMTPClient:EnableSsl"]),
                        Credentials =
                        new NetworkCredential(configuration["EmailCredential:Fromemail"].Trim(),
                        configuration["EmailCredential:FromPassword"].Trim())
                    };
                    smtp.Send(message);

                    if (isInsertInDB)
                    {
                        var mailInfo = new EmailModel
                        {
                            To = to,
                            From = email.From,
                            Body = email.Body,
                            Subject = email.Subject,
                            InsertedBy = userId,
                            MailType = email.MailType
                        };
                        emailRepository.SaveMailInformation(mailInfo);
                    }
                }
            }

        }

        public bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                m = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
