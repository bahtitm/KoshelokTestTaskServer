using Domain.Entities;

namespace Application.Features.Messages
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAll();
        Task<Message> GetById(int id);
        Task Create(CreateMessageRequest model);
        Task Update(int id, UpdateMessageRequest model);
        Task Delete(int id);
    }
}
