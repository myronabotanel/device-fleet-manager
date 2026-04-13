using DeviceFleetManager.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceFleetManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly AiService _aiService;

        public AiController(AiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] GenerateDescriptionRequest request)
        {
            var description = await _aiService.GenerateDescriptionAsync(
                request.Name,
                request.Manufacturer,
                request.Type,
                request.OperatingSystem,
                request.RamAmount,
                request.Processor
            );
            return Ok(new { description });
        }
    }

    public class GenerateDescriptionRequest
    {
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Type { get; set; } = "";
        public string OperatingSystem { get; set; } = "";
        public int RamAmount { get; set; }
        public string Processor { get; set; } = "";
    }
}