using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendVerificationCodeTo(string phoneNumber);
        Task<bool> CheckVerificationCode(string phoneNumber, string code);
    }
}
