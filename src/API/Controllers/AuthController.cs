using Attender.Server.Application.Auth.Commands.LoginOrGenerateToken;
using Attender.Server.Application.Auth.Commands.RefreshToken;
using Attender.Server.Application.Auth.Commands.RegisterUser;
using Attender.Server.Application.Auth.Commands.SendVerificationPhoneCode;
using Attender.Server.Application.Auth.Commands.VerifyPhone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Attender.Server.API.Controllers
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost("send-verification-phone-code")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<ActionResult> SendVerificationPhoneCode([FromBody] SendVerificationPhoneCodeCommand command)
        {
            var result = await Mediator.Send(command);

            if (result) return NoContent();

            return BadRequest("Phone number is invalid");
        }

        [HttpPost("verify-phone")]
        public async Task<ActionResult> VerifyPhone([FromBody] VerifyPhoneCommand command)
        {
            var isValidCode = await Mediator.Send(command);

            if (!isValidCode) return BadRequest("Verification code is invalid");

            var result = await Mediator.Send(new LoginOrGenerateTokenCommand { PhoneNumber = command.PhoneNumber });
            if (result.Success)
            {
                //TODO: Consider moving to separate method/class
                return Ok(new
                {
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Success)
            {
                return Ok(new
                {
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Success)
            {
                return Ok(new
                {
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken
                });
            }

            return BadRequest(result.Errors);
        }
    }
}
