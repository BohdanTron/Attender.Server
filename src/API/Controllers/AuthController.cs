using Attender.Server.API.Auth;
using Attender.Server.API.Common;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendVerificationPhoneCode([FromBody] SendVerificationPhoneCodeRequest request)
        {
            var result = await _smsService.SendVerificationCodeTo(request.PhoneNumber);

            if (result.Succeeded) return NoContent();

            return BadRequest(new ErrorResponse(result.Errors));
        }

        /// <summary>
        /// Verifies a phone number and generate tokens to register or to login
        /// </summary>
        /// <response code="200">Phone number successfully verified</response>
        /// <response code="400">Verification code is invalid</response>
        [HttpPost("verify-phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthTokens>> VerifyPhone([FromBody] VerifyPhoneRequest request)
        {
            var verification = await _smsService.CheckVerificationCode(request.PhoneNumber, request.Code);
            if (!verification.Succeeded)
            {
                return BadRequest(new ErrorResponse(verification.Errors));
            }

            var result = await _authService.LoginOrGenerateAccessToken(request.PhoneNumber);
            return Ok(result);
        }

        /// <summary>
        /// Registers a user in the system
        /// </summary>
        /// <response code="200">User successfully registered</response>
        /// <response code="400">User with such settings already exists</response>
        [HttpPost("register")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthTokens>> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.Register(request.PhoneNumber, request.UserName, request.Email, request.AvatarId);

            if (result.Succeeded) return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.Errors));
        }

        /// <summary>
        /// Generates a new pair of access/refresh tokens
        /// </summary>
        /// <response code="200">Pair of access/refresh tokens successfully generated</response>
        /// <response code="400">Tokens are invalid</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthTokens>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshToken(request.AccessToken, request.RefreshToken);

            if (result.Succeeded) return Ok(result.Data);

            return BadRequest(new ErrorResponse(result.Errors));
        }
    }
}
