using lab1_gr1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ListaZakupow.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using lab1_gr1.ViewModels.UserVM;
using lab1_gr1.Interfaces;
using lab1_gr1.Controllers;
using lab1_gr1.Services;

namespace ListaZakupow.Controllers
{
    public class AuthController : BaseController
    {
        private readonly MyDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IUserService _userService;

        public AuthController(MyDBContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
            _userService = userService;
        }
        public IActionResult Index()
        {

            return View();
        }

        // REJESTRACJA
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _userService.RegisterAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Użytkownik o takiej nazwie już istnieje.");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        // LOGOWANIE
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.LoginAsync(model);
            if (user == null)
            {
                ViewBag.Error = "Niepoprawny login lub hasło";
                return View(model);
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Recipe");
        }
        public IActionResult LoggedIn()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var model = new LoginVM { Username = username };
            return View(model);
        }


        // WYLOGOWANIE
        [HttpPost]
        public IActionResult Logout()
        {
            _userService.Logout(HttpContext);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            int userId = GetUserId();
            var result = await _userService.ChangePasswordAsync(userId, currentPassword, newPassword);
            if (!result)
            {
                ViewBag.Error = "Niepoprawne aktualne hasło.";
                return View();
            }
            ViewBag.Message = "Hasło zostało zmienione.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            int userId = GetUserId();
            await _userService.DeleteAccountAsync(userId);
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
