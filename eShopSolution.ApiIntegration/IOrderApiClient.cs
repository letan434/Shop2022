using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Orders;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;

namespace eShopSolution.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<bool> CreatOderDetail(CheckoutRequest request);
        Task<ApiResult<PagedResult<OrderVm>>> GetOrdersPagings(PagingRequestBase request);
        Task<ApiResult<OrderDetailAdminVm>> GetById(int id);
        Task<ApiResult<bool>> ChangeStatus(int id, OrderDetailAdminVm request);

    }
}