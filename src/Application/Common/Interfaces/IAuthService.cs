using Attender.Server.Application.Common.Dtos.Auth;
using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthTokens>> Register(UserRegistrationInfoDto dto);
        Task<AuthTokens> LoginOrGenerateAccessToken(string phoneNumber);
        Task<Result<AuthTokens>> RefreshToken(RefreshTokenDto dto);
    }
}
