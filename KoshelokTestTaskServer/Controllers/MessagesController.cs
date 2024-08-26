using Application.Features.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace KoshelokTestTaskServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private IMessageService _messageService;
        private readonly IConfiguration configuration;
        private readonly string websocketUri;

        public MessagesController(IMessageService messageService, IConfiguration configuration)
        {
            _messageService = messageService;
            this.configuration = configuration;
            websocketUri = configuration.GetSection("websocketUri").Value;
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
        [HttpPost("FromModel")]
        public async Task<IActionResult> Create(int number, string text)
        {
            CreateMessageRequest model = new();
            model.Number = number;
            model.Text = Encoding.UTF8.GetBytes(text);
            await _messageService.Create(model);
            return Ok(new { message = "User created" });
        }
        [HttpPost]
        public async Task<IActionResult> Create(int number)
        {
            CreateMessageRequest model = new();
            WebSocketMenager webSocketMenager = new WebSocketMenager();
            string text;
            byte[] data;
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;

            // If you need it as a string
            var remoteIpAddressString = remoteIpAddress?.ToString();

            try
            {

                using (var inputStream = Request.Body)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        inputStream.CopyTo(memoryStream);
                        data = memoryStream.ToArray();
                    }
                }
                model.Text = data;
                model.Number=number;
                model.Date = DateTime.Now;
                text = Encoding.UTF8.GetString(data);
                await _messageService.Create(model);
                Uri clientUri = new Uri(websocketUri);
                await webSocketMenager.ConnectAsync(clientUri);

                var modelForSendWebsocket = new { Text = text, Number = number, Date = model.Date };

                var json = JsonSerializer.Serialize(modelForSendWebsocket);
                await webSocketMenager.SendMessageAsync(json);
                return NoContent();


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
