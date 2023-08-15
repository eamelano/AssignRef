using Sabio.Models.Domain.Emails;
using sib_api_v3_sdk.Model;

namespace Sabio.Services.Interfaces
{
    public interface IEmailService
    {
        void ReceiveEmailRequest(EmailInformation model);
        void ContactUsRequest(ContactUsRequest userInfo);
        void SendUserAuthEmail(string token, string email, string urlExtension, EmailType emailType, int userRole = 0);
    }
}