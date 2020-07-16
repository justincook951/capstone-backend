using CapstoneQuizAPI.Models;

namespace CapstoneQuizAPI.DTOs
{
    /// <summary>
    /// Used to help describe token contents without requiring decoding by the consuming applications
    /// </summary>
    public class TokenDTO
    {
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
        public UserDTO User { get; set; }
    }
}
