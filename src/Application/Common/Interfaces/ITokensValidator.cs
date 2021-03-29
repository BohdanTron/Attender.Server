using Attender.Server.Application.Common.Models;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Interfaces
{
    public interface ITokensValidator
    {
        Task<ValidationTokenResult> ValidateRefreshToken(string accessToken, string refreshToken);
    }
}
