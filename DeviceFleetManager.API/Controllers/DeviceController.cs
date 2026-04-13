using DeviceFleetManager.API.Models;
using DeviceFleetManager.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceFleetManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceService _service;

        public DeviceController(DeviceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<Device>> GetAll() =>
            await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetById(string id)
        {
            var device = await _service.GetByIdAsync(id);
            if (device is null) return NotFound();
            return device;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Device device)
        {
            await _service.CreateAsync(device);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Device device)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null) return NotFound();
            await _service.UpdateAsync(id, device);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null) return NotFound();
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}