using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.UserVM
{
    /// <summary>
    /// ViewModel używany do logowania użytkownika.
    /// </summary>
    public class LoginVM
    {
        /// <summary>
        /// Nazwa użytkownika.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Hasło użytkownika.
        /// </summary>
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
