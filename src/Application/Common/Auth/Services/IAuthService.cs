using Attender.Server.Application.Common.Auth.Dtos;
using Attender.Server.Application.Common.Auth.Models;
using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Auth.Services
{
    public interface IAuthService
    {
        Task<Result<AuthTokens>> Register(UserRegistrationInfoDto dto);
        Task<AuthTokens> LoginOrGenerateAccessToken(string phoneNumber);
        Task<Result<AuthTokens>> RefreshToken(RefreshTokenDto dto);
    }
}
