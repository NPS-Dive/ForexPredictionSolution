using ForexPrediction.Application.Commands;
using ForexPrediction.Application.Commands.LoginUser;
using ForexPrediction.Application.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForexPrediction.WebApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AccountController : ControllerBase
        {
            private readonly IMediator _mediator;

            public AccountController ( IMediator mediator )
            {
                _mediator = mediator;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register ( [FromBody] RegisterUserCommand command )
            {
                var token = await _mediator.Send(command);
                return Ok(new { Token = token });
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login ( [FromBody] LoginUserCommand command )
            {
                var token = await _mediator.Send(command);
                return Ok(new { Token = token });
            }
        }
    }
