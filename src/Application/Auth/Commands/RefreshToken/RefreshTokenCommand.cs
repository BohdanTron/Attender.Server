using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResult>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }

    internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;

            return _authService.RefreshToken(accessToken, refreshToken);
        }
    }
}
