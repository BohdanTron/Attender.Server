﻿using Attender.Server.Application.Common.Auth.Dtos;
using Attender.Server.Application.Common.Auth.Models;
using Attender.Server.Application.Common.Auth.Services;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Common.Sms.Dtos;
using Attender.Server.Application.Common.Sms.Services;
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
        [HttpPost("send-phone-code")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendPhoneCode([FromBody] PhoneSendingDto dto)
        {
            var result = await _smsService.SendVerificationCode(dto);
            return result.Succeeded 
                ? NoContent() 
                : BadRequest(result.Error);
        }

        /// <summary>
        /// Verifies a phone number and generate tokens to register or to login
        /// </summary>
        /// <response code="200">Phone number successfully verified</response>
        /// <response code="400">Verification code is invalid</response>
        [HttpPost("verify-phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthInfo>> VerifyPhone([FromBody] PhoneVerificationDto dto)
        {
            var verification = await _smsService.CheckVerificationCode(dto);

            if (!verification.Succeeded) return BadRequest(verification.Error);

            var result = await _authService.LoginOrGenerateAccessToken(dto.PhoneNumber);
            return Ok(result);
        }

        /// <summary>
        /// Registers a user in the system
        /// </summary>
        /// <response code="200">User successfully registered</response>
        /// <response code="400">User settings are invalid</response>
        [HttpPost("register")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthInfo>> Register([FromBody] UserRegistrationInfoDto dto)
        {
            var result = await _authService.RegisterUser(dto);
            return result.Succeeded
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }

        /// <summary>
        /// Generates a new pair of access/refresh tokens
        /// </summary>
        /// <response code="200">Pair of access/refresh tokens successfully generated</response>
        /// <response code="400">Tokens are invalid</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthTokens>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var result = await _authService.RefreshToken(dto);
            return result.Succeeded
                ? Ok(result.Data)
                : BadRequest(result.Error);
        }
    }
}
