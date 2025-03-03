using DHP_Manager.Entities;

namespace DHP_Manager.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateUserAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
