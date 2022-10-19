using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using UmbracoCleanBlog.Models;
using Umbraco.Cms.Core;
using System.Data;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Views;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;

namespace UmbracoCleanBlog.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IContentService _contentService;
        public EmailService(IConfiguration configuration, IContentService contentService)
        {
            _configuration = configuration;
            _contentService = contentService;
        }


        public void Send(ContactFormModel model, int currentPageId)
        {

            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(model.Email.Trim() ?? _configuration.GetValue<string>("EmailSender:EmailFrom")));
            email.To.Add(MailboxAddress.Parse(_configuration.GetValue<string>("EmailSender:EmailTo")));
            email.Subject = model.Subject.Trim();
            email.Body = new TextPart(TextFormat.Html) { Text = model.Message.Trim() };

            // send email
            using (var smtp = new SmtpClient())
            {
                try
                {
                    CreateNewContactContent(model, currentPageId);
                    SendMail(email, smtp);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }

        }
        private void CreateNewContactContent(ContactFormModel model, int currentPageId)
        {
            var contentMessage = _contentService.CreateAndSave(String.Format("{0}-{1}", model.UserName,
                        DateTime.Now.ToString()), currentPageId, "contactUsContentPage");
            contentMessage.SetValue("userName", model.UserName);
            contentMessage.SetValue("email", model.Email);
            contentMessage.SetValue("subject", model.Subject);
            contentMessage.SetValue("message", model.Message);
            _contentService.SaveAndPublish(contentMessage);
        }

        private void SendMail(MimeMessage email, SmtpClient smtp)
        {
            var SmtpHost = _configuration.GetValue<string>("EmailSender:SmtpHost");
            var SmtpPort = _configuration.GetValue<int>("EmailSender:SmtpPort");
            var SmtpUser = _configuration.GetValue<string>("EmailSender:SmtpUser");
            var SmtpPass = _configuration.GetValue<string>("EmailSender:SmtpPass");
            smtp.Connect(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            smtp.Authenticate(SmtpUser, SmtpPass);
            smtp.Send(email);
        }
    }
}
