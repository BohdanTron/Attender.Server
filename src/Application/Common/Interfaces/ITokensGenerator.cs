using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ITokensGenerator
    {
        string GenerateAccessToken();

        Task<(string access, string refresh)> GenerateAccessRefreshTokens(int userId, string userRole);
    }
}
