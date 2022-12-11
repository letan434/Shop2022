using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Detail(int id, string culture)
        {
            var product = await _productApiClient.GetById(id);
            return View(new ProductDetailViewModel()
            {
                Product = product
            });
        }
        [HttpPost]
        public async Task<IActionResult> start(ProductStartVm request)
        {
            
            
            //TODO: Add to API
            var check = await _productApiClient.CreateProductStart(request);
            if (check)
            {
                TempData["SuccessMsg"] = "Gửi đánh giá thành công, chúng tôi sẽ liên hệ sớm cho bạn";

            }
            else
            {
                TempData["ErrorMsg"] = "Gửi phiếu mua hàng thất bại, mong bạn kiểm tra lại thông tin";

            }
            return View();
        }

        public async Task<IActionResult> Category(int id, string culture, int page = 1)
        {
            var products = await _productApiClient.GetPagings(new GetManageProductPagingRequest()
            {
                CategoryId = id,
                PageIndex = page,
                LanguageId = culture,
                PageSize = 10
            });
            var category = await _categoryApiClient.GetById(id);
            return View(new ProductCategoryViewModel()
            {
                Category = category,
                Products = products
            }); ;
        }
        public async Task<IActionResult> GetAll(string culture ,string keyword, int page = 1, int categoryId = 0)
        {
            var products = await _productApiClient.GetPagings(new GetManageProductPagingRequest()
            {
                CategoryId = categoryId,
                PageIndex = page,
                LanguageId = culture,
                PageSize = 10,
                Keyword = keyword,
            });
            return View(new ProductPagingViewModel()
            {
                Products = products
            }); 
        }
    }
}