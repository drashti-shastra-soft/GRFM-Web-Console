using GRFMWebConsole.Infrastucture.IService;
using GRFMWebConsole.Infrastucture;
using GRFMWebConsole.Domain;
using GRFMWebConsole.Filter;
using GRFMWebConsole.Helper;
using System.Dynamic;
using System.Web.Mvc;
using System;

namespace GRFMWebConsole.Controllers
{
    [Authentication]
    public class QuotationController : Controller
    {
        #region Deleclare
        private readonly IDropdownService _dropdownService;
        private readonly IPropertyService _propertyService;
        private readonly IQuotationService _service;
        private readonly IQuotationItemService _quotationItemService;
        private readonly IQuotationLabourService _quotationLabourService;
        private readonly IQuotationPDFTitleService _quotationPDFTitleService;

        public QuotationController(IDropdownService dropdownService, IPropertyService propertyService, IQuotationService quotationService, IQuotationItemService quotationItemService, IQuotationLabourService quotationLabourService, IQuotationPDFTitleService quotationPDFTitleService)
        {
            _dropdownService = dropdownService;
            _propertyService = propertyService;
            _service = quotationService;
            _quotationItemService = quotationItemService;
            _quotationLabourService = quotationLabourService;
            _quotationPDFTitleService = quotationPDFTitleService;
            _quotationLabourService = quotationLabourService;
        }
        #endregion

        #region Quotation

        public ActionResult Index()
        {
            return View(new Domain.QuotationLister());
        }

        public ActionResult List(Domain.QuotationLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
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
        public ActionResult Create(int subCategoryId = 0)
        {
            try
            {
                ViewBag.Clients = _dropdownService.GetClientOptionsAsync().GetAwaiter().GetResult();
                ViewBag.UnitMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            Domain.Quotation model = new Domain.Quotation();

            return View("Create", model);
        }


        [HttpPost]
        public ActionResult SaveQuotation(Domain.Quotation mQuotation)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mQuotation = StaticMethods.SetAuditOnUpsert(mQuotation);
                mQuotation = _service.CreateAsync(mQuotation).GetAwaiter().GetResult();

                if (mQuotation != null && mQuotation.Id > 0)
                {
                    resultData = new { IsSuccess = true, Message = "Quotation has been saved successfully!" };
                }
                else
                {
                    resultData = new { IsSuccess = false, Message = "Error occurred while saving quotation!" };
                }
            }
            catch (Exception)
            {
                resultData = new { IsSuccess = false, Message = "Internal server error!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Quotation has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting quotation!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mQuotation = new Domain.Quotation();

            try
            {
                System.Threading.Thread.Sleep(1000);
                mQuotation = _service.GetAsync(id).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }

            return View("Edit", mQuotation);
        }
        #endregion

        #region Quotation Item
        [HttpGet]
        public ActionResult AddQuotationItem()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                ViewBag.UnitMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
            return PartialView("_CreateItem", new Domain.QuotationItem());
        }

        [HttpPost]
        public ActionResult SaveQuotationItem(Domain.QuotationItem mQuotationItem)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mQuotationItem = StaticMethods.SetAuditOnUpsert(mQuotationItem);
                mQuotationItem = _quotationItemService.CreateAsync(mQuotationItem).GetAwaiter().GetResult();

                if (mQuotationItem != null && mQuotationItem.Id > 0)
                {
                    resultData = new { IsSuccess = true, Message = "Quotation Item has been saved successfully!", Data = mQuotationItem };
                }
                else
                {
                    resultData = new { IsSuccess = false, Message = "Error occurred while saving quotation item!" };
                }
            }
            catch (Exception)
            {
                resultData = new { IsSuccess = false, Message = "Internal server error!" };
            }

            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditQuotationItem(Domain.QuotationItem mItem)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.UnitMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_CreateItem", mItem);
        }

