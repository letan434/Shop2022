using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.ApiIntegration
{
    public class OrderApiClient : BaseApiClient, IOrderApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public OrderApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreatOderDetail(CheckoutRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);


            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            //var requestContent = new MultipartFormDataContent();
            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.Address.ToString()), "address");

            requestContent.Add(new StringContent(request.PhoneNumber.ToString()), "phoneNumber");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Email.ToString()), "email");
            //var dataOrderDetail = new MultipartFormDataContent();
            //if (request.OrderDetails.Count > 0)
            //{
            //    request.OrderDetails.ForEach(
            //        value =>
            //        {
            //            if (value != null)
            //            {
            //                dataOrderDetail.Add(new StringContent(value.ProductId.ToString()), "productId");
            //                dataOrderDetail.Add(new StringContent(value.Quantity.ToString()), "quantity");

            //            }
            //            requestContent.Add(dataOrderDetail, "orderDetail");

            //        });
            //}
            var response = await client.PostAsync($"/api/Orders/checkout/", requestContent);
            return response.IsSuccessStatusCode;

        }
    }
}
