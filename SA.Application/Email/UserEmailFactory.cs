using Microsoft.Extensions.Configuration;
using SA.Application.Account;
using SA.Application.Records;
using SA.Core.Model;
using System.Globalization;
using System.Threading.Tasks;

namespace SA.Application.Email
{
    public class UserEmailFactory : IUserEmailFactory
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public UserEmailFactory(
            IEmailConfiguration emailConfiguration,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _emailConfiguration = emailConfiguration;
            _emailService = emailService;
            _configuration = configuration;
        }

        private const string SENDER_NAME = "JeraTrading s.r.o.";

        public async Task<EmailMessage> ResetPassword(UserSimpleDto user, UserActivation activation)
        {
            var email = new EmailMessage();
            email.FromAddresses.Add(new EmailAddress
            {
                Name = SENDER_NAME,
                Address = _emailConfiguration.NoReplyEmail
            });

            email.ToAddresses.Add(new EmailAddress
            {
                Name = user.Email,
                Address = user.Email
            });

            var resetPasswordHyperlink = $"<a href='{_configuration["ApiUrl:Url"]}/userActivations/userPasswordReset?token={activation.Token}'>Reset hesla</a>";

            email.Subject = $"Reset hesla uživatele { user.UserName }";
            email.Content = $@"<p>Bylo zažádáno o obnovení hesla pro uživatele <strong>{ user.UserName }</strong>. Pokud jste o změnu nežádali, dejte nám prosím vědět a žádný úkon neprovádějte.</p>
<p>Pro obnovení hesla přejděte na stránky prostřednictvím odkazu { resetPasswordHyperlink }</p>
<p>Váš tým <strong>JERA Trading s.r.o.</strong></p>";

            await _emailService.Send(email);

            return email;
        }

        public async Task<EmailMessage> SendActivationEmail(User user, UserActivation activation)
        {
            var email = new EmailMessage();
            email.FromAddresses.Add(new EmailAddress
            {
                Name = SENDER_NAME,
                Address = _emailConfiguration.NoReplyEmail
            });

            email.ToAddresses.Add(new EmailAddress
            {
                Name = $"{user.Customer.FirstName} {user.Customer.LastName}",
                Address = user.Customer.Email
            });

            var registrationHyperlink = $"<a href='{_configuration["ApiUrl:Url"]}/userActivations/userValidation?token={activation.Token}'>Potvrdit registraci</a>";

            var language = "Český";
            switch (user.Language)
            {
                case "en":
                    language = "Anglický";
                    break;
                case "de":
                    language = "Německý";
                    break;
                case "ru":
                    language = "Ruský";
                    break;
                case "sk":
                    language = "Slovenský";
                    break;
            }

            email.Subject = "Registrace na webu JERA Trading s.r.o.";
            email.Content = $@"<h1>Děkujeme za registraci na portálu Jera Trading s.r.o.</h1>
<p>Rekapitulace registrovaných informací</p>
<h4>Uživatel</h4>
<ul>
    <li>Uživatelské jméno: <strong>{user.UserName}</strong></li>
    <li>Souhlas s obchodními podmínkami: <strong>{ (user.IsAgreementToTerms ? "Ano" : "Ne") }</strong></li>
    <li>Zasílání novinek: <strong>{ (user.SendingNews ? "Ano" : "Ne") }</strong></li>
    <li>Preferovaný jazyk: <strong>{ language }</strong></li>
</ul>
<h4>Zákazník</h4>
<ul>
    <li>Jméno a Příjmení: <strong>{ user.Customer.TitleBefore} { user.Customer.FirstName} {user.Customer.LastName} {user.Customer.TitleAfter}</strong></li>    
    <li>Rodné číslo: <strong>{ user.Customer.BirthNumber }</strong></li>
    <li>Email: <strong>{ user.Customer.Email }</strong></li>
    <li>Telefonní číslo: <strong>{ user.Customer.PhoneNumber }</strong></li>
    <li>Společnost: <strong>{ user.Customer.CompanyName }</strong></li>
    <li>IČO: <strong>{ user.Customer.CompanyNumber }</strong></li>
    <li>DIČ: <strong>{ user.Customer.CompanyLegalNumber }</strong></li>
    <li>Webové stránky: <strong>{ user.Customer.WebPageUrl }</strong></li>
</ul>
<h4>Adresa</h4>
<ul>
    <li>Ulice a PSČ: { user.Customer.Address.Street } { user.Customer.Address.PostCode }</strong></li>    
    <li>Město: { user.Customer.Address.City }</li>
</ul><br />
<p>Pro dokončení registrace prosím potvrďte kliknutím na tento odkaz { registrationHyperlink }</p>
<p>Váš tým <strong>JERA Trading s.r.o.</strong></p>";

            await _emailService.Send(email);

            return email;
        }

