using GRFMWebConsole.Filter;
using System.Web.Mvc;

namespace GRFMWebConsole.Controllers
{
    [Authentication]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}