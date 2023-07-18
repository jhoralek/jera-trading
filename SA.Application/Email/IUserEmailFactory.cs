using SA.Application.Account;
using SA.Application.Records;
using SA.Core.Model;
using System.Threading.Tasks;

namespace SA.Application.Email
{
    public interface IUserEmailFactory
    {
        Task<EmailMessage> SendActivationEmail(User user, UserActivation activation);

        Task<EmailMessage> SendAuctionWonEmail(User user, RecordDetailDto record);

        Task<EmailMessage> ResetPassword(UserSimpleDto user, UserActivation activation);

        Task<EmailMessage> SendAuctionOverbidenEmail(User user, RecordTableDto record);

        Task<EmailMessage> SendEmail(EmailMessage email);
    }
}
