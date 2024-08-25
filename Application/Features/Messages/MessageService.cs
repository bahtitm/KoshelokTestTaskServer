using Application.Interfaces;
using AutoMapper;
using Domain.Entities;


namespace Application.Features.Messages
{
    public class MessageService : IMessageService
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

        public async Task<IEnumerable<GetedMessage>> GetAll()
        {
            var messages = await _messageRepository.GetAll();
            var getedmessages = _mapper.Map<List<GetedMessage>>(messages);
            return getedmessages;
        }

        public async Task<GetedMessage> GetById(int id)
        {
            var message = await _messageRepository.GetById(id);
            if (message == null)
                throw new KeyNotFoundException("User not found");
            var getedmessage = _mapper.Map<GetedMessage>(message);

            return getedmessage;
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
