using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Common.Sms.Dtos;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Sms.Services
{
    public interface ISmsService
    {
        Task<Result<string>> SendVerificationCode(PhoneSendingDto dto);
        Task<Result<string>> CheckVerificationCode(PhoneVerificationDto dto);
    }
}
