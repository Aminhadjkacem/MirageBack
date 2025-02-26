using DHP_Manager.Entities;
using DHP_Manager.Models.DTOs;

namespace DHP_Manager.Services.Interface
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticationDTO dto);
        Task<User> CreateUserAsync(CreateUserDTO dto);
        IEnumerable<User> GetAll();
        User? GetById(int id);
    }
}
