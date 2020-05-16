# InfDev.Extensions.Email.MailKit

Implementing a Mailing Service using package [MailKit](https://github.com/jstedfast/MailKit).

Parameters are configured in the section appsettings.json, but can vary dynamically at run time.

```json
{
  "EmailSettings": {
    "DefaultMessage": {
      "To": "office@my.firm.domen",
      "Cc": null,
      "Bcc": null,
      "IsBodyHtml": true,
      "Subject": "OurSite! " // Prefix
    },
    "SmtpSettings": {
      "DefaultSender": "office@my.firm.domen",
      "Host": "smtp.server.host",
      "Port": 25,
      "UseDefaultCredentials": false,
      "RequireCredentials": true,
      "UserName": "user",
      "Password": "********",
      "AutoSelectEncryption": true, // Auto for SmtpEncryptionMethod
      "SmtpEncryptionMethod": "None", //  None | SSLTLS | STARTTLS
      "DeliveryMethod": "Network", // Network | SpecifiedPickupDirectory
      "PickupDirectoryLocation": ""
    }
}
```

Connecting a service in an application:

```csharp
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      // ...
      #region Add Smtp service
      // Add Smtp service with configuration from section 'EmailSettings'
      services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
      services.AddSmtp();
      // ONLY for API supports the ASP.NET Core Identity default UI infrastructure
      services.AddScoped<IEmailSender, EmailSender>();
      #endregion Add Smtp service
      // ...
    }
  }
```

Service Usage Example:

```csharp
    public class ContactModel : PageModel
    {
        private readonly ISmtpService _smtpService;
        private readonly IStringLocalizer _localizer;

        public ContactModel(ISmtpService smtpService, IStringLocalizer<SharedResource> localizer)
        {
            _smtpService = smtpService;
            _localizer = localizer;
        }

        public class InputModel
        {
            [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
            [StringLength(50, MinimumLength = 3, ErrorMessage = DataAnnotationsErrorMessages.StringLengthAttribute_ValidationErrorIncludingMinimum)]
            [Display(Name = "Your name")]
            public string Name { get; set; }

            [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
            [EmailAddress(ErrorMessage = DataAnnotationsErrorMessages.EmailAddressAttribute_Invalid)]
            [Display(Name = "Your email address")]
            public string Email { get; set; }

            [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
            [StringLength(100, MinimumLength = 4, ErrorMessage = DataAnnotationsErrorMessages.StringLengthAttribute_ValidationErrorIncludingMinimum)]
            [Display(Name = "Subject")]
            public string Subject { get; set; }

            [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
            [StringLength(1024, MinimumLength = 4, ErrorMessage = DataAnnotationsErrorMessages.StringLengthAttribute_ValidationErrorIncludingMinimum)]
            [Display(Name = "Your message")]
            public string Message { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SmtpResult EmailResult { get; set; }
        [TempData]
        public bool EmailSucceeded { get; set; }
        [TempData]
        public string EmailStatus { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var mailMessage = _smtpService.CreateDefaultMessage();
            mailMessage.ReplyTo = Input.Email;
            mailMessage.Subject += Input.Subject;
            mailMessage.Body = Input.Message;

            EmailResult = await _smtpService.SendAsync(mailMessage);
            if (EmailResult.Succeeded)
            {
                EmailSucceeded = true;
                EmailStatus = _localizer["Email successfully sent"];
                return RedirectToPage();
            }
            EmailStatus = string.Join("&lt;br /&gt;", EmailResult.Errors);
            return Page();
        }

    }
```
