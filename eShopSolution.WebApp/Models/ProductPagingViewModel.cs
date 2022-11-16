using System;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.WebApp.Models
{
    public class ProductPagingViewModel
    {
        
            public PagedResult<ProductVm> Products { get; set; }

        
    }
}
