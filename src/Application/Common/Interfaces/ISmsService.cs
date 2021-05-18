using Attender.Server.Application.Common.DTOs.Sms;
using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task<Result<string>> SendVerificationCode(PhoneSendingDto dto);
        Task<Result<string>> CheckVerificationCode(PhoneVerificationDto dto);
    }
}
