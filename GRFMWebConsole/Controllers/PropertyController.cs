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
    public class PropertyController : Controller
    {
        private readonly IPropertyService _service;
        private readonly IDropdownService _dropdownService;

        public PropertyController(IPropertyService service, IDropdownService dropdownService)
        {
            _service = service;
            _dropdownService = dropdownService;
        }

        public ActionResult Index()
        {
            return View(new Domain.PropertyLister());
        }

        public ActionResult List(Domain.PropertyLister mLister)
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
            try
            {
                ViewBag.Clients = _dropdownService.GetClientOptionsAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Create", new Domain.Property());
        }

        [HttpPost]
        public ActionResult Create(Domain.Property mProperty)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mProperty = StaticMethods.SetAuditOnUpsert(mProperty);
                mProperty = _service.CreateAsync(mProperty).GetAwaiter().GetResult();


                if (mProperty != null && mProperty.Id > 0)
                {
                    return Json(new
                    {
                        StatusCode = "Ok",
                        NotificationType = "success",
                        NotificationMsg = "Property has been saved successfully!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = "BadRequest",
                        NotificationType = "error",
                        NotificationMsg = "Error occurred while saving property!"
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
                ViewBag.Clients = _dropdownService.GetClientOptionsAsync().GetAwaiter().GetResult();
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mProperty = new Domain.Property();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mProperty = _service.GetAsync(id).GetAwaiter().GetResult();
                return PartialView("_Create", mProperty);
            }
            catch (UnauthorizedAccessException)
            {
                sessionExpired = true;
                return Json(new
                {
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
                    ViewBag.Clients = _dropdownService.GetClientOptionsAsync().GetAwaiter().GetResult();
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
                resultData = new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Property has been deleted!" };
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