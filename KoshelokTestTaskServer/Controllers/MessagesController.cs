using Application.Features.Messages;
using Application.Features.Users;
using Microsoft.AspNetCore.Mvc;

namespace KoshelokTestTaskServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var message = await _messageService.GetAll();
            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await _messageService.GetById(id);
            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageRequest model)
        {
            await _messageService.Create(model);
            return Ok(new { message = "User created" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMessageRequest model)
        {
            await _messageService.Update(id, model);
            return Ok(new { message = "User updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _messageService.Delete(id);
            return Ok(new { message = "User deleted" });
        }
    }
}
