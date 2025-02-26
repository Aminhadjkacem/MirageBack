using DHP_Manager.Entities;
using DHP_Manager.Helpers;
using DHP_Manager.Models;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.Interface;
using DHP_Manager.Utils.Interface;

namespace DHP_Manager.Services.Impl
{
    public class UserService : IUserService
    {

        private readonly IJwtUtils _jwtUtils;
        private readonly IUserRepository _userRepository;

        public UserService(IJwtUtils jwtUtils, IUserRepository userRepository)
        {
            _jwtUtils = jwtUtils;
            _userRepository = userRepository;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticationDTO dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || user.Password == null ||
                !PasswordHelper.VerifyPassword(dto.Password, user.Password))
            {
                return null;
            }

            // Generate JWT token.
            var token = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public async Task<User> CreateUserAsync(CreateUserDTO dto)
        {
            // Check if a user with the same username already exists.
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new Exception("Username already exists");

            // Create a new user with the hashed password.
            var newUser = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Password = PasswordHelper.HashPassword(dto.Password)
            };

            return await _userRepository.CreateUserAsync(newUser);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAllAsync().Result;
        }

        public User? GetById(int id)
        {
            return _userRepository.GetByIdAsync(id).Result;
        }
    }
}
