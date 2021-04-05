using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthTokens>> Register(string phoneNumber, string userName, string? email);
        Task<AuthTokens> LoginOrGenerateAccessToken(string phoneNumber);
        Task<Result<AuthTokens>> RefreshToken(string accessToken, string refreshToken);
    }
}
