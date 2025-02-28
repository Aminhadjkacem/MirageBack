using DHP_Manager.Entities;
using DHP_Manager.Models.input;

namespace DHP_Manager.Utils.Interface
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public int? ValidateJwtToken(string? token);
    }
}
