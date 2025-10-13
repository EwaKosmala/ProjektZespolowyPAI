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
    public class LoginController : Controller
    {
        private readonly MyDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        public LoginController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
        }

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
    }
}
