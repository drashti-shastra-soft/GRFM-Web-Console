using GRFMWebConsole.Infrastucture;
using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GRFMWebConsole.Controllers
{
    public class AuthVerificationLogController : Controller
    {

        private readonly IAuthVerificationLogService _service;

        public AuthVerificationLogController(IAuthVerificationLogService service)
        {
            _service = service;
        }

        public ActionResult Index(int id)
        {
            var mLog = new Domain.AuthVerificationLog();
            mLog.UserId = id;
            return View(mLog);
        }


        [HttpPost]
        public async Task<ActionResult> Index(Domain.AuthVerificationLog mAuthVerificationLog)
        {
            try
            {
                var mUser = await _service.VerifyOTPAsync(mAuthVerificationLog);
                if (mUser != null && mUser.Id > default(int))
                {
                    HttpContextHelper.LoginUser = mUser;
                    return Redirect(Url.Action("Index", "Dashboard"));
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Invalid OTP: The entered OTP is incorrect.";
                    return View(mAuthVerificationLog);
                }
            }
            catch (Exception ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }

            return View(mAuthVerificationLog);
        }


        [HttpPost]
        public async Task<JsonResult> ResendOTP()
        {
            try
            {
                var model = new Domain.AuthVerificationLog
                {
                    UserId = HttpContextHelper.LoginUser.Id
                };

                await _service.ResendOTPAsync(model);

                return Json(new { success = true, message = "OTP resent successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}