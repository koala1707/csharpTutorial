using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto //DTO: Data Transfer Object
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}