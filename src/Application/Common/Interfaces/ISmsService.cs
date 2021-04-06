using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task<Result<string>> SendVerificationCodeTo(string phoneNumber);
        Task<Result<string>> CheckVerificationCode(string phoneNumber, string code);
    }
}
