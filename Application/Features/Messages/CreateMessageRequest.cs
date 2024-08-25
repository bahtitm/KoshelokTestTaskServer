using System.ComponentModel.DataAnnotations;

namespace Application.Features.Messages
{
    public class CreateMessageRequest
    {
        [Required(ErrorMessage = "Не указано Text")]
        [StringLength(128, ErrorMessage = "Длина строки должна быть до 128 символов")]
        public byte[] Text { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }

    }
}