        public async Task<EmailMessage> SendAuctionWonEmail(User user, RecordDetailDto record)
        {
            var email = new EmailMessage();
            email.FromAddresses.Add(new EmailAddress
            {
                Name = SENDER_NAME,
                Address = _emailConfiguration.NoReplyEmail
            });

            email.ToAddresses.Add(new EmailAddress
            {
                Name = $"{user.Customer.FirstName} {user.Customer.LastName}",
                Address = user.Customer.Email
            });

            email.Subject = $"Výhra v aukci JERA Trading s.r.o. na položku { record.Name }";
            email.Content = $@"<h1>Gratulujeme k výhře v aukci ""{ record.Name }""</h1>
<h3>Rekapitulace aukce</h3>
<ul>
    <li>Název: <strong>{ record.Name }</strong></li>
    <li>Vydraženo za: <strong>{ record.CurrentPrice.ToString("C", new CultureInfo("cs-CZ")) }</strong></li>    
</ul>
<p>Brzy vás budeme kontaktovat pro další postup platby a předávky draženého předmětu</p>";

            await _emailService.Send(email);

            return email;
        }

        /// <summary>
        /// Email bude zaslan uzivateli, ktery vyhraval a byl prehozen
        /// </summary>
        /// <param name="user"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task<EmailMessage> SendAuctionOverbidenEmail(User user, RecordTableDto record)
        {
            var email = new EmailMessage();
            email.FromAddresses.Add(new EmailAddress
            {
                Name = SENDER_NAME,
                Address = _emailConfiguration.NoReplyEmail
            });

            email.ToAddresses.Add(new EmailAddress {
                Name = $"{user.Customer.FirstName} {user.Customer.LastName}",
                Address = user.Customer.Email
            });

            email.Subject = $"Byl jste přehozen v aukci JERA Trading s.r.o. na položce { record.Name }";
            email.Content = $@"<h1>Bohužel jste byl přehozen</h1>
<p>Aukce končí {record.ValidTo.ToString("dd.MM.yyyy HH:mm")}. Aktuální cena dražené položky je { record.CurrentPrice.ToString("C", new CultureInfo("cs-CZ")) } .</p>
<p>Pokud chcete v aukci nadále pokračovat můžete přejít na položku tímto odkazem. <a href='{_configuration["Web:Url"]}/auctionDetail?id={record.Id}'>{ record.Name }</a></p>";

            await _emailService.Send(email);

            return email;
        }

        /// <summary>
        /// Odesle email dle prevzatych parametru z venci.
        /// Odesilatel bude vzdy NoReply
        /// </summary>
        /// <param name="emessage"></param>
        /// <returns></returns>
        public async Task<EmailMessage> SendEmail(EmailMessage message)
        {
            message.FromAddresses.Add(new EmailAddress
            {
                Name = SENDER_NAME,
                Address = _emailConfiguration.NoReplyEmail
            });

            await _emailService.Send(message);

            return message;
        }
    }
}
