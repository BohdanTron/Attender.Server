using Attender.Server.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Auth.Commands.VerifyPhone
{
    public class VerifyPhoneCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    internal class VerifyPhoneCommandHandler : IRequestHandler<VerifyPhoneCommand, bool>
    {
        private readonly ISmsService _smsService;

        public VerifyPhoneCommandHandler(ISmsService smsService)
        {
            _smsService = smsService;
        }

        public Task<bool> Handle(VerifyPhoneCommand request, CancellationToken cancellationToken)
        {
            var phoneNumber = request.PhoneNumber;
            var code = request.Code;

            return _smsService.CheckVerificationCode(phoneNumber, code);
        }
    }
}
