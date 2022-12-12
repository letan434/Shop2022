using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVm>> GetPagings(GetManageProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<bool> UpdateProduct(ProductUpdateRequest request);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<ProductVm> GetById(int id);

        Task<List<ProductVm>> GetFeaturedProducts( int take);

        Task<List<ProductVm>> GetLatestProducts( int take);

        Task<bool> DeleteProduct(int id);
        Task<List<ProductOfOrder>> getProductOldOrder();
        Task<ApiResult<bool>> CreateProductStart(ProductStartCreateRequest request);
        Task<List<ProductStartVm>> getProductStartByProductId(int productId);


        Task<List<ProductVm>> GetRecommendationProducts(int take);


    }
}