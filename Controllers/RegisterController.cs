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
    public class RegisterController : Controller
    {
        private readonly MyDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public RegisterController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
        }

        // POST: api/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Nazwa użytkownika i hasło są wymagane.");
            }

            // sprawdzamy czy użytkownik o takim loginie już istnieje
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (existingUser != null)
            {
                return Conflict("Użytkownik o takiej nazwie już istnieje."); // HTTP 409
            }

            // tworzymy nowy obiekt użytkownika
            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password),
                RegistrationDate = DateTime.Now
            };

            // dodajemy do bazy
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok("Użytkownik został zarejestrowany pomyślnie.");
        }
    }
}
