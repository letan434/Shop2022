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
            var productStarts = await _productApiClient.getProductStartByProductId(id);

            return View(new ProductDetailViewModel()
            {
                Product = product,
                ProductStarts = productStarts
            });
        }
        [HttpPost]
        public async Task<IActionResult> Detail(ProductDetailViewModel request)
        {
            
            
            //TODO: Add to API
            var check = await _productApiClient.CreateProductStart(request.ProductStart);
            if (check.IsSuccessed)
            {
                TempData["SuccessMsg"] = "Gửi đánh giá thành công, cảm ơn bạn!";

            }
            else
            {
                if(check.Message != "")
                {
                    TempData["ErrorMsg"] = check.Message;

                }
                else
                {
                    TempData["ErrorMsg"] = "Gửi đánh giá thất bại, bạn kiểm tra lại thông tin!";

                }

            }
            var product = await _productApiClient.GetById(request.ProductStart.ProductId);
            var productStarts = await _productApiClient.getProductStartByProductId(request.ProductStart.ProductId);

            request.Product = product;
            request.ProductStarts = productStarts;
            return View(request);
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