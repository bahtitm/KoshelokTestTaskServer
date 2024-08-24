using Application.Interfaces;
using AutoMapper;
using Domain.Entities;


namespace Application.Features.Messages
{
    public class MessageService :IMessageService
    {
        private IRepository<Message> _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(
            IRepository<Message> messageRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            return await _messageRepository.GetAll();
        }

        public async Task<Message> GetById(int id)
        {
            var user = await _messageRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task Create(CreateMessageRequest model)
        {
            var message = _mapper.Map<Message>(model);
            await _messageRepository.Create(message);
        }

        public async Task Update(int id, UpdateMessageRequest model)
        {
            var message = await _messageRepository.GetById(id);

            if (message == null)
                throw new KeyNotFoundException("User not found");
            _mapper.Map(model, message);


            await _messageRepository.Update(message);
        }

        public async Task Delete(int id)
        {
            await _messageRepository.Delete(id);
        }       
    }
}
