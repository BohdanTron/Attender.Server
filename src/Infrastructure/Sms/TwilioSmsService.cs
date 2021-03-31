using Attender.Server.Application.Common.Interfaces;
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

        public async Task<bool> SendVerificationCodeTo(string phoneNumber)
        {
            InitTwilioClient();

            var serviceSid = _twilioSettings.VerificationServiceSid;

            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: phoneNumber,
                    channel: "sms",
                    pathServiceSid: serviceSid);

                return verification.Status == TwilioConstants.Status.Pending;
            }
            catch (ApiException)
            {
                return false;
            }
        }

        public async Task<bool> CheckVerificationCode(string phoneNumber, string code)
        {
            var serviceSid = _twilioSettings.VerificationServiceSid;

            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: phoneNumber,
                    code: code,
                    pathServiceSid: serviceSid);

                return verification.Status == TwilioConstants.Status.Approved;
            }
            catch (ApiException)
            {
                return false;
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
