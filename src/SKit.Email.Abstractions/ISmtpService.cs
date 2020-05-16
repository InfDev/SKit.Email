using System.Threading.Tasks;

namespace SKit.Email
{
    public interface ISmtpService
    {
        /// <summary>
        /// Sends the specified message to an SMTP server for delivery.
        /// </summary>
        Task<SmtpResult> SendAsync(MailMessage message);

        /// <summary>
        /// Create a default email message
        /// </summary>
        /// <returns>MailMessage</returns>
        /// <remarks>
        ///Central definition capability: To, Cc, Bcc, IsBodyHtml, Subject prefix
        /// </remarks>
        MailMessage CreateDefaultMessage();
    }
}