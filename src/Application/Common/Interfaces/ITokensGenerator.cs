using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ITokensGenerator
    {
        Token GenerateAccessToken();

        Task<AuthTokens> GenerateAuthTokens(int userId, string userName);
    }
}
