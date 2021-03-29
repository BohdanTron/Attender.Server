using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Auth.Commands.LoginOrGenerateToken
{
    public class LoginOrGenerateTokenCommand : IRequest<AuthResult>
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }

    internal class LoginOrGenerateTokenCommandHandler : IRequestHandler<LoginOrGenerateTokenCommand, AuthResult>
    {
        private readonly IAuthService _authService;

        public LoginOrGenerateTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<AuthResult> Handle(LoginOrGenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var phoneNumber = request.PhoneNumber;

            return _authService.LoginOrGenerateAccessToken(phoneNumber);
        }
    }
}
