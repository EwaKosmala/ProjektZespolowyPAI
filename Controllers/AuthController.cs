using lab1_gr1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ListaZakupow.Model.DataModels;
using lab1_gr1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ListaZakupow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly MyDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
        }


        // REJESTRACJA
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Nazwa użytkownika i hasło są wymagane.");
            }

            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (existingUser != null)
            {
                return Conflict("Użytkownik o takiej nazwie już istnieje.");
            }

            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password),
                RegistrationDate = DateTime.Now
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok("Użytkownik został zarejestrowany pomyślnie.");
        }

        // LOGOWANIE
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                return Unauthorized("Niepoprawny login lub hasło");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Niepoprawny login lub hasło");
            }

            return Ok("Zalogowano pomyślnie");
        }

        // WYLOGOWANIE
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // W przyszłości można dodać czyszczenie sesji/tokena
            return Ok("Wylogowano pomyślnie");
        }
    }
}
