using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace Iruka.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/" });
            }

            ViewBag.UserId = userId; return View();
        }
    }
}