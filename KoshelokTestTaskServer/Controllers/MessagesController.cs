using Application.Features.Messages;
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
        public async Task<IActionResult> Create()
        {
            CreateMessageRequest model = new();
            try
            {

                using (var inputStream = Request.Body)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        inputStream.CopyTo(memoryStream);
                        model.Text = memoryStream.ToArray();
                    }

                }

                await _messageService.Create(model);
                return Ok(new { message = "User created" });


            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Error processing stream: {ex.Message}");
            }

        }      

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _messageService.Delete(id);
            return Ok(new { message = "User deleted" });
        }
    }
}
