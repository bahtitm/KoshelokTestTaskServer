using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;


namespace Application.Features.Users
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IRepository<User> userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task Create(CreateUserRequest model)
        {
            // validate
            if (await _userRepository.Find(model.Email!) != null)
                throw new AppException("User with the email '" + model.Email + "' already exists");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = "";//BCrypt.HashPassword(model.Password);

            // save user
            await _userRepository.Create(user);
        }

        public async Task Update(int id, UpdateUserRequest model)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // validate
            var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
            if (emailChanged && await _userRepository.Find(model.Email!) != null)
                throw new AppException("User with the email '" + model.Email + "' already exists");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = "";//BCrypt.HashPassword(model.Password);

            // copy model props to user
            _mapper.Map(model, user);

            // save user
            await _userRepository.Update(user);
        }

        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }
    }
}