        [HttpGet]
        public ActionResult DeleteQuotationItem(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _quotationItemService.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Quotation Item has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting quotation item!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteFabricRooms(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _service.DeleteFabricRoomsAsync(id).GetAwaiter().GetResult();
                resultData = new { IsSuccess = true, Message = "Rooms has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { IsSuccess = false, Message = "Error occurred while deleting rooms!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Quotation Labour

        [HttpGet]
        public ActionResult AddLabour()
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                ViewBag.UnitMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
            return PartialView("_CreateLabour", new Domain.QuotationLabour());
        }

        [HttpPost]
        public ActionResult SaveQuotationLabour(Domain.QuotationLabour mQuotationLabour)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                mQuotationLabour = StaticMethods.SetAuditOnUpsert(mQuotationLabour);
                mQuotationLabour = _quotationLabourService.CreateAsync(mQuotationLabour).GetAwaiter().GetResult();

                if (mQuotationLabour != null && mQuotationLabour.Id > 0)
                {
                    resultData = new { IsSuccess = true, Message = "Quotation Labour has been saved successfully!", Data = mQuotationLabour };
                }
                else
                {
                    resultData = new { IsSuccess = false, Message = "Error occurred while saving quotation labour!" };
                }
            }
            catch (Exception)
            {
                resultData = new { IsSuccess = false, Message = "Internal server error!" };
            }

            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditQuotationLabour(Domain.QuotationLabour mLabour)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            finally
            {
                ViewBag.UnitMeasures = _dropdownService.GetUnitOfMeasureOptionsAsync().GetAwaiter().GetResult();
            }

            return PartialView("_CreateLabour", mLabour);
        }

        [HttpGet]
        public ActionResult DeleteQuotationLabour(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                _quotationLabourService.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { NotificationType = "success", NotificationMsg = "Quotation Labour has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { NotificationType = "error", NotificationMsg = "Error occurred while deleting quotation labour!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Generate Quote
        [HttpGet]
        public ActionResult GenerateQuote(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                System.Threading.Thread.Sleep(1000);
                var bas64String = _service.GenerateQuoteAsync(id).GetAwaiter().GetResult();
                if (bas64String.IsNotNullOrEmpty())
                {
                    resultData = new { StatusCode = "Ok", Base64String = bas64String };
                }
            }
            catch (UnauthorizedAccessException)
            {
                resultData = new { StatusCode = "Unauthorized" };
            }
            catch (Exception)
            {
                resultData = new { StatusCode = "InternalServerError", Message = "Internal server error!" };
            }
            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Quotation PDF Title
        [HttpGet]
        public ActionResult CreateQuotationPDFTitle(int id, decimal fabricGrandTotal, decimal labourGrandTotal)
        {
            var sessionExpired = false;
            dynamic resultData = new ExpandoObject();
            var mQuotationPDFTitle = new QuotationPDFTitle();
            try
            {
                mQuotationPDFTitle = _quotationPDFTitleService.GetPDFTitle(id).GetAwaiter().GetResult();
                if (mQuotationPDFTitle == null)
                    mQuotationPDFTitle = new Domain.QuotationPDFTitle();
                mQuotationPDFTitle.QuotationId = id;
                mQuotationPDFTitle.FabricGrandTotal = fabricGrandTotal;
                mQuotationPDFTitle.LabourGrandTotal = labourGrandTotal;
                return PartialView("_CreateQuotationPDFTitle", mQuotationPDFTitle);
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
                    ViewBag.Status = _dropdownService.GetStatusOptions();
                }
            }
        }

        [HttpPost]
        public ActionResult CreateQuotationPDFTitle(Domain.QuotationPDFTitle mQuotationPDFTitle)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mQuotationPDFTitle = StaticMethods.SetAuditOnUpsert(mQuotationPDFTitle);
                mQuotationPDFTitle = _quotationPDFTitleService.CreateAsync(mQuotationPDFTitle).GetAwaiter().GetResult();

                if (mQuotationPDFTitle != null && mQuotationPDFTitle.Id > 0)
                {
                    return Json(new { StatusCode = "Ok", NotificationType = "success", NotificationMsg = "Quotation PDF Title has been saved successfully!" });
                }
                else
                {
                    return Json(new { StatusCode = "BadRequest", NotificationType = "error", NotificationMsg = "Error occurred while saving Quotation PDF Title!" });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { StatusCode = "Unauthorized", RedirectUrl = Url.Action("Index", "Authentication") });
            }
            catch (Exception)
            {
                return Json(new { StatusCode = "InternalServerError", NotificationType = "error", NotificationMsg = "Internal server error!" });
            }

        }
        #endregion
    }
}