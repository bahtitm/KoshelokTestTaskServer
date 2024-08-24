using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Messages
{
    public class CreateMessageRequest
    {
        [Required]
        public string? Text { get; set; }


    }
}
