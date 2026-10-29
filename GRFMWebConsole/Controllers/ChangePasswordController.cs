using GRFMWebConsole.Filter;
using GRFMWebConsole.Infrastucture;
using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Web.Mvc;

namespace GRFMWebConsole.Controllers
{
    [Authentication]
    public class ChangePasswordController : Controller
    {
        private readonly IChangePasswordService _service;

        public ChangePasswordController(IChangePasswordService changepasswordService)
        {
            _service = changepasswordService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new Domain.ChangePassword());
        }

        [HttpPost]
        public ActionResult Index(Domain.ChangePassword mChangePassword)
        {
            try
            {
                mChangePassword.UserId = HttpContextHelper.LoginUser.Id;

                _service.Initiate(mChangePassword).GetAwaiter().GetResult();
                ViewBag.NotificationType = "success";
                ViewBag.NotificationMsg = "Your password has been updated successfully!";
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Authentication");
            }
            catch (BadRequestException ex)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = ex.Message;
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return View(mChangePassword);
        }
    }
}