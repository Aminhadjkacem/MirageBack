using DHP_Manager.Models.input;

namespace DHP_Manager.Models.DTOs
{
    public class CreateUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static UserInput toInput(CreateUserDTO Dto)
        {
            return new UserInput
            {
                FirstName = Dto.FirstName,
                LastName = Dto.LastName,
                mail = Dto.mail,
                Password = Dto.Password
            };
        }
    }
}
