using ForexPrediction.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForexPrediction.WebApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize]
        public class PredictionController : ControllerBase
        {
            private readonly IMediator _mediator;

            public PredictionController ( IMediator mediator )
            {
                _mediator = mediator;
            }

            [HttpGet("{pair}")]
            public async Task<IActionResult> GetPredictions ( string pair, [FromQuery] DateTime startDate, [FromQuery] int days = 7 )
            {
                var query = new GetPredictionsQuery { Pair = pair, StartDate = startDate, Days = days };
                var predictions = await _mediator.Send(query);
                return Ok(predictions);
            }
        }
    }
