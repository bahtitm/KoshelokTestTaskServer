using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Messages
{
    public class CreateMessageRequest
    {
        
        public IFormFile? Text { get; set; }


    }
}
