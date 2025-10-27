using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class BaseController : Controller
    {
        public int GetUserId()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                throw new Exception("Użytkownik niezalogowany");
            return userId.Value;
        }
    }
}
