using DeviceFleetManager.API.Models;
using DeviceFleetManager.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceFleetManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<User>> GetAll() =>
            await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user is null) return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            await _service.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null) return NotFound();
            await _service.UpdateAsync(id, user);
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