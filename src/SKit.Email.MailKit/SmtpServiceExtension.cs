using System;
using Microsoft.Extensions.DependencyInjection;

namespace SKit.Email.MailKit
{
    public static class SmtpServiceCollectionExtension
    {
        /// <summary>
        /// Add SMTP service to asp.net service collection
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSmtp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            _ = services.AddOptions();
            AddSmtpService(services);
            return services;
        }

        /// <summary>
        /// Add SMTP service to asp.net service collection and SMTP options
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSmtp(
            this IServiceCollection services,
            Action<EmailSettings> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (setupAction == null)
            {
                Console.Error.WriteLine("Setup Action seems to be null, The smtp options will not be override. For any helps create an issue at ");
                AddSmtpService(services);
            }
            AddSmtpService(services, setupAction);
            return services;
        }

        internal static void AddSmtpService(IServiceCollection services)
        {
            //services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            _ = services.AddScoped<ISmtpService, SmtpService>();
        }

        /// <summary>
        /// If user want to override default smtp options
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
        /// <param name="setupAction">The <see cref="EmailSettings"/> option to change default smtp settings</param>
        internal static void AddSmtpService(
            IServiceCollection services,
            Action<EmailSettings> setupAction)
        {
            AddSmtpService(services);
            _ = services.Configure(setupAction);
        }
    }
}
