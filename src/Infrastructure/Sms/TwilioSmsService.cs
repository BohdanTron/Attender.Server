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

        public async Task<Result<string>> SendVerificationCodeTo(string phoneNumber)
        {
            InitTwilioClient();

            var serviceSid = _twilioOptions.VerificationServiceSid;
            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: phoneNumber,
                    channel: TwilioConstants.Channel.Sms,
                    pathServiceSid: serviceSid);

                var status = verification.Status;

                return status == TwilioConstants.Status.Pending
                    ? Result.Success(status)
                    : Result.Failure<string>("Verification code was not sent");
            }
            catch (ApiException e)
            {
                switch (e.Code)
                {
                    case (int) TwilioErrorCodes.InvalidParameter:
                        return Result.Failure<string>("Phone number has incorrect format");
                    case (int) TwilioErrorCodes.MaxAttemptsReached:
                        return Result.Failure<string>("Max attempts reached");
                    default:
                        throw;
                }
            }
        }

        public async Task<Result<string>> CheckVerificationCode(string phoneNumber, string code)
        {
            InitTwilioClient();

            var serviceSid = _twilioOptions.VerificationServiceSid;
            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: phoneNumber,
                    code: code,
                    pathServiceSid: serviceSid);

                var status = verification.Status;

                return status == TwilioConstants.Status.Approved
                    ? Result.Success(status)
                    : Result.Failure<string>("Verification code is incorrect");
            }
            catch (ApiException e)
            {
                switch (e.Code)
                {
                    case (int) TwilioErrorCodes.InvalidParameter:
                        return Result.Failure<string>("Phone number or verification code has incorrect format");
                    case (int) TwilioErrorCodes.ResourceNotFound:
                        return Result.Failure<string>("Verification code already approved");
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
