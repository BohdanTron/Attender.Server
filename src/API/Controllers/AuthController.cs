using Attender.Server.API.Auth;
using Attender.Server.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISmsService _smsService;

        public AuthController(IAuthService authService, ISmsService smsService)
        {
            _authService = authService;
            _smsService = smsService;
        }

        [HttpPost("send-verification-phone-code")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<ActionResult> SendVerificationPhoneCode([FromBody] SendVerificationPhoneCodeRequest request)
        {
            var sent = await _smsService.SendVerificationCodeTo(request.PhoneNumber);

            if (sent) return NoContent();

            return BadRequest("Phone number is invalid");
        }

        [HttpPost("verify-phone")]
        public async Task<ActionResult> VerifyPhone([FromBody] VerifyPhoneRequest request)
        {
            var isValidCode = await _smsService.CheckVerificationCode(request.PhoneNumber, request.Code);

            if (!isValidCode) return BadRequest("Verification code is invalid");

            var result = await _authService.LoginOrGenerateAccessToken(request.PhoneNumber);
            if (result.Success)
            {
                return Ok(new AuthResponse
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.Register(request.PhoneNumber, request.UserName, request.Email);
            if (result.Success)
            {
                return Ok(new AuthResponse
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshToken(request.AccessToken, request.RefreshToken);
            if (result.Success)
            {
                return Ok(new AuthResponse
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken
                });
            }

            return BadRequest(result.Errors);
        }
    }
}
