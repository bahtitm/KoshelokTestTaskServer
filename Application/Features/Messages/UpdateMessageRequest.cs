using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Messages
{
    public class UpdateMessageRequest
    {
        [Required]
        public IFormFile? Text { get; set; }
        public int Number { get; set; }
    }
}
