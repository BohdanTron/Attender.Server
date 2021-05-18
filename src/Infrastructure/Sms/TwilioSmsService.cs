using Attender.Server.Application.Common.DTOs.Sms;
using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;

namespace Attender.Server.Infrastructure.Sms
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioOptions _twilioOptions;

        public TwilioSmsService(IOptions<TwilioOptions> twilioOptions)
        {
            _twilioOptions = twilioOptions.Value;
        }

        public async Task<Result<string>> SendVerificationCode(PhoneSendingDto dto)
        {
            InitTwilioClient();

            var serviceSid = _twilioOptions.VerificationServiceSid;
            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: dto.PhoneNumber,
                    channel: TwilioConstants.Channel.Sms,
                    pathServiceSid: serviceSid);

                var status = verification.Status;

                return status == TwilioConstants.Status.Pending
                    ? Result.Success(status)
                    : Result.Failure<string>(Errors.Sms.VerificationCodeCreationFailed());
            }
            catch (ApiException e)
            {
                switch (e.Code)
                {
                    case (int) TwilioErrorCodes.InvalidParameter:
                        return Result.Failure<string>(Errors.PhoneNumber.Invalid());
                    case (int) TwilioErrorCodes.MaxAttemptsReached:
                        return Result.Failure<string>(Errors.Sms.PhoneNumberFlood());
                    default:
                        throw;
                }
            }
        }

        public async Task<Result<string>> CheckVerificationCode(PhoneVerificationDto dto)
        {
            InitTwilioClient();

            var serviceSid = _twilioOptions.VerificationServiceSid;
            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: dto.PhoneNumber,
                    code: dto.Code,
                    pathServiceSid: serviceSid);

                var status = verification.Status;

                return status == TwilioConstants.Status.Approved
                    ? Result.Success(status)
                    : Result.Failure<string>(Errors.Sms.VerificationCodeInvalid());
            }
            catch (ApiException e)
            {
                switch (e.Code)
                {
                    case (int) TwilioErrorCodes.InvalidParameter:
                        return Result.Failure<string>(Errors.Sms.VerificationInvalidInput());
                    case (int) TwilioErrorCodes.ResourceNotFound:
                        return Result.Failure<string>(Errors.Sms.VerificationCodeApproved());
                    default:
                        throw;
                }
            }
        }

        private void InitTwilioClient()
        {
            var accountSid = _twilioOptions.AccountSid;
            var authToken = _twilioOptions.AuthToken;

            TwilioClient.Init(accountSid, authToken);
        }
    }
}
