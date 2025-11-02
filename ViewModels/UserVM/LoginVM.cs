using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.UserVM
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
