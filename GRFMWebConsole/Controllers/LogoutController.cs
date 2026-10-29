using GRFMWebConsole.Filter;
using GRFMWebConsole.Infrastucture;
using System.Web.Mvc;

namespace GRFMWebConsole.Controllers
{
    [Authentication]
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            HttpContextHelper.LoginUser = null;
            return RedirectToAction("Index", "Authentication");
        }
    }
}