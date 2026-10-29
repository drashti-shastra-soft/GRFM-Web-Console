using GRFMWebConsole.Infrastucture.IService;
using GRFMWebConsole.Service;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace GRFMWebConsole
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IAuthVerificationLogService, AuthVerificationLogService>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<IDropdownService, DropdownService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IHttpClientService, HttpClientService>();
            container.RegisterType<IPropertyService, PropertyService>();
            container.RegisterType<IQuotationItemService, QuotationItemService>();
            container.RegisterType<IQuotationLabourService, QuotationLabourService>();
            container.RegisterType<IQuotationPDFTitleService, QuotationPDFTitleService>();
            container.RegisterType<IQuotationService, QuotationService>();
            container.RegisterType<IUnitOfMeasureService, UnitOfMeasureService>();
            container.RegisterType<IUserService, UserService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}