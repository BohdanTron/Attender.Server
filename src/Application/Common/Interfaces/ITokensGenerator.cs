using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ITokensGenerator
    {
        Token GenerateAccessToken();

        Task<(Token access, Token refresh)> GenerateAccessRefreshTokens(int userId, string userName);
    }
}
