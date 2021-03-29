using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> Register(string phoneNumber, string userName, string? email);
        Task<AuthResult> LoginOrGenerateAccessToken(string phoneNumber);
        Task<AuthResult> RefreshToken(string accessToken, string refreshToken);
    }
}
