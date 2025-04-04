using ForexPrediction.Application.Commands;
using ForexPrediction.Application.Commands.UploadData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForexPrediction.WebApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize]
        public class DataController : ControllerBase
        {
            private readonly IMediator _mediator;

            public DataController ( IMediator mediator )
            {
                _mediator = mediator;
            }

            [HttpPost("upload/{pair}")]
            public async Task<IActionResult> UploadData ( string pair, IFormFile file )
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                using var stream = file.OpenReadStream();
                var command = new UploadDataCommand { Pair = pair, DataStream = stream };
                await _mediator.Send(command);
                return Ok("Data uploaded successfully");
            }
        }
    }
