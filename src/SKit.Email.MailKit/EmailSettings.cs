using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;


namespace SKit.Email.MailKit
{
    using SKit.Email.MailKit.Resources;

    public class EmailSettings
    {

        /// <summary>
        /// 
        /// </summary>
        public SmtpSettings SmtpSettings { get; set; }
        public MailMessage DefaultMessage{ get; set; }
    }

    public class SmtpSettings : IValidatableObject
    {
        [Required(AllowEmptyStrings = false), EmailAddress]
        public string DefaultSender { get; set; }

        [Required]
        public SmtpDeliveryMethod DeliveryMethod { get; set; }

        public string PickupDirectoryLocation { get; set; }

        public string Host { get; set; }
        [Range(0, 65535)]
        public int Port { get; set; } = 25;
        public bool AutoSelectEncryption { get; set; }
        public bool RequireCredentials { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public SmtpEncryptionMethod EncryptionMethod { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var S = validationContext.GetService<IStringLocalizer<SmtpSettings>>();

            switch (DeliveryMethod)
            {
                case SmtpDeliveryMethod.Network:
                    if (String.IsNullOrEmpty(Host))
                    {
                        yield return new ValidationResult(S[SR.FieldRequired, SR.HostName], new[] { nameof(Host) });
                    }
                    break;
                case SmtpDeliveryMethod.SpecifiedPickupDirectory:
                    if (String.IsNullOrEmpty(PickupDirectoryLocation))
                    {
                        yield return new ValidationResult(S[SR.FieldRequired, SR.PickupDirectoryLocation], new[] { nameof(PickupDirectoryLocation) });
                    }
                    break;
                default:
                    throw new NotSupportedException(S[SR.DeliveryMethodNotSupported, DeliveryMethod]);
            }
        }
    }

}
