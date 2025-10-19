using lab1_gr1.Models.DTO;
using lab1_gr1.Models;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListaZakupow.Controllers
{
    public class AuthController : Controller
    {
        private readonly MyDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
        }

        // GET: /Auth/Index
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;
            return View();
        }


        // GET: /Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid || await _dbContext.Users.AnyAsync(u => u.Username == dto.Username))
            {
                ModelState.AddModelError("", "Niepoprawne dane lub użytkownik już istnieje.");
                return View("Index", dto); 
            }

            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password),
                RegistrationDate = DateTime.Now
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = "Rejestracja zakończona pomyślnie. Zaloguj się.";
            return RedirectToAction("Index");
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View("Index", dto);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Niepoprawny login lub hasło.");
                return View("Index", dto);
            }

            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("LoggedIn"); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Wylogowano pomyślnie.";
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult LoggedIn()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index"); 

            ViewBag.Username = username;
            return View();
        }

    }
}
