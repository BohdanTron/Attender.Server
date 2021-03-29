using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<AuthResult>
    {
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
    {
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var phoneNumber = request.PhoneNumber;
            var userName = request.UserName;
            var email = request.Email;

            return _authService.Register(phoneNumber, userName, email);
        }
    }
}
