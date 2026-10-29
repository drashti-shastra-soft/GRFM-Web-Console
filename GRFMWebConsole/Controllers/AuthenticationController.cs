using GRFMWebConsole.Infrastucture;
using GRFMWebConsole.Infrastucture.IService;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GRFMWebConsole.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _service;
        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            Domain.Credential mCredential = null;
            try
            {
                var cookie = Request.Cookies["GREMLoginUser"];
                if (!string.IsNullOrWhiteSpace(cookie?.Value))
                {
                    mCredential = JsonConvert.DeserializeObject<Domain.Credential>(cookie.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(mCredential ?? new Domain.Credential());
        }

        [HttpPost]
        public async Task<ActionResult> Index(Domain.Credential mCredential)
        {
            try
            {
                var mUser = await _service.AuthenticateAsync(mCredential);

                if (mUser == null || mUser.Id <= 0)
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Authentication failed: user identity could not be verified!";
                    return View(mCredential);
                }

                if (!mUser.IsActive)
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Account Suspended: Access has been temporarily disabled. Kindly reach out to your administrator.";
                    return View(mCredential);
                }

                var cookie = new HttpCookie("GREMLoginUser", mCredential.RememberMe ? JsonConvert.SerializeObject(mCredential) : null)
                {
                    Expires = mCredential.RememberMe ? DateTime.Now.AddYears(1) : DateTime.Now.AddDays(-1)
                };

                HttpContext.Response.Cookies.Add(cookie);

                return Redirect(Url.Action("Index", "AuthVerificationLog", new { id = mUser.Id }));
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }

            return View(mCredential);
        }
    }
}