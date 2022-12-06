using System;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Catalog.Orders;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Sales;

namespace eShopSolution.Application.Catalog.orders
{
    
    public interface IOrderService
    {
        //Task<List<CategoryVm>> GetAll();

        Task<bool> Checkout(CheckoutRequest checkoutRequest);
        Task<ApiResult<PagedResult<OrderVm>>> GetOrdersPaging(PagingRequestBase request);
        Task<ApiResult<OrderDetailAdminVm>> GetById(int id);

    }
}
