using Attender.Server.API.Auth;
using Attender.Server.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Produces("application/json")]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISmsService _smsService;

        public AuthController(IAuthService authService, ISmsService smsService)
        {
            _authService = authService;
            _smsService = smsService;
        }

        /// <summary>
        /// Sends a verification code to phone number
        /// </summary>
        /// <response code="204">Verification code successfully sent</response>
        /// <response code="400">Phone number is invalid</response>
        [HttpPost("send-verification-phone-code")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendVerificationPhoneCode([FromBody] SendVerificationPhoneCodeRequest request)
        {
            var sent = await _smsService.SendVerificationCodeTo(request.PhoneNumber);

            if (sent) return NoContent();

            return BadRequest("Phone number is invalid");
        }

        /// <summary>
        /// Verifies a phone number and generate tokens to register or to login
        /// </summary>
        /// <response code="200">Phone number successfully verified</response>
        /// <response code="400">Verification code is invalid</response>
        [HttpPost("verify-phone")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> VerifyPhone([FromBody] VerifyPhoneRequest request)
        {
            var isValidCode = await _smsService.CheckVerificationCode(request.PhoneNumber, request.Code);

            if (!isValidCode) return BadRequest("Verification code is invalid");

            var result = await _authService.LoginOrGenerateAccessToken(request.PhoneNumber);
            return Ok(new AuthResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            });
        }

        /// <summary>
        /// Registers a user in the system
        /// </summary>
        /// <response code="200">User successfully registered</response>
        /// <response code="400">User with such settings already exists</response>
        [HttpPost("register")]
        [Authorize]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<string>), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Generates a new pair of access/refresh tokens
        /// </summary>
        /// <response code="200">Pair of access/refresh tokens successfully generated</response>
        /// <response code="400">Tokens are invalid</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<string>), StatusCodes.Status400BadRequest)]
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
