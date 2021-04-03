using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ITokensGenerator
    {
        AccessToken GenerateAccessToken();

        Task<(AccessToken access, string refresh)> GenerateAccessRefreshTokens(int userId, string userName);
    }
}
