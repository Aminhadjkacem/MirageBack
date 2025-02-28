using DHP_Manager.Entities;
using DHP_Manager.Models.Details;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.input;
using DHP_Manager.Models.response;

namespace DHP_Manager.Services.Interface
{
    public interface IUserService
    {
        Task<UserDetails?> Authenticate(AuthenticationDTO dto);
        Task<UserDetails> CreateUserAsync(UserInput input);
        UserDetails? GetById(int id);
    }
}
