using Attender.Server.Application.Common.Auth.Dtos;
using Attender.Server.Application.Common.Auth.Models;
using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Auth.Services
{
    public interface IAuthService
    {
        Task<Result<AuthInfo>> RegisterUser(UserRegistrationInfoDto dto);
        Task<AuthInfo> LoginOrGenerateAccessToken(string phoneNumber);
        Task<Result<AuthTokens>> RefreshToken(RefreshTokenDto dto);
    }
}
