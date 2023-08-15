    public interface IEmailService
    {
        void ReceiveEmailRequest(EmailInformation model);
        void ContactUsRequest(ContactUsRequest userInfo);
        void SendUserAuthEmail(string token, string email, string urlExtension, EmailType emailType, int userRole = 0);
    }
