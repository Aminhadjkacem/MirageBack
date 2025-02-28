using DHP_Manager.Entities;

namespace DHP_Manager.Models.Details
{
    public class UserDetails
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public string? Token { get; set; }

        public static UserDetails fromEntity(User user)
        {
            return new UserDetails
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username
            };
        }
    }
}
