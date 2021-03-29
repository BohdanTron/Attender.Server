using Attender.Server.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Auth.Commands.SendVerificationPhoneCode
{
    public class SendVerificationPhoneCodeCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }

    internal class SendVerificationCodeCommandHandler : IRequestHandler<SendVerificationPhoneCodeCommand, bool>
    {
        private readonly ISmsService _smsService;

        public SendVerificationCodeCommandHandler(ISmsService smsService)
        {
            _smsService = smsService;
        }

        public Task<bool> Handle(SendVerificationPhoneCodeCommand request, CancellationToken cancellationToken)
        {
            var phoneNumber = request.PhoneNumber;

            return _smsService.SendVerificationCodeTo(phoneNumber);
        }
    }
}
