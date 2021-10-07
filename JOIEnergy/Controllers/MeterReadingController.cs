using System.Threading.Tasks;
using JOIEnergy.Application;
using JOIEnergy.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [ApiController]
    [Route("readings")]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IMediator _mediator;

        public MeterReadingController(IMeterReadingService meterReadingService, IMediator mediator)
        {
            _meterReadingService = meterReadingService;
            _mediator = mediator;
        }
        // POST api/values
        [HttpPost("store")]
        public async Task<IActionResult> Post([FromBody] ElectricityReadingCommand electricityReadingCommand)
        {
            await _mediator.Send(electricityReadingCommand);
            return Ok();
        }

        [HttpGet("read/{smartMeterId}")]
        public IActionResult GetReading(string smartMeterId)
        {
            return Ok(_meterReadingService.GetReadings(smartMeterId));
        }
    }
}
