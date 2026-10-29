using GRFMWebConsole.Filter;
using GRFMWebConsole.Helper;
using GRFMWebConsole.Infrastucture;
using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Dynamic;
using System.Web.Mvc;

namespace GRFMWebConsole.Controllers
{
    [Authentication]
    public class ClientController : Controller
    {
        private readonly IClientService _service;
        private readonly IDropdownService _dropdownService;

        public ClientController(IClientService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.ClientLister());
        }

        public ActionResult List(Domain.ClientLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.RedirectUrl = Url.Action("Index", "Authentication");
                return PartialView("_Unauthorized");
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Something went wrong!";
            }
            finally
            {
                ViewBag.Pagination = _dropdownService.GetPageSizeOptions(mLister.Pagination.PageSize);
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
                ViewBag.Status = _dropdownService.GetStatusOptions();
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create", new Domain.Client());
        }

        [HttpPost]
        public ActionResult Create(Domain.Client mClient)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClient = StaticMethods.SetAuditOnUpsert(mClient);
                mClient = _service.CreateAsync(mClient).GetAwaiter().GetResult();

                if (mClient != null && mClient.Id > 0)
                {
                    return Json(new
                    {
                        StatusCode = "Ok",
                        NotificationType = "success",
                        NotificationMsg = "Client has been saved successfully!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = "BadRequest",
                        NotificationType = "error",
                        NotificationMsg = "Error occurred while saving client!"
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new
                {
                    StatusCode = "Unauthorized",
                    RedirectUrl = Url.Action("Index", "Authentication")
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    StatusCode = "InternalServerError",
                    NotificationType = "error",
                    NotificationMsg = "Internal server error!"
                });
            }
            finally
            {
                ViewBag.Status = _dropdownService.GetStatusOptions();
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mClient = new Domain.Client();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mClient = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mClient);
            }
            catch (UnauthorizedAccessException)
            {
                sessionExpired = true;
                return Json(new {
                    StatusCode = "Unauthorized",
                    RedirectUrl = Url.Action("Index", "Authentication")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (BadRequestException ex)
            {
                return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal server error!" }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (!sessionExpired)
                {
                    ViewBag.Status = _dropdownService.GetStatusOptions();
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Client has been deleted!" };
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new
                {
                    StatusCode = "Unauthorized",
                    RedirectUrl = Url.Action("Index", "Authentication")
                };
            }
            catch (BadRequestException ex)
            {
                resultData = new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = ex.Message };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Internal server error!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

    }
}