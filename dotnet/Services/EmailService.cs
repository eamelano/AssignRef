using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sabio.Models.AppSettings;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Emails;
using Sabio.Services.Interfaces;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Collections.Generic;
using System.IO;
using Task = System.Threading.Tasks.Task;

namespace Sabio.Services
{
    public class EmailService : IEmailService
    {

        private readonly AppKeys _appKeys;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IConfiguration _configuration { get; set; }
        public ILookUpService _lookupService { get; set; }
        public EmailService(IOptions<AppKeys> appKeys, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ILookUpService lookupService)
        {
            _appKeys = appKeys.Value;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _lookupService = lookupService;
        }

        public async void ReceiveEmailRequest(EmailInformation model)
        {
            string template = StandardTemplate(model);

            SendSmtpEmail email = StandardTransacEmail(model.RecipientEmail, template);

            await SendTransacEmailAsync(email);
        }

        public async void ContactUsRequest(ContactUsRequest userInfo)
        {
            string adminTemplate = ContactUsTemplate(userInfo);

            string adminEmail = _appKeys.SendInBlueAdminEmail;

            SendSmtpEmail email = StandardTransacEmail(adminEmail, adminTemplate);

            await SendTransacEmailAsync(email);

            string ConfirmationTemplate = ContactUsTemplate(null);

            SendSmtpEmail sendConfirmationEmail = StandardTransacEmail(userInfo.SenderEmail, ConfirmationTemplate);

            await SendTransacEmailAsync(sendConfirmationEmail);
        }

        private async Task SendTransacEmailAsync(SendSmtpEmail sendSmtpEmail)
        {
            Configuration.Default.ApiKey.Clear();
            Configuration.Default.ApiKey.Add("api-key", $"{_appKeys.SendInBlueAppKey}");

            TransactionalEmailsApi apiInstance = new TransactionalEmailsApi();

            await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
        }

        /* Replace a placeholder in the HTML template using {{Placeholder}} and .Replace(), examples already
        below, make sure placeholder is located where plain text would go or it will break template retrieval. */
        private string StandardTemplate(EmailInformation model)
        {
            string htmlPath = _webHostEnvironment.WebRootPath + "/EmailTemplates/StandardTemplate.html";
            string htmlTemplate = File.ReadAllText(htmlPath)
                .Replace("{{Header}}", model.Header)
                .Replace("{{Body}}", model.Body);

            return htmlTemplate;
        }

        private string ContactUsTemplate(ContactUsRequest userInfo)
        {
            if (userInfo != null)
            {
                string htmlPath = _webHostEnvironment.WebRootPath + "/EmailTemplates/StandardTemplate.html";
                string htmlTemplate = File.ReadAllText(htmlPath).Replace("{{Body}}", $"{userInfo.SenderMessage}");

                return htmlTemplate;
            }
            else
            {
                string htmlPath = _webHostEnvironment.WebRootPath + "/EmailTemplates/StandardTemplate.html";
                string htmlTemplate = File.ReadAllText(htmlPath).Replace("{{Body}}", "We appreciate you contacting AssignRef. One of our colleagues will get back in touch with you soon! Have a great day! (In Spanish) ").Replace("{{Header}}","Thank you for getting in touch!");

                return htmlTemplate;
            }
        }

        private SendSmtpEmail StandardTransacEmail(string recipientEmail, string template)
        {
            SendSmtpEmailSender emailSender = new SendSmtpEmailSender(name: "AssignRef", email: _appKeys.SendInBlueAdminEmail);
            SendSmtpEmailTo sendSmtpEmail = new SendSmtpEmailTo(recipientEmail);
            List<SendSmtpEmailTo> emailList = new List<SendSmtpEmailTo>();
            emailList.Add(sendSmtpEmail);
            SendSmtpEmail email = new SendSmtpEmail(sender: emailSender, to: emailList, htmlContent: template, subject: "Test");

            return email;
        }

        public void SendUserAuthEmail(string token, string email, string urlExtension, EmailType emailType, int userRole = 0)
        {
            string emailHref = $"{_configuration.GetValue<string>("HostUrl:Url").Replace("https", "http")}/authentication/{urlExtension}?token={token}&email={email}";
            string header = "";
            string emailContent = "";
            string emailBodyPath = _webHostEnvironment.WebRootPath + "/EmailTemplates/";
            switch (emailType)
            {
                case EmailType.Activation:
                    header = "Assign Ref Account Activation";
                    emailBodyPath += "ConfirmEmailBody.html";
                    emailContent = File.ReadAllText(emailBodyPath).Replace("{emailHref}", emailHref);
                    break;
                case EmailType.Invitation:
                    List<LookUp> roles = _lookupService.GetLookUp("Roles");
                    if (userRole != 0)
                    {
                        header = "Assign Ref Invitation";
                        emailBodyPath += "InvitationEmailBody.html";
                        emailContent = File.ReadAllText(emailBodyPath).Replace("{Role}", roles[userRole - 1].Name).Replace("{emailHref}", emailHref);
                    }
                    break;
                case EmailType.PasswordChange:
                    header = "Assign Ref Password Reset";
                    emailBodyPath += "PasswordResetEmailBody.html";
                    emailContent = File.ReadAllText(emailBodyPath).Replace("{emailHref}", emailHref);
                    break;
            }
            ReceiveEmailRequest(new Sabio.Models.Domain.Emails.EmailInformation()
            {
                Body = emailContent,
                RecipientEmail = email,
                Header = header,
            });

        }
    }
}