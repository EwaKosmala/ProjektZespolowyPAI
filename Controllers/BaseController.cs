using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    /// <summary>
    /// Klasa bazowa dla kontrolerów aplikacji.
    /// Zawiera wspólne metody pomocnicze związane z sesją użytkownika.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Zwraca identyfikator aktualnie zalogowanego użytkownika
        /// zapisany w sesji HTTP.
        /// </summary>
        /// <returns>
        /// Identyfikator użytkownika zalogowanego w aplikacji.
        /// </returns>
        /// <exception cref="Exception">
        /// Rzucany, gdy użytkownik nie jest zalogowany
        /// lub brak identyfikatora w sesji.
        /// </exception>
        public int GetUserId()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                throw new Exception("Użytkownik niezalogowany");

            return userId.Value;
        }
    }
}
