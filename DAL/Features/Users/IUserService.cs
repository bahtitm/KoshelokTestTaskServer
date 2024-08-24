using Domain.Entities;

namespace Application.Features.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task Create(CreateUserRequest model);
        Task Update(int id, UpdateUserRequest model);
        Task Delete(int id);
    }
}
