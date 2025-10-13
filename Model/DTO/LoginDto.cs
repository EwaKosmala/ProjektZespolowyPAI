using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.Models.DTO
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
