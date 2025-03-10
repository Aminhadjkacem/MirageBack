using DHP_Manager.Entities;
using DHP_Manager.Helpers;

namespace DHP_Manager.Models.input
{
    public class UserInput
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static User toEntity(UserInput input)
        {
            return new User
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                mail = input.mail,
                Password = PasswordHelper.HashPassword(input.Password)
            };
        }
    }
}
