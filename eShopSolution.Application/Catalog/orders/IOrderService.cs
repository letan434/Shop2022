using System;
using System.Threading.Tasks;
using eShopSolution.ViewModels.Sales;

namespace eShopSolution.Application.Catalog.orders
{
    
    public interface IOrderService
    {
        //Task<List<CategoryVm>> GetAll();

        Task<bool> Checkout(CheckoutRequest checkoutRequest);
    }
}
