using Attender.Server.Application.Common.Models;
using Attender.Server.Domain.Entities;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ITokensValidator
    {
        Task<Result<RefreshToken>> ValidateRefreshToken(string accessToken, string refreshToken);
    }
}
