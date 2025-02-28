using DHP_Manager.Entities;
using DHP_Manager.Models.Details;

namespace DHP_Manager.Models.response
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string Token { get; set; }

        public static AuthenticateResponse fromDetails(UserDetails user)
        {
            return new AuthenticateResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Token = user.Token,
            };
        }
    }
}
