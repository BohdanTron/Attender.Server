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
        private readonly TwilioSettings _twilioSettings;

        public TwilioSmsService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
        }

        public async Task<Result<string>> SendVerificationCodeTo(string phoneNumber)
        {
            InitTwilioClient();

            var serviceSid = _twilioSettings.VerificationServiceSid;

            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: phoneNumber,
                    channel: "sms",
                    pathServiceSid: serviceSid);

                var status = verification.Status;

                return status == TwilioConstants.Status.Pending
                    ? Result.Succeeded(status)
                    : Result.Failure<string>("Verification code was not sent");
            }
            catch (ApiException e)
            {
                switch (e.Code)
                {
                    case 60200:
                        return Result.Failure<string>("Phone number is invalid");
                    case 60203:
                        return Result.Failure<string>("Max attempts reached");
                    default:
                        throw;
                }
            }
        }

        public async Task<Result<string>> CheckVerificationCode(string phoneNumber, string code)
        {
            InitTwilioClient();

            var serviceSid = _twilioSettings.VerificationServiceSid;

            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: phoneNumber,
                    code: code,
                    pathServiceSid: serviceSid);

                var status = verification.Status;

                return status == TwilioConstants.Status.Approved
                    ? Result.Succeeded(status)
                    : Result.Failure<string>("Verification code is incorrect");
            }
            catch (ApiException e)
            {
                switch (e.Code)
                {
                    case 60200:
                        return Result.Failure<string>("Phone number or verification code has incorrect format");
                    case 20404:
                        return Result.Failure<string>("Verification code already approved");
                    default:
                        throw;
                }
            }
        }

        private void InitTwilioClient()
        {
            var accountSid = _twilioSettings.AccountSid;
            var authToken = _twilioSettings.AuthToken;

            TwilioClient.Init(accountSid, authToken);
        }
    }
}
