
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeevanSetu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DonationHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
