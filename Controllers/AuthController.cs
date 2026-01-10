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
    /// <summary>
    /// Kontroler odpowiedzialny za uwierzytelnianie użytkowników.
    /// Obsługuje rejestrację, logowanie, wylogowanie,
    /// zmianę hasła oraz usuwanie konta.
    /// </summary>
    public class AuthController : BaseController
    {
        /// <summary>
        /// Kontekst bazy danych aplikacji.
        /// </summary>
        private readonly MyDBContext _dbContext;

        /// <summary>
        /// Narzędzie do haszowania i weryfikacji haseł użytkowników.
        /// </summary>
        private readonly PasswordHasher<User> _passwordHasher;

        /// <summary>
        /// Serwis odpowiedzialny za logikę użytkowników.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Konstruktor kontrolera uwierzytelniania.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych</param>
        /// <param name="userService">Serwis użytkowników</param>
        public AuthController(MyDBContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
            _userService = userService;
        }

        /// <summary>
        /// Strona główna kontrolera uwierzytelniania.
        /// </summary>
        /// <returns>Widok startowy</returns>
        public IActionResult Index()
        {
            return View();
        }

        // ===================== REJESTRACJA =====================

        /// <summary>
        /// Wyświetla formularz rejestracji użytkownika.
        /// </summary>
        /// <returns>Widok rejestracji</returns>
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        /// <summary>
        /// Obsługuje wysłanie formularza rejestracji.
        /// Tworzy nowe konto użytkownika.
        /// </summary>
        /// <param name="model">Model danych rejestracyjnych</param>
        /// <returns>
        /// Przekierowanie do logowania lub ponowne wyświetlenie formularza
        /// w przypadku błędu
        /// </returns>
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

        // ===================== LOGOWANIE =====================

        /// <summary>
        /// Wyświetla formularz logowania użytkownika.
        /// </summary>
        /// <returns>Widok logowania</returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Obsługuje wysłanie formularza logowania.
        /// Uwierzytelnia użytkownika i zapisuje dane w sesji.
        /// </summary>
        /// <param name="model">Model logowania</param>
        /// <returns>
        /// Przekierowanie do strony z przepisami lub ponowne wyświetlenie widoku
        /// w przypadku błędu
        /// </returns>
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

        /// <summary>
        /// Widok dostępny po poprawnym zalogowaniu użytkownika.
        /// Sprawdza, czy użytkownik posiada aktywną sesję.
        /// </summary>
        /// <returns>
        /// Widok użytkownika lub przekierowanie do logowania
        /// </returns>
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

        // ===================== WYLOGOWANIE =====================

        /// <summary>
        /// Wylogowuje aktualnie zalogowanego użytkownika
        /// i czyści sesję.
        /// </summary>
        /// <returns>Przekierowanie do logowania</returns>
        [HttpPost]
        public IActionResult Logout()
        {
            _userService.Logout(HttpContext);
            return RedirectToAction("Login");
        }

        // ===================== ZMIANA HASŁA =====================

        /// <summary>
        /// Wyświetla formularz zmiany hasła.
        /// </summary>
        /// <returns>Widok zmiany hasła</returns>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Obsługuje zmianę hasła aktualnie zalogowanego użytkownika.
        /// </summary>
        /// <param name="currentPassword">Aktualne hasło użytkownika</param>
        /// <param name="newPassword">Nowe hasło użytkownika</param>
        /// <returns>Widok z informacją o powodzeniu lub błędzie</returns>
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

        // ===================== USUWANIE KONTA =====================

        /// <summary>
        /// Usuwa konto aktualnie zalogowanego użytkownika
        /// oraz czyści dane sesji.
        /// </summary>
        /// <returns>Przekierowanie do rejestracji</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            int userId = GetUserId();
            await _userService.DeleteAccountAsync(userId);

            HttpContext.Session.Clear();
            return RedirectToAction("Register");
        }
    }
}
