using DHP_Manager.Entities;
using DHP_Manager.Helpers;
using DHP_Manager.Models;
using DHP_Manager.Models.Details;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.input;
using DHP_Manager.Models.response;
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

        public async Task<UserDetails?> Authenticate(AuthenticationDTO authenticationDTO)
        {
            if (authenticationDTO.Username == null || authenticationDTO.Password == null)
            {
                throw new Exception("Wrong credentials");
            }
            User? user = await _userRepository.GetByUsernameAsync(authenticationDTO.Username);
            if (user == null)
            {
                throw new Exception("Wrong credentials");
            }
            if (!PasswordHelper.VerifyPassword(authenticationDTO.Password, user.Password))
            {
                throw new Exception("Wrong credentials");
            }

            // Generate JWT token.
            string token = _jwtUtils.GenerateJwtToken(user);
            UserDetails userDetails = UserDetails.fromEntity(user);
            userDetails.Token = token;
            return userDetails;
        }

        public async Task<UserDetails> CreateUserAsync(UserInput userInput)
        {
            // Check if a user with the same username already exists.
            User? existingUser = await _userRepository.GetByUsernameAsync(userInput.mail);
            if (existingUser != null)
                throw new Exception("Username already exists");
            User createdUser = await _userRepository.CreateUserAsync(UserInput.toEntity(userInput));
            var token = _jwtUtils.GenerateJwtToken(createdUser);
            UserDetails userDetails = UserDetails.fromEntity(createdUser);
            userDetails.Token = token;
            return userDetails;
        }

        public UserDetails? GetById(int id)
        {
            User? user = _userRepository.GetByIdAsync(id).Result;
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return UserDetails.fromEntity(user);
        }
    }
}
