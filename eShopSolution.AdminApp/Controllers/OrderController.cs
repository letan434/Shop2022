using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Orders;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.AdminApp.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderApiClient _orderApiClient;
        private readonly IConfiguration _configuration;
        public OrderController(
            IConfiguration configuration, IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            var request = new PagingRequestBase()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _orderApiClient.GetOrdersPagings(request);
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderApiClient.GetById(id);
            return View(result.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(OrderDetailAdminVm request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _orderApiClient.ChangeStatus(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập trạng thái thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["result"] = "Cập trạng thái thất bại";
                return RedirectToAction("Details");
            }
        }
    }
}