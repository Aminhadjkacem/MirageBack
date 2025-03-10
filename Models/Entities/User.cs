using System.Text.Json.Serialization;

namespace DHP_Manager.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string mail { get; set; }

        [JsonIgnore]
        public required string Password { get; set; }
    }
}
