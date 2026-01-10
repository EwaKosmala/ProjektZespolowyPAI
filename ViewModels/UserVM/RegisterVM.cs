using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.UserVM
{
    /// <summary>
    /// ViewModel używany do rejestracji nowego użytkownika.
    /// </summary>
    public class RegisterVM
    {
        /// <summary>
        /// Nazwa użytkownika (unikalna).
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Hasło użytkownika.
        /// </summary>
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Potwierdzenie hasła. Musi być zgodne z polem <see cref="Password"/>.
        /// </summary>
        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
