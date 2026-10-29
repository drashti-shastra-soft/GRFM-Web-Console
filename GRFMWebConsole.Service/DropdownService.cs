using GRFMWebConsole.Infrastucture;
using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GRFMWebConsole.Service
{
    public class DropdownService : IDropdownService
    {
        #region Declaration
        private readonly IHttpClientService _httpClientService;
        private readonly string baseUrl;

        public DropdownService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");
        }

        #endregion

        #region Status
        public dynamic GetStatusOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Active", Value = "Active" },
                new SelectListItem { Text = "Disabled", Value = "Disabled" }
            };
        }

        #endregion

        #region Pagenation

        public dynamic GetPageSizeOptions(int selectedSize = 20)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "20", Value = "20" },
                new SelectListItem { Text = "50", Value = "50" },
                new SelectListItem { Text = "100", Value = "100" },
                new SelectListItem { Text = "All", Value = "-1" }
            };

            list.ForEach(item => item.Selected = item.Value == selectedSize.ToString());
            return list;
        }

        #endregion

        #region User Type
        public async Task<dynamic> GetUserTypeOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mUserTypes = await _httpClientService.GetAsync<List<Domain.UserType>>(
                url: $"{baseUrl}/UserType",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mUserType in mUserTypes)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mUserType.Name, Value = Convert.ToString(mUserType.Id) });
            }
            return mSelectListItems;
        }
        #endregion


       

        

        #region Unit Measure
        public async Task<dynamic> GetUnitOfMeasureOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mUnitofMeasures = await _httpClientService.GetAsync<List<Domain.UnitOfMeasure>>(
                url: $"{baseUrl}/UnitOfMeasure/GetActiveUnitOfMeasure",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mUnitOfMeasure in mUnitofMeasures)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mUnitOfMeasure.Name, Value = Convert.ToString(mUnitOfMeasure.Id) });
            }
            return mSelectListItems;
        }
        #endregion


        #region Client
        public async Task<dynamic> GetClientOptionsAsync()
        {
            var mSelectListItems = new List<SelectListItem>();

            var mClients = await _httpClientService.GetAsync<List<Domain.Client>>(
                url: $"{baseUrl}/Client/GetActiveClient",
                token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var mClient in mClients)
            {
                mSelectListItems.Add(new SelectListItem() { Text = mClient.Name, Value = Convert.ToString(mClient.Id) });
            }
            return mSelectListItems;
        }


        public async Task<dynamic> GetPropertyOptionsAsync(int clientId, int propertyId = 0)
        {
            var mSelectListItems = new List<SelectListItem>();

            var mProperties = await _httpClientService.GetAsync<List<Domain.Property>>(
                    url: $"{baseUrl}/Property/GetActiveBy/ClientId/{clientId}",
                    token: HttpContextHelper.LoginUser?.Token
            ).ConfigureAwait(false);

            foreach (var prop in mProperties)
            {
                var fullAddress = $"{prop.SocietyName}, {prop.LandMark}, {prop.Area}, {prop.City}, {prop.State} - {prop.ZipCode}";

                mSelectListItems.Add(new SelectListItem()
                {
                    Text = fullAddress,
                    Value = prop.Id.ToString()
                });
            }

            if (propertyId > 0)
                mSelectListItems.ForEach(item => item.Selected = item.Value == propertyId.ToString());

            return mSelectListItems;
        }

        #endregion
      
    }
}
